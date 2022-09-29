using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class UsersTicketsEntity
    {
        [Key]
        public long TicketId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string CreatedAt { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
}
