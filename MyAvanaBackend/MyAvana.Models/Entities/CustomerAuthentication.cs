using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class CustomerAuthentication
    {
        [Key]
        public int CustomerAuthenticationId { get; set; }
        public Guid UserId { get; set; }
        public Guid token { get; set; }
        public DateTime ExpireOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
