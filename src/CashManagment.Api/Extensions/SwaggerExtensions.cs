using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlfaBank.AdApi.Kerberos.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;

namespace CashManagment.Api.Extensions
{
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Выполняет подготовку и добавление Swagger в коллекцию <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        public static void PrepareAndAddSwagger(this IServiceCollection services)
        {
            services
            .AddOwnSwagger(o =>
            {
                o.IsUFR = !ServiceEnvironments.IsLocal;
                o.Description = "Сервис для дополнительного менеджмента кассет системой заявок НДО";
                o.VersionFormat = "RRRR";
                o.ServiceName = ServiceEnvironments.ServiceName;
                o.DocumentTitle = "CashManagmentApi";
                o.ShowExtensions = true;
                o.NewtonsoftJsonSupport = true;
            })
            .AddXmlDocumentations()
            .AddApiKeySecurity(o => o.HeaderName = KerberosAuthenticationDefaults.HeaderTokenNameDefault)
            .AddVersioning();
        }

        /// <summary>
        /// Выполняет подготовку и подключение Swagger.
        /// </summary>
        /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> to add the middleware to.</param>
        public static void PrepareAndUseSwagger(this IApplicationBuilder app)
        {
            var serviceName = GetServiceName();
            app.UseOwnSwaggerUI((c, o) =>
            {
                c.DefaultModelsExpandDepth(-1);

                var apiPrefix = string.Join("/", new[] { GetServiceName(), o.RoutePrefix }.Where(s => !string.IsNullOrEmpty(s)));
                app.UseRewriter(new RewriteOptions().AddRedirect("^$", $"{apiPrefix}/"));
            });
        }

        private static string GetServiceName()
        {
            return ServiceEnvironments.IsLocal ? string.Empty : ServiceEnvironments.ServiceName;
        }
    }
}
