using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Initial.Infrastructure
{
    public class RequestValidation<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator> validators;

        public RequestValidation(IEnumerable<IValidator> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);
            var errors = new List<ValidationFailure>();
            foreach (var validator in validators.Where(v => v.CanValidateInstancesOfType(typeof(TRequest))))
            {
                var result = await validator.ValidateAsync(context, cancellationToken);
                if (!result.IsValid)
                {
                    errors.AddRange(result.Errors);
                }
            }

            if (errors.Count > 0)
            {
                throw new ValidationException($"Request {request} is not valid.", errors);
            }
            
            return await next();
        }
    }
}