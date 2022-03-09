using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace RMotownFestival.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                
                if (!context.HostingEnvironment.IsDevelopment())
                {
                    IConfiguration configuration = config.Build();

                    var tokenProvider = new AzureServiceTokenProvider();
                    var client = new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));

                    config.AddAzureKeyVault($"https://{configuration["AzureKeyVaultName"]}.vault.azure.net/", client, new DefaultKeyVaultSecretManager());
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            webBuilder.ConfigureAppConfiguration(config =>
            {
                var settings = config.Build();
                var connection = settings.GetConnectionString("AppConfig");
                config.AddAzureAppConfiguration(connection);
                config.AddAzureAppConfiguration(options =>
                options.Connect(connection).UseFeatureFlags());
            }).UseStartup<Startup>())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}