using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class SalonsStylist
    {
        [Key]
        public int SalonStylistId { get; set; }
        [ForeignKey("SalonId")]
        public int? SalonId { get; set; }
        [ForeignKey("StylistId")]
        public int? StylistId { get; set; }
        public bool IsActive { get; set; }

        public virtual Salons Salon { get; set; }
        public virtual Stylist Stylist { get; set; }
    }
}
