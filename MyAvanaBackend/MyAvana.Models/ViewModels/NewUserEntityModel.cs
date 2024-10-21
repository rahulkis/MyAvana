using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using MyAvana.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace MyAvana.Models.ViewModels
{
    public class NewUserEntityModel : IdentityUser<Guid>
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
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string StripeCustomerId { get; set; }
        public string HubSpotContactId { get; set; }
        public long TicketUserId { get; set; }

        public int CountryCode { get; set; }
        public string HairType { get; set; }
        public string ImageURL { get; set; }
        public string DeviceId { get; set; }
        public string AIResult { get; set; }
        public bool? CustomerType { get; set; }
        public int? SalonId { get; set; }
        public bool? IsProCustomer { get; set; }
        public bool? IsPaid { get; set; }
        [ForeignKey("CustomerTypeId")]
        public int? CustomerTypeId { get; set; }
        public virtual CustomerType CustomerTypes { get; set; }
        public string SignupFrom { get; set; }
        public bool? IsOnTrial { get; set; }
        public DateTimeOffset? TrialExpiredOn { get; set; }
        public bool? BuyHairKit { get; set; }

        public string HairAnalysisImage { get; set; }
        public bool? IsAIV2Mobile { get; set; }
        public bool? IsInfluencer { get; set; }


    }
}
