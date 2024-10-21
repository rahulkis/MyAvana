using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class DiscountCodes
    {
        [Key]
        public int DiscountCodeId { get; set; }
        public string DiscountCode { get; set; }
        public int DiscountPercent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
