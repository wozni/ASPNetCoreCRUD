using System;
using System.Net.Http;
using System.Threading.Tasks;
using App.Tests.Infractructure;
using Initial.Infrastructure;
using Initial.Models;
using MediatR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace App.Tests.Infrastructure
{
    public class IntegrationTests : IClassFixture<IntegrationTestsAppFactory>
    {
        protected TestServer Server { get; }
        protected HttpClient Client { get; }
        
        protected DomainEventsCollector PublishedEvents { get; } = new DomainEventsCollector();

        protected IntegrationTests(ITestOutputHelper testOutputHelper,
            IntegrationTestsAppFactory factory)
        {
            var configuredFactory = factory.WithWebHostBuilder(host => host
                .ConfigureTestServices(services =>
                {
                    services.AddLogging(options => options
                        .ClearProviders()
                        .AddSerilog(new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.TestOutput(testOutputHelper)
                            .CreateLogger()));
                    services.AddSingleton<INotificationHandler<DomainEventNotification>>(PublishedEvents);
                }));
            Client = configuredFactory.CreateClient();
            Server = configuredFactory.Server;
        }
        
        protected async Task UpdateContext(Action<ConferenceContext> setup)
        {
            using (var scope = Server.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ConferenceContext>();
                setup(context);
                await context.SaveChangesAsync();
            }
        }
        
        protected async Task UpdateContext(Action<IServiceProvider, ConferenceContext> setup)
        {
            using (var scope = Server.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ConferenceContext>();
                setup(scope.ServiceProvider, context);
                await context.SaveChangesAsync();
            }
        }

        protected async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)            
        {
            using (var scope = Server.Services.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetService<IMediator>();
                return await mediator.Send(request);
            }
        } 
        
    }
}