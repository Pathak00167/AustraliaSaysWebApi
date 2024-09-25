using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Entity
{
    public class Post
    {
        [Key]
        public Guid PostId { get; set; }

        [Required]
        public string Content { get; set; }           

        public string ImageUrl { get; set; }         

        public int LikesCount { get; set; } = 0;    

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("User")]
        public string UserId { get; set; }            
        public ApplicationUser User { get; set; }   
    }
}
