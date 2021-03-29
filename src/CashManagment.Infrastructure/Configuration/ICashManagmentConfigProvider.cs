namespace CashManagment.Infrastructure.Configuration
{
    public interface ICashManagmentConfigProvider
    {
        /// <summary>
        /// Наименование заголовка токена авторизации.
        /// </summary>
        string AdToken { get; }

        /// <summary>
        /// Возвращает URL из конфигурации WebService:AD_API.
        /// </summary>
        /// <returns>URL сервиса AD_API.</returns>
        string GetAdApiUrl();

        string GetDROUrl();
        string GetExternalSystemCode();
        string GetExternalUserCode();
        string GetTechnicalUser();
        string GetTechnicalPassword();
        int GetRequestTimeout();
    }
}
