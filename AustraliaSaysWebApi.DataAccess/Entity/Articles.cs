using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Entity
{
    public class Articles
    {
        [Key]
        public int ArticleId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        public string? ArticleTitle { get; set; }
        [Required]
        public string? ArticleDescription { get; set; }
        [Required]
        public string? ArticleImage { get; set; }
    }
}
