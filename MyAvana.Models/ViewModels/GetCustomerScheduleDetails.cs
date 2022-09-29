using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{

 public   class GetCustomerScheduleDetails
    {
        public string UserEmail { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string ContactNo { get; set; }
        public string FocusAreaDescription { get; set; }
        public string TimeZone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int LiveConsultationUserDetailsId { get; set; }
        public string[] dates { get; set; }
        public int Status { get; set; }
        public bool IsPaid { get; set; }
        public bool? CustomerType { get; set; }
    }
 
}
