using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class CustomerHairAIModel
    {
        public int CustomerAIResultId { get; set; }
        public Guid UserId { get; set; }
        public string AIResult { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }

        public int? HairProfileId { get; set; }

        public bool? IsVersion2 { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
