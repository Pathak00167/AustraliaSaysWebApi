using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Entity
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } // e.g., "new-user", "comment", "message", "like"
        public DateTime Time { get; set; }
        public string UserId { get; set; } 
        public ApplicationUser User { get; set; }
        public bool Seen { get; set; } 
    }
}
