using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Wiser.DataLogger;

namespace Wiser
{
    public static class Registrations
    {
        public static IServiceCollection AddDataLogger<T>(this IServiceCollection services)
            where T : class, IDataLogger
        {
            services.AddTransient<IDataLogger, T>();
            services.AddTransient<T>();

            return services;
        }

        public static IServiceCollection AddDataLogger<T, V>(this IServiceCollection services, Action<V> configure)
            where T : class, IDataLogger
            where V : class, IDataLoggerOptions<T>
        {
            services.AddDataLogger<T>();

            services.AddOptions<V>();
            services.Configure<V>(configure);

            return services;
        }

        public static IServiceCollection AddWiser(this IServiceCollection services, Action<WiserConnectionOptions> configure)
        {
            services.AddTransient<IWiserDataProvider, WiserConnection>();
            services.AddOptions<WiserConnectionOptions>();
            services.Configure<WiserConnectionOptions>(configure);

            return services;
        }
    }
}
