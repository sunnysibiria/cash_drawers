using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using CashManagment.Api.Constants;
using Swashbuckle.AspNetCore.Annotations;
using CashManagment.Application.V10;

namespace CashManagment.Api.Controllers.V10
{
    [Produces("application/json")]
    [Route("storage")]
    [ApiController]
    [ApiVersion(Consts.ApiVersion)]
    public class StorageController : BaseController
    {
        private IStorageTransferService _transfer;
        public StorageController(IStorageTransferService transferService)
        {
            _transfer = transferService;
        }

        [HttpGet("RealContainerTransferUnHold")]
        [SwaggerResponse(200, Description = "Операция выполнена успешно")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public int RealContainerTransferUnHold(
            [Required(ErrorMessage = "Не задан обязательный параметр `realContainerId`")] int realContainerId,
            [Required(ErrorMessage = "Не задан обязательный параметр `userId`")] int userId)
        {
            _transfer.RealContainerTransferUnHold(realContainerId, userId);
            return 1;
        }

        [HttpGet("RealContainerTransferHold")]
        [SwaggerResponse(200, Description = "Операция выполнена успешно")]
        [SwaggerResponse(400, Description = "Переданы некорректные данные")]
        public int RealContainerTransferHold(
            [Required(ErrorMessage = "Не задан обязательный параметр `cashRequestId`")] int cashRequestId,
            [Required(ErrorMessage = "Не задан обязательный параметр `realContainerId`")] int realContainerId,
            [Required(ErrorMessage = "Не задан обязательный параметр `userId`")] int userId)
        {
            _transfer.RealContainerTransferHold(cashRequestId, realContainerId, userId);
            return 1;
        }
    }
}