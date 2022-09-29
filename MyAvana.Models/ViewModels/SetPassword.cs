using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.ViewModels
{
    public class SetPassword
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
