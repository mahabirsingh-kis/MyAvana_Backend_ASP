using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class LiveConsultationCustomer
    {
        public int LiveConsultationCustomerId { get; set; }
        public bool IsCustomerJoined { get; set; }
        public bool IsAdminJoined { get; set; }
        public DateTime CustomerJoinDateTime { get; set; }
        public DateTime AdminJoinDateTime { get; set; }
        public string CustomerToken { get; set; }
        public string AdminToken { get; set; }
        public string CustomerParticipantId { get; set; }
        public string AdminParticipantId { get; set; }
        public string TwilioRoomName { get; set; }
        public string TwilioRoomSid { get; set; }
        public string TwilioCompositionSid { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CompletedDateTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TwilioCompositionStatus { get; set; }
        public string TwilioCompositionUrl { get; set; }
        public bool IsLeft { get; set; }
        public DateTime LeftDateTime { get; set; }
        public string customerId { get; set; }
        public string adminId { get; set; }
        public int LiveConsultationUserDetailsId { get; set; }

    }
}
