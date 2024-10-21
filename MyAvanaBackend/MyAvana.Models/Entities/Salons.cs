using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class Salons
    {
        [Key]
        public int SalonId { get; set; }
        public string SalonName { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublicNotes { get; set; }
        public string SalonLogo { get; set; }
    }
}
