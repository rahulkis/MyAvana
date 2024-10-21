using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class InAppPayload
    {
        [Key]
        public int Id { get; set; }
        public string Payload { get; set; }
        public string Platform { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
