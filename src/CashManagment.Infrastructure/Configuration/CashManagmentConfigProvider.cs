using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CashManagment.Infrastructure.Configuration
{
    public class CashManagmentConfigProvider : ConfigProvider, ICashManagmentConfigProvider
    {
        /// <summary>
        /// Наименование заголовка токена авторизации.
        /// </summary>
        public string AdToken => "ad-token";

        /// <summary>
        /// Возвращает URL из конфигурации WebService:AD_API.
        /// </summary>
        /// <returns>URL сервиса AD_API.</returns>
        public string GetAdApiUrl() => GetConfigValue("WebService", "AD_API");

        public int GetRequestTimeout() => Convert.ToInt32(GetConfigValue("WebService", "RequestTimeout"));

        /// <summary>
        /// Возращает URL городов ДРО
        /// </summary>
        /// <returns>URL сервиса ДРО</returns>
        public string GetDROUrl() => GetConfigValue("WebService", "DRO_URL");

        /// <summary>
        /// Возвращает зарегистрированный код системы
        /// </summary>
        /// <returns></returns>
        public string GetExternalSystemCode() => GetConfigValue("WebService", "ExternalSystemCode");

        /// <summary>
        /// Возвращает зарегистрированный код пользователя системы
        /// </summary>
        /// <returns></returns>
        public string GetExternalUserCode() => GetConfigValue("WebService", "ExternalUserCode");

        public string GetTechnicalUser() => GetConfigValue("WebService", "TechnicalUser");

        public string GetTechnicalPassword() => GetConfigValue("WebService", "TechnicalPassword");

        public CashManagmentConfigProvider(IConfiguration config)
            : base(config)
        {
        }
    }
}
