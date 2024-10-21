using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace MyAvanaQuestionaireModel
{
    public class SetPassword
    {
        public string Email { get; set; }

        public string Code { get; set; }

        public string Password { get; set; }
    }

    public class ForgotPassword
    {
        public string contentType { get; set; }
        public string serializerSettings { get; set; }
        public int statusCode { get; set; }
        public string value { get; set; }
    }

    public class ForgotPasswordValue
    {
        public string item1 { get; set; }
        public string item2 { get; set; }
    }

    public class UserEntityModel
    {
        public string AccountNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? LastModifiedAt { get; set; }
        public bool LoginAlert { get; set; }
        public bool TwoFactorTrans { get; set; }
        public bool IsBlocked { get; set; }
        public bool Subscribe { get; set; }
        public bool Active { get; set; }
        public string Address { get; set; }
        //public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string StripeCustomerId { get; set; }
        public string HubSpotContactId { get; set; }
        public long TicketUserId { get; set; }

        public int CountryCode { get; set; }
    }
    public class UserModel
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string user_name { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string AccountNo { get; set; }
        public bool TwoFactor { get; set; }
        public string hairType { get; set; }
        public string imageURL { get; set; }
        public Guid Id { get; set; }
        public bool LoginAlert { get; set; }
        public bool? IsProCustomer { get; set; }
        public bool? IsPaid { get; set; }
        public int CustomerTypeId { get; set; }
        public bool? IsOnTrial { get; set; }
        public int? SubscriptionType { get; set; }

        public bool? IsHairAIAllowed { get; set; }
    }
    public class AlexaAccountLinkingModel
    {
        public string client_id { get; set; }
        public string response_type { get; set; }
        public string scope { get; set; }
        public string state { get; set; }
        public string redirect_uri { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class SetCustomerPassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserProfileImageModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string ImageURL { get; set; }
    }
}