using System;
using CashManagment.Api.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LoggingMvcBuilderExtensions
    {
        /// <summary>
        /// Adds an action logging filter <see cref="ActionLoggingFilter"/>
        /// to the <see cref="MvcOptions" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/> to add a filter to.</param>
        /// <returns>An <see cref="IMvcBuilder"/> which need to be configured.</returns>
        public static IMvcBuilder AddActionLogging(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<MvcOptions>(opt => opt.Filters.Add<ActionLoggingFilter>());
            builder.Services.AddTransient<ActionLoggingFilter>();

            return builder;
        }

        /// <summary>
        /// Adds a model state validation and logging filter <see cref="ModelStateValidationLoggingFilter"/>
        /// to the <see cref="MvcOptions" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/> to add a filter to.</param>
        /// <returns>An <see cref="IMvcBuilder"/> which need to be configured.</returns>
        public static IMvcBuilder AddModelStateValidationLoggingFilter(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<MvcOptions>(opt => opt.Filters.Add<ModelStateValidationLoggingFilter>());
            builder.Services.AddTransient<ModelStateValidationLoggingFilter>();

            return builder;
        }

        /// <summary>
        /// Adds an action logging filter <see cref="ActionLoggingFilter"/> and a model state validation
        /// and logging filter <see cref="ModelStateValidationLoggingFilter"/> to the <see cref="MvcOptions" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/> to add a filter to.</param>
        /// <returns>An <see cref="IMvcBuilder"/> which need to be configured.</returns>
        public static IMvcBuilder AddActionAndModelStateValidationLogging(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<MvcOptions>(opt =>
            {
                opt.Filters.Add<ActionLoggingFilter>();
                opt.Filters.Add<ModelStateValidationLoggingFilter>();
            });

            builder.Services.AddTransient<ActionLoggingFilter>();
            builder.Services.AddTransient<ModelStateValidationLoggingFilter>();

            return builder;
        }

        public static IMvcBuilder AddActionAndModelStateValidationAndBadRequestLogging(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<MvcOptions>(opt =>
            {
                opt.Filters.Add<ActionLoggingFilter>();
                opt.Filters.Add<ModelStateValidationLoggingFilter>();
                opt.Filters.Add<BadRequestLoggingFilter>();
            });

            builder.Services.AddTransient<ActionLoggingFilter>();
            builder.Services.AddTransient<ModelStateValidationLoggingFilter>();
            builder.Services.AddTransient<BadRequestLoggingFilter>();

            return builder;
        }
    }
}
