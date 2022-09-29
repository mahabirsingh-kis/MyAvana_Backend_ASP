using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class GroupPost
    {
        public int Id { get; set; }
        public string HairType { get; set; }
        public String UserEmail { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string AudioUrl { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}
