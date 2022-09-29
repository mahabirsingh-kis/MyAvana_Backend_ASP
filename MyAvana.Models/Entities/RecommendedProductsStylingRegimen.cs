using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class RecommendedProductsStylingRegimen
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }

        [ForeignKey("HairProfileId")]
        public int HairProfileId { get; set; }

        public virtual HairProfile HairProfile { get; set; }
        public bool? IsAllStyling { get; set; }
    }
}
