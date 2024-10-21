using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MyAvanaApi.Models.Entities;
namespace MyAvana.Models.Entities
{
    public class ShopifyRequest
    {
        public int Id { get; set; }
        public string Payload { get; set; }
        public DateTime RequestDate { get; set; }
        public bool? IsExistingCustomer { get; set; }
        public int? SubscriptionType { get; set; }
        public bool? AlreadyActiveSubscription { get; set; }
        public string Email { get; set; }
        
        [ForeignKey("CustomerId")]
        public Guid? CustomerId { get; set; }
        public UserEntity Customer { get; set; }
  
    }
}
