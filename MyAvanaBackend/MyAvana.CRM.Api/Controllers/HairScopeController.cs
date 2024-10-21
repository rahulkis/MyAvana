using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Cors;
namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HairScopeController : ControllerBase
    {
        IHairScopeService Hair_Scope;      
        private readonly IBaseBusiness _baseBusiness;
        private IHostingEnvironment _env;
        public HairScopeController(IHairScopeService _HairScopService, IBaseBusiness baseBusiness, IHostingEnvironment environment)
        {
            Hair_Scope = _HairScopService;
            _baseBusiness = baseBusiness;
            _env = environment;
        }
        [HttpPost]
        [Route("AddNewHairScope")]
        [EnableCors("AllowCors")]
        public JObject AddNewHairScope([FromBody] HairScopeModel HairScope)
        {        
            HairScopeModel result = Hair_Scope.AddNewHairScope(HairScope);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
        [HttpPost]
        [Route("GetHairScopeResultData")]
        public JObject GetHairScopeResultData(HairScopeModelParameters HairScopParam)
        {
            HairScopeModel result = Hair_Scope.GetHairScopeResultData(HairScopParam);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
            //return BadRequest(new JsonResult("Something goes wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost]
        [Route("GetHairScopeResultDataWeb")]
        public JObject GetHairScopeResultDataWeb(HairScopeModel HairScopParam)
        {
            HairScopeModel result = Hair_Scope.GetHairScopeResultDataWeb(HairScopParam);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
            //return BadRequest(new JsonResult("Something goes wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
    }
}
