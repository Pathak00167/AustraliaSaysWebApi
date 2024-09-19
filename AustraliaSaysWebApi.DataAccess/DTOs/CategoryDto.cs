using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.DTOs
{
    public class CategoryDto
    {
        
        public int? Id { get; set; }
        [Required]
        public string? CategoryName { get; set; }
    }
}
