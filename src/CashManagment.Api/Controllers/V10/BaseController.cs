using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CashManagment.Api.Controllers.V10
{
    /// <summary>
    /// Базовый webapi контроллер
    /// </summary>
    [Authorize(Policy = "AuthenticatedUsers")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class BaseController : ControllerBase
    {
    }
}