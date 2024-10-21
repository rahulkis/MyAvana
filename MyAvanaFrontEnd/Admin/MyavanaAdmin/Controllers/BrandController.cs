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
    public class BrandController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public BrandController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
        }
        public async Task<IActionResult> CreateTag(string id)
        {
            if (id != null)
            {
                TagsModel TagEntity = new TagsModel();
                TagEntity.TagId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetTagById(TagEntity);
                TagEntity = response.Data;
                return View(TagEntity);

            }
            return View();
        }
        public IActionResult Tags()
        {
            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> CreateTag(TagsModel TagEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveTag(TagEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTag(TagsModel TagEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteTag(TagEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpGet]
        public async Task<IActionResult> GetBrandTagList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<TagsModel> filterTag = await MyavanaAdminApiClientFactory.Instance.GetBrandTagList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<TagsModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterTag =
                                sortDirection == "asc"
                                    ? filterTag.OrderBy(orderingFunctionString)
                                    : filterTag.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<TagsModel> codes = filterTag.Select(e => new TagsModel
                {
                    TagId = e.TagId,
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

        public async Task<IActionResult> CreateBrand(string id)
        {
            if (id != null)
            {
                BrandsEntityModel brandsModel = new BrandsEntityModel();
                brandsModel.BrandId = Convert.ToInt32(id);
                return View(brandsModel);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand(Brands productsEntity)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        var response = await MyavanaAdminApiClientFactory.Instance.SaveBrand(productsEntity);
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

        [HttpPost]
        public async Task<IActionResult> SaveBrand(Brands brandsEntity)
        {

            var response = await MyavanaAdminApiClientFactory.Instance.SaveBrand(brandsEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        public IActionResult Brands()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetBrandsList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            //bool isNotSorted = true;
            //if (Request.Cookies["isSorted"].ToString() == "0")
            //{
            //    isNotSorted = false;
            //}
            //CookieOptions option = new CookieOptions();
            //int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            //int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][data]"].FirstOrDefault();
            //var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            //var searchValue = Request.Form["search[value]"].FirstOrDefault();
            //if (sortColumn != "ProductName")
            //{
            //    Response.Cookies.Append("isSorted", "0", option);
            //    isNotSorted = false;
            //}
            //else if (sortColumn == "ProductName" && sortColumnDirection == "desc")
            //{
            //    Response.Cookies.Append("isSorted", "0", option);
            //    isNotSorted = false;
            //}
            //if (isNotSorted)
            //{
            //    sortColumn = "CreatedOn";
            //    sortColumnDirection = "desc";
            //}


            //SearchProductResponse gridParams = new SearchProductResponse();
            //gridParams.pageSize = pageSize;
            //gridParams.skip = skip;
            //gridParams.sortColumn = sortColumn;
            //gridParams.sortDirection = sortColumnDirection;
            //gridParams.searchValue = searchValue;

            var res = await MyavanaAdminApiClientFactory.Instance.GetBrandsList();
            IEnumerable<BrandModelList> filterProducts = res.ToList();


            try
            {
                IEnumerable<BrandModelList> codes = filterProducts.Select(e => new BrandModelList
                {
                    BrandName = e.BrandName,
                    HairChallenge = e.HairChallenge,
                    HairGoalsDes = e.HairGoalsDes,
                    HairTypes = e.HairTypes,
                    Tags = e.Tags,
                    Tag = e.Tag,
                    HairChallenges = e.HairChallenges,
                    HairType = e.HairType,
                    HairGoals = e.HairGoals,
                    BrandClassifications = e.BrandClassifications,
                    HairState = e.HairState,
                    BrandRecommendationStatus = e.BrandRecommendationStatus,
                    MolecularWeight = e.MolecularWeight,
                    HairStates = e.HairStates,
                    RecommendationStatuses = e.RecommendationStatuses,
                    MolecularWeights = e.MolecularWeights,
                    FeaturedIngredients = e.FeaturedIngredients,
                    Rank = e.Rank,
                    BrandId = e.BrandId,
                    HideInSearch=e.HideInSearch
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, res.Count));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public IActionResult GetBrands(AdvanceSearchProduct advanceSearchProduct)
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
                    HairGoalsDes = e.HairGoalsDes
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
        public async Task<IActionResult> GetBrandDetails(BrandsEntityModel brandsEntity)
        {
            if (brandsEntity != null)
            {
                var response = await MyavanaAdminApiClientFactory.Instance.GetBrandDetailsById(brandsEntity);
                if (response != null)
                {
                    return Json(response.Data);
                }
                return Content("0");
            }
            return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBrand(BrandsEntityModel brandsEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteBrand(brandsEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        [HttpPost]
        public async Task<IActionResult> ShowHideBrand(Brands brandsEntity)
        {
           
            var response = await MyavanaAdminApiClientFactory.Instance.ShowHideBrand(brandsEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }

        public async Task<IActionResult> ExportToCsv()
        {
            var res = await MyavanaAdminApiClientFactory.Instance.GetBrandsList();
           
            IEnumerable<BrandModelList> brandData =res.ToList();
            IEnumerable<BrandModelDownload> downloadData = brandData.Select(brand => new BrandModelDownload
            {
                
                BrandName = brand.BrandName
   
            }).ToList();
            
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)))
            {
  
                csv.WriteRecords(downloadData);
                writer.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);
                byte[] bytes = memoryStream.ToArray();
                return File(bytes, "text/csv", "brands.csv");
            }
        }

        public IActionResult BrandHairState()
        {
            return View();
        }
        public IActionResult BrandRecommendationStatus()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetBrandHairStateList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<HairStateModel> filterTag = await MyavanaAdminApiClientFactory.Instance.GetBrandHairStateList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<HairStateModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterTag =
                                sortDirection == "asc"
                                    ? filterTag.OrderBy(orderingFunctionString)
                                    : filterTag.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<HairStateModel> codes = filterTag.Select(e => new HairStateModel
                {
                    HairStateId = e.HairStateId,
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
        public async Task<IActionResult> CreateHairState(string id)
        {
            if (id != null)
            {
                HairStateModel HairstateEntity = new HairStateModel();
                HairstateEntity.HairStateId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetHairStateById(HairstateEntity);
                HairstateEntity = response.Data;
                return View(HairstateEntity);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHairState(HairStateModel HairstateEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveHairState(HairstateEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHairState(HairStateModel HairstateEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteHairState(HairstateEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpGet]
        public async Task<IActionResult> GetBrandRecommendationStatusList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<BrandRecommendationStatusModel> filterStatus = await MyavanaAdminApiClientFactory.Instance.GetBrandRecommendationStatusList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<BrandRecommendationStatusModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterStatus =
                                sortDirection == "asc"
                                    ? filterStatus.OrderBy(orderingFunctionString)
                                    : filterStatus.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<BrandRecommendationStatusModel> codes = filterStatus.Select(e => new BrandRecommendationStatusModel
                {
                    BrandRecommendationStatusId = e.BrandRecommendationStatusId,
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
        public async Task<IActionResult> CreateBrandRecommendationStatus(string id)
        {
            if (id != null)
            {
                BrandRecommendationStatusModel brandRecommendationEntity = new BrandRecommendationStatusModel();
                brandRecommendationEntity.BrandRecommendationStatusId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetBrandRecommmendationStatusById(brandRecommendationEntity);
                brandRecommendationEntity = response.Data;
                return View(brandRecommendationEntity);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrandRecommendationStatus(BrandRecommendationStatusModel brandRecommendationEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveBrandRecommendationStatus(brandRecommendationEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBrandRecommendationStatus(BrandRecommendationStatusModel brandRecommendationEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteBrandRecommendationStatus(brandRecommendationEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        public IActionResult MolecularWeight()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetBrandMolecularWeightList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<MolecularWeightModel> filterStatus = await MyavanaAdminApiClientFactory.Instance.GetBrandMolecularWeightList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<MolecularWeightModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterStatus =
                                sortDirection == "asc"
                                    ? filterStatus.OrderBy(orderingFunctionString)
                                    : filterStatus.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<MolecularWeightModel> codes = filterStatus.Select(e => new MolecularWeightModel
                {
                    MolecularWeightId = e.MolecularWeightId,
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
        public async Task<IActionResult> CreateMolecularWeight(string id)
        {
            if (id != null)
            {
                MolecularWeightModel molecularWeightEntity = new MolecularWeightModel();
                molecularWeightEntity.MolecularWeightId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetMolecularWeightById(molecularWeightEntity);
                molecularWeightEntity = response.Data;
                return View(molecularWeightEntity);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMolecularWeight(MolecularWeightModel molecularWeightEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveMolecularWeight(molecularWeightEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMolecularWeight(MolecularWeightModel molecularWeightEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteMolecularWeight(molecularWeightEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
    }
}
