using Microsoft.Extensions.Configuration;

namespace BP.Security
{
    /// <summary>Точка авторизаци и делегирования</summary>
    public interface IAuthorization
    {
        IConfiguration Configuration { get; }

        /// <summary>Запись в базу делегирования информации о смене пользователя</summary>
        /// <param name="user">Пользователь от имени которого будут выполняться действия в системе</param>
        void ChangeUser(EntryUser user);

        /// <summary>
        /// Получение информации о пользователе, от имени которого будут выполняться действия в системе
        /// </summary>
        /// <param name="token">Ключ, обычно это Windows-логин авторизованного пользователя</param>
        /// <returns>Пользователь от имени которого будут выполняться действия в системе</returns>
        EntryUser GetUser(string token);
    }
}
