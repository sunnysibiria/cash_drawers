using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace CashManagment.Api.Middleware
{
    /// <summary>
    /// Класс валидации и логирования ошибки <see cref="ModelStateDictionary"/> в контексте HTTP запроса.
    /// </summary>
    public class ModelStateValidationLoggingFilter : BaseLoggingFilter, IAsyncActionFilter
    {
        private ILogger<ModelStateValidationLoggingFilter> Logger { get; }

        public ModelStateValidationLoggingFilter(ILogger<ModelStateValidationLoggingFilter> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Выполняет валидацию и логирование ошибки <see cref="ModelStateDictionary"/> в контексте HTTP запроса
        /// и возращает <see cref="BadRequestObjectResult"/> в ответе запроса, если валидация не была пройдена.
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

            if (!context.ModelState.IsValid)
            {
                // Если состояние модели не валидно, вернём BadRequest, запишем в лог сообщение об ошибке и выйдем
                var messages = string.Join(Environment.NewLine, context.ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage));
                context.Result = new BadRequestObjectResult(new { errorMessage = messages });
                Logger.LogError(new ArgumentException(messages), messages);
                return;
            }

            await next();
        }
    }
}
