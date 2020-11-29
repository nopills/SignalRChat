using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Models
{
    public class Dialog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public string RecieverId { get; set; }
        public User Reciever { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset LastActivity { get; set; }
        public string LastMessage { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
