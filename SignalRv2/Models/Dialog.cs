using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Models
{
    public class Dialog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public User CreatedBy { get; set; }
        public User Reciever { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
