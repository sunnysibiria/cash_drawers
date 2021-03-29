using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BP.Security
{
    /// <summary>Точка авторизаци и делегирования</summary>
    public class Authorization : IAuthorization
    {
        private const string ConnectionStringName = "EntryConnectionString";
        private static string connectionString;

        public IConfiguration Configuration { get; }

        public Authorization(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>Запись в базу делегирования информации о смене пользователя</summary>
        /// <param name="user">Пользователь от имени которого будут выполняться действия в системе</param>
        public void ChangeUser(EntryUser user)
        {
            var connectionString = GetConnectionString();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return;
            }

            using (var conn = new SqlConnection(connectionString))
            {
                var sql = @"
if not exists (
    select top 1 1
    from [dbo].[entry]
    where token = @token
)
begin
    insert into [dbo].[entry](token, login, name, admin)
    values(@token, @login, @name, @admin)
end
else
begin    
    update [dbo].[entry]
    set login = @login, name = @name, admin = @admin
    where token = @token
end
";
                conn.Execute(sql, new
                {
                    token = user.Token,
                    login = user.WinLogin,
                    name = user.UserName,
                    admin = user.IsAdmin,
                });
            }
        }

        /// <summary>
        /// Получение информации о пользователе, от имени которого будут выполняться действия в системе
        /// </summary>
        /// <param name="token">Ключ, обычно это Windows-логин авторизованного пользователя</param>
        /// <returns>Пользователь от имени которого будут выполняться действия в системе</returns>
        public EntryUser GetUser(string token)
        {
            var connectionString = GetConnectionString();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            using (var conn = new SqlConnection(connectionString))
            {
                var sql = @"
select token as Token
      ,login as WinLogin
      ,name as UserName
      ,admin as IsAdmin
from [dbo].[entry]
where token = @token
";
                return conn.QuerySingleOrDefault<EntryUser>(sql, new
                {
                    token
                });
            }
        }

        /// <summary>Строка подкючения к mongo к базе entry</summary>
        /// <returns>Значение</returns>
        private string GetConnectionString()
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                return connectionString;
            }

            if (Configuration.GetConnectionString(ConnectionStringName) != null)
            {
                connectionString = Configuration.GetConnectionString(ConnectionStringName);
            }

            return connectionString;
        }
    }
}
