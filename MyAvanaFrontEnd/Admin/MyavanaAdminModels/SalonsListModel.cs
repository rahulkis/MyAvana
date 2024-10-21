using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class SalonsListModel
    {
        public int SalonId { get; set; }
        public string SalonName { get; set; }
    }
    public class SalonHairProfileModel
    {
        public int SalonId { get; set; }
        public int HairProfileId { get; set; }
        public string userId { get; set; }
    }
    public class SalonNotesModel
    {
        public int HairProfileId { get; set; }
        public string SalonNotes { get; set; }
        public string LoginUserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
