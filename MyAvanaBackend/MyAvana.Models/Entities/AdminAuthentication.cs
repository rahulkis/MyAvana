using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class AdminAuthentication
    {
        [Key]
        public int AdminAuthenticationId { get; set; }
        [ForeignKey("UserId")]
        public int? UserId { get; set; }
        public virtual WebLogin User { get; set; }
        public Guid token { get; set; }
        public DateTime ExpireOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
