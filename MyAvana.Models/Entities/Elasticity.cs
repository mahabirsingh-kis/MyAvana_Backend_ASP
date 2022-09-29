using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class Elasticity
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
