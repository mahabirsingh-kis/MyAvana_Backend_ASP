using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.Entities
{
    public class SubscriptionsEntity
    {
        [Key]
        public string StripePlanId { get; set; }
        public string PlanName { get; set; }
        public double Amount { get; set; }
        public bool Active { get; set; }
        public string Details { get; set; }
        public double Validity { get; set; }
        
    }
}
