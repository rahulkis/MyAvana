using MyAvana.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
	public class BlogArticleModel
	{
		public List<BlogArticle> Article { get; set; }
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
		public List <ArticleMoods> ArticleMoodsList { get; set; }
		public List<ArticleGuidances> ArticleGuidancesList { get; set; }
		//public string Mood { get; set; }
		//public string Guidance { get; set; }
		public string ArticleMoods { get; set; }
		public string ArticleGuidances { get; set; }
	}

	public class BlogArticleListModel
	{
		public List<BlogArticlePostModel> Article { get; set; }
	}

	public class ArticleHairStyleModel
    {
		public string HairStyleName { get; set; }
    }
	public class BlogArticleHairDairyModel
	{
		public string UserId { get; set; }
		public int BlogArticleId { get; set; }
		public string HeadLine { get; set; }
		public string Details { get; set; }
		public string ImageUrl { get; set; }
		public string Url { get; set; }
		public bool IsActive { get; set; }
		public DateTime? CreatedOn { get; set; }
		//public string Mood { get; set; }
		//public string Guidance { get; set; }

	}

}
