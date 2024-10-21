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
        public async Task<Message<ProductsEntity>> SaveProducts(ProductsEntity productsEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/SaveProducts"));
            var result = await PostAsync<ProductsEntity>(requestUrl, productsEntity);
            return result;
        }

        public async Task<Message<SearchProductResponse>> GetProducts(SearchProductResponse gridParams)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetProducts"));
            var response = await PostAsync(requestUrl, gridParams);
            return response;
        }
        public async Task<List<ProductsEntity>> GetProductsList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetProductsList"));
            var response = await GetAsyncData<ProductsEntity>(requestUrl);
            List<ProductsEntity> products = JsonConvert.DeserializeObject<List<ProductsEntity>>(Convert.ToString(response.value));
            return products;
        }

        public async Task<List<HairStyles>> GetHairStyles()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetHairStyles"));
            var response = await GetAsyncData<HairStyles>(requestUrl);
            List<HairStyles> products = JsonConvert.DeserializeObject<List<HairStyles>>(Convert.ToString(response.value));
            return products;
        }
        public async Task<List<ProductsModelList>> GetStylingProducts()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetStylingProducts"));
            var response = await GetAsyncData<ProductsModelList>(requestUrl);
            List<ProductsModelList> products = JsonConvert.DeserializeObject<List<ProductsModelList>>(Convert.ToString(response.value));
            return products;
        }
        public async Task<ProductsListings> GetAllProducts()
        {
            //var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetAllProducts"));
            //var response = await GetAsyncData<ProductsListings>(requestUrl);
            //ProductsListings productsListings = JsonConvert.DeserializeObject<ProductsListings>(Convert.ToString(response.value));
            //List<string> productBrands = new List<string>();
            //List<ProductBrand> productBrandsList = new List<ProductBrand>();
            //foreach (var product in productsListings.ProductsModelLists)
            //{
            //    ProductBrand pBrand = new ProductBrand();
            //    pBrand.BrandName = product.BrandName;
            //    pBrand.ProductTypeId = product.ProductTypeId;
            //    pBrand.ProductId = product.Id;
            //    if (!(productBrands.Contains(pBrand.BrandName)))
            //    {
            //        productBrands.Add(pBrand.BrandName);
            //        productBrandsList.Add(pBrand);
            //    }
            //}
            //productsListings.ProductBrand = productBrandsList;
            //List<ProductsModelList> list = productsListings.ProductsModelLists;
            //List<ProductsModelList> list1 = new List<ProductsModelList>();
            //foreach (var bd in list)
            //{
            //    if (bd.BrandName == "Alikay Naturals")
            //    {
            //        list1.Add(bd);
            //    }
            //}
            //return productsListings;
            return new ProductsListings();
        }
        public async Task<ProductsListings> GetAllProducts2()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetAllProducts"));
            var response = await GetAsyncData<ProductsListings>(requestUrl);
            ProductsListings productsListings = JsonConvert.DeserializeObject<ProductsListings>(Convert.ToString(response.value));
            List<string> productBrands = new List<string>();
            List<ProductBrand> productBrandsList = new List<ProductBrand>();
            foreach (var product in productsListings.ProductsModelLists)
            {
                ProductBrand pBrand = new ProductBrand();
                pBrand.BrandName = product.BrandName;
                pBrand.ProductTypeId = product.ProductTypeId;
                pBrand.ProductId = product.Id;
                if (!(productBrands.Contains(pBrand.BrandName)))
                {
                    productBrands.Add(pBrand.BrandName);
                    productBrandsList.Add(pBrand);
                }
            }
            productsListings.ProductBrand = productBrandsList;
            //List<ProductsModelList> list = productsListings.ProductsModelLists;
            //List<ProductsModelList> list1 = new List<ProductsModelList>();
            //foreach (var bd in list)
            //{
            //    if (bd.BrandName == "Alikay Naturals")
            //    {
            //        list1.Add(bd);
            //    }
            //}
            return productsListings;
        }
        public async Task<Message<ProductsEntity>> DeleteProduct(ProductsEntity productsEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/DeleteProduct"));
            var result = await PostAsync<ProductsEntity>(requestUrl, productsEntity);
            return result;
        }
        public async Task<Message<ProductsEntity>> ShowHideProduct(ProductsEntity productsEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/ShowHideProduct"));
            var result = await PostAsync<ProductsEntity>(requestUrl, productsEntity);
            return result;
        }
        public async Task<Message<ProductEntityEditModel>> GetProductById(ProductEntityEditModel productsEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Products/GetProductById"));
            var result = await PostAsync<ProductEntityEditModel>(requestUrl, productsEntity);
            return result;
        }

        public async Task<Message<ProductTypeCategoryModel>> SaveProductType(ProductTypeCategoryModel productTypeEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/SaveProductType"));
            var result = await PostAsync<ProductTypeCategoryModel>(requestUrl, productTypeEntity);
            return result;
        }

        public async Task<Message<ProductTagsModel>> SaveProductTag(ProductTagsModel productTypeEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/SaveProductTag"));
            var result = await PostAsync<ProductTagsModel>(requestUrl, productTypeEntity);
            return result;
        }
        public async Task<Message<HairGoalsModel>> SaveHairGoal(HairGoalsModel hairGoalEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/SaveHairGoal"));
            var result = await PostAsync<HairGoalsModel>(requestUrl, hairGoalEntity);
            return result;
        }
        public async Task<Message<HairStyles>> SaveHairStyle(HairStyles hairStyleEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/SaveHairStyle"));
            var result = await PostAsync<HairStyles>(requestUrl, hairStyleEntity);
            return result;
        }
        public async Task<Message<HairChallengesModel>> SaveHairChallenge(HairChallengesModel hairChallengesEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/SaveHairChallenge"));
            var result = await PostAsync<HairChallengesModel>(requestUrl, hairChallengesEntity);
            return result;
        }
        public async Task<Message<ProductTypeCategoriesList>> SaveProductCategory(ProductTypeCategoriesList productTypeEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/SaveProductCategory"));
            var result = await PostAsync<ProductTypeCategoriesList>(requestUrl, productTypeEntity);
            return result;
        }

        public async Task<List<ProductTypeEntity>> GetProductType()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetProductType"));
            var response = await GetAsyncData<ProductTypeEntity>(requestUrl);
            List<ProductTypeEntity> blogPosts = JsonConvert.DeserializeObject<List<ProductTypeEntity>>(Convert.ToString(response.value));
            return blogPosts;
        }

        public async Task<List<ProductTypeCategoriesList>> GetProductsCategoryList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetProductsCategoryList"));
            var response = await GetAsyncData<ProductTypeCategoriesList>(requestUrl);
            List<ProductTypeCategoriesList> blogPosts = JsonConvert.DeserializeObject<List<ProductTypeCategoriesList>>(Convert.ToString(response.value));
            return blogPosts;
        }
        public async Task<Message<ProductTypeEntity>> DeleteProductType(ProductTypeEntity productTypeEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/DeleteProductType"));
            var result = await PostAsync<ProductTypeEntity>(requestUrl, productTypeEntity);
            return result;
        }
        public async Task<Message<ProductTagsModel>> DeleteProductTag(ProductTagsModel productTypeEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/DeleteProductTag"));
            var result = await PostAsync<ProductTagsModel>(requestUrl, productTypeEntity);
            return result;
        }
        public async Task<Message<HairGoalsModel>> DeleteHairGoal(HairGoalsModel hairGoalEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/DeleteHairGoal"));
            var result = await PostAsync<HairGoalsModel>(requestUrl, hairGoalEntity);
            return result;
        }
        public async Task<Message<HairStyles>> DeleteHairStyle(HairStyles hairStyleEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/DeleteHairStyle"));
            var result = await PostAsync<HairStyles>(requestUrl, hairStyleEntity);
            return result;
        }
        public async Task<Message<HairChallengesModel>> DeleteHairChallenge(HairChallengesModel hairChallengeEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/DeleteHairChallenge"));
            var result = await PostAsync<HairChallengesModel>(requestUrl, hairChallengeEntity);
            return result;
        }
        public async Task<Message<ProductTypeEntity>> DeleteProductCategory(ProductTypeEntity productTypeEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/DeleteProductCategory"));
            var result = await PostAsync<ProductTypeEntity>(requestUrl, productTypeEntity);
            return result;
        }
        public async Task<Message<ProductTypeEntity>> GetProductTypeById(ProductTypeEntity productTypeEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Products/GetProductTypeById"));
            var result = await PostAsync<ProductTypeEntity>(requestUrl, productTypeEntity);
            return result;
        }
        public async Task<Message<ProductTagsModel>> GetProductTagById(ProductTagsModel productTypeEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Products/GetProductTagById"));
            var result = await PostAsync<ProductTagsModel>(requestUrl, productTypeEntity);
            return result;
        }
        public async Task<Message<HairGoalsModel>> GetHairGoalById(HairGoalsModel hairGoalEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Products/GetHairGoalById"));
            var result = await PostAsync<HairGoalsModel>(requestUrl, hairGoalEntity);
            return result;
        }
        public async Task<Message<HairStyles>> GetHairStyleById(HairStyles hairStyleEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Products/GetHairStyleById"));
            var result = await PostAsync<HairStyles>(requestUrl, hairStyleEntity);
            return result;
        }
        public async Task<Message<HairChallengesModel>> GetHairChallengeById(HairChallengesModel hairGoalEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Products/GetHairChallengeById"));
            var result = await PostAsync<HairChallengesModel>(requestUrl, hairGoalEntity);
            return result;
        }

        public async Task<Message<ProductTypeCategoriesList>> GetProductCategoryById(ProductTypeCategoriesList productTypeEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Products/GetProductCategoryById"));
            var result = await PostAsync<ProductTypeCategoriesList>(requestUrl, productTypeEntity);
            return result;
        }

        public async Task<List<ProductBrand>> GetBrands()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetBrands"));
            var response = await GetAsyncData<ProductsEntity>(requestUrl);
            List<ProductsEntity> products = JsonConvert.DeserializeObject<List<ProductsEntity>>(Convert.ToString(response.value));
            List<string> productBrands = new List<string>();
            List<ProductBrand> productBrandsList = new List<ProductBrand>();
            foreach (var product in products)
            {
                ProductBrand pBrand = new ProductBrand();
                pBrand.BrandName = product.BrandName;
                pBrand.ProductTypeId = product.ProductTypeId;
                pBrand.ProductId = product.Id;
                if (!(productBrands.Contains(pBrand.BrandName)))
                {
                    productBrands.Add(pBrand.BrandName);
                    productBrandsList.Add(pBrand);
                }
            }
            return productBrandsList;
        }

        public async Task<List<ProductTypesList>> GetProductTypes()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetProductTypes"));
            var response = await GetAsyncData<ProductTypesList>(requestUrl);
            List<ProductTypesList> products = JsonConvert.DeserializeObject<List<ProductTypesList>>(Convert.ToString(response.value));
            return products;
        }

        public async Task<List<ProductsEntity>> AddProductList(List<ProductsEntity> productModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/AddProductList"));
            var result = await PostAsync<List<ProductsEntity>>(requestUrl, productModel);
            return result.Data;
        }

        public async Task<List<HairTypeModel>> GetHairTypesList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetHairTypesList"));
            var response = await GetAsyncData<HairTypeModel>(requestUrl);
            List<HairTypeModel> blogPosts = JsonConvert.DeserializeObject<List<HairTypeModel>>(Convert.ToString(response.value));
            return blogPosts;
        }

        public async Task<List<HairChallengesModel>> GetHairChallengesList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetHairChallengesList"));
            var response = await GetAsyncData<HairChallengesModel>(requestUrl);
            List<HairChallengesModel> blogPosts = JsonConvert.DeserializeObject<List<HairChallengesModel>>(Convert.ToString(response.value));
            return blogPosts;
        }
        public async Task<List<HairGoalsModel>> GetHairGoalsList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetHairGoalsList"));
            var response = await GetAsyncData<HairGoalsModel>(requestUrl);
            List<HairGoalsModel> blogPosts = JsonConvert.DeserializeObject<List<HairGoalsModel>>(Convert.ToString(response.value));
            return blogPosts;
        }

        public async Task<List<ProductIndicatorsModel>> GetProductIndicatorsList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetProductIndicatorsList"));
            var response = await GetAsyncData<ProductIndicatorsModel>(requestUrl);
            List<ProductIndicatorsModel> blogPosts = JsonConvert.DeserializeObject<List<ProductIndicatorsModel>>(Convert.ToString(response.value));
            return blogPosts;
        }

        public async Task<List<ProductTagsModel>> GetProductTagList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetProductTagList"));
            var response = await GetAsyncData<ProductTagsModel>(requestUrl);
            List<ProductTagsModel> blogPosts = JsonConvert.DeserializeObject<List<ProductTagsModel>>(Convert.ToString(response.value));
            return blogPosts;
        }

        public async Task<List<ProductClassificationModel>> GetProductClassificationList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetProductClassificationList"));
            var response = await GetAsyncData<ProductClassificationModel>(requestUrl);
            List<ProductClassificationModel> blogPosts = JsonConvert.DeserializeObject<List<ProductClassificationModel>>(Convert.ToString(response.value));
            return blogPosts;
        }
        public async Task<List<CustomerPreference>> GetCustomerPreferenceList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetCustomerPreferenceList"));
            var response = await GetAsyncData<CustomerPreference>(requestUrl);
            List<CustomerPreference> blogPosts = JsonConvert.DeserializeObject<List<CustomerPreference>>(Convert.ToString(response.value));
            return blogPosts;
        }
        public async Task<List<BrandClassificationModel>> GetBrandClassificationList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetBrandClassificationList"));
            var response = await GetAsyncData<BrandClassificationModel>(requestUrl);
            List<BrandClassificationModel> blogPosts = JsonConvert.DeserializeObject<List<BrandClassificationModel>>(Convert.ToString(response.value));
            return blogPosts;
        }
        public async Task<List<IngredientEntityModel>> GetIngredientsList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetIngredientsList"));
            var response = await GetAsyncData<IngredientEntityModel>(requestUrl);
            List<IngredientEntityModel> blogPosts = JsonConvert.DeserializeObject<List<IngredientEntityModel>>(Convert.ToString(response.value));
            return blogPosts;
        }

        public async Task<List<ProductTypeCategory>> GetProductCategory()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetProductCategory"));
            var response = await GetAsyncData<ProductTypeCategory>(requestUrl);
            List<ProductTypeCategory> blogPosts = JsonConvert.DeserializeObject<List<ProductTypeCategory>>(Convert.ToString(response.value));
            return blogPosts;
        }


        public async Task<Message<IndicatorModel>> SaveIndicator(IndicatorModel productTypeEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Indicator/SaveIndicator"));
            var result = await PostAsync<IndicatorModel>(requestUrl, productTypeEntity);
            return result;
        }

        public async Task<Message<IndicatorModel>> DeleteProductIndicator(IndicatorModel indicatorModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Indicator/DeleteIndicator"));
            var result = await PostAsync<IndicatorModel>(requestUrl, indicatorModel);
            return result;
        }

        public async Task<Message<IndicatorModel>> GetProductIndicatorById(IndicatorModel productTypeEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Indicator/GetIndicatorById"));
            var result = await PostAsync<IndicatorModel>(requestUrl, productTypeEntity);
            return result;
        }


        public async Task<List<IndicatorModel>> GetIndicators()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Indicator/GetIndicators"));
            var response = await GetAsyncData<IndicatorModel>(requestUrl);
            List<IndicatorModel> indicators = JsonConvert.DeserializeObject<List<IndicatorModel>>(Convert.ToString(response.data));
            return indicators;
        }


        public async Task<Message<ProductClassificationModel>> GetProductClassificationById(ProductClassificationModel productClassificationModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Products/GetProductClassificationById"));
            var result = await PostAsync<ProductClassificationModel>(requestUrl, productClassificationModel);
            return result;
        }

        public async Task<Message<CustomerPreference>> GetCustomerPreferenceById(CustomerPreference customerPreferenceModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Products/GetCustomerPreferenceById"));
            var result = await PostAsync<CustomerPreference>(requestUrl, customerPreferenceModel);
            return result;
        }
        public async Task<Message<BrandClassificationModel>> GetBrandClassificationById(BrandClassificationModel brandClassificationModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Products/GetBrandClassificationById"));
            var result = await PostAsync<BrandClassificationModel>(requestUrl, brandClassificationModel);
            return result;
        }
        public async Task<Message<ProductClassificationModel>> SaveProductClassification(ProductClassificationModel productClassificationModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/SaveProductClassification"));
            var result = await PostAsync<ProductClassificationModel>(requestUrl, productClassificationModel);
            return result;
        }
        public async Task<Message<CustomerPreference>> SaveCustomerPreference(CustomerPreference customerPreferenceModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/SaveCustomerPreference"));
            var result = await PostAsync<CustomerPreference>(requestUrl, customerPreferenceModel);
            return result;
        }
        public async Task<Message<BrandClassificationModel>> SaveBrandClassification(BrandClassificationModel brandClassificationModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/SaveBrandClassification"));
            var result = await PostAsync<BrandClassificationModel>(requestUrl, brandClassificationModel);
            return result;
        }
        public async Task<Message<ProductClassificationModel>> DeleteProductClassification(ProductClassificationModel productClassificationModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/DeleteProductClassification"));
            var result = await PostAsync<ProductClassificationModel>(requestUrl, productClassificationModel);
            return result;
        }
        public async Task<Message<CustomerPreference>> DeleteCustomerPreference(CustomerPreference productClassificationModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/DeleteCustomerPreference"));
            var result = await PostAsync<CustomerPreference>(requestUrl, productClassificationModel);
            return result;
        }
        public async Task<Message<BrandClassificationModel>> DeleteBrandClassification(BrandClassificationModel brandClassificationModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/DeleteBrandClassification"));
            var result = await PostAsync<BrandClassificationModel>(requestUrl, brandClassificationModel);
            return result;
        }
        public async Task<List<BrandsModelList>> GetProductBrandsList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetAllBrands"));
            var response = await GetAsyncData<BrandsModelList>(requestUrl);
            List<BrandsModelList> blogPosts = JsonConvert.DeserializeObject<List<BrandsModelList>>(Convert.ToString(response.value));
            return blogPosts;
        }
        public async Task<Message<ProductImage>> DeleteProductImage(ProductImage productImage)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/DeleteImageS3"));
            var result = await PostAsync<ProductImage>(requestUrl, productImage);
            return result;
        }
        public async Task<List<ProductRecommendationStatusModel>> GetProductRecommendationStatusList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetProductRecommendationStatusList"));
            var response = await GetAsyncData<ProductRecommendationStatusModel>(requestUrl);
            List<ProductRecommendationStatusModel> productRecommendationStatus = JsonConvert.DeserializeObject<List<ProductRecommendationStatusModel>>(Convert.ToString(response.value));
            return productRecommendationStatus;
        }
        public async Task<Message<ProductRecommendationStatusModel>> SaveProductRecommendationStatus(ProductRecommendationStatusModel recommendationStatusEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/SaveProductRecommendationStatus"));
            var result = await PostAsync<ProductRecommendationStatusModel>(requestUrl, recommendationStatusEntity);
            return result;
        }
        public async Task<Message<ProductRecommendationStatusModel>> GetProductRecommendationStatusById(ProductRecommendationStatusModel recommendationStatusEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Products/GetProductRecommendationStatusById"));
            var result = await PostAsync<ProductRecommendationStatusModel>(requestUrl, recommendationStatusEntity);
            return result;
        }
        public async Task<Message<ProductRecommendationStatusModel>> DeleteProductRecommendationStatus(ProductRecommendationStatusModel recommendationStatusEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/DeleteProductRecommendationStatus"));
            var result = await PostAsync<ProductRecommendationStatusModel>(requestUrl, recommendationStatusEntity);
            return result;
        }
    }
}
