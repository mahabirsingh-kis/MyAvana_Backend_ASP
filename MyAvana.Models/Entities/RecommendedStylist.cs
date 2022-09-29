using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class RecommendedStylist
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
		public int StylistId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }

		[ForeignKey("HairProfileId")]
		public int HairProfileId { get; set; }

		public virtual HairProfile HairProfile { get; set; }
	}
}
