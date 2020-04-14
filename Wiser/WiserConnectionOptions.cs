using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Wiser
{
    public class WiserConnectionOptions
    {
        public const string ConfigurationSectionName = @"Wiser";
        public const string HubSecretConfigurationKey = @"HubSecret";
        public const string HubIPAddressConfigurationKey = @"HubIPAddress";

        public WiserConnectionOptions(IConfiguration config)
        {
            var wiserSection = config.GetSection(ConfigurationSectionName);
            if (wiserSection != null)
            {
                HubSecret = wiserSection[HubSecretConfigurationKey];
                HubIPAddress = wiserSection[HubIPAddressConfigurationKey];
                return;
            }

            HubSecret = config[HubSecretConfigurationKey];
            HubIPAddress = config[HubIPAddressConfigurationKey];
        }

        public WiserConnectionOptions()
        {
        }

        public string HubSecret { get; set; }
        public string HubIPAddress { get; set; }


        public static IOptions<WiserConnectionOptions> Default { get
            {
                var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

                if (config.HasFile)
                {
                    var wiserOptions = new WiserConnectionOptions()
                    {
                        HubIPAddress = config.AppSettings.Settings["HubAddress"].Value,
                        HubSecret = config.AppSettings.Settings["HubSecret"].Value
                    };

                    return new OptionsWrapper<WiserConnectionOptions>(wiserOptions);
                }

                throw new ConfigurationErrorsException("Configuration file 'Wiser.dll.config' was not found",
                    new FileNotFoundException("Configuration file 'Wiser.dll.config' was not found",
                    $"{Path.Combine(Assembly.GetExecutingAssembly().Location, "Wiser.dll.config")}"));
            }
        }
    }
}
