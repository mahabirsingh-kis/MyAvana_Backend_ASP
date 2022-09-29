using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
	public class BlogArticle
	{
		[Key]
		public int BlogArticleId { get; set; }
		public string HeadLine { get; set; }
		public string Details { get; set; }
		public string ImageUrl { get; set; }
		public string Url { get; set; }
		public bool IsActive { get; set; }
		public DateTime? CreatedOn { get; set; }
	}
}
