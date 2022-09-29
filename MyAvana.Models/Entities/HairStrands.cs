using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
	public class HairStrands
	{
		public int Id { get; set; }
		public string TopLeftPhoto { get; set; }
		public string TopLeftStrandDiameter { get; set; }
		public string TopLeftHealthText { get; set; }
		public string TopRightPhoto { get; set; }
		public string TopRightStrandDiameter { get; set; }
		public string TopRightHealthText { get; set; }
		public string BottomLeftPhoto { get; set; }
		public string BottomLeftStrandDiameter { get; set; }
		public string BottomLeftHealthText { get; set; }
		public string BottomRightPhoto { get; set; }
		public string BottomRightStrandDiameter { get; set; }
		public string BottomRightHealthText { get; set; }
		public string CrownPhoto { get; set; }
		public string CrownStrandDiameter { get; set; }
		public string CrownHealthText { get; set; }
		[ForeignKey("HairProfileId")]
		public int HairProfileId { get; set; }

		public virtual HairProfile HairProfile { get; set; }

	}
}
