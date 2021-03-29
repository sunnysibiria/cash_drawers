using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CashManagment.Domain.Models;
using CashManagment.Application.V10;
using CashManagment.Api.Models;
using CashManagment.Api.Constants;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Extensions.Logging;
using CashManagment.Application.V10.Models;

namespace CashManagment.Api.Controllers.V10
{
    [Produces("application/json")]
    [Route("cassette")]
    [ApiController]
    [ApiVersion(Consts.ApiVersion)]
    public class CassetteController : BaseController
    {
        private readonly ICassetteService _cassetteService;
        public ILogger<CassetteController> Logger { get; }

        public CassetteController(ICassetteService cassetteService, ILogger<CassetteController> logger)
        {
            _cassetteService = cassetteService;
            Logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(Description = "Метод реализует получение: выборки кассет, кассеты по ее номеру.")]
        [SwaggerResponse(200, Type = typeof(CassetteList), Description = "Выборка из списка кассет")]
        [SwaggerResponse(200, Type = typeof(Cassette), Description = "Выборка из списка кассет")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные.")]
        public async Task<IActionResult> GetAsync(CassetteRequest request)
        {
            if (request.Limit.HasValue && request.Offset.HasValue)
            {
                var result = await _cassetteService.GetAllAsync(request.Offset.Value, request.Limit.Value);
                return Ok(result);
            }

            if (!string.IsNullOrEmpty(request.Num))
            {
                var result = await _cassetteService.GetAsync(request.Num);
                if (result == null)
                {
                    return BadRequest(new { errorMessage = $"Кассета с номером {request.Num} в базе не обнаружена" });
                }
                else
                {
                    return Ok(result);
                }
            }

            return BadRequest();
        }

        [HttpPost]
        [SwaggerOperation(Description = "Изменяет статус(признак) кассеты по ее номеру.")]
        [SwaggerResponse(200, Type = typeof(CassetteProperties), Description = "Параметры кассеты")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные.")]
        public async Task<IActionResult> UpdateAsync([FromBody][Required] CassetteProperties request)
        {
            var result = await _cassetteService.UpdateAsync(request);
            if (result == null)
            {
                return BadRequest(new { errorMessage = $"Кассета с номером {request.Num} в базе не обнаружена" });
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("UpdateCities")]
        [SwaggerOperation(Description = "Загружает и обновляет список городов из ДРО сервиса")]
        [SwaggerResponse(200, Type = typeof(int), Description = "Количество загруженных городов")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные.")]
        public async Task<IActionResult> UpdateCitiesAsync()
        {
            Logger.LogDebug("Start updating cities");
            try
            {
                var result = await _cassetteService.UpdateCitiesAsync();
                Logger.LogDebug($"Finished download {result} new cities");
                return Ok(result);
            }
            catch(ApplicationException ex)
            {
                Logger.LogError($"Failed updating: {ex.Message}");
                return BadRequest(new { errorMessage = ex.Message });
            }
        }

        [HttpGet("GetCities")]
        [SwaggerOperation(Description = "Возвращает список городов кассет")]
        [SwaggerResponse(200, Type = typeof(List<RealContainerCity>), Description = "Список городов")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные.")]
        public async Task<IActionResult> GetCitiesAsync()
        {
            var result = await _cassetteService.GetCitiesAsync();
            return Ok(result);
        }
    }
}
