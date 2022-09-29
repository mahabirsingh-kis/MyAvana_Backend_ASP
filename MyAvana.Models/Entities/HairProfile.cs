using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
	public class HairProfile
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string HairId { get; set; }
		public string HealthSummary { get; set; }
		public string ConsultantNotes { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedOn { get; set; }
		public bool IsDrafted { get; set; }

	}
}
