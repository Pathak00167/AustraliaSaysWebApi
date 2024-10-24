using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.DTOs
{
    public class SendFriendRequest
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
    }
}
