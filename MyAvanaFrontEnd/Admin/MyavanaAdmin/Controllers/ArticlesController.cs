using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;

namespace MyavanaAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookies")]
    public class ArticlesController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public ArticlesController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
        }

        public async Task<IActionResult> CreateArticles(string id)
        {
            if (id != null)
            {
                BlogArticlePostModel blogPostModel = new BlogArticlePostModel();
                blogPostModel.BlogArticleId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetArticleById(blogPostModel);
                blogPostModel = response.Data;
                return View(blogPostModel);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticles(BlogArticlePostModel blogPostModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveBlogPost(blogPostModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        public IActionResult ViewArticles()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<BlogPostModel> filteredProducts = await MyavanaAdminApiClientFactory.Instance.GetArticles();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<BlogPostModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Headline);
                            filteredProducts =
                                sortDirection == "asc"
                                    ? filteredProducts.OrderBy(orderingFunctionString)
                                    : filteredProducts.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 1:
                        {
                            orderingFunctionString = (c => c.Details);
                            filteredProducts =
                                sortDirection == "asc"
                                    ? filteredProducts.OrderBy(orderingFunctionString)
                                    : filteredProducts.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 2:
                        {
                            orderingFunctionString = (c => c.ImageUrl);
                            filteredProducts =
                                sortDirection == "asc"
                                    ? filteredProducts.OrderBy(orderingFunctionString)
                                    : filteredProducts.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 3:
                        {
                            orderingFunctionString = (c => c.Url);
                            filteredProducts =
                                sortDirection == "asc"
                                    ? filteredProducts.OrderBy(orderingFunctionString)
                                    : filteredProducts.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }

            try
            {
                IEnumerable<BlogPostModel> blogs = filteredProducts.Select(e => new BlogPostModel
                {
                    Headline = e.Headline,
                    Details = e.Details,
                    ImageUrl = e.ImageUrl,
                    Url = e.Url,
                    BlogArticleId = e.BlogArticleId,
                    IsActive = e.IsActive,
                    ArticleHairStyles = e.ArticleHairStyles,
                    ArticleProducts = e.ArticleProducts,
                    ArticleMoods=e.ArticleMoods,
                    ArticleGuidances=e.ArticleGuidances

                }).OrderByDescending(x => x.CreatedOn);
                return Json(blogs.ToDataTablesResponse(dataRequest, blogs.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteArticle(BlogPostModel blogPostModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteArticle(blogPostModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }
    }
}
