using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class TrackingDetails
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string HairStyle { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
