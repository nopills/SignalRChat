using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Models
{
    public class ChatClient
    {
        public string  Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string AvatarUrl { get; set; } = "default";

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTimeOffset LastActivity { get; set; }

        public ICollection<Dialog> Dialogs { get; set; }
    }
}
