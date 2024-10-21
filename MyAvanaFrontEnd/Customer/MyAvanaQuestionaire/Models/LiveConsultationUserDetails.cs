using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaQuestionaire.Models
{
    public class LiveConsultationUserDetails
    {
        public string UserEmail { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }

        public int ContactNo { get; set; }
        public string FocusAreaDescription { get; set; }
    }
}
