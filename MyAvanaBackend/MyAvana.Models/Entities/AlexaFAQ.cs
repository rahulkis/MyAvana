using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class AlexaFAQ
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string ShortResponse { get; set; }
        public string DetailedResponse { get; set; }
        public string Category { get; set; }
        public bool IsDeleted { get; set; }
    }
}
