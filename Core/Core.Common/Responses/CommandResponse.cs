using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Response
{
    public class CommandResponse
    {
        public CommandResponse()
        {
            isSuccess = false;
        }

        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public Object Object { get; set; }
    }
}
