using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
	public class BlogPostModel
	{
		[JsonProperty(PropertyName = "blogArticleId")]
		public int BlogArticleId { get; set; }
		[JsonProperty(PropertyName = "headline")]
		public string Headline { get; set; }
		[JsonProperty(PropertyName = "details")]
		public string Details { get; set; }
		[JsonProperty(PropertyName = "imageUrl")]
		public string ImageUrl { get; set; }
		[JsonProperty(PropertyName = "url")]
		public string Url { get; set; }

		[JsonProperty(PropertyName = "isActive")]
		public bool IsActive { get; set; }
		//public IFormFile FormFile { get; set; }

		[JsonProperty(PropertyName = "createdOn")]
		public DateTime? CreatedOn { get; set; }
		[JsonProperty(PropertyName = "ArticleProducts")]
		public string ArticleProducts { get; set; }
		[JsonProperty(PropertyName = "ArticleHairStyles")]
		public string ArticleHairStyles { get; set; }
		[JsonProperty(PropertyName = "ArticleMoods")]
		public string ArticleMoods { get; set; }
		[JsonProperty(PropertyName = "ArticleGuidances")]
		public string ArticleGuidances { get; set; }
		//[JsonProperty(PropertyName = "Mood")]
		//public string Mood { get; set; }
		//[JsonProperty(PropertyName = "Guidance")]
		//public string Guidance { get; set; }
	}

	public class BlogArticleModel
	{
		public List<BlogPostModel> Article { get; set; }
	}
	public class BlogArticlePostModel
	{
		public int BlogArticleId { get; set; }
		public string HeadLine { get; set; }
		public string Details { get; set; }
		public string ImageUrl { get; set; }
		public string Url { get; set; }
		public bool IsActive { get; set; }
		public DateTime? CreatedOn { get; set; }
		public string ArticleProducts { get; set; }
		public string ArticleHairStyles { get; set; }
		public List<ArticleProduct> ArticleProductsList { get; set; }
		public List<ArticleHairStyle> ArticleHairStylesList { get; set; }
		public List<ArticleMoods> ArticleMoodsList { get; set; }
		public List<ArticleGuidances> ArticleGuidancesList { get; set; }
		public string ArticleMoods { get; set; }
		public string ArticleGuidances { get; set; }
		//public string Mood { get; set; }
		//public string Guidance { get; set; }
	}
	public class BlogArticleListModel
	{
		public List<BlogArticlePostModel> Article { get; set; }
	}
	public class ArticleHairStyle
	{
		public int ArticleHairStyleId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public bool? IsActive { get; set; }
		public int? BlogArticleId { get; set; }
		public int? HairStylesId { get; set; }
	}

	public class ArticleProduct
	{
		public int ArticleProductsId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public bool? IsActive { get; set; }
		public int? BlogArticleId { get; set; }
		public int? ProductEntityId { get; set; }
	}
	public class QuestModel
    {
        public string Email { get; set; }
		public int? QA { get; set; }
	}

    public class GroupPost
    {
        public string HairType { get; set; }
        public string UserEmail { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Audio { get; set; }
        public string AudioUrl { get; set; }
        public IFormFile Video { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public IFormFile Thumbnail { get; set; }
    }

	public class GroupPostModelParam
	{
		public string HairType { get; set; }
		public string UserEmail { get; set; }
		public string Description { get; set; }
		public IFormFile Image { get; set; }
		public string ImageUrl { get; set; }
		public IFormFile Audio { get; set; }
		public string AudioUrl { get; set; }
		public IFormFile Video { get; set; }
		public string VideoUrl { get; set; }
		public string ThumbnailUrl { get; set; }
		public IFormFile Thumbnail { get; set; }
		public List<TaggedUsersList> TaggedUsersList { get; set; }
	}
	public class TaggedUsersList
	{
		public int? PostId { get; set; }
		public Guid UserId { get; set; }
		public string UserName { get; set; }

	}

	public class Moods
	{
		[JsonProperty(PropertyName = "Id")]
		public int Id { get; set; }
		[JsonProperty(PropertyName = "Mood")]
		public string Mood { get; set; }
		[JsonProperty(PropertyName = "CreatedOn")]
		public DateTime? CreatedOn { get; set; }
		[JsonProperty(PropertyName = "IsActive")]
		public bool? IsActive { get; set; }


	}

	public class Guidances
	{
		[JsonProperty(PropertyName = "Id")]
		public int Id { get; set; }
		[JsonProperty(PropertyName = "Guidance")]
		public string Guidance { get; set; }
		[JsonProperty(PropertyName = "CreatedOn")]
		public DateTime? CreatedOn { get; set; }
		[JsonProperty(PropertyName = "IsActive")]
		public bool? IsActive { get; set; }


	}

	public class ArticleMoods
	{
		public int ArticleMoodId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public bool? IsActive { get; set; }
		public int? BlogArticleId { get; set; }
		public int? MoodId { get; set; }
	}

	public class ArticleGuidances
	{
		public int ArticleGuidanceId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public bool? IsActive { get; set; }
		public int? BlogArticleId { get; set; }
		public int? GuidanceId { get; set; }
	}
}
