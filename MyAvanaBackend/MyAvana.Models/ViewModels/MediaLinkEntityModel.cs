using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class MediaLinkEntityModel
    {
        public int MediaLinkEntityId { get; set; }
        public Guid Id { get; set; }
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string ImageLink { get; set; }
        public string Description { get; set; }
        public string Header { get; set; }
        public bool IsFeatured { get; set; }
        public int VideoCategoryId { get; set; }
        public bool IsActive { get; set; }
        public bool ShowOnMobile { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List<UserSalonOwnerModel> userSalons { get; set; }
        public string HairChallenges { get; set; }
        public string HairGoals { get; set; }
        public List<int> SelectedHairChallenges { get; set; }
        public List<int> SelectedHairGoals { get; set; }
    }

    public class EducationTipAndVideo
    {
        public int EducationTipsId { get; set; }
        public string Description { get; set; }
        public bool ShowOnMobile { get; set; }
        public int MediaLinkEntityId { get; set; }
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string ImageLink { get; set; }
        public string VideoDescription { get; set; }
    }
}
