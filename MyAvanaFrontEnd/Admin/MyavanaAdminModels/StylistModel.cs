using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class StylistModel
    {
        public int StylistId { get; set; }
        public string StylistName { get; set; }
        public string SalonName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Background { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string StylistSpecialty { get; set; }
        public int? SalonId { get; set; }
    }

    public class StylishAdminModel
    {
        public int StylistId { get; set; }
        public string StylistName { get; set; }
        public string SalonName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Background { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

        public List<StylishCommon> stylishCommons { get; set; }
        public int? SalonId { get; set; }
    }

    public class StylishCommon
    {
        public int StylishCommonId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int StylistId { get; set; }
        public int StylistSpecialtyId { get; set; }
    }

    public class StylistSpecialtyModel
    {
        public int StylistSpecialtyId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class StylistListModel
    {
        public int StylistId { get; set; }
        public string StylistName { get; set; }
        public string SalonName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Background { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<StylistSpecialtyModel> stylistSpecialties { get; set; }
    }
}
