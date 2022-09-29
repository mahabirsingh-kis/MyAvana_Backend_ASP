using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }

        [ForeignKey("QuestionId")]
        public int QuestionId { get; set; }

        public Questions Questions { get; set; }
    }
}
