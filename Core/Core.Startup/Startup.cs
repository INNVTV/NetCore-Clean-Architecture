using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Startup
{
    public static class Routines
    {
        /// <summary>
        /// Any consumers of Core must call the Initialize function on startup.
        /// We do this to simplify startup on services/clients.
        /// This avoids than having to copy/paste startup routines to various Main and Startup methods.
        /// They are all encapsulated here and can run with a single line of code from Startup/Main.
        /// </summary>
        public static void Initialize()
        {
            // Configure AutoMapper Mappings
            AutoMapperConfiguration.Configure();
        }
    }
}
