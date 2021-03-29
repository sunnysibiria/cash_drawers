using System;

namespace CashManagment.Api
{
    public static class ServiceEnvironments
    {
        /// <summary>
        /// Имя сервиса
        /// </summary>
        public static string ServiceName { get; } = Environment.GetEnvironmentVariable("SERVICE_NAME");

        /// <summary>
        /// Признак запуска в локальной среде
        /// </summary>
        public static bool IsLocal { get; } = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LOCAL"));
    }
}