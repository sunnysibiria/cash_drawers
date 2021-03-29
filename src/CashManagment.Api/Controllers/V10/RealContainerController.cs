using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CashManagment.Api.Constants;
using Swashbuckle.AspNetCore.Annotations;
using CashManagment.Application.V10;
using CashManagment.Domain.Models;
using CashManagment.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace CashManagment.Api.Controllers.V10
{
    [Produces("application/json")]
    [Route("realContainer")]
    [ApiController]
    [ApiVersion(Consts.ApiVersion)]
    public class RealContainerController : BaseController
    {
        private readonly IStorageTransferService _serviceStorage;
        private readonly IRealContainerService _serviceReal;
        public RealContainerController(
            IStorageTransferService serviceStorage,
            IRealContainerService serviceReal)
        {
            _serviceStorage = serviceStorage;
            _serviceReal = serviceReal;
        }

        [HttpPost("GetRealContainersById")]
        [SwaggerResponse(200, Type = typeof(List<RealContainer>), Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<List<RealContainer>> GetRealContainersByIdAsync(
            [FromBody][Required(ErrorMessage = "Не задан обязательный параметр `realContainersId`")] int[] realContainersId)
        {
            return await _serviceReal.GetRealContainersByIdAsync(realContainersId);
        }

        [HttpGet("FindRealContainers")]
        [SwaggerResponse(200, Type = typeof(List<RealContainer>), Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<List<RealContainer>> FindRealContainersAsync(
            [FromQuery]string qrCode,
            [FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `creditOrgId`")] int creditOrgId,
            [FromQuery]int? typeId,
            [FromQuery]int? excludeTypeId,
            [FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `method`")] string method,
            [FromQuery]string sortField = "RealContainerId", [FromQuery]string sortType = "DESC", [FromQuery]int offset = 0, [FromQuery]int limit = 25)
        {
            var result = await _serviceReal.FindRealContainersAsync(qrCode, creditOrgId, typeId, excludeTypeId, method, sortField, sortType, offset, limit);
            return result;
        }

        [HttpGet("GetCountRealContainers")]
        [SwaggerResponse(200, Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<int> GetCountRealContainersAsync(
            [FromQuery]string qrCode,
            [FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `creditOrgId`")] int creditOrgId,
            [FromQuery]int? typeId,
            [FromQuery]int? excludeTypeId,
            [FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `method`")] string method)
        {
            return await _serviceReal.GetCountRealContainersAsync(qrCode, creditOrgId, typeId, excludeTypeId, method);
        }

        [HttpPost("UpdateRealContainersStatus")]
        [SwaggerResponse(200, Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<int> UpdateRealContainersStatusAsync([FromBody][Required(ErrorMessage = "Не задан обязательный параметр `RealContainerPropertiesRequest`")] RealContainerStatusRequest request)
        {
            return await _serviceReal.UpdateRealContainersStatusAsync(request.ContainersId, request.StatusId, force: request.Force);
        }

        [HttpPost("SetRealContainersProperties")]
        [SwaggerResponse(200, Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<int> SetRealContainersPropertiesAsync([FromBody][Required(ErrorMessage = "Не задан обязательный параметр `RealContainerPropertiesRequest`")] RealContainerPropertiesRequest request)
        {
            return await _serviceReal.SetRealContainersPropertiesAsync(request.ContainersId, request.WroteOff, request.NeedCheck);
        }

        [HttpGet("GetRealContainerStatuses")]
        [SwaggerResponse(200, Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<List<RealContainerStatus>> GetRealContainerStatusesAsync()
        {
            var result = await _serviceReal.GetRealContainerStatusesAsync();
            return result;
        }

        [HttpPost("UpdateRealContainer")]
        [SwaggerResponse(200, Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<int> UpdateRealContainerAsync(
            [FromBody][Required(ErrorMessage = "Не задан обязательный параметр `realContainer`")] RealContainer realContainer,
            [FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `force`")] bool force)
        {
            return await _serviceReal.UpdateRealContainerAsync(realContainer, force);
        }

        [HttpPost("InsertRealContainer")]
        [SwaggerResponse(200, Type = typeof(RealContainer), Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<RealContainer> InsertRealContainerAsync([FromBody][Required(ErrorMessage = "Не задан обязательный параметр `realContainer`")] RealContainer realContainer)
        {
            return await _serviceReal.InsertRealContainerAsync(realContainer);
        }

        [HttpPost("InsertRealContainerCopy")]
        [SwaggerResponse(200, Type = typeof(List<RealContainer>), Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<List<RealContainer>> InsertRealContainerCopyAsync([FromBody][Required(ErrorMessage = "Не задан обязательный параметр `realContainer`")] RealContainer realContainer)
        {
            return await _serviceReal.InsertRealContainerCopyAsync(realContainer);
        }

        [HttpGet("DeleteRealContainer")]
        [SwaggerResponse(200, Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<int> DeleteRealContainerAsync(
            [FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `realContainerId`")] int realContainerId,
            [FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `force`")] bool force)
        {
            int[] realContainersId = new int[] { realContainerId };
            return await _serviceReal.DeleteRealContainersAsync(realContainersId, force);
        }

        [HttpPost("DeleteRealContainers")]
        [SwaggerResponse(200, Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<int> DeleteRealContainersAsync(
            [FromBody][Required(ErrorMessage = "Не задан обязательный параметр `realContainersId`")] int[] realContainersId,
            [FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `force`")] bool force)
        {
            return await _serviceReal.DeleteRealContainersAsync(realContainersId, force);
        }

        [HttpGet("CheckRealContainerInWorth")]
        [SwaggerResponse(200, Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<bool> CheckRealContainerInWorthAsync([FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `realContainerId`")] int realContainerId, [FromQuery] int? userId)
        {
            return await _serviceReal.CheckRealContainerInWorthAsync(realContainerId, userId);
        }

        [HttpGet("GetRealContainerDetailsInWorth")]
        [SwaggerResponse(200, Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<List<RealContainerDetail>> GetRealContainerDetailsInWorthAsync(
            [FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `realContainerId`")] int realContainerId,
            [FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `userId`")] int userId)
        {
            var result = await _serviceReal.GetRealContainerDetailsInWorthAsync(realContainerId, userId);
            return result;
        }

        [HttpPost("BreakRealContainers")]
        [SwaggerResponse(200, Description = "Операция завершена")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public async Task<int> BreakRealContainersAsync([FromBody][Required(ErrorMessage = "Не задан обязательный параметр `RealContainerRequest`")] RealContainerRequest request)
        {
            return await _serviceStorage.UnbindRealContainersAsync(request.ContainersId, request.UserId);
        }
    }
}
