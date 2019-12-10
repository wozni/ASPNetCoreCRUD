using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Teldat.Reporting.Server.Infrastructure
{
    public class RequestLogging<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<RequestLogging<TRequest, TResponse>> logger;

        public RequestLogging(ILogger<RequestLogging<TRequest, TResponse>> logger)
        {
            this.logger = logger;
            
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            // ReSharper disable once AccessToModifiedClosure
            var watch = Stopwatch.StartNew();
            using (LogContext.PushProperty("requestType", typeof(TRequest).FullName))
            using (LogContext.PushProperty("requestId", Guid.NewGuid()))
            {
                var response = await next();
                logger.LogInformation("Processed {requestName} {@request} and returned {@result} in {processingTime}.",
                    request.GetType().FullName, request, response, watch.Elapsed);
                return response;
            }
        }
    }
}