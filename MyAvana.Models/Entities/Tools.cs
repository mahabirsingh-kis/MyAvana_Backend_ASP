using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class Tools
    {
        public int Id { get; set; }
        public string ToolName { get; set; }
        public string ActualName { get; set; }
        public string BrandName { get; set; }
        public string Image { get; set; }
        public string ToolLink { get; set; }
        public string ToolDetails { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
