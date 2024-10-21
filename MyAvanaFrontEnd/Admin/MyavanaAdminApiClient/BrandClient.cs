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
        public async Task<Message<TagsModel>> SaveTag(TagsModel TagEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/SaveTag"));
            var result = await PostAsync<TagsModel>(requestUrl, TagEntity);
            return result;
        }
        public async Task<Message<Brands>> SaveBrand(Brands TagEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/SaveBrand"));
            var result = await PostAsync<Brands>(requestUrl, TagEntity);
            return result;
        }
        public async Task<Message<TagsModel>> GetTagById(TagsModel TagEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Brand/GetTagById"));
            var result = await PostAsync<TagsModel>(requestUrl, TagEntity);
            return result;
        }
        public async Task<Message<TagsModel>> DeleteTag(TagsModel TagEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/DeleteTag"));
            var result = await PostAsync<TagsModel>(requestUrl, TagEntity);
            return result;
        }
        public async Task<List<TagsModel>> GetBrandTagList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/GetBrandTagList"));
            var response = await GetAsyncData<TagsModel>(requestUrl);
            List<TagsModel> blogPosts = JsonConvert.DeserializeObject<List<TagsModel>>(Convert.ToString(response.value));
            return blogPosts;
        }

        public async Task<List<BrandModelList>> GetBrandsList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/GetBrandsList"));
            try
            {
                var response = await GetAsyncData<BrandModelList>(requestUrl);
                List<BrandModelList> products = JsonConvert.DeserializeObject<List<BrandModelList>>(Convert.ToString(response.value));
                return products;
            }catch(Exception ex)
            {
                return null;
            }
        }
        public async Task<List<BrandList>> GetAllBrandsList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/GetAllBrandsList"));
            var response = await GetAsyncData<BrandList>(requestUrl);
            List<BrandList> blogPosts = JsonConvert.DeserializeObject<List<BrandList>>(Convert.ToString(response.value));
            return blogPosts;
        }
        public async Task<Message<BrandsEntityModel>> GetBrandDetailsById(BrandsEntityModel brandsEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Brand/GetBrandDetailsById"));
            var result = await PostAsync<BrandsEntityModel>(requestUrl, brandsEntity);
            return result;
        }
        public async Task<Message<BrandsEntityModel>> DeleteBrand(BrandsEntityModel productsEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/DeleteBrand"));
            var result = await PostAsync<BrandsEntityModel>(requestUrl, productsEntity);
            return result;
        }
        public async Task<Message<Brands>> ShowHideBrand(Brands brandsEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/ShowHideBrand"));
            var result = await PostAsync<Brands>(requestUrl, brandsEntity);
            return result;
        }
        public async Task<List<HairStateModel>> GetBrandHairStateList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/GetBrandHairStateList"));
            var response = await GetAsyncData<HairStateModel>(requestUrl);
            List<HairStateModel> hairState = JsonConvert.DeserializeObject<List<HairStateModel>>(Convert.ToString(response.value));
            return hairState;
        }
        public async Task<Message<HairStateModel>> SaveHairState(HairStateModel HairstateEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/SaveHairState"));
            var result = await PostAsync<HairStateModel>(requestUrl, HairstateEntity);
            return result;
        }
        public async Task<Message<HairStateModel>> GetHairStateById(HairStateModel HairstateEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/GetHairStateById"));
            var result = await PostAsync<HairStateModel>(requestUrl, HairstateEntity);
            return result;
        }
        public async Task<Message<HairStateModel>> DeleteHairState(HairStateModel HairstateEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/DeleteHairState"));
            var result = await PostAsync<HairStateModel>(requestUrl, HairstateEntity);
            return result;
        }
        public async Task<List<BrandRecommendationStatusModel>> GetBrandRecommendationStatusList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/GetBrandRecommendationStatusList"));
            var response = await GetAsyncData<BrandRecommendationStatusModel>(requestUrl);
            List<BrandRecommendationStatusModel> status = JsonConvert.DeserializeObject<List<BrandRecommendationStatusModel>>(Convert.ToString(response.value));
            return status;
        }
        public async Task<Message<BrandRecommendationStatusModel>> SaveBrandRecommendationStatus(BrandRecommendationStatusModel brandRecommendationEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/SaveBrandRecommendationStatus"));
            var result = await PostAsync<BrandRecommendationStatusModel>(requestUrl, brandRecommendationEntity);
            return result;
        }
        public async Task<Message<BrandRecommendationStatusModel>> GetBrandRecommmendationStatusById(BrandRecommendationStatusModel brandRecommendationEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/GetBrandRecommmendationStatusById"));
            var result = await PostAsync<BrandRecommendationStatusModel>(requestUrl, brandRecommendationEntity);
            return result;
        }
        public async Task<Message<BrandRecommendationStatusModel>> DeleteBrandRecommendationStatus(BrandRecommendationStatusModel brandRecommendationEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/DeleteBrandRecommendationStatus"));
            var result = await PostAsync<BrandRecommendationStatusModel>(requestUrl, brandRecommendationEntity);
            return result;
        }
        public async Task<List<MolecularWeightModel>> GetBrandMolecularWeightList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/GetBrandMolecularWeightList"));
            var response = await GetAsyncData<MolecularWeightModel>(requestUrl);
            List<MolecularWeightModel> molecularWeight = JsonConvert.DeserializeObject<List<MolecularWeightModel>>(Convert.ToString(response.value));
            return molecularWeight;
        }
        public async Task<Message<MolecularWeightModel>> SaveMolecularWeight(MolecularWeightModel molecularWeightEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/SaveMolecularWeight"));
            var result = await PostAsync<MolecularWeightModel>(requestUrl, molecularWeightEntity);
            return result;
        }
        public async Task<Message<MolecularWeightModel>> GetMolecularWeightById(MolecularWeightModel molecularWeightEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/GetMolecularWeightById"));
            var result = await PostAsync<MolecularWeightModel>(requestUrl, molecularWeightEntity);
            return result;
        }
        public async Task<Message<MolecularWeightModel>> DeleteMolecularWeight(MolecularWeightModel molecularWeightEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Brand/DeleteMolecularWeight"));
            var result = await PostAsync<MolecularWeightModel>(requestUrl, molecularWeightEntity);
            return result;
        }
    }
}
