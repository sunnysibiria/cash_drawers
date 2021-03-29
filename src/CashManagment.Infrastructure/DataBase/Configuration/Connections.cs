using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CashManagment.Infrastructure.DataBase.Configuration
{
    /// <summary>
    /// Соединения с базами
    /// Пул соединений создается из коробки
    /// см. https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-connection-pooling
    /// </summary>
    public static class Connections
    {
        private const string LmConnectionName = "LM";
        private const string LogConnectionName = "Log";

        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Подключение к базе Loan Manager'а
        /// </summary>
        /// <returns>Значение</returns>
        public static SqlConnection GetLM()
        {
            return new SqlConnection(GetConnectionString(LmConnectionName));
        }

        /// <summary>
        /// Подключение к базе где хранится лог
        /// </summary>
        /// <returns>Значение</returns>
        public static SqlConnection GetLog()
        {
            return new SqlConnection(GetConnectionString(LogConnectionName));
        }

        private static string GetConnectionString(string connectionName)
        {
            if (Configuration.GetConnectionString(connectionName) == null)
            {
                throw new Exception("Ошибка при загрузке пользователя: ConnectionString is empty");
            }

            return Configuration.GetConnectionString(connectionName);
        }
    }
}
