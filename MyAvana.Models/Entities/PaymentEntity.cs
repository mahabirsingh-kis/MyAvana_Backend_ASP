using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.Entities
{
    public class PaymentEntity
    {
        [Key]
        public Guid PaymentId { get; set; }
        public string PaymentAmount { get; set; }
        public string SubscriptionId { get; set; }
        public string EmailAddress { get; set; }
        public string CCNumber { get; set; }
        public string ProviderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProviderName { get; set; }
        public bool IsActive { get; set; }
    }
}
