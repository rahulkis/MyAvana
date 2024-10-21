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
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IBaseBusiness _baseBusiness;
        private readonly IHostingEnvironment _environment;
        private readonly HttpClient _httpClient;
        private readonly AvanaContext _context;
        private readonly IAws3Services _aws3Services;
        public BrandController(IBrandService brandService, IBaseBusiness baseBusiness, AvanaContext avanaContext, IAws3Services aws3Services)
        {
            _brandService = brandService;
            _baseBusiness = baseBusiness;
            _httpClient = new HttpClient();
           // _httpClient.BaseAddress = new Uri("http://localhost:5002/");
            _context = avanaContext;
            _aws3Services = aws3Services;
        }
        [HttpPost("GetTagById")]
        public JObject GetTagById(Tags BrandTag)
        {
            Tags result = _brandService.GetTagById(BrandTag);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("SaveTag")]
        public JObject SaveTag(Tags BrandTag)
        {
            Tags result = _brandService.SaveTag(BrandTag);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }
        [HttpPost]
        [Route("DeleteTag")]
        public JObject DeleteTag(Tags Tag)
        {
            bool result = _brandService.DeleteTag(Tag);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", Tag);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpGet("GetBrandTagList")]
        public IActionResult GetBrandTagList()
        {
            List<Tags> result = _brandService.GetBrandTagList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("SaveBrand")]
        public JObject SaveBrand(BrandsEntityModel brandEntity)
        {
            BrandsEntityModel result = _brandService.SaveBrand(brandEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpGet("GetBrandsList")]
        public IActionResult GetBrandsList()
        {
            List<BrandModelList> result =  _brandService.GetBrandsList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost("GetBrandDetailsById")]
        public JObject GetBrandDetailsById(BrandsEntityModel brandEntity)
        {
            var result = _brandService.GetBrandDetailsById(brandEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost]
        [Route("DeleteBrand")]
        public JObject DeleteBrand(BrandsEntityModel brandEntity)
        {
            bool result = _brandService.DeleteBrand(brandEntity);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", brandEntity);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
        [HttpPost("ShowHideBrand")]
        public JObject ShowHideBrand(Brands brandEntity)
        {
            Brands result = _brandService.ShowHideBrand(brandEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", brandEntity);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpGet("GetAllBrandsList")]
        public IActionResult GetAllBrandsList()
        {
            List<BrandList> result = _brandService.GetAllBrandsList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetBrandHairStateList")]
        public IActionResult GetBrandHairStateList()
        {
            List<HairState> result = _brandService.GetBrandHairStateList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("SaveHairState")]
        public JObject SaveHairState(HairState hairState)
        {
            HairState result = _brandService.SaveHairState(hairState);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("GetHairStateById")]
        public JObject GetHairStateById(HairState hairState)
        {
            HairState result = _brandService.GetHairStateById(hairState);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost]
        [Route("DeleteHairState")]
        public JObject DeleteHairState(HairState hairState)
        {
            bool result = _brandService.DeleteHairState(hairState);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", hairState);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
        [HttpPost("GetBrandRecommmendationStatusById")]
        public JObject GetBrandRecommmendationStatusById(BrandRecommendationStatus recommendationStatus)
        {
            BrandRecommendationStatus result = _brandService.GetBrandRecommmendationStatusById(recommendationStatus);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("SaveBrandRecommendationStatus")]
        public JObject SaveBrandRecommendationStatus(BrandRecommendationStatus recommendationStatus)
        {
            BrandRecommendationStatus result = _brandService.SaveBrandRecommendationStatus(recommendationStatus);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }
        [HttpPost]
        [Route("DeleteBrandRecommendationStatus")]
        public JObject DeleteBrandRecommendationStatus(BrandRecommendationStatus recommendationStatus)
        {
            bool result = _brandService.DeleteBrandRecommendationStatus(recommendationStatus);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", recommendationStatus);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpGet("GetBrandRecommendationStatusList")]
        public IActionResult GetBrandRecommendationStatusList()
        {
            List<BrandRecommendationStatus> result = _brandService.GetBrandRecommendationStatusList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpGet("GetBrandMolecularWeightList")]
        public IActionResult GetBrandMolecularWeightList()
        {
            List<MolecularWeight> result = _brandService.GetBrandMolecularWeightList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("SaveMolecularWeight")]
        public JObject SaveMolecularWeight(MolecularWeight weight)
        {
            MolecularWeight result = _brandService.SaveMolecularWeight(weight);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("GetMolecularWeightById")]
        public JObject GetMolecularWeightById(MolecularWeight weight)
        {
            MolecularWeight result = _brandService.GetMolecularWeightById(weight);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost]
        [Route("DeleteMolecularWeight")]
        public JObject DeleteMolecularWeight(MolecularWeight weight)
        {
            bool result = _brandService.DeleteMolecularWeight(weight);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", weight);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
    }
}
