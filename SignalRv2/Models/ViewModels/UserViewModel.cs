using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Models.ViewModels
{
    public class UserViewModel
    {    
        public string AvatarUrl { get; set; } = "default";
        public UserStatus Status { get; set; }     
        public DateTimeOffset LastActivity { get; set; } = DateTimeOffset.UtcNow;
        public bool IsBanned { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
    }
}
