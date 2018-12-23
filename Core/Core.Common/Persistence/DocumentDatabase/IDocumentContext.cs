using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Persistence.DocumentDatabase
{
    public interface IDocumentContext
    {
        DocumentClient Client { get; set; }
        Settings Settings { get; set; }
    }

    public class Settings
    {
        public string Url { get; set; }
        public string Key { get; set; }
        public string ReadOnlyKey { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
}
