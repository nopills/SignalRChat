using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Abstract
{
    public class AppSettings
    {
        public int MaxMessageLength { get; set; }
        public AppSettings()
        {
            MaxMessageLength = 4096;
        }
    }
}
