using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.DTOs
{
    public class UpdateUserProfile
    {
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public IFormFile? UserProfilePicture { get; set; }
        public DateTime? Dob { get; set; }

    }
}
