using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.DTOs
{
    public class RegisterData
    {
        [Required]
        public String Name { get; set; }
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public IFormFile UserImage { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }

    public class RegisterUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
