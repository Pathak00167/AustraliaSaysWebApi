using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.DTOs
{
    public class ReturnMessage
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
    }
}
