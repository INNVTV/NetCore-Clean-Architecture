using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.BaseClasses
{
    public abstract class ResponseBase
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
    }
}
