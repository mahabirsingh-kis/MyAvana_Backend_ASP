using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.Entities
{
    public class UserSubscriptionValidity
    {
        [Key]
        public string EmailAddress { get; set; }
        public DateTime ValidTill { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
