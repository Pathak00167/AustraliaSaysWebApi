using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Models
{
    public class UserInfo
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DateofBirth { get; set; }
        public string? Bio { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
