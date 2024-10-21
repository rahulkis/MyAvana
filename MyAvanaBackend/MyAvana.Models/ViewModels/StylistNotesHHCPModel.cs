using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
   public class StylistNotesHHCPModel
    {
        public string Notes { get; set; }
        public int HairProfileId { get; set; }     
        public string CustomerID { get; set; }
        public int UserId { get; set; }
    }
}
