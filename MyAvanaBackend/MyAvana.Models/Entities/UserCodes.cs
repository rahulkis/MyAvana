using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class UserCodes
    {
        [Key]
        public int UserCodeId { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
