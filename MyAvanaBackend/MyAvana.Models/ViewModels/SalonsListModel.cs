using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
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
}
