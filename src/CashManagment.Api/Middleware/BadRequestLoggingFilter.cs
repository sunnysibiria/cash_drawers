using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace CashManagment.Api.Middleware
{
    public class BadRequestLoggingFilter : BaseLoggingFilter, IAsyncActionFilter
    {
        private ILogger<BadRequestLoggingFilter> Logger { get; }

        public BadRequestLoggingFilter(ILogger<BadRequestLoggingFilter> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Выполняет логирование результатов типа <see cref="BadRequestObjectResult"/> в контексте HTTP запроса.
        /// </summary>
        /// <param name="context">Контест фильтра запроса.</param>
        /// <param name="next">Делегат выполнения запроса или фильтра запроса.</param>
        /// <returns>Task</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Если задан атрибут ApiExplorerSettings(IgnoreApi = true)
            if (IsIgnoreApi(context))
            {
                // Не выполняем валидацию и логирование
                await next();
                return;
            }

            var actionExecutionContext = await next();
            if (actionExecutionContext.Result is BadRequestObjectResult)
            {
                var result = actionExecutionContext.Result as BadRequestObjectResult;
                dynamic content = result.Value;
                string message = content.errorMessage;
                Logger.LogError(message);
            }
        }
    }
}
