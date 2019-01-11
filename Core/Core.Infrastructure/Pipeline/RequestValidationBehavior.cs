using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Infrastructure.Pipeline
{
    /// <summary>
    /// Ensures that all Mediatr Requests that have validators are processed.
    /// Throws all ValidationExceptions to the ExceptionHandlingMiddleware to process
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            // MediatR will automatically inject the correct validator for the request using the assemblies registered in the ServiceProvider DI Container.
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // Pass the request into a FluentValidation ValidationContect
            var context = new ValidationContext(request);

            // Run the associated validator against the request
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                // Throe a validation exception for the middleware to handle
                throw new ValidationException("One or more validation failures occured.", failures);
            }
            else
            {
                // If validation passed, allow the command or query to continue:
                return next();
            }

        }
    }
}
