using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ReportPosts
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
