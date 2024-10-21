using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class Brands
    {
        [JsonProperty(PropertyName = "BrandId")]
        public int BrandId { get; set; }

        [JsonProperty(PropertyName = "BrandName")]
        public string BrandName { get; set; }

        [JsonProperty(PropertyName = "FeaturedIngredients")]
        public string FeaturedIngredients { get; set; }

        [JsonProperty(PropertyName = "Rank")]
        public int? Rank { get; set; }

        [JsonProperty(PropertyName = "IsActive")]
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }

        [JsonProperty(PropertyName = "BrandClassification")]
        public string BrandClassification { get; set; }


        [JsonProperty(PropertyName = "HairChallenges")]
        public string HairChallenges { get; set; }

        [JsonProperty(PropertyName = "HairGoals")]
        public string HairGoals { get; set; }

        [JsonProperty(PropertyName = "BrandTags")]
        public string BrandTags { get; set; }

        [JsonProperty(PropertyName = "HairStates")]
        public string HairStates { get; set; }

        [JsonProperty(PropertyName = "RecommendationStatuses")]
        public string RecommendationStatuses { get; set; }

        [JsonProperty(PropertyName = "MolecularWeights")]
        public string MolecularWeights { get; set; }

        [JsonProperty(PropertyName = "HairState")]
        public List<HairStateModel> HairState { get; set; }

        [JsonProperty(PropertyName = "BrandRecommendationStatus")]
        public List<BrandRecommendationStatusModel> BrandRecommendationStatus { get; set; }

        [JsonProperty(PropertyName = "MolecularWeight")]
        public List<MolecularWeightModel> MolecularWeight { get; set; }

        [JsonProperty(PropertyName = "HairType")]
        public List<HairTypeModel> HairType { get; set; }

        [JsonProperty(PropertyName = "HairChallenge")]
        public List<HairChallengesModel> HairChallenge { get; set; }

        [JsonProperty(PropertyName = "Tags")]
        public List<TagsModel> Tags { get; set; }


        [JsonProperty(PropertyName = "HairTypes")]
        public string HairTypes { get; set; }

        [JsonProperty(PropertyName = "BrandClassifications")]
        public string BrandClassifications { get; set; }

        [JsonProperty(PropertyName = "HairGoalsDes")]
        public string HairGoalsDes { get; set; }

        [JsonProperty(PropertyName = "HideInSearch")]
        public bool? HideInSearch { get; set; }
    }
    public class BrandModelDownload
    {
        public string BrandName { get; set; }
    }
    public class BrandModelList
    {

        public int BrandId { get; set; }
        [JsonProperty(PropertyName = "BrandName")]
        public string BrandName { get; set; }
        [JsonProperty(PropertyName = "FeaturedIngredients")]
        public string FeaturedIngredients { get; set; }
        [JsonProperty(PropertyName = "Rank")]
        public int? Rank { get; set; }
        public List<HairType> HairType { get; set; }
        [JsonProperty(PropertyName = "HairState")]
        public List<HairState> HairState { get; set; }
        [JsonProperty(PropertyName = "BrandRecommendationStatus")]
        public List<BrandRecommendationStatus> BrandRecommendationStatus { get; set; }
        [JsonProperty(PropertyName = "MolecularWeight")]
        public List<MolecularWeight> MolecularWeight { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        [JsonProperty(PropertyName = "HairChallenge")]
        public List<HairChallenges> HairChallenge { get; set; }
        public List<HairGoal> HairGoals { get; set; }
        public List<Tags> Tag { get; set; }
        public List<BrandClassification> BrandClassification { get; set; }

        [JsonProperty(PropertyName = "HairChallenges")]
        public string HairChallenges { get; set; }

        [JsonProperty(PropertyName = "HairTypes")]
        public string HairTypes { get; set; }

        [JsonProperty(PropertyName = "Tags")]
        public string Tags { get; set; }
        [JsonProperty(PropertyName = "BrandClassifications")]
        public string BrandClassifications { get; set; }
        [JsonProperty(PropertyName = "HairGoalsDes")]
        public string HairGoalsDes { get; set; }

        [JsonProperty(PropertyName = "HairStates")]
        public string HairStates { get; set; }

        [JsonProperty(PropertyName = "RecommendationStatuses")]
        public string RecommendationStatuses { get; set; }

        [JsonProperty(PropertyName = "MolecularWeights")]
        public string MolecularWeights { get; set; }

        [JsonProperty(PropertyName = "HideInSearch")]
        public bool? HideInSearch { get; set; }
    }
    public class HairGoal
    {
        public int HairGoalId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class Tags
    {
        public int TagId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class BrandClassification
    {
        public int BrandClassificationId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class HairType
    {
        public int HairTypeId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class HairState
    {
        public int HairStateId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class BrandRecommendationStatus
    {
        public int BrandRecommendationStatusId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class MolecularWeight
    {
        public int MolecularWeightId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class BrandsEntityModel
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string BrandClassification { get; set; }
        public string HairTypes { get; set; }
        public string HairChallenges { get; set; }
        public string HairGoals { get; set; }
        public string BrandTags { get; set; }
        public string HairStates { get; set; }
        public string RecommendationStatuses { get; set; }
        public string MolecularWeights { get; set; }
        public string FeaturedIngredients { get; set; }
        public int? Rank { get; set; }

        public List<ClassificationBrand> BrandClassifications { get; set; }
        public List<BrandHairType> HairType { get; set; }
        public List<BrandHairChallenge> HairChallenge { get; set; }
        public List<BrandHairGoal> HairGoal { get; set; }
        public List<BrandTag> BrandTag { get; set; }
        public List<BrandHairState> HairState { get; set; }
        public List<BrandsBrandRecommendationStatus> BrandRecommendationStatus { get; set; }
        public List<BrandMolecularWeight> MolecularWeight { get; set; }

    }
    public class ClassificationBrand
    {
        public int ClassificationBrandId { get; set; }

        public int BrandClassificationId { get; set; }

        public int BrandId { get; set; }
        public bool? IsActive { get; set; }
    }
    public class BrandHairType
    {
        public int BrandHairTypeId { get; set; }

        public int HairTypeId { get; set; }
        public int BrandId { get; set; }

        public bool IsActive { get; set; }
    }
    public class BrandHairChallenge
    {
        public int BrandHairChallengeId { get; set; }

        public int? HairChallengeId { get; set; }

        public int BrandId { get; set; }

        public bool IsActive { get; set; }
    }
    public class BrandHairGoal
    {
        public int BrandHairGoalId { get; set; }

        public int? HairGoalId { get; set; }
        public int BrandId { get; set; }

        public bool IsActive { get; set; }
    }
    public class BrandTag
    {
        public int BrandTagId { get; set; }

        public int TagsId { get; set; }
        public int BrandId { get; set; }

        public bool? IsActive { get; set; }
    }
    public class BrandList
    {
        public string BrandName { get; set; }
        public int BrandId { get; set; }
    }

    public class BrandHairState
    {
        public int BrandHairStateId { get; set; }

        public int HairStateId { get; set; }
        public int BrandId { get; set; }

        public bool IsActive { get; set; }
    }
    public class BrandsBrandRecommendationStatus
    {
        public int BrandsBrandRecommendationStatusId { get; set; }

        public int BrandRecommendationStatusId { get; set; }
        public int BrandId { get; set; }

        public bool IsActive { get; set; }
    }
    public class BrandMolecularWeight
    {
        public int BrandMolecularWeightId { get; set; }

        public int MolecularWeightId { get; set; }
        public int BrandId { get; set; }

        public bool IsActive { get; set; }
    }
}
