using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ProductTypeCategory
    {
        [Key]
		public int Id { get; set; }
        public string CategoryName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsHair { get; set; }
        public bool? IsRegimens { get; set; }
        public bool IsActive { get; set; }
    }
}
