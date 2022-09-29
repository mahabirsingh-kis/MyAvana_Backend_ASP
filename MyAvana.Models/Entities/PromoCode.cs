using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.Entities
{
    public class PromoCode
    {
        [Key]
        public string Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
		public string StripePlanId { get; set; }
		public bool Active { get; set; }
        public string CreatedBy { get; set; }
    }
}
