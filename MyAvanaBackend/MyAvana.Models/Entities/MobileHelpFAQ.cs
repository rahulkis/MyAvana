using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyAvana.Models.Entities
{
    public class MobileHelpFAQ
    {
        [Key]
		public int MobileHelpFAQId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Videolink { get; set; }
        public string ImageLink { get; set; }
        public string VideoThumbnail { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public int? MobileHelpTopicId { get; set; }
        public virtual MobileHelpTopic MobileHelpTopic { get; set; }
    }
}
