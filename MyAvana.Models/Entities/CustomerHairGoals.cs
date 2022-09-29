using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class CustomerHairGoals
    {
        public int Id { get; set; }
        public int HairInfoId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
