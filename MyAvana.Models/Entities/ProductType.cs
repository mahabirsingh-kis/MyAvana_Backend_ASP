using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ProductType
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public int? ParentId { get; set; }
        public string ProductName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }

        public ProductTypeCategory ProductTypeCategory { get; set; }
    }
}
