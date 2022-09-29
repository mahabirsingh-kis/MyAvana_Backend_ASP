using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.Entities
{
    public class UsersCode
    {
        [Key]
        public Guid Id { get; set; }        
        public string Code { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }
}
