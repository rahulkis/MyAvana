using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlexaController : ControllerBase
    {
        private readonly IAlexaService _alexaService;
		private readonly IBaseBusiness _baseBusiness;
		private readonly IHostingEnvironment _environment;
		public AlexaController(IAlexaService articleService, IBaseBusiness baseBusiness, IHostingEnvironment hostingEnvironment)
        {
			_baseBusiness = baseBusiness;
			_alexaService = articleService;
			_environment = hostingEnvironment;
		}
        
		[HttpGet("GetAlexaFAQs")]
		public JObject GetAlexaFAQs(int start, int length)
		{
			List<AlexaFAQModel> result = _alexaService.GeAlexaFAQs(start, length);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);

		}

		[HttpPost("AddAlexaFAQ")]
		public JObject AddAlexaFAQ(AlexaFAQ alexaFAQ)
		{
			AlexaFAQ result = _alexaService.AddAlexaFAQ(alexaFAQ);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

		}

		[HttpPost("GetAlexaFAQById")]
		public JObject GetAlexaFAQById(AlexaFAQ alexaFAQ)
		{
			AlexaFAQ result = _alexaService.GetAlexaFAQById(alexaFAQ);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

		}

        [HttpPost]
        [Route("DeleteAlexaFAQ")]
        public JObject DeleteAlexaFAQ(AlexaFAQ alexaFAQ)
        {
            bool result = _alexaService.DeleteAlexaFAQ(alexaFAQ);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", alexaFAQ);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

		[HttpGet("GetFAQFullDetails")]
		public JObject GetFAQFullDetails(string keywords, string category)
		{
			FAQFullDetailsModel result = _alexaService.GetFAQFullDetails(keywords, category);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);

		}

		[HttpGet("GetFAQShortResponse")]
		public JObject GetFAQShortResponse(string keywords, string category)
		{
			FAQShortResponseModel result = _alexaService.GetFAQShortResponse(keywords, category);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);

		}

		[HttpGet("GetSalonResponse")]
		public JObject GetSalonResponse(string zipcode)
		{
			AlexaSalonModel result = _alexaService.GetSalonResponse(zipcode);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);

		}
	}
}