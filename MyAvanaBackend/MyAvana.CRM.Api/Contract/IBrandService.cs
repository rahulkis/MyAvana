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
    public interface IBrandService
    {
        Tags GetTagById(Tags Tag);
        Tags SaveTag(Tags Tag);
        List<Tags> GetBrandTagList();

        bool DeleteTag(Tags Tag);
        BrandsEntityModel SaveBrand(BrandsEntityModel brandEntity);
        List<BrandModelList> GetBrandsList();
        BrandsEntityModel GetBrandDetailsById(BrandsEntityModel brandEntity);
        bool DeleteBrand(BrandsEntityModel brandEntity);

        Brands ShowHideBrand(Brands brandEntity);
        List<BrandList> GetAllBrandsList();
        List<HairState> GetBrandHairStateList();
        HairState SaveHairState(HairState hairState);
        HairState GetHairStateById(HairState hairState);
        bool DeleteHairState(HairState hairState);
        List<BrandRecommendationStatus> GetBrandRecommendationStatusList();
        BrandRecommendationStatus SaveBrandRecommendationStatus(BrandRecommendationStatus recommendationStatus);
        BrandRecommendationStatus GetBrandRecommmendationStatusById(BrandRecommendationStatus recommendationStatus);
        bool DeleteBrandRecommendationStatus(BrandRecommendationStatus recommendationStatus);
        List<MolecularWeight> GetBrandMolecularWeightList();
        MolecularWeight SaveMolecularWeight(MolecularWeight weight);
        MolecularWeight GetMolecularWeightById(MolecularWeight weight);
        bool DeleteMolecularWeight(MolecularWeight weight);
    }
}
