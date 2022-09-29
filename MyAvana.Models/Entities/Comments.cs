using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class Comments
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int GroupPostId { get; set; }
        public virtual GroupPost GroupPost { get; set; }
    }
}
