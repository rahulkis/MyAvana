using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class CustomerAIResult
    {
        [Key]
        public int CustomerAIResultId { get; set; }
        public Guid UserId { get; set; }
        public string AIResult { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("HairProfileId")]
        public int? HairProfileId { get; set; }

        public virtual HairProfile HairProfile { get; set; }
        public bool? IsVersion2 { get; set; }
    }
}
