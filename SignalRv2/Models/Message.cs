using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Models
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Content { get; set; }
        public DateTimeOffset When { get; set; }

        public string ChatClientId { get; set; }
        public ChatClient ChatClient { get; set; }

        public long DialogId { get; set; }
        public Dialog Dialog { get; set; }
    }
}
