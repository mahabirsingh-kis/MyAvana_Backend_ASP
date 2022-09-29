using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
	public class HairElasticity
	{
		public int Id { get; set; }
		[ForeignKey("HairProfileId")]
		public int HairProfileId { get; set; }
		[ForeignKey("ElasticityId")]
		public int ElasticityId { get; set; }
		public bool IsTopLeft { get; set; }
		public bool IsTopRight { get; set; }
		public bool IsBottomLeft { get; set; }
		public bool IsBottomRight { get; set; }
		public bool IsCrown { get; set; }

		public virtual HairProfile HairProfile { get; set; }
		public virtual Elasticity Elasticity { get; set; }
	}
}
