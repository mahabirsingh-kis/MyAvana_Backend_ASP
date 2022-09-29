using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class HairStrandsImages
    {
        [Key]
        public int StrandsImagesId { get; set; }
        public string TopLeftImage { get; set; }
        public string TopRightImage { get; set; }
        public string BottomLeftImage { get; set; }
        public string BottomRightImage { get; set; }
        public string CrownImage { get; set; }       
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

        [ForeignKey("Id")]
        public int Id { get; set; }

        public virtual HairStrands HairStrands { get; set; }
    }
}
