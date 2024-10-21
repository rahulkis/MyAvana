using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class Moods
    {
        [Key]
        public int Id { get; set; }
        public string Mood { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
