using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.Entities
{
    public class PaymentEntity
    {
        [Key]
        public Guid PaymentId { get; set; }
        public string PaymentAmount { get; set; }
        public string SubscriptionId { get; set; }
        public string EmailAddress { get; set; }
        public string CCNumber { get; set; }
        public string ProviderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProviderName { get; set; }
        public bool IsActive { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public int State { get; set; }
        //public int Country { get; set; }
        //public string ZipCode { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsHairAIAvailed { get; set; }
        public DateTime? HairAIAvailDate { get; set; }
        public string PurchaseToken { get; set; }


    }

}