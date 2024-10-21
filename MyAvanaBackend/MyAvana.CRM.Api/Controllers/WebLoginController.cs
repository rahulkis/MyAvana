using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json.Linq;
using MyAvanaApi.Models.ViewModels;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebLoginController : ControllerBase
    {
        private readonly IWebLogin _webService;
        private readonly IBaseBusiness _baseBusiness;
        public WebLoginController(IWebLogin webService, IBaseBusiness baseBusiness)
        {
            _webService = webService;
            _baseBusiness = baseBusiness;
        }

        [HttpPost("Login")]
        public JObject Login(WebLogin webLogin)
        {
            WebLogin result = _webService.Login(webLogin);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetUsers")]
        public JObject GetUsers(string id)
        {
            List<WebLoginDetails> result = _webService.GetUsers(id);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost]
        [Route("AddNewUser")]
        public JObject AddNewUser(WebLoginDetails webLogin)
        {
            WebLoginDetails result = _webService.AddNewUser(webLogin);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", webLogin);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost("GetUserByid")]
        public JObject GetUserByid(WebLoginDetails webLogin)
        {
            WebLoginDetails result = _webService.GetUserByid(webLogin);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost]
        [Route("DeleteUser")]
        public JObject DeleteUser(WebLogin webLogin)
        {
            bool result = _webService.DeleteUser(webLogin);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", webLogin);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
        [HttpGet("GetOwnerSalons")]
        public IActionResult GetOwnerSalons()
        {
            List<UserSalonOwnerModel> result = _webService.GetOwnerSalons();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost]
        [Route("ResetAdminPassword")]
        public JObject ResetAdminPassword(ResetPasswordModel resetPassword)
        {
            bool result = _webService.ResetAdminPassword(resetPassword);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", resetPassword);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }


        [HttpGet("ForgotAdminPassword")]
        public IActionResult ForgotAdminPassword(string email)
        {
            var result = _webService.ForgotAdminPassword(email);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost]
        [Route("SetAdminPass")]
        public IActionResult SetAdminPass(SetPassword setPassword)
        {
            var result = _webService.SetAdminPass(setPassword);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpGet("GetHairStrandNotificationCount")]
        public IActionResult GetHairStrandNotificationCount()
        {
            int result = _webService.GetHairStrandNotificationCount();
            if (result != 0) return Ok(result);
            return BadRequest(new JsonResult(0));
        }
        [HttpGet("GetUserTypeList")]
        public IActionResult GetUserTypeList()
        {
            List<UserType> result = _webService.GetUserTypeList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetHairDiaryNotificationCount")]
        public IActionResult GetHairDiaryNotificationCount()
        {
            int result = _webService.GetHairDiaryNotificationCount();
            if (result != 0) return Ok(result);
            return BadRequest(new JsonResult(0));
        }

    }
}
