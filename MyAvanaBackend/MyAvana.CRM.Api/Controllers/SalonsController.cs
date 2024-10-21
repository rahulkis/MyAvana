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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalonsController : ControllerBase
    {
        private readonly ISalons _salonService;
        private readonly IBaseBusiness _baseBusiness;
        public SalonsController(ISalons salonService, IBaseBusiness baseBusiness)
        {
            _salonService = salonService;
            _baseBusiness = baseBusiness;
        }


        [HttpGet("GetSalons")]
        public JObject GetSalons(int start, int length)
        {
            List<SalonDetails> result = _salonService.GetSalons(start, length);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
      
        [HttpPost]
        [Route("AddNewSalon")]
        public JObject AddNewSalon([FromForm] SalonModel salon)
        {
            SalonModel result = _salonService.AddNewSalon(salon);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", salon);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost("GetSalonByid")]
        public JObject GetSalonByid(Salons salon)
        {
            Salons result = _salonService.GetSalonByid(salon);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpGet("GetSalonList")]
        public JObject GetSalonList()
        {
            List<SalonsListModel> result = _salonService.GetSalonsList();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        

        [HttpPost("UpdateHairProfileSalon")]
        public async Task<IActionResult> UpdateHairProfileSalon([FromBody] SalonHairProfileModel salonHairProfileModel)
        {
            var result = await _salonService.UpdateHairProfileSalon(salonHairProfileModel);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
    }
}
