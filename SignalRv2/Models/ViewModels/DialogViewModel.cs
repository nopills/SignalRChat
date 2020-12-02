using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Models.ViewModels
{
    public class DialogViewModel
    {
        public string DialogId { get; set; }
        public string SenderId { get; set; }
        public string RecieverName { get; set; }
        public string RecieverUsername { get; set; }
        public string Status { get; set; }
        public string LastMessage { get; set; }
        public string LastActivity { get; set; }
        public long UnreadMessage { get; set; }
    }
}
