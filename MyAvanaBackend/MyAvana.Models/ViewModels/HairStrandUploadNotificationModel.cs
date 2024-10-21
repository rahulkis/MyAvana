using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
   public class HairStrandUploadNotificationModel
    {
        public int Id { get; set; }
        public string SalonName { get; set; }

        public bool? IsRead { get; set; }

        public string UserName { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string UserId { get; set; }
        public int HairProfileId { get; set; }
    }
}
