using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using DataTables.AspNetCore.Mvc.Binder;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;
using Newtonsoft.Json;

namespace MyavanaAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookies")]
    public class ProductsController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        private readonly HttpClient _httpClient;
        public ProductsController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
            _httpClient = new HttpClient();
        }

        public IActionResult Products()
        {
            CookieOptions option = new CookieOptions();
            Response.Cookies.Append("isSorted", "1", option);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetProductsList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            bool isNotSorted = true;
            if (Request.Cookies["isSorted"].ToString() == "0")
            {
                isNotSorted = false;
            }
            CookieOptions option = new CookieOptions();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][data]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            if (sortColumn != "ProductName")
            {
                Response.Cookies.Append("isSorted", "0", option);
                isNotSorted = false;
            }
            else if (sortColumn == "ProductName" && sortColumnDirection == "desc")
            {
                Response.Cookies.Append("isSorted", "0", option);
                isNotSorted = false;
            }
            if (isNotSorted)
            {
                sortColumn = "CreatedOn";
                sortColumnDirection = "desc";
            }


            SearchProductResponse gridParams = new SearchProductResponse();
            gridParams.pageSize = pageSize;
            gridParams.skip = skip;
            gridParams.sortColumn = sortColumn;
            gridParams.sortDirection = sortColumnDirection;
            gridParams.searchValue = searchValue;

            var res = await MyavanaAdminApiClientFactory.Instance.GetProducts(gridParams);
            IEnumerable<ProductsEntity> filterProducts = res.Data.Data;


            try
            {
                IEnumerable<ProductsEntity> codes = filterProducts.Select(e => new ProductsEntity
                {
                    guid = e.guid,
                    ProductName = e.ProductName,
                    ActualName = e.ActualName,
                    BrandName = e.BrandName,
                    //TypeFor = e.TypeFor,
                    ImageName = e.ImageName,
                    Ingredients = e.Ingredients,
                    ProductDetails = e.ProductDetails,
                    ProductLink = e.ProductLink,
                    IsActive = e.IsActive,
                    Id = e.Id,
                    ProductTypes = e.ProductTypes == null ? e.ProductType : e.ProductTypes,
                    Price = e.Price,
                    HairChallenges = e.HairChallenges,
                    ProductIndicates = e.ProductIndicates,
                    ProductTags = e.ProductTags,
                    ProductClassifications = e.ProductClassifications,
                    HairType = e.HairType,
                    ProductIndicate = e.ProductIndicate,
                    HairChallenge = e.HairChallenge,
                    HairGoals = e.HairGoals,
                    ProductTag = e.ProductTag,
                    ProductClassificatio = e.ProductClassificatio,
                    BrandClassification = e.BrandClassification,
                    HairTypes = e.HairTypes,
                    UPCCode = e.UPCCode
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, res.Data.RecordsTotal));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public IActionResult GetProducts(AdvanceSearchProduct advanceSearchProduct)
        {
            bool isNotSorted = true;
            if (Request.Cookies["isSorted"].ToString() == "0")
            {
                isNotSorted = false;
            }
            CookieOptions option = new CookieOptions();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][data]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var draw = Request.Form["draw"].FirstOrDefault();

            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            if (sortColumn != "ProductName")
            {
                Response.Cookies.Append("isSorted", "0", option);
                isNotSorted = false;
            }
            else if (sortColumn == "ProductName" && sortColumnDirection == "desc")
            {
                Response.Cookies.Append("isSorted", "0", option);
                isNotSorted = false;
            }
            if (isNotSorted)
            {
                sortColumn = "CreatedOn";
                sortColumnDirection = "desc";
            }

            SearchProductResponse gridParams = new SearchProductResponse();
            gridParams.pageSize = pageSize;
            gridParams.skip = skip;
            gridParams.sortColumn = sortColumn;
            gridParams.sortDirection = sortColumnDirection;
            gridParams.searchValue = searchValue;
            gridParams.advanceSearchProduct = advanceSearchProduct;

            var res = MyavanaAdminApiClientFactory.Instance.GetProducts(gridParams).GetAwaiter().GetResult();
            IEnumerable<ProductsEntity> filterProducts = res.Data.Data;


            try
            {
                IEnumerable<ProductsEntity> codes = filterProducts.Select(e => new ProductsEntity
                {
                    guid = e.guid,
                    ProductName = e.ProductName,
                    ActualName = e.ActualName,
                    BrandName = e.BrandName,
                    //TypeFor = e.TypeFor,
                    ImageName = e.ImageName,
                    Ingredients = e.Ingredients,
                    ProductDetails = e.ProductDetails,
                    ProductLink = e.ProductLink,
                    IsActive = e.IsActive,
                    Id = e.Id,
                    ProductTypes = e.ProductTypes == null ? e.ProductType : e.ProductTypes,
                    Price = e.Price,
                    HairChallenges = e.HairChallenges,
                    ProductIndicates = e.ProductIndicates,
                    ProductTags = e.ProductTags,
                    ProductClassifications = e.ProductClassifications,
                    HairType = e.HairType,
                    ProductIndicate = e.ProductIndicate,
                    HairChallenge = e.HairChallenge,
                    ProductTag = e.ProductTag,
                    ProductClassificatio = e.ProductClassificatio,
                    HairTypes = e.HairTypes,
                    CreatedOn = e.CreatedOn,
                    HairGoalsDes = e.HairGoalsDes,
                    CustomerPreferences = e.CustomerPreferences,
                    CustomerPreference= e.CustomerPreference,
                    ProductRecommendationStatus = e.ProductRecommendationStatus,
                    MolecularWeight = e.MolecularWeight,
                    ProductRecommendationStatuses = e.ProductRecommendationStatuses,
                    MolecularWeights = e.MolecularWeights,
                    HideInSearch = e.HideInSearch,
                    HairStyles=e.HairStyles,
                    UPCCode = e.UPCCode
                }).OrderByDescending(x => x.CreatedOn);

                var jsonData = new { draw = draw, recordsFiltered = res.Data.RecordsTotal, recordsTotal = res.Data.RecordsTotal, data = codes };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public IActionResult ExportToCsv(AdvanceSearchProduct advanceSearchProduct)
        {
           // AdvanceSearchProduct search = new AdvanceSearchProduct();
            SearchProductResponse gridParams = new SearchProductResponse();
            gridParams.pageSize = 10000;
            gridParams.skip = 0;
            gridParams.sortColumn = "CreatedOn";
            gridParams.sortDirection = "desc";
            gridParams.searchValue = "";
            gridParams.advanceSearchProduct = advanceSearchProduct;
            var res =MyavanaAdminApiClientFactory.Instance.GetProducts(gridParams).GetAwaiter().GetResult();
            IEnumerable<ProductsEntity> filterProducts = res.Data.Data;
           

            IEnumerable<ProductModelDownload> downloadData = filterProducts.Select(Prod => new ProductModelDownload
            {
                ProductName= Prod.ProductName,
                ActualName=Prod.ActualName,
                BrandName = Prod.BrandName,
                ProductType=Prod.ProductTypes,
                HairTypes=Prod.HairTypes,
                Ingredients = Prod.Ingredients,
                Price = Prod.Price

            }).ToList();

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)))
            {

                csv.WriteRecords(downloadData);
                writer.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);
                byte[] bytes = memoryStream.ToArray();
                return File(bytes, "text/csv", "Products.csv");

                //writer.Flush();
                //memoryStream.Seek(0, SeekOrigin.Begin);

                //// Return the file as a downloadable attachment
                //return File(memoryStream, "text/csv", "Products.csv");
            }
        }
        
        public async Task<IActionResult> CreateProduct(string id)
        {
            if (id != null)
            {
                ProductsEntity productsModel = new ProductsEntity();
                productsModel.guid = Guid.Parse(id);
                // var response = await MyavanaAdminApiClientFactory.Instance.GetProductById(productsModel);
                //productsModel = response.Data;
                return View(productsModel);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductAdmin(ProductEntityEditModel productEntityEditModel)
        {
            if (productEntityEditModel != null)
            {
                var response = await MyavanaAdminApiClientFactory.Instance.GetProductById(productEntityEditModel);
                if (response != null)
                {
                    return Json(response.Data);
                }
                return Content("0");
            }
            return Content("0");
        }


        //[HttpPost]
        //public async Task<IActionResult> CreateProduct(ProductsEntity productsEntity, List<IFormFile> Files)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            try
        //            {
        //                client.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
        //                MultipartFormDataContent multiContent = new MultipartFormDataContent();
        //                PropertyInfo[] productProperties = typeof(ProductsEntity).GetProperties();

        //                foreach (PropertyInfo prop in productProperties)
        //                {
        //                    if (prop.Name != "File")
        //                    {
        //                        if (prop.GetValue(productsEntity) != null)
        //                        {
        //                            multiContent.Add(new StringContent(prop.GetValue(productsEntity).ToString()), prop.Name);
        //                        }
        //                        else
        //                        {
        //                            multiContent.Add(new StringContent(string.Empty), prop.Name);
        //                        }
        //                    }
        //                }

        //                foreach (var file in Files)
        //                {
        //                    multiContent.Add(new StreamContent(file.OpenReadStream())
        //                    {
        //                        Headers =
        //                    {
        //                       ContentLength = file.Length,
        //                       ContentType = new MediaTypeHeaderValue(file.ContentType)
        //                    }
        //                    }, "Files", file.FileName);
        //                }


        //                var result = client.PostAsync("Products/SaveProducts", multiContent).Result;
        //            }
        //            catch (Exception ex)
        //            {
        //                return Content("0");
        //            }
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception)
        //    {
        //        return Content("0");
        //    }

        //}

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductsEntity productsEntity, IFormFile File)
        {
            if (File != null)
            {
                try
                {
                    Random generator = new Random();
                    String random = generator.Next(0, 1000000).ToString("D6");
                    string imgExt = Path.GetExtension(File.FileName);
                    string fileName = File.FileName.Substring(0, File.FileName.IndexOf(".")) + "_" + random + imgExt;

                    //string fileName = File.FileName;
                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "product")))
                    {
                        Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "product"));
                    }

                    //var dir = Directory.GetCurrentDirectory();
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "product", fileName);


                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await File.CopyToAsync(stream);
                        productsEntity.ImageName = "http://admin.myavana.com/product/" + fileName;
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            var response = await MyavanaAdminApiClientFactory.Instance.SaveProducts(productsEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductsEntity productsEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteProduct(productsEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        [HttpPost]
        public async Task<IActionResult> ShowHideProduct(ProductsEntity productsEntity)
        {

            var response = await MyavanaAdminApiClientFactory.Instance.ShowHideProduct(productsEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }
        public IActionResult ProductsCategory()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsCategoryList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<ProductTypeCategoriesList> filterProductType = await MyavanaAdminApiClientFactory.Instance.GetProductsCategoryList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<ProductTypeCategoriesList, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.CategoryName);
                            filterProductType =
                                sortDirection == "asc"
                                    ? filterProductType.OrderBy(orderingFunctionString)
                                    : filterProductType.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<ProductTypeCategoriesList> codes = filterProductType.Select(e => new ProductTypeCategoriesList
                {
                    ProductTypeId = e.ProductTypeId,
                    IsHair = e.IsHair,
                    IsRegimen = e.IsRegimen,
                    CategoryName = e.CategoryName
                }); //.OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IActionResult ProductsType()
        {
            return View();
        }
        public IActionResult ProductsTag()
        {
            return View();
        }
        public IActionResult HairGoal()
        {
            return View();
        }
        public IActionResult HairStyle()
        {
            return View();
        }
        public IActionResult HairChallenge()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsTypeList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<ProductTypeEntity> filterProductType = await MyavanaAdminApiClientFactory.Instance.GetProductType();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<ProductTypeEntity, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.ProductName);
                            filterProductType =
                                sortDirection == "asc"
                                    ? filterProductType.OrderBy(orderingFunctionString)
                                    : filterProductType.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<ProductTypeEntity> codes = filterProductType.Select(e => new ProductTypeEntity
                {
                    Id = e.Id,
                    ProductName = e.ProductName,
                    CreatedOn = e.CreatedOn,
                    IsActive = e.IsActive,
                    CategoryName = e.CategoryName,
                    Rank = e.Rank
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsTagList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<ProductTagsModel> filterProductType = await MyavanaAdminApiClientFactory.Instance.GetProductTagList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<ProductTagsModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterProductType =
                                sortDirection == "asc"
                                    ? filterProductType.OrderBy(orderingFunctionString)
                                    : filterProductType.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<ProductTagsModel> codes = filterProductType.Select(e => new ProductTagsModel
                {
                    ProductTagsId = e.ProductTagsId,
                    Description = e.Description,
                    CreatedOn = e.CreatedOn,
                    IsActive = e.IsActive
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetHairStylesList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<HairStyles> filterProductType = await MyavanaAdminApiClientFactory.Instance.GetHairStyles();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<HairStyles, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Style);
                            filterProductType =
                                sortDirection == "asc"
                                    ? filterProductType.OrderBy(orderingFunctionString)
                                    : filterProductType.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<HairStyles> codes = filterProductType.Select(e => new HairStyles
                {
                    Id = e.Id,
                    Style = e.Style,
                    CreatedOn = e.CreatedOn,
                    IsActive = e.IsActive
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetHairGoalsList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<HairGoalsModel> filterProductType = await MyavanaAdminApiClientFactory.Instance.GetHairGoalsList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<HairGoalsModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterProductType =
                                sortDirection == "asc"
                                    ? filterProductType.OrderBy(orderingFunctionString)
                                    : filterProductType.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<HairGoalsModel> codes = filterProductType.Select(e => new HairGoalsModel
                {
                    HairGoalId = e.HairGoalId,
                    Description = e.Description,
                    CreatedOn = e.CreatedOn,
                    IsActive = e.IsActive
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetHairChallengesList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<HairChallengesModel> filterProductType = await MyavanaAdminApiClientFactory.Instance.GetHairChallengesList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<HairChallengesModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterProductType =
                                sortDirection == "asc"
                                    ? filterProductType.OrderBy(orderingFunctionString)
                                    : filterProductType.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<HairChallengesModel> codes = filterProductType.Select(e => new HairChallengesModel
                {
                    HairChallengeId = e.HairChallengeId,
                    Description = e.Description,
                    CreatedOn = e.CreatedOn,
                    IsActive = e.IsActive
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IActionResult> CreateProductCategory(string id)
        {
            if (id != null)
            {
                ProductTypeCategoriesList productTypeEntity = new ProductTypeCategoriesList();
                productTypeEntity.ProductTypeId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetProductCategoryById(productTypeEntity);
                productTypeEntity = response.Data;
                ProductTypeCategoryModel productTypeEntityModel = new ProductTypeCategoryModel();
                productTypeEntityModel.CategoryId = productTypeEntity.ProductTypeId;
                productTypeEntityModel.ProductName = productTypeEntity.CategoryName;
                productTypeEntityModel.IsHair = productTypeEntity.IsHair;
                productTypeEntityModel.IsRegimens = productTypeEntity.IsRegimen;
                productTypeEntityModel.Id = productTypeEntity.ProductTypeId;
                return View(productTypeEntityModel);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductCategory(ProductTypeCategoriesList productTypeEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveProductCategory(productTypeEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductType(ProductTypeEntity productTypeEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteProductType(productTypeEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductTag(ProductTagsModel productTypeEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteProductTag(productTypeEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHairGoal(HairGoalsModel hairGoalEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteHairGoal(hairGoalEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteHairStyle(HairStyles hairStylesEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteHairStyle(hairStylesEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteHairChallenge(HairChallengesModel hairChallengeEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteHairChallenge(hairChallengeEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        public async Task<IActionResult> CreateProductType(string id)
        {
            if (id != null)
            {
                ProductTypeEntity productTypeEntity = new ProductTypeEntity();
                productTypeEntity.Id = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetProductTypeById(productTypeEntity);
                productTypeEntity = response.Data;
                return View(productTypeEntity);

            }
            return View();
        }

        public async Task<IActionResult> CreateProductTag(string id)
        {
            if (id != null)
            {
                ProductTagsModel productTagEntity = new ProductTagsModel();
                productTagEntity.ProductTagsId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetProductTagById(productTagEntity);
                productTagEntity = response.Data;
                return View(productTagEntity);

            }
            return View();
        }
        public async Task<IActionResult> CreateHairGoal(string id)
        {
            if (id != null)
            {
                HairGoalsModel hairGoalEntity = new HairGoalsModel();
                hairGoalEntity.HairGoalId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetHairGoalById(hairGoalEntity);
                hairGoalEntity = response.Data;
                return View(hairGoalEntity);

            }
            return View();
        }
        public async Task<IActionResult> CreateHairStyle(string id)
        {
            if (id != null)
            {
                HairStyles hairStylesEntity = new HairStyles();
                hairStylesEntity.Id = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetHairStyleById(hairStylesEntity);
                hairStylesEntity = response.Data;
                return View(hairStylesEntity);

            }
            return View();
        }
        public async Task<IActionResult> CreateHairChallenge(string id)
        {
            if (id != null)
            {
                HairChallengesModel hairGoalEntity = new HairChallengesModel();
                hairGoalEntity.HairChallengeId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetHairChallengeById(hairGoalEntity);
                hairGoalEntity = response.Data;
                return View(hairGoalEntity);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductType(ProductTypeCategoryModel productTypeEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveProductType(productTypeEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductTag(ProductTagsModel productTypeEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveProductTag(productTypeEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> CreateHairGoal(HairGoalsModel hairGoalsEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveHairGoal(hairGoalsEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        [HttpPost]
        public async Task<IActionResult> CreateHairStyle(HairStyles hairStylesEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveHairStyle(hairStylesEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        [HttpPost]
        public async Task<IActionResult> CreateHairChallenge(HairChallengesModel hairChallengesEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveHairChallenge(hairChallengesEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        public IActionResult ProductIndicators()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProductIndicatorsList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<IndicatorModel> filterProductType = await MyavanaAdminApiClientFactory.Instance.GetIndicators();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<IndicatorModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterProductType =
                                sortDirection == "asc"
                                    ? filterProductType.OrderBy(orderingFunctionString)
                                    : filterProductType.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 1:
                        {
                            orderingFunctionString = (c => c.CreatedDate);
                            filterProductType =
                                sortDirection == "asc"
                                    ? filterProductType.OrderBy(orderingFunctionString)
                                    : filterProductType.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<IndicatorModel> codes = filterProductType.Select(e => new IndicatorModel
                {
                    ProductIndicatorId = e.ProductIndicatorId,
                    Description = e.Description,
                    CreatedDate = e.CreatedOn.ToString("yyyy-MM-dd"),//(DateTime.ParseExact(e.CreatedOn.ToString(), "dd-MM-yyyy hh:mm:ss",
                                                                     // CultureInfo.InvariantCulture)).ToString("yyyy-MM-dd"),
                    IsActive = e.IsActive
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIndicator(IndicatorModel indicatorModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteProductIndicator(indicatorModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        public async Task<IActionResult> CreateIndicator(string id)
        {
            if (id != null)
            {
                IndicatorModel indicator = new IndicatorModel();
                indicator.ProductIndicatorId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetProductIndicatorById(indicator);
                indicator = response.Data;
                return View(indicator);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIndicator(IndicatorModel indicatorModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveIndicator(indicatorModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProductCategory(ProductTypeEntity productTypeEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteProductCategory(productTypeEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelsheet(ProductsData fileModel)
        {
            string fileName = fileModel.file.FileName.Replace(" ", "").Trim();
            string fileType = null;
            FileStream stream = null;
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "ProductsDocs")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "ProductsDocs"));
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductsDocs", fileName);
            using (stream = new FileStream(path, FileMode.Create))
            {
                await fileModel.file.CopyToAsync(stream);
                fileType = Path.GetExtension(path);
            }

            var productModel = new List<ProductsEntity>();
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (FileStream mStream = System.IO.File.Open("wwwroot/ProductsDocs/" + fileName, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        IExcelDataReader reader = null;

                        if (fileType == ".csv") reader = ExcelReaderFactory.CreateCsvReader(mStream);
                        else reader = ExcelReaderFactory.CreateReader(mStream);
                    }
                    catch (Exception ex) { return Content("Your excel file version is old. Please save it in greater than 2007 version"); }
                    using (IExcelDataReader reader = fileType == ".csv" ? ExcelReaderFactory.CreateCsvReader(mStream) : ExcelReaderFactory.CreateReader(mStream))
                    {
                        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true // To set First Row As Column Names  
                            }
                        });

                        if (dataSet.Tables.Count > 0)
                        {
                            var dataTable = dataSet.Tables[0];
                            int i = 2;
                            foreach (DataRow objDataRow in dataTable.Rows)
                            {
                                if (objDataRow.ItemArray.All(x => string.IsNullOrEmpty(x?.ToString()))) continue;

                                ProductsEntity productEntity = new ProductsEntity();

                                if (objDataRow["ProductName"] == null || objDataRow["ProductName"] is System.DBNull)
                                    return Content("ProductName is Empty at Row no : " + i);
                                else
                                    productEntity.ProductName = objDataRow["ProductName"].ToString();

                                if (objDataRow["ActualName"] == null || objDataRow["ActualName"] is System.DBNull)
                                    return Content("ActualName is Empty at Row no : " + i);
                                else
                                    productEntity.ActualName = objDataRow["ActualName"].ToString();

                                if (objDataRow["BrandName"] == null || objDataRow["BrandName"] is System.DBNull)
                                    return Content("BrandName is Empty at Row no : " + i);
                                else
                                    productEntity.BrandName = objDataRow["BrandName"].ToString();

                                if (objDataRow["ProductClassification"] == null || objDataRow["ProductClassification"] is System.DBNull)
                                    return Content("ProductClassification is Empty at Row no : " + i);
                                else
                                    productEntity.ProductClassification = objDataRow["ProductClassification"].ToString();

                                if (objDataRow["ProductType"] == null || objDataRow["ProductType"] is System.DBNull)
                                    return Content("ProductType is Empty at Row no : " + i);
                                else
                                    productEntity.ProductType = objDataRow["ProductType"].ToString();

                                productEntity.ProductIndicator = objDataRow["ProductIndicator"] is System.DBNull ? null : objDataRow["ProductIndicator"].ToString();

                                productEntity.HairChallenges = objDataRow["HairChallenges"] is System.DBNull ? null : objDataRow["HairChallenges"].ToString();

                                productEntity.ProductTags = objDataRow["ProductTags"] is System.DBNull ? null : objDataRow["ProductTags"].ToString();

                                if (objDataRow["TypeFor"] == null || objDataRow["TypeFor"] is System.DBNull)
                                    return Content("TypeFor is Empty at Row no : " + i);
                                else
                                    productEntity.TypeFor = objDataRow["TypeFor"].ToString();

                                productEntity.ImageName = objDataRow["ImageName"] is System.DBNull ? null : objDataRow["ImageName"].ToString();

                                productEntity.Ingredients = objDataRow["Ingredients"] is System.DBNull ? null : objDataRow["Ingredients"].ToString();

                                productEntity.ProductDetails = objDataRow["ProductDetails"] is System.DBNull ? null : objDataRow["ProductDetails"].ToString();

                                productEntity.ProductLink = objDataRow["ProductLink"] is System.DBNull ? null : objDataRow["ProductLink"].ToString();

                                productEntity.UPCCode = objDataRow["UPCCode"] is System.DBNull ? null : objDataRow["UPCCode"].ToString();
                                try
                                {
                                    productEntity.Price = Convert.ToDecimal(objDataRow["Price"] is System.DBNull ? 0.00 : objDataRow["Price"]);
                                }
                                catch (Exception ex) { return Content("Please mention price in decimal format(0.00)"); }
                                productModel.Add(productEntity);
                                i++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);

                var response = await MyavanaAdminApiClientFactory.Instance.AddProductList(productModel);
                if (response != null)
                    return Content("1");
            }
            return Content("0");

        }

        public IActionResult CustomerPreference()
        {
            return View();
        }

        #region Product Classification
        public IActionResult ProductClassification()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProductClassificationList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<ProductClassificationModel> filterProductType = await MyavanaAdminApiClientFactory.Instance.GetProductClassificationList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<ProductClassificationModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterProductType =
                                sortDirection == "asc"
                                    ? filterProductType.OrderBy(orderingFunctionString)
                                    : filterProductType.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<ProductClassificationModel> codes = filterProductType.Select(e => new ProductClassificationModel
                {
                    ProductClassificationId = e.ProductClassificationId,
                    Description = e.Description,
                    CreatedOn = e.CreatedOn,
                    IsActive = e.IsActive
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        
        [HttpGet]
        public async Task<IActionResult> GetCustomerPreferenceList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<CustomerPreference> filterProductType = await MyavanaAdminApiClientFactory.Instance.GetCustomerPreferenceList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<CustomerPreference, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterProductType =
                                sortDirection == "asc"
                                    ? filterProductType.OrderBy(orderingFunctionString)
                                    : filterProductType.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<CustomerPreference> codes = filterProductType.Select(e => new CustomerPreference
                {
                    CustomerPreferenceId = e.CustomerPreferenceId,
                    Description = e.Description,
                    CreatedOn = e.CreatedOn,
                    IsActive = e.IsActive
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IActionResult> CreateCustomerPreference(string id)
        {
            if (id != null)
            {
                CustomerPreference productClassificationModel = new CustomerPreference();
                productClassificationModel.CustomerPreferenceId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetCustomerPreferenceById(productClassificationModel);
                productClassificationModel = response.Data;
                return View(productClassificationModel);

            }
            return View();
        }

        public async Task<IActionResult> CreateProductClassification(string id)
        {
            if (id != null)
            {
                ProductClassificationModel productClassificationModel = new ProductClassificationModel();
                productClassificationModel.ProductClassificationId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetProductClassificationById(productClassificationModel);
                productClassificationModel = response.Data;
                return View(productClassificationModel);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductClassification(ProductClassificationModel productClassificationModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveProductClassification(productClassificationModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerPreference(CustomerPreference customerPreferenceModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveCustomerPreference(customerPreferenceModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductClassification(ProductClassificationModel productClassificationModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteProductClassification(productClassificationModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomerPreference(CustomerPreference productClassificationModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteCustomerPreference(productClassificationModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        #endregion

        #region Brand Classification
        public IActionResult BrandClassification()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetBrandClassificationList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<BrandClassificationModel> filterProductType = await MyavanaAdminApiClientFactory.Instance.GetBrandClassificationList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<BrandClassificationModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterProductType =
                                sortDirection == "asc"
                                    ? filterProductType.OrderBy(orderingFunctionString)
                                    : filterProductType.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<BrandClassificationModel> codes = filterProductType.Select(e => new BrandClassificationModel
                {
                    BrandClassificationId = e.BrandClassificationId,
                    Description = e.Description,
                    CreatedOn = e.CreatedOn,
                    IsActive = e.IsActive
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IActionResult> CreateBrandClassification(string id)
        {
            if (id != null)
            {
                BrandClassificationModel brandClassificationModel = new BrandClassificationModel();
                brandClassificationModel.BrandClassificationId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetBrandClassificationById(brandClassificationModel);
                brandClassificationModel = response.Data;
                return View(brandClassificationModel);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrandClassification(BrandClassificationModel brandClassificationModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveBrandClassification(brandClassificationModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBrandClassification(BrandClassificationModel brandClassificationModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteBrandClassification(brandClassificationModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        #endregion

        private async Task<ByteArrayContent> AddProductImage(IFormFile formFile)
        {
            string fileName = formFile.FileName;
            //if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Regimens")))
            //{
            //    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Regimens"));
            //}
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Regimens", fileName);
            //using (var stream = new FileStream(path, FileMode.Create))
            //{
            //    await formFile.CopyToAsync(stream);
            //}

            byte[] dataStep1Photo;
            using (var br = new BinaryReader((formFile).OpenReadStream()))
                dataStep1Photo = br.ReadBytes((int)(formFile).OpenReadStream().Length);
            ByteArrayContent bytesStep1Photo = new ByteArrayContent(dataStep1Photo);
            return bytesStep1Photo;

        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(ProductImage productImage)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteProductImage(productImage);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        [HttpPost]
        public async Task<IActionResult> UploadProductImages(List<IFormFile> Files)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
                        MultipartFormDataContent multiContent = new MultipartFormDataContent();
                        PropertyInfo[] productProperties = typeof(ProductsEntity).GetProperties();
                        multiContent.Add(new StringContent("Test"), "name");
                        foreach (var file in Files)
                        {
                            multiContent.Add(new StreamContent(file.OpenReadStream())
                            {
                                Headers =
                            {
                               ContentLength = file.Length,
                               ContentType = new MediaTypeHeaderValue(file.ContentType)
                            }
                            }, "Files", file.FileName);
                        }


                        var result = await client.PostAsync("Products/UploadDocumentToS3", multiContent);
                        if (result.IsSuccessStatusCode != true)
                        {
                            return Content("0");
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content("0");
                    }
                }
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }

        }
        public IActionResult ProductRecommendationStatus()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProductRecommendationStatusList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<ProductRecommendationStatusModel> filterProductType = await MyavanaAdminApiClientFactory.Instance.GetProductRecommendationStatusList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<ProductRecommendationStatusModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterProductType =
                                sortDirection == "asc"
                                    ? filterProductType.OrderBy(orderingFunctionString)
                                    : filterProductType.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<ProductRecommendationStatusModel> codes = filterProductType.Select(e => new ProductRecommendationStatusModel
                {
                    ProductRecommendationStatusId = e.ProductRecommendationStatusId,
                    Description = e.Description,
                    CreatedOn = e.CreatedOn,
                    IsActive = e.IsActive
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IActionResult> CreateProductRecommendationStatus(string id)
        {
            if (id != null)
            {
                ProductRecommendationStatusModel recommendationStatusEntity = new ProductRecommendationStatusModel();
                recommendationStatusEntity.ProductRecommendationStatusId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetProductRecommendationStatusById(recommendationStatusEntity);
                recommendationStatusEntity = response.Data;
                return View(recommendationStatusEntity);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductRecommendationStatus(ProductRecommendationStatusModel recommendationStatusEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveProductRecommendationStatus(recommendationStatusEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductRecommendationStatus(ProductRecommendationStatusModel recommendationStatusEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteProductRecommendationStatus(recommendationStatusEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

    }
}
