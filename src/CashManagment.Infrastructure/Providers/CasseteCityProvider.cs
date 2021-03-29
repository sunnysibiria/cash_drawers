using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using CashManagment.Domain.InfrastructureEntities;
using CashManagment.Domain.Models;
using CashManagment.Infrastructure.Configuration;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
namespace CashManagment.Infrastructure.Providers
{
    public class CasseteCityProvider : ICasseteCityProvider
    {
        public ILogger<CasseteCityProvider> Logger { get; }
        private readonly HttpClient _client;
        private readonly ICashManagmentConfigProvider _config;

        public CasseteCityProvider(ICashManagmentConfigProvider config, ILogger<CasseteCityProvider> logger)
        {
            _config = config;
            Logger = logger;
            _client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(_config.GetRequestTimeout())
            };
            var technicalUser = _config.GetTechnicalUser();
            var technicalPassword = _config.GetTechnicalPassword();
            var authInfo = Convert.ToBase64String(Encoding.Default.GetBytes($"{technicalUser}:{technicalPassword}"));
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Basic " +authInfo);
            _client.DefaultRequestHeaders.Add("X-External-System-Code", _config.GetExternalSystemCode());
            _client.DefaultRequestHeaders.Add("X-External-User-Code", _config.GetExternalUserCode());
        }

        public async Task<List<RealContainerCity>> GetAsync()
        {
            var requestUrl = _config.GetDROUrl();
            Logger.LogDebug($"Sending request to {requestUrl}");
            var responce = await _client.GetAsync(requestUrl);
            var jsonString = await responce.Content.ReadAsStringAsync();
            if (responce.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(jsonString);
            }

            var result = JsonConvert.DeserializeObject<List<RealContainerCity>>(jsonString);
            return result;
        }
    }
}
