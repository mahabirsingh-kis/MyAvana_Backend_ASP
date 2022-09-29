using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.ViewModels
{
    public class AppleRequest
    {
        [Required]
        public string TransactionID { get; set; }
        [Required]
        public string TransactionDate { get; set; }
        [Required]
        public string SubscriptionId { get; set; }
    }
}
