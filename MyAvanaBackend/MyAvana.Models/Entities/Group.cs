using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string HairType { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublic { get; set; }
        public bool AccessRequested { get; set; }
        public bool ShowOnMobile { get; set; }
        public DateTime? UpdatedOn { get; set; }
        [ForeignKey("UpdatedByUserId")]
        public int? UpdatedByUserId { get; set; }
    }
}
