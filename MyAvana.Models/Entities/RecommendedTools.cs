using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyAvana.Models.Entities
{
    public class RecommendedTools
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ToolId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        [ForeignKey("HairProfileId")]
        public int HairProfileId { get; set; }

        public virtual HairProfile HairProfile { get; set; }



    }
}
