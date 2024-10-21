using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAvana.Models.Entities
{
    public class SharedHHCP
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("HairProfileId")]
        public int HairProfileId { get; set; }
        public virtual HairProfile HairProfile { get; set; }
        public Guid SharedBy { get; set; }
        public Guid SharedWith { get; set; }
        public DateTime SharedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsRevoked { get; set; }
    }
}
