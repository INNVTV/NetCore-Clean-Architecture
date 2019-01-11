using Core.Common.Responses;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Common.Response
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            isSuccess = false;
        }

        public BaseResponse(IEnumerable<ValidationFailure> failures)
        {
            isSuccess = false;

            ValidationIssues = new List<ValidationIssue>();

            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                
                // Each PropertyName get's an array of failures associated with it:
                var PropertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                var propertyFailure = new ValidationIssue { PropertyName = propertyName, PropertyFailures = PropertyFailures.ToList() };
                ValidationIssues.Add(propertyFailure);
            }

        }

        public bool isSuccess { get; set; }
        public string Message { get; set; }
        //public Object Object { get; set; }

        public IList<ValidationIssue> ValidationIssues;
    }

    
}
