using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace CashManagment.Api.Middleware
{
    /// <summary>
    /// Класс логирования HTTP запросов.
    /// </summary>
    public class ActionLoggingFilter : BaseLoggingFilter, IAsyncActionFilter
    {
        private ILogger<ActionLoggingFilter> Logger { get; }

        public ActionLoggingFilter(ILogger<ActionLoggingFilter> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Выполняет логирование выполнения запроса.
        /// </summary>
        /// <param name="context">Контест фильтра запроса.</param>
        /// <param name="next">Делегат выполнения запроса или фильтра запроса.</param>
        /// <returns>Task</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Если задан атрибут ApiExplorerSettings(IgnoreApi = true)
            if (IsIgnoreApi(context))
            {
                // Не выполняем логирование
                await next();
                return;
            }

            var debugMessage = string.Empty;

            try
            {
                debugMessage = GetContextDebugMessage(context);
                Logger.LogDebug($"Start: {debugMessage}");

                var infoMessage = GetContextInfoMessage(context);
                Logger.LogInformation(infoMessage);

                var resultContext = await next();
                if (resultContext.Exception != null)
                {
                    // Логирование ошибок выполнения запроса или фильтра запроса.
                    var e = resultContext.Exception;
                    Logger.LogError(e, e.Message);
                }
            }
            catch (Exception e)
            {
                // Логирование локальных ошибок
                Logger.LogError(e, e.Message);
                throw;
            }
            finally
            {
                Logger.LogDebug($"End: {debugMessage}");
            }
        }

        private string GetContextDebugMessage(ActionExecutingContext context)
        {
            var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var message = $"{descriptor.ControllerName}Controller.{descriptor.ActionName}";
            return message;
        }

        private string GetContextInfoMessage(ActionExecutingContext context)
        {
            // Получим текстовое представление параметров со значениями
            var args = GetContextParameters(context.ActionArguments);
            var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            // Проверим наличие атрибута SwaggerOperationAttribute с описанием у выполняемого действия
            var swaggerOperations = descriptor.MethodInfo.GetCustomAttributes(typeof(SwaggerOperationAttribute), false);
            var description = swaggerOperations.Length > 0
                ? ((SwaggerOperationAttribute)swaggerOperations[0]).Description
                : string.Empty;

            if (!string.IsNullOrEmpty(description))
            {
                description = $" - {description.Trim().TrimEnd('.')}";
            }

            var message = $"Вызван метод {descriptor.ControllerName}Controller.{descriptor.ActionName}{description}: {args}";
            return message;
        }

        private string GetContextParameters(IDictionary<string, object> parameters)
        {
            if (parameters.Count == 0)
            {
                return "(без параметров)";
            }

            var list = new List<string>();
            foreach (var parameter in parameters)
            {
                var obj = parameter.Value == null ? "null" : JsonConvert.SerializeObject(parameter.Value, Formatting.None);
                list.Add($"{parameter.Key}={obj}");
            }

            return string.Join(", ", list);
        }
    }
}
