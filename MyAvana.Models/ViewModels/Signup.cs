using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.ViewModels
{
    public class Signup
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
		public int CountryCode { get; set; }
        public bool? CustomerType { get; set; }
    }

    public class fileData
    {
        public string access_token { get; set; }
        public string ImageURL { get; set; }
        public string user_name { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string AccountNo { get; set; }
        public bool TwoFactor { get; set; }
        public string HairType { get; set; }

    }
}
