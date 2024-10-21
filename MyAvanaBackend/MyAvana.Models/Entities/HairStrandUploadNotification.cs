using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAvana.Models.Entities
{
    public class HairStrandUploadNotification
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("HairProfileId")]
        public int HairProfileId { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("SalonId")]
        public int SalonId { get; set; }
        public virtual Salons Salon { get; set; }
        public virtual HairProfile HairProfile { get; set; }
    }
}
