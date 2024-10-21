using Flurl;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyAvana.CRM.Api.Services
{
    public class ArticleService : IArticleService
    {
        private readonly AvanaContext _context;
        private readonly Logger.Contract.ILogger _logger;
        public ArticleService(AvanaContext avanaContext,Logger.Contract.ILogger logger)
        {
            _context = avanaContext;
            _logger = logger;
        }
        public BlogArticleListModel GetArticles()
        {
            try
            {
                List<BlogArticlePostModel> blogArticle = _context.BlogArticles.Where(x => x.IsActive == true).Select(x => new BlogArticlePostModel
                {
                    HeadLine = x.HeadLine,
                    Details = x.Details,
                    Url = x.Url,
                    ImageUrl = x.ImageUrl,
                    BlogArticleId = x.BlogArticleId,
                    IsActive = x.IsActive,
                    CreatedOn=x.CreatedOn,
                    ArticleMoods = string.Join(", ", _context.ArticleMoods.Where(z => z.IsActive == true && z.BlogArticleId == x.BlogArticleId).Join(
                        _context.Moods,
                        ArticleMoods => ArticleMoods.MoodId,
                        Moods => Moods.Id,
                        (ArticleMoods, Moods) => new ArticleHairStyleModel
                        {
                            HairStyleName = Moods.Mood
                        }).Select(z => z.HairStyleName).ToArray()),
                    ArticleGuidances = string.Join(", ", _context.ArticleGuidances.Where(z => z.IsActive == true && z.BlogArticleId == x.BlogArticleId).Join(
                        _context.Guidances,
                        ArticleGuidances => ArticleGuidances.GuidanceId,
                        Guidances => Guidances.Id,
                        (ArticleGuidances, Guidances) => new ArticleHairStyleModel
                        {
                            HairStyleName = Guidances.Guidance
                        }).Select(z => z.HairStyleName).ToArray()),
                    ArticleHairStyles = string.Join(", ", _context.ArticleHairStyles.Where(z => z.IsActive == true && z.BlogArticleId == x.BlogArticleId).Join(
                        _context.HairStyles,
                        ArticleHairStyle => ArticleHairStyle.HairStylesId,
                        HairStyles => HairStyles.Id,
                        (ArticleHairStyle, HairStyles) => new ArticleHairStyleModel
                        {
                            HairStyleName = HairStyles.Style
                        }).Select(z => z.HairStyleName).ToArray()),
                    ArticleProducts = string.Join(", ", _context.ArticleProducts.Where(z => z.IsActive == true && z.BlogArticleId == x.BlogArticleId).Join(
                        _context.ProductEntities,
                        ArticleProducts => ArticleProducts.ProductEntityId,
                        ProductEntities => ProductEntities.Id,
                        (ArticleProducts, ProductEntities) => new ArticleHairStyleModel
                        {
                            HairStyleName = ProductEntities.ActualName
                        }).Select(z => z.HairStyleName).ToArray())           
                }).OrderByDescending(x => x.CreatedOn).ToList();


                BlogArticleListModel blogArticleModel = new BlogArticleListModel();
                blogArticleModel.Article = blogArticle;
                return blogArticleModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetArticles, Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<BlogArticleHairDairyModel> GetBlogArticlesForHairDairy(BlogArticleHairDairyModel blogArticleHairDairyModel)
        {
            try
            {
                int hairStyleId;
                int moodId;
                int guidanceId;
                List<BlogArticleHairDairyModel> blogArticle = new List<BlogArticleHairDairyModel>();
                var dairyDate = Convert.ToDateTime(blogArticleHairDairyModel.CreatedOn).Date;
                DailyRoutineTracker dailyRoutineTracker = _context.DailyRoutineTracker.Where(x => x.IsActive == true && x.UserId == blogArticleHairDairyModel.UserId && x.TrackTime.Date == blogArticleHairDairyModel.CreatedOn).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                if(dailyRoutineTracker != null)
                {
                    hairStyleId = _context.HairStyles.Where(x => x.Style == dailyRoutineTracker.HairStyle).Select(x => x.Id).FirstOrDefault();
                    moodId = _context.Moods.Where(x => x.Mood == dailyRoutineTracker.CurrentMood).Select(x => x.Id).FirstOrDefault();
                    guidanceId = _context.Guidances.Where(x => x.Guidance == dailyRoutineTracker.GuidanceNeeded).Select(x => x.Id).FirstOrDefault();
                    var productIds = _context.DailyRoutineProducts.Where(x => x.RoutineTrackerId == dailyRoutineTracker.Id).Select(x => x.ProductId).ToArray();
                    var blogStyleIds = _context.ArticleHairStyles.Where(x => x.HairStylesId == hairStyleId).Select(x => x.BlogArticleId).ToArray();
                    var blogProductIds = _context.ArticleProducts.Where(u => productIds.Any(x => x == u.ProductEntityId)).Select(x => x.BlogArticleId).ToArray();
                    var blogMoodIds = _context.ArticleMoods.Where(x => x.MoodId == moodId).Select(x => x.BlogArticleId).ToArray();
                    var blogGuidanceIds = _context.ArticleGuidances.Where(x => x.GuidanceId == guidanceId).Select(x => x.BlogArticleId).ToArray();

                    blogArticle = _context.BlogArticles.Where(x => x.IsActive == true)
                   .Select(x => new BlogArticleHairDairyModel
                   {
                       HeadLine = x.HeadLine,
                       Details = x.Details,
                       Url = x.Url,
                       ImageUrl = x.ImageUrl,
                       BlogArticleId = x.BlogArticleId,
                       CreatedOn=x.CreatedOn
                   })
                   .OrderByDescending(x => x.CreatedOn).ToList();

                    blogArticle = blogArticle.Where(u => blogStyleIds.Any(x => x == u.BlogArticleId) || blogProductIds.Any(x => x == u.BlogArticleId)  || blogMoodIds.Any(x => x == u.BlogArticleId) || blogGuidanceIds.Any(x => x == u.BlogArticleId)).ToList();
                }
               
                return blogArticle;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetBlogArticlesForHairDairy, Error: " + ex.Message, ex);
                return null;
            }
        }

        public BlogArticlePostModel UploadArticles(BlogArticlePostModel blogArticle)
        {
            try
            {
                List<ArticleProduct> listArticleProduct = JsonConvert.DeserializeObject<List<ArticleProduct>>(blogArticle.ArticleProducts);
                List<ArticleHairStyle> listArticleHairStyle = JsonConvert.DeserializeObject<List<ArticleHairStyle>>(blogArticle.ArticleHairStyles);
                List<ArticleMoods> listArticleMood = JsonConvert.DeserializeObject<List<ArticleMoods>>(blogArticle.ArticleMoods);
                List<ArticleGuidances> listArticleGuidance = JsonConvert.DeserializeObject<List<ArticleGuidances>>(blogArticle.ArticleGuidances);
                if (blogArticle.BlogArticleId != 0)
                {
                    var objArticle = _context.BlogArticles.Where(x => x.BlogArticleId == blogArticle.BlogArticleId).FirstOrDefault();
                    objArticle.HeadLine = blogArticle.HeadLine;
                    objArticle.Details = blogArticle.Details;
                    objArticle.ImageUrl = blogArticle.ImageUrl;
                    objArticle.Url = blogArticle.Url;
                    var articleMood = _context.ArticleMoods.Where(x => x.BlogArticleId == blogArticle.BlogArticleId).ToList();
                    var articleGuidance = _context.ArticleGuidances.Where(x => x.BlogArticleId == blogArticle.BlogArticleId).ToList();
                    var articleProduct = _context.ArticleProducts.Where(x => x.BlogArticleId == blogArticle.BlogArticleId).ToList();
                    var articleHairStyle = _context.ArticleHairStyles.Where(x => x.BlogArticleId == blogArticle.BlogArticleId).ToList();
                    if(articleProduct.Count > 0)
                    {
                        _context.ArticleProducts.RemoveRange(articleProduct);
                        _context.SaveChanges();
                    }
                    if (articleHairStyle.Count > 0)
                    {
                        _context.ArticleHairStyles.RemoveRange(articleHairStyle);
                        _context.SaveChanges();
                    }
                    if (articleMood.Count > 0)
                    {
                        _context.ArticleMoods.RemoveRange(articleMood);
                        _context.SaveChanges();
                    }
                    if (articleGuidance.Count > 0)
                    {
                        _context.ArticleGuidances.RemoveRange(articleGuidance);
                        _context.SaveChanges();
                    }

                    foreach (var spec in listArticleProduct)
                    {
                        ArticleProduct objcommon = new ArticleProduct();
                        objcommon.BlogArticleId = blogArticle.BlogArticleId;
                        objcommon.IsActive = true;
                        objcommon.CreatedOn = DateTime.Now;
                        objcommon.ProductEntityId = spec.ProductEntityId;

                        _context.Add(objcommon);
                        _context.SaveChanges();
                    }
                    foreach (var spec in listArticleHairStyle)
                    {
                        ArticleHairStyle objcommon = new ArticleHairStyle();
                        objcommon.BlogArticleId = blogArticle.BlogArticleId;
                        objcommon.IsActive = true;
                        objcommon.CreatedOn = DateTime.Now;
                        objcommon.HairStylesId = spec.HairStylesId;

                        _context.Add(objcommon);
                        _context.SaveChanges();
                    }
                    foreach (var spec in listArticleMood)
                    {
                        ArticleMoods objcommon = new ArticleMoods();
                        objcommon.BlogArticleId = blogArticle.BlogArticleId;
                        objcommon.IsActive = true;
                        objcommon.CreatedOn = DateTime.Now;
                        objcommon.MoodId = spec.MoodId;

                        _context.Add(objcommon);
                        _context.SaveChanges();
                    }
                    foreach (var spec in listArticleGuidance)
                    {
                        ArticleGuidances objcommon = new ArticleGuidances();
                        objcommon.BlogArticleId = blogArticle.BlogArticleId;
                        objcommon.IsActive = true;
                        objcommon.CreatedOn = DateTime.Now;
                        objcommon.GuidanceId = spec.GuidanceId;

                        _context.Add(objcommon);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    BlogArticle objBlogArticle = new BlogArticle();
                    objBlogArticle.HeadLine = blogArticle.HeadLine;
                    objBlogArticle.Details = blogArticle.Details;
                    objBlogArticle.ImageUrl = blogArticle.ImageUrl;
                    objBlogArticle.Url = blogArticle.Url;
                    objBlogArticle.IsActive = true;
                    objBlogArticle.CreatedOn = DateTime.UtcNow;
                    //objBlogArticle.Mood = blogArticle.Mood;
                    //objBlogArticle.Guidance = blogArticle.Guidance;
                    _context.BlogArticles.Add(objBlogArticle);
                    _context.SaveChanges();

                    foreach (var spec in listArticleProduct)
                    {
                        ArticleProduct objcommon = new ArticleProduct();
                        objcommon.BlogArticleId = objBlogArticle.BlogArticleId;
                        objcommon.IsActive = true;
                        objcommon.CreatedOn = DateTime.Now;
                        objcommon.ProductEntityId = spec.ProductEntityId;

                        _context.Add(objcommon);
                        _context.SaveChanges();
                    }
                    foreach (var spec in listArticleHairStyle)
                    {
                        ArticleHairStyle objcommon = new ArticleHairStyle();
                        objcommon.BlogArticleId = objBlogArticle.BlogArticleId;
                        objcommon.IsActive = true;
                        objcommon.CreatedOn = DateTime.Now;
                        objcommon.HairStylesId = spec.HairStylesId;

                        _context.Add(objcommon);
                        _context.SaveChanges();
                    }
                    foreach (var spec in listArticleMood)
                    {
                        ArticleMoods objcommon = new ArticleMoods();
                        objcommon.BlogArticleId = objBlogArticle.BlogArticleId;
                        objcommon.IsActive = true;
                        objcommon.CreatedOn = DateTime.Now;
                        objcommon.MoodId = spec.MoodId;

                        _context.Add(objcommon);
                        _context.SaveChanges();
                    }
                    foreach (var spec in listArticleGuidance)
                    {
                        ArticleGuidances objcommon = new ArticleGuidances();
                        objcommon.BlogArticleId = objBlogArticle.BlogArticleId;
                        objcommon.IsActive = true;
                        objcommon.CreatedOn = DateTime.Now;
                        objcommon.GuidanceId = spec.GuidanceId;

                        _context.Add(objcommon);
                        _context.SaveChanges();
                    }



                }
                _context.SaveChanges();
                return blogArticle;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: UploadArticles, Error: " + ex.Message, ex);
                return null;
            }
        }

        public BlogArticlePostModel GetArticleById(BlogArticlePostModel blogArticle)
        {
            try
            {
                BlogArticlePostModel blogArticleModel = _context.BlogArticles.Where(x => x.BlogArticleId == blogArticle.BlogArticleId).Select(
                    x => new BlogArticlePostModel {
                        BlogArticleId = x.BlogArticleId,
                        HeadLine = x.HeadLine,
                        Details = x.Details,
                        ImageUrl = x.ImageUrl,
                        Url = x.Url,
                        IsActive = x.IsActive,
                        ArticleMoodsList= _context.ArticleMoods.Where(p => p.BlogArticleId == blogArticle.BlogArticleId).ToList(),
                        ArticleGuidancesList = _context.ArticleGuidances.Where(p => p.BlogArticleId == blogArticle.BlogArticleId).ToList(),
                        ArticleProductsList = _context.ArticleProducts.Where(p => p.BlogArticleId == blogArticle.BlogArticleId).ToList(),
                        ArticleHairStylesList = _context.ArticleHairStyles.Where(p => p.BlogArticleId == blogArticle.BlogArticleId).ToList()
                    }).FirstOrDefault();
                return blogArticleModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetArticleById, BlogArticleId:"+ blogArticle.BlogArticleId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool DeleteArticle(BlogArticle articleModel)
        {
            try
            {
                var objCode = _context.BlogArticles.FirstOrDefault(x => x.BlogArticleId == articleModel.BlogArticleId);
                {
                    if (objCode != null)
                    {
                        objCode.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteArticle, BlogArticleId:" + articleModel.BlogArticleId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public List<Moods> GetMoods()
        {
            try
            {
                List<Moods> moods = _context.Moods.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();

                return moods;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetMoods, Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<Guidances> GetGuidances()
        {
            try
            {
                List<Guidances> guidances = _context.Guidances.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();

                return guidances;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetGuidances, Error: " + ex.Message, ex);
                return null;
            }
        }
    }
}
