namespace CashManagment.Infrastructure.Configuration
{
    public interface IConfigProvider
    {
        string GetConfigValue(string section, string value, string description = null);
    }
}
