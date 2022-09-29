using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.Entities
{
    public class UserEmails
    {
        [Key]
        public Guid UserEmailId { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }
}
