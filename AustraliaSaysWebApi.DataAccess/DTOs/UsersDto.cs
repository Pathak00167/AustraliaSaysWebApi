using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.DTOs
{
    public class UsersDto
    {
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
