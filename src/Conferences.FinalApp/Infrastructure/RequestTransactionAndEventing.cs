using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Initial.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Initial.Infrastructure
{
    public class RequestTransactionAndEventing<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ConferenceContext context;
        private readonly IMediator mediator;
        private readonly ILogger<RequestTransactionAndEventing<TRequest, TResponse>> logger;

        public RequestTransactionAndEventing(ConferenceContext context, IMediator mediator,
            ILogger<RequestTransactionAndEventing<TRequest, TResponse>> logger)
        {
            this.context = context;
            this.mediator = mediator;
            this.logger = logger;
        }
        
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var response = await next();
                    var events = context.ChangeTracker.Entries()
                        .Select(e => e.Entity)
                        .OfType<Entity>()
                        .SelectMany(e => e.Events)
                        .ToList();
                    
                    await context.SaveChangesAsync(cancellationToken);
                    transaction.Commit();
                    try
                    {
                        foreach (var @event in events)
                        {
                            await mediator.Publish(new DomainEventNotification(@event), cancellationToken);
                            logger.LogInformation("Published notification: {@event}", @event);
                        }                            
                    }
                    catch (Exception exception)
                    {                          
                        logger.LogError("Error during publishing notifications. {error}", 
                            exception);
                    }
                    return response;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }                
            }
        }
    }
}