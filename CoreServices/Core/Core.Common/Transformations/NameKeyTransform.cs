using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Common.Transformations
{
    public static class NameKey
    {
        public static string Transform(string name)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            var transformedName = rgx.Replace(name, "");

            return transformedName.ToLower();
        }
    }
}
