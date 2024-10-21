using Microsoft.AspNetCore.Http;
using MyAvana.Models.Entities;
using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class BrandsModel
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string TypeFor { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string HairChallenges { get; set; }
        public string BrandTags { get; set; }
        public string BrandClassification { get; set; }
    }

    public class BrandsEntityModel
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string FeaturedIngredients { get; set; }
        public int? Rank { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string BrandClassification { get; set; }
        public string HairTypes { get; set; }
        public string HairChallenges { get; set; }
        public string HairGoals { get; set; }
        public string BrandTags { get; set; }
        public string HairStates { get; set; }
        public string MolecularWeights { get; set; }
        public string RecommendationStatuses { get; set; }
        public List<ClassificationBrand> BrandClassifications { get; set; }
        public List<BrandHairType> HairType { get; set; }
        public List<BrandHairChallenge> HairChallenge { get; set; }
        public List<BrandHairGoal> HairGoal { get; set; }
        public List<BrandTag> BrandTag { get; set; }
        public List<BrandHairState> HairState { get; set; }
        public List<BrandsBrandRecommendationStatus> BrandRecommendationStatus { get; set; }
        public List<BrandMolecularWeight> MolecularWeight { get; set; }


    }
    public class BrandModelList
    {
        //public List<ProductEntity> product { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string FeaturedIngredients { get; set; }
        public int? Rank { get; set; }
        public List<HairType> HairType { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List<HairChallenges> HairChallenge { get; set; }
        public List<HairGoal> HairGoals { get; set; }
        public List<Tags> Tag { get; set; }
        public List<BrandClassification> BrandClassification { get; set; }
        public List<HairState> HairState { get; set; }
        public List<BrandRecommendationStatus> BrandRecommendationStatus { get; set; }
        public List<MolecularWeight> MolecularWeight { get; set; }
        public string HairChallenges { get; set; }
        public string HairTypes { get; set; }
        public string Tags { get; set; }
        public string BrandClassifications { get; set; }
        public string HairStates { get; set; }
        public string MolecularWeights { get; set; }
        public string RecommendationStatuses { get; set; }
        public string HairGoalsDes { get; set; }
        public bool? HideInSearch { get; set; }
    }

    public class BrandList { 
        public string BrandName { get; set; }
    	public int BrandId { get; set; }


    }
}
