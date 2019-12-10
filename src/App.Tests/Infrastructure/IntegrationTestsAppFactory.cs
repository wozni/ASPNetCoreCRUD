using Initial;
using Initial.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Tests.Infrastructure
{
    public class IntegrationTestsAppFactory : WebApplicationFactory<Startup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);
            return host;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
                services.AddDbContext<ConferenceContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting")                        
                        .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    options.UseInternalServiceProvider(serviceProvider);
                });

                using (var scope = services.BuildServiceProvider()
                    .CreateScope())
                {
                    var ctx = scope.ServiceProvider.GetService<ConferenceContext>();
                    ctx.Database.EnsureCreated();
                }
                
            });
            base.ConfigureWebHost(builder);
        }
    }
}