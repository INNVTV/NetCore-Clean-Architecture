using Core.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.BaseClasses
{
    /// <summary>
    /// The command handler base class allows us to inject common routines
    /// such as logging, caching and authorization into every query handler
    /// </summary>
    public abstract class CommandHandlerBase
    {
        public CommandHandlerBase()
        {
            //Log Activity

            //Authorization
        }

    }
}
