using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
	public class RecommendedVideos
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int MediaLinkEntityId { get; set; }
		public string ThumbNail { get; set; }
		public DateTime? CreatedOn { get; set; }
		public bool? IsActive { get; set; }

		[ForeignKey("HairProfileId")]
		public int HairProfileId { get; set; }

		public virtual HairProfile HairProfile { get; set; }
	}
}
