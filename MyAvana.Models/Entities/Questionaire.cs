using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class Questionaire
    {
        [Key]
        public int QuestionaireId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("QuestionId")]
        public int QuestionId { get; set; }
        [ForeignKey("AnswerId")]
        public int? AnswerId { get; set; }
        public string DescriptiveAnswer { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int QA { get; set; }
        public Questions Questions { get; set; }
        public Answer Answer { get; set; }
    }
}
