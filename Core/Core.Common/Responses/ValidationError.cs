using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Responses
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public List<string> PropertyErrors { get; set; }
    }
}
