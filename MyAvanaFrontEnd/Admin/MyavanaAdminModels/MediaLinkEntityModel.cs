using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MyavanaAdminModels
{
    public class MediaLinkEntityModel
    {
        public Guid Id { get; set; }
		
		[JsonProperty(PropertyName = "MediaLinkEntityId")]
		public int MediaLinkEntityId { get; set; }

		[JsonProperty(PropertyName = "VideoId")]
        public string VideoId { get; set; }

        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }

        //[JsonProperty(PropertyName = "ImageLink")]
        public string ImageLink { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "ShowOnMobile")]
        public bool ShowOnMobile { get; set; }

        //[JsonProperty(PropertyName = "Header")]
        public string Header { get; set; }

        //[JsonProperty(PropertyName = "IsFeatured")]
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }
        public string CategoryId { get; set; }
        public int VideoCategoryId { get; set; }
        [JsonProperty(PropertyName = "VideoCategory")]
        public string VideoCategory { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List<UserSalonOwnerModel> userSalons { get; set; }

        [JsonProperty(PropertyName = "HairChallenges")]
        public string HairChallenges { get; set; }

        [JsonProperty(PropertyName = "HairGoals")]
        public string HairGoals { get; set; }
        public List<int> SelectedHairChallenges { get; set; }
        public List<int> SelectedHairGoals { get; set; }
    }

    public class VideoCategory
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    public class ThumbnailModel
    {
        public string imageData { get; set; }
    }
}
