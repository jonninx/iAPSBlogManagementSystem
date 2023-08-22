using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public Guid BlogId { get; set; }  // The related blog post
        public BlogPost Blog { get; set; }
        public string UserId { get; set; }  // The recipient of the notification
        public ApplicationUser User { get; set; }
        public string Content { get; set; }  // Notification message
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
