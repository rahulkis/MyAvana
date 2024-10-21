using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class SalonNotesHHCP
    {
        [Key]
        public int SalonNoteId { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedOn { get; set; }
        [ForeignKey("HairProfileId")]
        public int HairProfileId { get; set; }

        public virtual HairProfile HairProfile { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public virtual WebLogin WebLogin { get; set; }
    }
}
