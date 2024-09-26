using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.DTOs
{
    public class Verify_Otp
    {
        public string Email { get; set; }
        [StringLength(6)]
        public string Otp { get; set; }
    }
}
