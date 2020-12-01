using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Models
{
    public class Message
    {
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]    
        public string Content { get; set; }
        [Required]
        public DateTimeOffset When { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public string DialogId { get; set; }
        [Required]
        public Dialog Dialog { get; set; }
        [Required]
        public bool IsRead { get; set; }
    }
}
