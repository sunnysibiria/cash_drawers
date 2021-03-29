using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using CashManagment.Api.Constants;
using Swashbuckle.AspNetCore.Annotations;
using CashManagment.Application.V10;
using CashManagment.Domain.Models;

namespace CashManagment.Api.Controllers.V10
{
    [Produces("application/json")]
    [Route("CasseteQrCodes")]
    [ApiController]
    [ApiVersion(Consts.ApiVersion)]
    public class CasseteQrCodesController : BaseController
    {
        private ICasseteQrCodesService _serviceQR;

        public CasseteQrCodesController(ICasseteQrCodesService serviceQR)
        {
            _serviceQR = serviceQR;
        }

        /// <summary>
        /// Получение настроек для печати из БД по кредитной организации
        /// </summary>
        /// <param name="creditOrgId">ИД кредитной организации</param>
        /// <param name="userId">ИД пользователя</param>
        /// <returns>настройки печати</returns>
        [HttpGet("GetUserPrintProperties")]
        [SwaggerResponse(200, Description = "Операция выполнена успешно")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public ReportRequestPrintParameters GetUserPrintProperties(
            [FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `creditOrgId`")] int creditOrgId,
            [FromQuery][Required(ErrorMessage = "Не задан обязательный параметр `userId`")] int userId)
        {
            return _serviceQR.GetUserPrintProperties(creditOrgId, userId);
        }

        /// <summary>
        /// Сохранение настроек печати в БД по кредитной организации
        /// </summary>
        /// <param name="parameters">настройки печати</param>
        /// <returns>результат выполнения</returns>
        [HttpPost("SetUserPrintProperties")]
        [SwaggerResponse(200, Description = "Операция выполнена успешно")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public int SetUserPrintProperties(
            [FromBody][Required(ErrorMessage = "Не задан обязательный параметр `ReportRequestPrintParameters`")] ReportRequestPrintParameters parameters)
        {
            return _serviceQR.SetUserPrintProperties(parameters);
        }
    }
}
