using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class CustomerQuestionaire
    {
        public int QuestionaireId { get; set; }
        public Guid UserId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string DescriptiveAnswer { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
