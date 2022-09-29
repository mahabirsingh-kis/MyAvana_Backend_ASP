using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class PrePopulateTypes
    {
        [Key]
        public int PrePopulateTypesId { get; set; }

        [ForeignKey("QuestionId")]
        public int QuestionId { get; set; }

        public virtual Questions Questions { get; set; }

        [ForeignKey("AnswerId")]
        public int AnswerId { get; set; }

        public virtual Answer Answer { get; set; }

        [ForeignKey("ProductTypeId")]
        public int ProductTypeId { get; set; }

        public virtual ProductType ProductType { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
