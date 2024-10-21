using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IProductsService
    {
        ProductEntityModel SaveProducts(ProductEntityModel productEntity);

        List<ProductsModelList> GetProducts();
        ProductsListings GetAllProducts();
        List<ProductsModelList> GetStylingProducts();
        List<ProductsModelList> GetBrands();
        ProductEntityEditModel GetProductById(ProductEntityEditModel productEntity);
        bool DeleteProduct(ProductEntity productEntity);

        ProductEntity ShowHideProduct(ProductEntity productEntity);
        List<ProductTypeCategoryModelList> GetProductsType();
        List<HairStyles> GetHairStyles();
        bool DeleteProductType(ProductType productType);
        bool DeleteProductTag(ProductTags productType);
        bool DeleteHairGoal(HairGoal hairGoal);
        bool DeleteHairStyle(HairStyles hairStyle);
        bool DeleteHairChallenge(HairChallenges hairChallenge);
        bool DeleteProductCategory(ProductType productType);
        ProductType GetProductTypeById(ProductType productType);
        ProductTags GetProductTagById(ProductTags productTag);
        HairChallenges GetHairChallengeById(HairChallenges hairChallenges);
        HairGoal GetHairGoalById(HairGoal hairGoal);
        HairStyles GetHairStyleById(HairStyles hairStyle);
        ProductTypeCategoriesList GetProductCategoryById(ProductTypeCategoriesList productType);
        ProductTypeCategoryModel SaveProductType(ProductTypeCategoryModel productType);
        ProductTags SaveProductTag(ProductTags productType);
        HairGoal SaveHairGoal(HairGoal hairGoal);
        HairStyles SaveHairStyle(HairStyles hairStyle);
        HairChallenges SaveHairChallenge(HairChallenges hairChallenge);
        ProductTypeCategoriesList SaveProductCategory(ProductTypeCategoriesList productType);
        List<ProductTypesList> GetProductTypes();
        List<ProductEntity> GetProductsList();
        List<ProductTypeCategoriesList> GetProductsCategoryList();
        IEnumerable<ProductEntityModel> AddProductList(IEnumerable<ProductEntityModel> productData);
        (JsonResult result, bool success, string error) UploadFile(IFormFile file, UserEntity entity);

        List<HairType> GetHairTypesList();
        List<HairChallenges> GetHairChallengesList();
        List<HairGoal> GetHairGoalsList();
        List<ProductIndicator> GetProductIndicatorsList();
        List<ProductTags> GetProductTagList();
        List<ProductClassification> GetProductClassificationList();
        List<BrandClassification> GetBrandClassificationList();
        List<IngedientsEntity> GetIngredientsList();
        List<ProductTypeCategory> GetProductCategory();
        Task<SearchProductResponse> GetAllAsync(SearchProductResponse searchProductResponse);
        ProductClassification GetProductClassificationById(ProductClassification productClassification);
        BrandClassification GetBrandClassificationById(BrandClassification brandClassification);
        ProductClassification SaveProductClassification(ProductClassification productClassification);
        bool DeleteProductClassification(ProductClassification productClassification);
        BrandClassification SaveBrandClassification(BrandClassification brandClassification);
        bool DeleteBrandClassification(BrandClassification brandClassification);
        List<BrandsModelList> GetAllBrands();
        CustomerPreference SaveCustomerPreference(CustomerPreference customerPreference);
        bool DeleteCustomerPreference(CustomerPreference customerPreference);
        CustomerPreference GetCustomerPreferenceById(CustomerPreference customerPreference);
        List<CustomerPreference> GetCustomerPreferenceList();
        List<ProductRecommendationStatus> GetProductRecommendationStatusList();
        ProductRecommendationStatus SaveProductRecommendationStatus(ProductRecommendationStatus recommendationStatus);
        ProductRecommendationStatus GetProductRecommendationStatusById(ProductRecommendationStatus recommendationStatus);
        bool DeleteProductRecommendationStatus(ProductRecommendationStatus recommendationStatus);
    }
}
