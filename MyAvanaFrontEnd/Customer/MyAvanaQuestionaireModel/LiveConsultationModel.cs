using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvanaQuestionaireModel
{
    public class ScheduleTime
    {
        public int ScheduleTimeId { get; set; }
        public string Time { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    public class TimeZoneLiveSchedule
    {
        public List<ScheduleTime> Time { get; set; }

        public List<TimeZones> TimeZones { get; set; }
        public List<LiveConsultationUserDetails> LiveConsultationUserDetail { get; set; }

    }

    public class TimeZones
    {
        public int Id { get; set; }

        public string Timezones { get; set; }
        public bool IsActive { get; set; }
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
        //public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string userId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsValidToJoin { get; set; }
        public string assignedTo { get; set; }

    }

    public class LiveConsultationUser
    {
        public string Email { get; set; }
    }

    public class CheckoutRequest
    {
        public string SubscriptionId { get; set; }
        public double Amount { get; set; }
        public string CardOwnerFirstName { get; set; }
        public string CardOwnerLastName { get; set; }
        public long? ExpirationYear { get; set; }
        public string CardNumber { get; set; }
        public long? ExpirationMonth { get; set; }
        public string CVV2 { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string Address { get; set; }
        public string userId { get; set; }
        public bool? IsSubscriptionPayment { get; set; }

    }

    public class SignupAndPayment
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public int CountryCode { get; set; }
        public bool? CustomerType { get; set; }
        public bool? IsProCustomer { get; set; }
        public bool? IsPaid { get; set; }
        public int CustomerTypeId { get; set; }
        public bool? BuyHairKit { get; set; }
        public int? SalonId { get; set; }

        public int? CreatedByUserId { get; set; }
        public string SubscriptionId { get; set; }
        public double Amount { get; set; }
        public string CardOwnerFirstName { get; set; }
        public string CardOwnerLastName { get; set; }
        public long? ExpirationYear { get; set; }
        public string CardNumber { get; set; }
        public long? ExpirationMonth { get; set; }
        public string CVV2 { get; set; }
        public string State { get; set; }
        public int  StateId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CountryId { get; set; }
        public string Zipcode { get; set; }
        public string Address { get; set; }
        public string userId { get; set; }
        public string KitSerialNumber { get; set; }
        public bool? IsSubscriptionPayment { get; set; }
    }
    public class TokenModel
    {
        public string Token { get; set; }
    }

    public class LiveConsultationModel
    {
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
    }

    public class ResponseModel
    {
        public string StatusCode { get; set; }
        public string Value { get; set; }
    }
}
