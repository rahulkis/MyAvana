using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
	public class PromoCodeModel
	{
		public string PromoCode { get; set; }
		public DateTime ExpireDate { get; set; }
		public string StripePlanId { get; set; }
		public string InitialDate { get; set; }
	}

	public class CodeListModel
	{
		[JsonProperty(PropertyName = "promoCode")]
		public string PromoCode { get; set; }
		[JsonProperty(PropertyName = "createdDate")]
		public DateTime? CreatedDate { get; set; }
		[JsonProperty(PropertyName = "expireDate")]
		public DateTime? ExpireDate { get; set; }
		[JsonProperty(PropertyName = "isActive")]
		public bool IsActive { get; set; }
		public DateTime? CreatedOn { get; set; }

	}

	public class DiscountCodeListModel
    {
		[JsonProperty(PropertyName = "DiscountCodeId")]
		public int DiscountCodeId { get; set; }
		[JsonProperty(PropertyName = "DiscountCode")]
		public string DiscountCode { get; set; }
		[JsonProperty(PropertyName = "CreatedDate")]
		public DateTime? CreatedDate { get; set; }
		[JsonProperty(PropertyName = "ExpireDate")]
		public DateTime? ExpireDate { get; set; }
		[JsonProperty(PropertyName = "DiscountPercent")]
		public int DiscountPercent { get; set; }
		[JsonProperty(PropertyName = "CreatedBy")]
		public string CreatedBy { get; set; }
		[JsonProperty(PropertyName = "IsActive")]
        public bool IsActive { get; set; }

    }
	public class DiscountCodesModel
	{
		public int DiscountCodeId { get; set; }
		public string DiscountCode { get; set; }
		public int DiscountPercent { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime ExpireDate { get; set; }
		public string CreatedBy { get; set; }
		public bool IsActive { get; set; }
	}

}
