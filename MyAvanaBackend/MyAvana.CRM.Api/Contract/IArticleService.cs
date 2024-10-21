using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
	public interface IArticleService
	{
		BlogArticleListModel GetArticles();
		List<BlogArticleHairDairyModel> GetBlogArticlesForHairDairy(BlogArticleHairDairyModel blogArticleHairDairyModel);
		BlogArticlePostModel UploadArticles(BlogArticlePostModel blogArticle);
		BlogArticlePostModel GetArticleById(BlogArticlePostModel blogArticle);
        bool DeleteArticle(BlogArticle blogArticle);
		List<Moods> GetMoods();
		List<Guidances> GetGuidances();
	}
}
