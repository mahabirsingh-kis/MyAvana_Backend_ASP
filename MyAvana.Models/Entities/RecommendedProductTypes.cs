using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class RecommendedProductTypes
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }
        public bool IsHealthyRegimen { get; set; }
        public bool IsStylingRegimen { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }

        [ForeignKey("HairProfileId")]
        public int HairProfileId { get; set; }

        public virtual HairProfile HairProfile { get; set; }
    }
}
