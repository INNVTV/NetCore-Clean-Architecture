using FluentValidation;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Pipeline
{
    /*
    public class ExceptionHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {

        private readonly IRequestHandler<TRequest, TResponse> inner;

        public ExceptionHandler(IRequestHandler<TRequest, TResponse> inner)
        {
            this.inner = inner;
        }

        public TResponse Handle(TRequest request)
        {
            var response = default(TResponse);

            try
            {
                response = this.inner.Handle(request);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Error("");
                //this.logger.LogError(request, response, ex);
            }

            return response;
        }
    }
    */
}
