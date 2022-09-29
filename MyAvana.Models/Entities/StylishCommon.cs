using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class StylishCommon
    {
        [Key]
        public int StylishCommonId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

        [ForeignKey("StylistId")]
        public int StylistId { get; set; }

        public virtual Stylist Stylist { get; set; }

        [ForeignKey("StylistSpecialtyId")]
        public int StylistSpecialtyId { get; set; }

        public virtual StylistSpecialty StylistSpecialty { get; set; }
    }
}
