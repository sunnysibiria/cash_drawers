using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace CashManagment.Api.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly IWebHostEnvironment _env;

        public ErrorHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            // Мы попали сюда, потому что произошло необработанное исключение.
            // Установим код веб ошибки 500.
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            // Попробуем получить экземпляр исключения, приведшего к ошибке
            var e = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
            if (e != null)
            {
                dynamic error = new ExpandoObject();

                // Возвратим сообщение об ошибке
                error.errorMessage = e.Message;

                if (_env.IsDevelopment() || _env.IsEnvironment("Debug"))
                {
                    // Если мы в режиме разработчика, то вернём ещё и информацию со стека
                    error.errorDetail = e.StackTrace;
                }

                string result = JsonConvert.SerializeObject(error);
                await context.Response.WriteAsync(result);
            }
        }
    }
}
