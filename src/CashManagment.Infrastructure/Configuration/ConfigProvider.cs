using System;
using Microsoft.Extensions.Configuration;

namespace CashManagment.Infrastructure.Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        private IConfiguration _config;

        public ConfigProvider(IConfiguration config) => _config = config;

        public string GetConfigValue(string section, string key, string description = null)
        {
            string result = _config.GetSection(section)[key];

            if (result == null)
            {
                string messagePart = $"В конфигурационном файле отсутствует настройка {section}.{key}";
                string descriptionPart = string.IsNullOrEmpty(description) ? string.Empty : $" ({description})";

                throw new ApplicationException($"{messagePart}{descriptionPart}");
            }

            return result;
        }
    }
}
