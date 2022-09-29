using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class LiveConsultationStatus
    {
        public int LiveConsultationStatusId { get; set; }
        public string StatusDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
