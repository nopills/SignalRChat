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
        public string UserId { get; set; }
        public User User { get; set; }
        public string DialogId { get; set; }
        public Dialog Dialog { get; set; }
    }
}
