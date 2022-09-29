using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
	public class HairPorosity
	{
		public int Id { get; set; }
		[ForeignKey("HairProfileId")]
		public int HairProfileId { get; set; }
		[ForeignKey("PorosityId")]
		public int PorosityId { get; set; }
		public bool IsTopLeft { get; set; }
		public bool IsTopRight { get; set; }
		public bool IsBottomLeft { get; set; }
		public bool IsBottomRight { get; set; }
		public bool IsCrown { get; set; }

		public virtual HairProfile HairProfile { get; set; }
		public virtual Pororsity Porosity { get; set; }
	}
}
