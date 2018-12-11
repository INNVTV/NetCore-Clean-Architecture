using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Settings
{
    public class CoreSettings : ICoreSettings
    {
        public ApplicationSettings Application { get; set; }
        public AzureSettings Azure { get; set; }
    }
}
