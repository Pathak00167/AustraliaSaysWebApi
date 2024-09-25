using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Entity
{
    public class ChatMessage
    {
        [Key]
        public Guid MessageId { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }           
        public ApplicationUser Sender { get; set; }   

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }       
        public ApplicationUser Receiver { get; set; }  

        [Required]
        public string Content { get; set; }         

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;      
    }
}
