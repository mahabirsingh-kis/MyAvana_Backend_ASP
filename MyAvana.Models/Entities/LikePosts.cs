using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class LikePosts
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public int PostId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsLike { get; set; }
    }
}
