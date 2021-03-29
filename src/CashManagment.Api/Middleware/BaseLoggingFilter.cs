using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashManagment.Api.Middleware
{
    /// <summary>
    /// Базовый класс логирования HTTP запросов.
    /// </summary>
    public abstract class BaseLoggingFilter
    {
        protected bool IsIgnoreApi(ActionExecutingContext context)
        {
            var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            // Проверим наличие ApiExplorerSettings(IgnoreApi = true) у выполняемого действия
            var actionExplorerSettings = descriptor.MethodInfo.GetCustomAttributes(typeof(ApiExplorerSettingsAttribute), false);
            if (actionExplorerSettings.Length > 0 && ((ApiExplorerSettingsAttribute)actionExplorerSettings[0]).IgnoreApi)
            {
                return true;
            }

            // Проверим наличие ApiExplorerSettings(IgnoreApi = true) у контроллера выполняемого действия
            var controllerExplorerSettings = descriptor.ControllerTypeInfo.GetCustomAttributes(typeof(ApiExplorerSettingsAttribute), false);
            if (controllerExplorerSettings.Length > 0 && ((ApiExplorerSettingsAttribute)controllerExplorerSettings[0]).IgnoreApi)
            {
                return true;
            }

            return false;
        }
    }
}
