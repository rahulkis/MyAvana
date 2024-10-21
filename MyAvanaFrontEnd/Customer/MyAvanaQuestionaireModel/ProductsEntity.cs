using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvanaQuestionaireModel
{
	public class ProductsEntity
	{
		public int Id { get; set; }
		[JsonProperty(PropertyName = "guid")]
		public Guid guid { get; set; }

		[JsonProperty(PropertyName = "ProductName")]
		public string ProductName { get; set; }

		[JsonProperty(PropertyName = "ActualName")]
		public string ActualName { get; set; }

		[JsonProperty(PropertyName = "BrandName")]
		public string BrandName { get; set; }

		[JsonProperty(PropertyName = "TypeFor")]
		public string TypeFor { get; set; }

		[JsonProperty(PropertyName = "ImageName")]
		public string ImageName { get; set; }

		[JsonProperty(PropertyName = "Ingredients")]
		public string Ingredients { get; set; }

		[JsonProperty(PropertyName = "ProductDetails")]
		public string ProductDetails { get; set; }

		[JsonProperty(PropertyName = "ProductLink")]
		public string ProductLink { get; set; }

		[JsonProperty(PropertyName = "IsActive")]
		public bool IsActive { get; set; }
		public DateTime? CreatedOn { get; set; }
		public Guid? ProductTypeId { get; set; }

		[JsonProperty(PropertyName = "ProductType")]
		public string ProductType { get; set; }
	}
}
