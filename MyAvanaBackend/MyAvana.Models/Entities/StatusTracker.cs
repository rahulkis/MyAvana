using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MyAvanaApi.Models.Entities;
namespace MyAvana.Models.Entities
{
    public class StatusTracker
    {
        [Key]
        public int StatusTrackerId { get; set; }

        [ForeignKey("CustomerId")]
        public Guid CustomerId { get; set; }
        public UserEntity Customer { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string KitSerialNumber { get; set; } 

        [ForeignKey("HairAnalysisStatusId")]
        public int HairAnalysisStatusId { get; set; } 

        public bool WelcomeEmailSent { get; set; }

        public DateTime? WelcomeEmailSentOn { get; set; }

        public DateTime? EmailCommunicationLastSentOn { get; set; }

        public int EmailCommunicationCount { get; set; }

        public DateTime? InAppNotificationLastSentOn { get; set; }

        public int InAppNotificationCount { get; set; }

        public bool ExtensionRequested { get; set; }

        public DateTime? ExtensionDate { get; set; }

        public DateTime LastUpdatedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public virtual HairAnalysisStatus TrackingStatus { get; set; }
    }
}
