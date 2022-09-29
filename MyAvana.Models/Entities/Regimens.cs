using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
	public class Regimens
	{
		public int RegimensId { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int Step1 { get; set; }
		public int Step2 { get; set; }
		public int Step3 { get; set; }
		public int Step4 { get; set; }
		public int Step5 { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedOn { get; set; }
		[ForeignKey("RegimenStepsId")]
		public int RegimenStepsId { get; set; }

		public RegimenSteps RegimenSteps { get; set; }
	}
}
