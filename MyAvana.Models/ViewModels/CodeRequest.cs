using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.ViewModels
{
    public class CodeRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public long MobileNo { get; set; }
        [Required]
        public string Code { get; set; }
    }

    public class HairTypeUserEntity
    {
        public string HairType { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; }
    }
    public class GroupModel
    {
        public string UserEmail { get; set; }
    }
}
