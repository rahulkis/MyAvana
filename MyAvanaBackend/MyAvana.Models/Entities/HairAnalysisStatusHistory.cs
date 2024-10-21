using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class HairAnalysisStatusHistory
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("OldHairAnalysisStatusId")]
        public int? OldHairAnalysisStatusId { get; set; }
        public virtual HairAnalysisStatus OldHairAnalysisStatus { get; set; }
        [ForeignKey("NewHairAnalysisStatusId")]
        public int? NewHairAnalysisStatusId { get; set; }
        public virtual HairAnalysisStatus NewHairAnalysisStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("UpdatedByUserId")]
        public int? UpdatedByUserId { get; set; }
        public virtual WebLogin UpdatedBy { get; set; }
        [ForeignKey("CustomerId")]
        public Guid CustomerId { get; set; }
        public UserEntity Customer { get; set; }
    }
}
