using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class MediaLinkEntity
    {
        [Key]
		public int MediaLinkEntityId { get; set; }
		public Guid Id { get; set; }
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string ImageLink { get; set; }
        public string Description { get; set; }
        public string Header { get; set; }
        public bool IsFeatured { get; set; }
        public int VideoCategoryId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }

        public virtual VideoCategory VideoCategory { get; set; }

    }
}
