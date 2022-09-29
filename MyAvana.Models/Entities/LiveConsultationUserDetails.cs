using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class LiveConsultationUserDetails
    {
        public int LiveConsultationUserDetailsId { get; set; }
        public string UserEmail { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public int Status { get; set; }
        public string ContactNo { get; set; }
        public string FocusAreaDescription { get; set; }
        public int PaymentTypeId { get; set; }
        public bool IsPaid { get; set; }
        public string TimeZone { get; set; }
        public bool isApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string userId { get; set; }
    }
}
