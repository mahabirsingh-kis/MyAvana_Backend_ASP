using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class IndicatorModel
    {
        public int ProductIndicatorId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedDate { get; set; }
    }
}
