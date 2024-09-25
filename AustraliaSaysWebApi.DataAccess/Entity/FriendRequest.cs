using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Entity
{
    public class FriendRequest
    {
        [Key]
        public Guid RequestId { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }         
        public ApplicationUser Sender { get; set; }   

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }       
        public ApplicationUser Receiver { get; set; } 

        [Required]
        public string Status { get; set; } = "Pending"; 

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }    // Date of acceptance/rejection
    }
}
