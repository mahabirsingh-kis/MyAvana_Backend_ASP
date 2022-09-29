using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class LiveConsultationModel
    {
        public int InformationId { get; set; }
        public int LiveConsultationUserDetailsId { get; set; }
        public string CustomerId { get; set; }
        public string AdminId { get; set; }
        public bool IsCustomerJoined { get; set; }
        public bool IsAdminJoined { get; set; }
        public DateTimeOffset? CustomerJoinDatetime { get; set; }
        public DateTimeOffset? AdminJoinDatetime { get; set; }
        public string CustomerToken { get; set; }
        public string AdminToken { get; set; }
        public string CustomerParticipantId { get; set; }
        public string AdminParticipantId { get; set; }
        public string TwilioRoomName { get; set; }
        public string TwilioRoomSid { get; set; }
        public string TwilioCompositionSid { get; set; }
        public bool IsCompleted { get; set; }
        public bool? IsActive { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? CompletedDateTime { get; set; }
        public bool UpdateCustomer { get; set; }
        public string AspNetUserId { get; set; }
        public string CustomerName { get; set; }
        public string AdminName { get; set; }
        public bool? IsLeft { get; set; }
        public string TwilioCompositionStatus { get; set; }
        public bool UpdateComposition { get; set; }
        public bool? ExpireConsultation { get; set; }
        public bool CompleteConsultation { get; set; }
        public bool AlreadyJoined { get; set; }
        public string UserEmail { get; set; }
    }
}
