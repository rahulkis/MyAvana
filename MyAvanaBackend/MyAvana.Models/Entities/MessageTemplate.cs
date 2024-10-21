using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class MessageTemplate
    {
        [Key]
        public int MessageTemplateId { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateSubject { get; set; }
        public string TemplateBody { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
