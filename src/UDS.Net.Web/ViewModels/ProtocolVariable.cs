using System;
using System.Collections.Generic;

namespace UDS.Net.Web.ViewModels
{
    public class ProtocolVariable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Codes { get; set; }
    }
}
