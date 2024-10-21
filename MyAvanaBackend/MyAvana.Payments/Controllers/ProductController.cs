using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyAvana.Models.Entities;
using MyAvana.Payments.Api.Contract;
using MyAvanaApi.Models.Entities;
using Newtonsoft.Json;

namespace MyAvana.Payments.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductService _productService;
        private readonly HttpClient _httpClient;
        public ProductController(IProductService productService)
        {
            _productService = productService;
            _httpClient = new HttpClient();
        }
        [HttpGet("HairTypes")]
        public IActionResult GetAllHairTypes()
        {
            var result = _productService.GetAllHairTypes();
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest }); ;
        }

        [HttpGet("ProductsTypes")]
        public IActionResult GetAllProductTypes(string hairType)
        {
            var result = _productService.GetAllProductTypes(hairType);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest }); ;
        }

        [HttpGet("GetProductsByTypes")]
        public IActionResult GetProductsByTypes(string hairTypes)
        {
            var result = _productService.GetProductsByTypes(hairTypes);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest }); ;
        }

        [HttpGet("suggestions")]
        public IActionResult GetProductSuggetion(string hairType, string productType, string hairChallenge, int pageNumber, string userId)
        {
            _httpClient.BaseAddress = new Uri("http://localhost:5002/");
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
            string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            UserEntity entity = JsonConvert.DeserializeObject<UserEntity>(content);
            var result = _productService.GetSuggestions(hairType, productType, hairChallenge,pageNumber, entity.Id.ToString());
            //var result = _productService.GetSuggestionsSP(hairType, productType, hairChallenge,pageNumber, entity.Id.ToString());
            if (result.success) return Ok(result.result.Value);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest }); ;
        }

        [HttpGet("GetProductDetails")]
        public IActionResult GetProductDetails(string id)
        {
            var result = _productService.GetProductDetails(id);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest }); ;

        }

        [HttpGet("suggestionsSP")]
        public IActionResult GetProductSuggetionSP(string hairType, string productType, string hairChallenge, int pageNumber, string userId)
        {
            _httpClient.BaseAddress = new Uri("http://localhost:5002/");
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
            string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            UserEntity entity = JsonConvert.DeserializeObject<UserEntity>(content);
            var result = _productService.GetSuggestionsSP(hairType, productType, hairChallenge,pageNumber, entity.Id.ToString());
            if (result.success) return Ok(result.result.Value);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest }); ;
        }

    }
}