using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CashManagment.Api.Extensions
{
    public static class HttpResponseMessageExtension
    {
        /// <summary>
        /// Обработка результата webapi-запроса.
        /// </summary>
        /// <param name="response">Экземпляр объекта <see cref="HttpResponseMessage"/></param>
        /// <returns>Строка ответа</returns>
        public static async Task<string> HandleWebApiResponseAsync(this HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ServiceError error = null;

                try
                {
                    // Преобразуем строковый ответ в ошибку сервиса
                    error = JsonConvert.DeserializeObject<ServiceError>(responseContent);
                }
                catch
                {
                    // Не смогли десериализовать ответ, поэтому вызовем дефолтный "ругатель"
                    response.EnsureSuccessStatusCode();
                }

                throw new HttpRequestException(error?.ErrorMessage ?? $"{(int)response.StatusCode}: {response.ReasonPhrase}");
            }

            return responseContent;
        }
    }
}
