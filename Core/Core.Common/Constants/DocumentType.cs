using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Constants
{
    /// <summary>
    /// This is what I call a "Dynamic Constant".
    /// There are a few set values that can be used for our paritioning strategy in CosmosDB
    /// Some of the types require the appending of an id to group documents that belong together as a subset
    /// This static class allows us to control the use of these sets of document types
    /// </summary>
    public static class DocumentType
    {
        // Platform Documents --------------

        public static string Platform()
        {
            return "Platform";
        }

        // Account Documents --------------

        public static string Account()
        {
            return "Account";
        }

        public static string Account(string id)
        {
            return String.Concat("Account-", id);
        }
    }
}
