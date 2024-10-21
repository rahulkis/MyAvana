using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productService;
        private readonly IBaseBusiness _baseBusiness;
        private readonly IHostingEnvironment _environment;
        private readonly HttpClient _httpClient;
        private readonly AvanaContext _context;
        private readonly IAws3Services _aws3Services;
        private readonly IConfiguration _configuration;
        private readonly string _adminUrl;
        public ProductsController(IProductsService productService, IBaseBusiness baseBusiness, AvanaContext avanaContext, IAws3Services aws3Services, IConfiguration configuration)
        {
            _productService = productService;
            _baseBusiness = baseBusiness;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5002/");
            _context = avanaContext;
            _aws3Services = aws3Services;
            _configuration = configuration;
            _adminUrl = configuration["AdminUrl"];
        }

        [HttpPost("GetProducts")]
        //public IActionResult GetProducts()
        //{
        //    //List<ProductsModelList> result = _productService.GetProducts();
        //    List<ProductsModelList> result = _productService.GetProducts();
        //    if (result != null)
        //        return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

        //    return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        //}
        public async Task<JObject> GetProducts(SearchProductResponse searchProductResponse)
        {
            SearchProductResponse result = await _productService.GetAllAsync(searchProductResponse);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetBrands")]
        public IActionResult GetBrands()
        {
            List<ProductsModelList> result = _productService.GetBrands();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("SaveProducts")]
        public JObject SaveProducts(ProductEntityModel productEntity)
        {
            ProductEntityModel result = _productService.SaveProducts(productEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("GetProductById")]
        public JObject GetProductById(ProductEntityEditModel productEntity)
        {
            var result = _productService.GetProductById(productEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost]
        [Route("DeleteProduct")]
        public JObject DeleteProduct(ProductEntity productEntity)
        {
            bool result = _productService.DeleteProduct(productEntity);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", productEntity);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
        [HttpPost("ShowHideProduct")]
        public JObject ShowHideProduct(ProductEntity productEntity)
        {
            ProductEntity result = _productService.ShowHideProduct(productEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", productEntity);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }


        [HttpGet("GetProductType")]
        public IActionResult GetProductsType()
        {
            List<ProductTypeCategoryModelList> result = _productService.GetProductsType();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });

        }

        [HttpGet("GetHairStyles")]
        public IActionResult GetHairStyles()
        {
            List<HairStyles> result = _productService.GetHairStyles();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });

        }

        [HttpPost("SaveProductType")]
        public JObject SaveProductType(ProductTypeCategoryModel productType)
        {
            ProductTypeCategoryModel result = _productService.SaveProductType(productType);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }
        [HttpPost("SaveProductTag")]
        public JObject SaveProductTag(ProductTags productType)
        {
            ProductTags result = _productService.SaveProductTag(productType);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("SaveHairGoal")]
        public JObject SaveHairGoal(HairGoal hairGoal)
        {
            HairGoal result = _productService.SaveHairGoal(hairGoal);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }
        [HttpPost("SaveHairStyle")]
        public JObject SaveHairStyle(HairStyles hairStyle)
        {
            HairStyles result = _productService.SaveHairStyle(hairStyle);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }
        [HttpPost("SaveHairChallenge")]
        public JObject SaveHairChallenge(HairChallenges hairChallenge)
        {
            HairChallenges result = _productService.SaveHairChallenge(hairChallenge);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("SaveProductCategory")]
        public JObject SaveProductCategory(ProductTypeCategoriesList productType)
        {
            ProductTypeCategoriesList result = _productService.SaveProductCategory(productType);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }


        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            ProductsListings result = _productService.GetAllProducts();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpGet("GetStylingProducts")]
        public IActionResult GetStylingProducts()
        {
            List<ProductsModelList> result = _productService.GetStylingProducts();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("GetProductTypeById")]
        public JObject GetProductTypeById(ProductType productType)
        {
            ProductType result = _productService.GetProductTypeById(productType);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetProductTagById")]
        public JObject GetProductTagById(ProductTags productTag)
        {
            ProductTags result = _productService.GetProductTagById(productTag);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetHairChallengeById")]
        public JObject GetHairChallengeById(HairChallenges hairChallenges)
        {
            HairChallenges result = _productService.GetHairChallengeById(hairChallenges);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetHairGoalById")]
        public JObject GetHairGoalById(HairGoal hairGoal)
        {
            HairGoal result = _productService.GetHairGoalById(hairGoal);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpPost("GetHairStyleById")]
        public JObject GetHairStyleById(HairStyles hairStyle)
        {
            HairStyles result = _productService.GetHairStyleById(hairStyle);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpPost]
        [Route("DeleteProductType")]
        public JObject DeleteProductType(ProductType productType)
        {
            bool result = _productService.DeleteProductType(productType);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", productType);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost]
        [Route("DeleteProductTag")]
        public JObject DeleteProductTag(ProductTags productType)
        {
            bool result = _productService.DeleteProductTag(productType);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", productType);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost]
        [Route("DeleteHairGoal")]
        public JObject DeleteHairGoal(HairGoal hairGoal)
        {
            bool result = _productService.DeleteHairGoal(hairGoal);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", hairGoal);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
        [HttpPost]
        [Route("DeleteHairStyle")]
        public JObject DeleteHairStyle(HairStyles hairStyle)
        {
            bool result = _productService.DeleteHairStyle(hairStyle);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", hairStyle);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
        [HttpPost]
        [Route("DeleteHairChallenge")]
        public JObject DeleteHairChallenge(HairChallenges hairChallenge)
        {
            bool result = _productService.DeleteHairChallenge(hairChallenge);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", hairChallenge);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost]
        [Route("DeleteProductCategory")]
        public JObject DeleteProductCategory(ProductType productType)
        {
            bool result = _productService.DeleteProductCategory(productType);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", productType);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpGet("GetProductTypes")]
        public IActionResult GetProductTypes()
        {
            List<ProductTypesList> result = _productService.GetProductTypes();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetProductsList")]
        public IActionResult GetProductsList()
        {
            List<ProductEntity> result = _productService.GetProductsList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("GetProductCategoryById")]
        public JObject GetProductCategoryById(ProductTypeCategoriesList productType)
        {
            ProductTypeCategoriesList result = _productService.GetProductCategoryById(productType);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetProductsCategoryList")]
        public IActionResult GetProductsCategoryList()
        {
            List<ProductTypeCategoriesList> result = _productService.GetProductsCategoryList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("AddProductList")]
        public JObject AddProductList(IEnumerable<ProductEntityModel> productData)
        {
            var result = _productService.AddProductList(productData);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("UploadFile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UploadFile([FromBody] Imagerequest imagerequest)
        {
            var file = Request.Form.Files[0];
            //UserEntity entity;
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            //if (token != "")
            //{
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;

            var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
            string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            UserEntity entity = JsonConvert.DeserializeObject<UserEntity>(content);
            //}
            var result = _productService.UploadFile(file, entity);
            if (result.success)
                return Ok(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost]
        [Route("FileUpload")]
        public JObject FileUpload(fileData file)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(file.access_token);
                var tokenS = handler.ReadToken(file.access_token) as JwtSecurityToken;
                string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;

                var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
                string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                UserEntity entity = JsonConvert.DeserializeObject<UserEntity>(content);
                if (entity != null)
                {
                    UserEntity us = _context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();
                    us.ImageURL = _adminUrl + "imageUpload/" + file.ImageURL;
                    _context.SaveChanges();
                }
                file.user_name = entity.UserName;
                file.Email = entity.Email;
                file.Name = entity.FirstName + " " + entity.LastName;
                file.AccountNo = entity.AccountNo;
                file.TwoFactor = entity.TwoFactorEnabled;
                file.HairType = entity.HairType;
                file.ImageURL = _adminUrl + "imageUpload/" + file.ImageURL;
                return _baseBusiness.AddDataOnJson("Success", "1", file);
            }
            catch (Exception Ex)
            {
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
            }
        }

        [HttpPost]
        [Route("FileUploadCustomer")]
        public JObject FileUploadCustomer(fileData file)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(file.access_token);
                var tokenS = handler.ReadToken(file.access_token) as JwtSecurityToken;
                string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;

                var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
                string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                UserEntity entity = JsonConvert.DeserializeObject<UserEntity>(content);
                if (entity != null)
                {
                    UserEntity us = _context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();
                    us.ImageURL = "https://customerstaging.myavana.com/imageUpload/" + file.ImageURL;
                    _context.SaveChanges();
                }
                file.user_name = entity.UserName;
                file.Email = entity.Email;
                file.Name = entity.FirstName + " " + entity.LastName;
                file.AccountNo = entity.AccountNo;
                file.TwoFactor = entity.TwoFactorEnabled;
                file.HairType = entity.HairType;
                file.ImageURL = "https://customerstaging.myavana.com/imageUpload/" + file.ImageURL;
                return _baseBusiness.AddDataOnJson("Success", "1", file);
            }
            catch (Exception Ex)
            {
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
            }
        }

        [HttpGet("GetIngredientsList")]
        public IActionResult GetIngredientsList()
        {
            List<IngedientsEntity> result = _productService.GetIngredientsList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetHairTypesList")]
        public IActionResult GetHairTypesList()
        {
            List<HairType> result = _productService.GetHairTypesList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetHairGoalsList")]
        public IActionResult GetHairGoalsList()
        {
            List<HairGoal> result = _productService.GetHairGoalsList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetHairChallengesList")]
        public IActionResult GetHairChallengesList()
        {
            List<HairChallenges> result = _productService.GetHairChallengesList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetProductIndicatorsList")]
        public IActionResult GetProductIndicatorsList()
        {
            List<ProductIndicator> result = _productService.GetProductIndicatorsList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpGet("GetProductTagList")]
        public IActionResult GetProductTagList()
        {
            List<ProductTags> result = _productService.GetProductTagList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetProductClassificationList")]
        public IActionResult GetProductClassificationList()
        {
            List<ProductClassification> result = _productService.GetProductClassificationList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetCustomerPreferenceList")]
        public IActionResult GetCustomerPreferenceList()
        {
            List<CustomerPreference> result = _productService.GetCustomerPreferenceList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetBrandClassificationList")]
        public IActionResult GetBrandClassificationList()
        {
            List<BrandClassification> result = _productService.GetBrandClassificationList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpGet("GetProductCategory")]
        public IActionResult GetProductCategory()
        {
            List<ProductTypeCategory> result = _productService.GetProductCategory();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("GetProductClassificationById")]
        public JObject GetProductClassificationById(ProductClassification productClassification)
        {
            ProductClassification result = _productService.GetProductClassificationById(productClassification);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetCustomerPreferenceById")]
        public JObject GetCustomerPreferenceById(CustomerPreference customerPreference)
        {
            CustomerPreference result = _productService.GetCustomerPreferenceById(customerPreference);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetBrandClassificationById")]
        public JObject GetBrandClassificationById(BrandClassification brandClassification)
        {
            BrandClassification result = _productService.GetBrandClassificationById(brandClassification);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("SaveProductClassification")]
        public JObject SaveProductClassification(ProductClassification productClassification)
        {
            ProductClassification result = _productService.SaveProductClassification(productClassification);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost]
        [Route("DeleteProductClassification")]
        public JObject DeleteProductClassification(ProductClassification productClassification)
        {
            bool result = _productService.DeleteProductClassification(productClassification);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", productClassification);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost("SaveCustomerPreference")]
        public JObject SaveCustomerPreference(CustomerPreference customerPreference)
        {
            CustomerPreference result = _productService.SaveCustomerPreference(customerPreference);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost]
        [Route("DeleteCustomerPreference")]
        public JObject DeleteCustomerPreference(CustomerPreference customerPreference)
        {
            bool result = _productService.DeleteCustomerPreference(customerPreference);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", customerPreference);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost("SaveBrandClassification")]
        public JObject SaveBrandClassification(BrandClassification brandClassification)
        {
            BrandClassification result = _productService.SaveBrandClassification(brandClassification);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost]
        [Route("DeleteBrandClassification")]
        public JObject DeleteBrandClassification(BrandClassification brandClassification)
        {
            bool result = _productService.DeleteBrandClassification(brandClassification);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", brandClassification);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpGet("GetAllBrands")]
        public IActionResult GetAllBrands()
        {
            List<BrandsModelList> result = _productService.GetAllBrands();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost]
        [Route("DeleteImageS3")]
        public JObject DeletetDocumentFromS3(ProductImage productImage)
        {
            try
            {
                bool result = _aws3Services.DeleteFileAsync(productImage.ImageName, productImage.Id);
                if (result)
                    return _baseBusiness.AddDataOnJson("Success", "1", productImage);
                else
                    return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
            }
            catch (Exception ex)
            {
                return _baseBusiness.AddDataOnJson("Internal server error", "0", string.Empty);
            }
        }

        [DisableRequestSizeLimit]
        [HttpPost("UploadDocumentToS3")]
        public IActionResult UploadDocumentToS3([FromForm] ImagesModel imagesModel)
        {
            try
            {
                if (imagesModel.Files is null || imagesModel.Files.Count <= 0)
                    return BadRequest(new JsonResult("file is required to upload") { StatusCode = (int)HttpStatusCode.BadRequest });

                //var _aws3Services = new Aws3Services(_appConfiguration.AwsAccessKey, _appConfiguration.AwsSecretAccessKey, _appConfiguration.Region, _appConfiguration.BucketName);

                var result = _aws3Services.UploadFileAsync(imagesModel.Files, 0);

                return Ok(string.Empty);
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError });
            }
        }

        [HttpGet("GetProductRecommendationStatusList")]
        public IActionResult GetProductRecommendationStatusList()
        {
            List<ProductRecommendationStatus> result = _productService.GetProductRecommendationStatusList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("SaveProductRecommendationStatus")]
        public JObject SaveProductRecommendationStatus(ProductRecommendationStatus recommendationStatus)
        {
            ProductRecommendationStatus result = _productService.SaveProductRecommendationStatus(recommendationStatus);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("GetProductRecommendationStatusById")]
        public JObject GetProductRecommendationStatusById(ProductRecommendationStatus recommendationStatus)
        {
            ProductRecommendationStatus result = _productService.GetProductRecommendationStatusById(recommendationStatus);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost]
        [Route("DeleteProductRecommendationStatus")]
        public JObject DeleteProductRecommendationStatus(ProductRecommendationStatus recommendationStatus)
        {
            bool result = _productService.DeleteProductRecommendationStatus(recommendationStatus);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", recommendationStatus);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

    }
}

