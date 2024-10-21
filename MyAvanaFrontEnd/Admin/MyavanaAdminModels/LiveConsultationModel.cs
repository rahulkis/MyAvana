using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class LiveConsultationModel
    {
        public List<LiveConsultationUserDetails> LiveConsultationUserDetail { get; set; }
        public int InformationId { get; set; }
        public int LiveConsultationUserDetailsId { get; set; }
        public int? CustomerId { get; set; }
        public int? AdminId { get; set; }
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
        public bool IsCancel { get; set; }
    }
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
        //public bool isApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string AssignedTo { get; set; }

    }

    public class GetCustomerScheduleDetails
    {
        public string[] dates;
        public int LiveConsultationUserDetailsId { get; set; }
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
        public int Status { get; set; }
        public bool? CustomerType { get; set; }
        public string AssignedTo { get; set; }


    }
    public class ChangeStatus
    {
        public int LiveConsultationUserDetailsId { get; set; }
    }
}
