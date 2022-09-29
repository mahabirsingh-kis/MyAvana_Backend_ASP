using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class HairStyles
    {
        public int Id { get; set; }
        public string Style { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
