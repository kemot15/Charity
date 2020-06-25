using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Tools
{
    public class AppVariableConfiguration
    {
        public static IConfigurationRoot ConfigurationRoot()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables("FOODAPP_");
            var configuration = configurationBuilder.Build();
            return configuration;
        }
    }
}
