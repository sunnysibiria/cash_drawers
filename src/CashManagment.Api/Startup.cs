using AlfaBank.AdApi.Kerberos.Authentication;
using AlfaBank.Barcode.PackageExport;
using AlfaBank.Logging.Extensions;
using AlfaBank.Services.Healthcheck;
using BP.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using CashManagment.Api.Constants;
using CashManagment.Api.Extensions;
using CashManagment.Api.Middleware;
using CashManagment.Application.V10;
using CashManagment.Infrastructure;
using CashManagment.Domain.InfrastructureEntities;
using CashManagment.Infrastructure.Specifications;
using CashManagment.Domain.Specifications;
using CashManagment.Infrastructure.DataBase.Configuration;
using CashManagment.Infrastructure.DataBase.Repositories;
using CashManagment.Infrastructure.DataBase.Proxies;
using CashManagment.Infrastructure.Providers;
using CashManagment.Infrastructure.Configuration;
using AutoMapper;

namespace CashManagment.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
            ConfigProvider = new CashManagmentConfigProvider(Configuration);
            Connections.Configuration = Configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        public ICashManagmentConfigProvider ConfigProvider { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(KerberosAuthenticationDefaults.AuthenticationScheme).AddKerberos(ConfigProvider.GetAdApiUrl());

            services.AddAuthorization(options =>
            {
                // Политика проверки пользователя при выполнении запросов
                options.AddPolicy("AuthenticatedUsers", policy => policy.RequireAuthenticatedUser());
            });

            if (!Env.IsProduction())
            {
                // Добавляем Swagger
                services.PrepareAndAddSwagger();
            }

            services.AddControllers(opt => opt.SuppressAsyncSuffixInActionNames = false)
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddActionAndModelStateValidationAndBadRequestLogging()
                .ConfigureApiBehaviorOptions(opt =>
                {
                    opt.SuppressConsumesConstraintForFormFileParameters = true;
                    opt.SuppressInferBindingSourcesForParameters = true;
                    opt.SuppressMapClientErrors = true;
                    opt.SuppressModelStateInvalidFilter = true;
                })
                .AddNewtonsoftJson(opt =>
                {
                    var contractResolver = (DefaultContractResolver)opt.SerializerSettings.ContractResolver;
#if DEBUG
                    // В отладочном режиме сериализуем все свойства, независимо от работы ShouldSerialize
                    contractResolver.IgnoreShouldSerializeMembers = true;

                    // В отладочном режиме форматируем json для наглядности
                    opt.SerializerSettings.Formatting = Formatting.Indented;
#endif

                    contractResolver.NamingStrategy = new CamelCaseNamingStrategy();
                    opt.SerializerSettings.DateParseHandling = DateParseHandling.None;
                    opt.SerializerSettings.Converters.Add(new StringEnumConverter());

                    // Set default MVC JSON.NET settings
                    JsonConvert.DefaultSettings = () => opt.SerializerSettings;
                });

            services.AddApiVersioning(o =>
                {
                    o.DefaultApiVersion = new ApiVersion(int.Parse(Consts.ApiVersion), 0);
                    o.AssumeDefaultVersionWhenUnspecified = true;
                })
            .AddVersionedApiExplorer(o => o.SubstituteApiVersionInUrl = true);

            services.AddScoped<IAuthorization, Authorization>();
            services.AddSingleton<ICashManagmentConfigProvider, CashManagmentConfigProvider>();
            services.AddTransient<ISpecificationCreator, SpecificationCreator>();
            services.AddTransient<IRealContainerService, RealContainerService>();
            services.AddTransient<IRealContainerRepository, RealContainerRepository>();
            services.AddTransient<ICasseteCityProvider, CasseteCityProvider>();
            services.AddTransient<IStorageTransferService, StorageTransferService>();
            services.AddTransient<ICassetteService, CassetteService>();
            services.AddTransient<IStorageTransferRepository, StorageTransferRepository>();
            services.AddTransient<IStrorageTransferProxy, StrorageTransferProxy>();
            services.AddTransient<ICasseteQrCodesService, CasseteQrCodesService>();
            services.AddTransient<ICasseteQrCodesRepository, CasseteQrCodesRepository>();
            services.AddAutoMapper(typeof(Startup));
            services.AddBarcodeService(o => Configuration.GetSection("UWS").Bind(o));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            // Подключаем глобальный обработчик необработанных исключений
            app.UseExceptionHandler(appBuilder => appBuilder.UseMiddleware<ErrorHandlerMiddleware>());

            app.UseHealth("/health");
            app.UseAuthentication();

            if (!Env.IsProduction())
            {
                // Подключаем Swagger
                app.PrepareAndUseSwagger();
            }

            app.UseLogDefaultState();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
