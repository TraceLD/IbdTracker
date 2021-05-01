using System.IO;
using IbdTracker.Core.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Tests
{
    public sealed class SharedFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder.ConfigureAppConfiguration(
                    (_, conf) =>
                    {
                        conf.SetBasePath(Path.Combine(Directory.GetCurrentDirectory()));
                        conf.AddJsonFile("appsettings.json", false, true);
                    }
                )
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                });
        
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = builder.Build();
            var configuration = host.Services.GetRequiredService<IConfiguration>();
            var connString =  configuration.GetSection("Database").Get<DbConfig>().ConnectionString;

            host.StartAsync().GetAwaiter().GetResult();
            return host;
        }
    }
}