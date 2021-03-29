namespace BP.Security
{
    /// <summary>Пользователь от имени которого будут выполняться действия в системе (через делегирование или смену пользователя)</summary>
    public class EntryUser
    {
        /// <summary>Уникальный ключ, используется логин Active Directory (под которым заходим в windows)</summary>
        public string Token { get; set; }

        /// <summary>Логин Active Directory пользователя от имени которого будут выполняться действия в системе</summary>
        public string WinLogin { get; set; }

        /// <summary>Необязательное поле наименования пользователя от имени которого будут выполняться действия в системе</summary>
        public string UserName { get; set; }

        /// <summary>Признак наличия прав администратора у пользователя от имени которого будут выполняться действия в системе</summary>
        public bool IsAdmin { get; set; }
    }
}
