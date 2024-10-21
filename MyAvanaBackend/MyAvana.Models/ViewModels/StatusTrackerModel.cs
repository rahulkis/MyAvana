using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class StatusTrackerModel
    {
        
            public int StatusTrackerId { get; set; }
            public string CustomerName { get; set; }
            public string HairAnalysisStatus { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int HairAnalysisStatusId { get; set; }
        public string LastModifiedBy { get; set; }
        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string KitSerialNumber { get; set; }
    }

    public class HairAnalysisStatusModel
    {
        public int HairAnalysisStatusId { get; set; }
        public string StatusName { get; set; }
    }

    public class HairAnalysisStatusHistoryList
    {
        public string OldStatusName { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedByUser { get; set; }
    }
}
