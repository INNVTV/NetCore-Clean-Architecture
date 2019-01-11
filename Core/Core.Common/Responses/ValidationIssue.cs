using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Responses
{
    public class ValidationIssue
    {
        public string PropertyName { get; set; }
        public List<string> PropertyFailures { get; set; }
    }
}
