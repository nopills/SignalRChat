using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Models
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; } = "default";

        public UserStatus Status { get; set; }
        public DateTimeOffset RegisteredDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LastActivity { get; set; } = DateTimeOffset.UtcNow;
        public bool IsBanned { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
    }
}
