using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Models.ViewModels
{
    public class DialogViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string RecieverName { get; set; }
        public string LastMessage { get; set; }
        public DateTimeOffset LastActivity { get; set; }
        public long UnreadMessage { get; set; }
    }
}
