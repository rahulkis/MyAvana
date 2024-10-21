using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.ViewModels;
using MyAvana.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
	public interface IPromoCodeService
	{
		bool SavePromoCode(PromoCodeModel codeModel);
		List<PromoCodeModel> GetPromoCodes();
        bool DeletePromoCode(PromoCodeModel codeModel);
		List<DiscountCodesModel> GetDiscountCodes();
		DiscountCodesModel SaveDiscountCode(DiscountCodesModel discountCode);
		DiscountCodesModel GetDiscountCodeById(DiscountCodesModel discountCode);
		bool DeleteDiscountCode(DiscountCodesModel discountCode);
	}
}
