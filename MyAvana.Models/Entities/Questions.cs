using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class Questions
    {
        [Key]
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsTagged { get; set; }
        public int SerialNo { get; set; }
    }
}
