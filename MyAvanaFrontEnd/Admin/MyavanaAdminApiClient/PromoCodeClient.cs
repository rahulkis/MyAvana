using MyavanaAdminModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyavanaAdminApiClient
{
	public partial class ApiClient
	{
		public List<int> GetDaysList()
		{
			List<int> days = new List<int>();
			int start = 1; int end = 30;
			while (start <= end)
			{
				days.Add(start);
				start = start + 1;
			}
			return days;
		}

		public List<int> GetWeeksList()
		{
			List<int> weeks = new List<int>();
			int start = 1; int end = 52;
			while (start <= end)
			{
				weeks.Add(start);
				start = start + 1;
			}
			return weeks;
		}

		public async Task<Message<PromoCodeModel>> SavePromoCode(PromoCodeModel promoCodeModel)
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,"PromoCode/SavePromoCode"));
			var result =  await PostAsync<PromoCodeModel>(requestUrl, promoCodeModel);
			return result;
		}

		public async Task<List<CodeListModel>> GetPromoCodes()
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "PromoCode/GetPromoCodes"));
			List<CodeListModel> response = await GetAsyncList<CodeListModel>(requestUrl);
			return response;

		}
        public async Task<Message<PromoCodeModel>> DeletePromoCode(PromoCodeModel promoCodeModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "PromoCode/DeletePromoCode"));
            var result = await PostAsync<PromoCodeModel>(requestUrl, promoCodeModel);
            return result;
        }

		public async Task<List<DiscountCodeListModel>> GetDiscountCodes()
        {
			var requestUrl = CreateRequestUri(string.Format
				(System.Globalization.CultureInfo.InvariantCulture, "PromoCode/GetDiscountCodes"));
			List<DiscountCodeListModel> response = await GetAsyncList<DiscountCodeListModel>(requestUrl);
			return response;

		}

		public async Task<Message<DiscountCodeListModel>> SaveDiscountCode(DiscountCodeListModel saveDiscountCode)
        {
            try
            {
				var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "PromoCode/SaveDiscountCode"));
				var result = await PostAsync<DiscountCodeListModel>(requestUrl, saveDiscountCode);
				return result;
			}
            catch (Exception ex)
            {

                throw ex;
            }
			
		}

        public async Task<Message<DiscountCodesModel>> GetDiscountCodeById(DiscountCodesModel discountCodeEntity)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "PromoCode/GetDiscountCodeById"));
                var result = await PostAsync<DiscountCodesModel>(requestUrl, discountCodeEntity);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public async Task<Message<DiscountCodesModel>> DeleteDiscountCode(DiscountCodesModel discountCodeEntity)
        {
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "PromoCode/DeleteDiscountCode"));
			var result = await PostAsync<DiscountCodesModel>(requestUrl, discountCodeEntity);
			return result;
		}

	}
}
