using Microsoft.Extensions.Configuration;
using System.IO;

namespace DesafioConexaLabs.Utils
{
    public class AppConfigUtil
    {
        private static IConfiguration _config;

        private static IConfiguration Config
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json");

                    _config = builder.Build();
                }

                return _config;
            }
        }


        public static string GetValue(string key)
        {
            var ConfigSection = Config.GetSection(key);

            return ConfigSection.Value;
        }

    }
}
