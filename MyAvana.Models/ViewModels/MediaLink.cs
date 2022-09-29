using MyAvana.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class MediaLink
    {
        public List<MediaLinkEntity> Media { get; set; }
    }

    public class MediaLinkCategory
    {
        public string Category { get; set; }
        public List<MediaLinkEntity> MediaEntity { get; set; }
        
    }

    public class MediaEntity
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string ImageLink { get; set; }
        public string Description { get; set; }
        public string Header { get; set; }
        public bool IsFeatured { get; set; }
    }
}
