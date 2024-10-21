﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PromoCodeController : ControllerBase
	{
		private readonly IPromoCodeService _codeService;
		private readonly IBaseBusiness _baseBusiness;
		public PromoCodeController(IPromoCodeService codeService,IBaseBusiness baseBusiness)
		{
			_codeService = codeService;
			_baseBusiness = baseBusiness;
		}		
		
		[HttpPost]
		[Route("SavePromoCode")]
		public JObject SavePromoCode(PromoCodeModel codeModel)
		{
			bool result =  _codeService.SavePromoCode(codeModel);
			if (result)
				return _baseBusiness.AddDataOnJson("Success", "1", codeModel);
			else
				return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
		}

		[HttpGet("GetPromoCodes")]
		public JObject GetPromoCodes()
		{
			List<PromoCodeModel> result = _codeService.GetPromoCodes();
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
			//if (result != null)
			//	return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

			//return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });

		}

        [HttpPost]
        [Route("DeletePromoCode")]
        public JObject DeletePromoCode(PromoCodeModel codeModel)
        {
            bool result = _codeService.DeletePromoCode(codeModel);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", codeModel);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

		[HttpGet("GetDiscountCodes")]
		public JObject GetDiscountCodes()
		{
			List<DiscountCodesModel> result = _codeService.GetDiscountCodes();
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
		}

		[HttpPost("SaveDiscountCode")]
		public JObject SaveDiscountCode(DiscountCodesModel discountCode)
        {
			DiscountCodesModel result = _codeService.SaveDiscountCode(discountCode);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

		[HttpPost("GetDiscountCodeById")]
		public JObject GetDiscountCodeById(DiscountCodesModel discountCode)
        {
			DiscountCodesModel result = _codeService.GetDiscountCodeById(discountCode);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

		[HttpPost]
		[Route("DeleteDiscountCode")]
		public JObject DeleteDiscountCode(DiscountCodesModel discountCode)
        {
			bool result = _codeService.DeleteDiscountCode(discountCode);
			if (result)
				return _baseBusiness.AddDataOnJson("Success", "1", discountCode);
			else
				return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
	}
}