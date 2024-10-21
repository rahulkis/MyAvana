using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class CustomerSubscriptionHistory
    {
        [Key]
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Status { get; set; }
        public string ProductId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
