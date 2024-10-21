using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class SalonVideo
    {
        [Key]
        public int SalonVideoId { get; set; }
        public int SalonId { get; set; }
        public int MediaLinkEntityId { get; set; }
        public bool IsActive { get; set; }
    }
}
