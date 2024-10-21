using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class WebLogin
    {
        [Key]
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsActive { get; set; }
        public bool? UserType { get; set; }
        public int? OwnerSalonId { get; set; }
        [ForeignKey("UserTypeId")]
        public int? UserTypeId { get; set; }
        public virtual UserType UserTypes { get; set; }
    }
}
