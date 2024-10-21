using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class ProductTypeEntity
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "ProductName")]
        public string ProductName { get; set; }
        [JsonProperty(PropertyName = "CreatedOn")]
        public DateTime? CreatedOn { get; set; }
        [JsonProperty(PropertyName = "IsActive")]
        public bool? IsActive { get; set; }
        [JsonProperty(PropertyName = "CategoryName")]
        public string CategoryName { get; set; }

        public int? ParentId { get; set; }
        [JsonProperty(PropertyName = "Rank")]
        public int Rank { get; set; }
    }
    public class ProductTypeCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsHair { get; set; }
        public bool? IsRegimens { get; set; }
    }

    public class ProductTypeCategoriesList
    {
        [JsonProperty(PropertyName = "ProductTypeId")]
        public int ProductTypeId { get; set; }
        [JsonProperty(PropertyName = "CategoryName")]
        public string CategoryName { get; set; }
        [JsonProperty(PropertyName = "IsHair")]
        public bool? IsHair { get; set; }
        [JsonProperty(PropertyName = "IsRegimen")]
        public bool? IsRegimen { get; set; }
       
    }
    public class ProductTypeCategoryModel
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string ProductName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsHair { get; set; }
        public bool? IsRegimens { get; set; }
        public int Rank { get; set; }
    }

    public class IndicatorModel
    {
        [JsonProperty(PropertyName = "ProductIndicatorId")]
        public int ProductIndicatorId { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "IsActive")]
        public bool? IsActive { get; set; }
        [JsonProperty(PropertyName = "CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [JsonProperty(PropertyName = "CreatedDate")]
        public string CreatedDate { get; set; }
    }

    public class GetUsers
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
       
    }
    //public class productSubscribers
    //{
    //    public IEnumerable<ProductTypeCategoryModel> ProductTypeCategoryModel { get; set; }
    //    public IEnumerable<SubscriberModel> SubscriberModel { get; set; }
    //}

}
