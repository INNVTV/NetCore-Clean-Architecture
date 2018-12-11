using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Configuration
{
    /// <summary>
    /// We use the ICoreConfiguration type as a way to inject settings
    /// and resource connections into our classes from our main entry points.
    /// </summary>
    public class CoreConfiguration : ICoreConfiguration
    {
        public ApplicationConfiguration Application { get; set; }
        public AzureConfiguration Azure { get; set; }
    }
}
