using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Entity
{
    public class ApplicationUser:IdentityUser
    {
        public string? Name { get; set; }
        public string? ProfilePicUrl { get; set; }
        public ICollection<Notification> Notification { get; set; }
    }
}
