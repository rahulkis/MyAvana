using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MyAvanaQuestionaireModel
{

    public class HairProfile
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string HairId { get; set; }
        public string HealthSummary { get; set; }
        public string TopLeftStrandDiameter { get; set; }
        public string TopLeftHealthText { get; set; }
        public string TopRightStrandDiameter { get; set; }
        public string TopRightHealthText { get; set; }
        public string BottomLeftStrandDiameter { get; set; }
        public string BottomLeftHealthText { get; set; }
        public string BottoRightStrandDiameter { get; set; }
        public string BottomRightHealthText { get; set; }
        public string CrownStrandDiameter { get; set; }
        public string CrownHealthText { get; set; }

        public List<Observation> Observation { get; set; }
        public List<Pororsity> Pororsity { get; set; }
        public List<Elasticity> Elasticity { get; set; }

        public string TempObservation { get; set; }
        public string TempPororsity { get; set; }
        public string TempElasticity { get; set; }

        public IFormFile TopLeftFile { get; set; }
        public IFormFile TopRightFile { get; set; }
        public IFormFile BottomLeftFile { get; set; }
        public IFormFile BottomRightFile { get; set; }
        public IFormFile CrownFile { get; set; }

        public string TopLeftPhoto { get; set; }
        public string TopRightPhoto { get; set; }
        public string BottomLeftPhoto { get; set; }
        public string BottomRightPhoto { get; set; }
        public string CrownPhoto { get; set; }

        public List<RecommendedProducts> RecommendedProducts { get; set; }
        public List<RecommendedIngredients> RecommendedIngredients { get; set; }
        public List<RecommendedRegimens> RecommendedRegimens { get; set; }
        public List<RecommendedVideos> RecommendedVideos { get; set; }

        public string TempRecommendedProducts { get; set; }
        public string TempRecommendedIngredients { get; set; }
        public string TempRecommendedRegimens { get; set; }
        public string TempRecommendedVideos { get; set; }
    }

    public class HairProfileAdminModel
    {
        public string UserId { get; set; }
        public string HairId { get; set; }
        public string HealthSummary { get; set; }
        public TopLeft TopLeft { get; set; }
        public TopRight TopRight { get; set; }
        public BottomLeft BottomLeft { get; set; }
        public BottomRight BottomRight { get; set; }
        public CrownStrand CrownStrand { get; set; }
        public List<RecommendedVideos> RecommendedVideos { get; set; }
        public List<RecommendedProductsModel> RecommendedProducts { get; set; }
        public List<RecommendedIngredients> RecommendedIngredients { get; set; }
        public List<RecommendedRegimens> RecommendedRegimens { get; set; }
    }


    public class TopLeft
    {
        public string TopLeftPhoto { get; set; }
        public string TopLeftStrandDiameter { get; set; }
        public string TopLeftHealthText { get; set; }
        public List<Observation> Observation { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }

    public class TopRight
    {
        public string TopRightPhoto { get; set; }
        public string TopRightStrandDiameter { get; set; }
        public string TopRightHealthText { get; set; }
        public List<Observation> Observation { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }

    public class BottomLeft
    {
        public string BottomLeftPhoto { get; set; }
        public string BottomLeftStrandDiameter { get; set; }
        public string BottomLeftHealthText { get; set; }
        public List<Observation> Observation { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }

    public class BottomRight
    {
        public string BottomRightPhoto { get; set; }
        public string BottomRightStrandDiameter { get; set; }
        public string BottomRightHealthText { get; set; }
        public List<Observation> Observation { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }

    public class CrownStrand
    {
        public string CrownPhoto { get; set; }
        public string CrownStrandDiameter { get; set; }
        public string CrownHealthText { get; set; }
        public List<Observation> Observation { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }
    public class HairStrandImageInfo
    {
        public string StrandImage { get; set; }
        public int StrandImageId { get; set; }
    }
    public class TopLeftAdmin
    {
        public List<HairStrandImageInfo> TopLeftPhoto { get; set; }
        public string TopLeftStrandDiameter { get; set; }
        public string TopLeftHealthText { get; set; }
        public List<HealthModel> Health { get; set; }
        public List<Observation> Observation { get; set; }
        public List<ObsElasticity> obsElasticities { get; set; }
        public List<ObsChemicalProducts> obsChemicalProducts { get; set; }
        public List<ObsPhysicalProducts> obsPhysicalProducts { get; set; }
        public List<ObsDamage> obsDamages { get; set; }
        public List<ObsBreakage> obsBreakages { get; set; }
        public List<ObsSplitting> obsSplittings { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }

    public class TopRightAdmin
    {
        public List<HairStrandImageInfo> TopRightPhoto { get; set; }
        public string TopRightStrandDiameter { get; set; }
        public string TopRightHealthText { get; set; }
        public List<HealthModel> Health { get; set; }
        public List<Observation> Observation { get; set; }
        public List<ObsElasticity> obsElasticities { get; set; }
        public List<ObsChemicalProducts> obsChemicalProducts { get; set; }
        public List<ObsPhysicalProducts> obsPhysicalProducts { get; set; }
        public List<ObsDamage> obsDamages { get; set; }
        public List<ObsBreakage> obsBreakages { get; set; }
        public List<ObsSplitting> obsSplittings { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }

    public class BottomLeftAdmin
    {
        public List<HairStrandImageInfo> BottomLeftPhoto { get; set; }
        public string BottomLeftStrandDiameter { get; set; }
        public string BottomLeftHealthText { get; set; }
        public List<HealthModel> Health { get; set; }
        public List<Observation> Observation { get; set; }
        public List<ObsElasticity> obsElasticities { get; set; }
        public List<ObsChemicalProducts> obsChemicalProducts { get; set; }
        public List<ObsPhysicalProducts> obsPhysicalProducts { get; set; }
        public List<ObsDamage> obsDamages { get; set; }
        public List<ObsBreakage> obsBreakages { get; set; }
        public List<ObsSplitting> obsSplittings { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }

    public class BottomRightAdmin
    {
        public List<HairStrandImageInfo> BottomRightPhoto { get; set; }
        public string BottomRightStrandDiameter { get; set; }
        public string BottomRightHealthText { get; set; }
        public List<HealthModel> Health { get; set; }
        public List<Observation> Observation { get; set; }
        public List<ObsElasticity> obsElasticities { get; set; }
        public List<ObsChemicalProducts> obsChemicalProducts { get; set; }
        public List<ObsPhysicalProducts> obsPhysicalProducts { get; set; }
        public List<ObsDamage> obsDamages { get; set; }
        public List<ObsBreakage> obsBreakages { get; set; }
        public List<ObsSplitting> obsSplittings { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }

    public class CrownStrandAdmin
    {
        public List<HairStrandImageInfo> CrownPhoto { get; set; }
        public string CrownStrandDiameter { get; set; }
        public string CrownHealthText { get; set; }
        public List<HealthModel> Health { get; set; }
        public List<Observation> Observation { get; set; }
        public List<ObsElasticity> obsElasticities { get; set; }
        public List<ObsChemicalProducts> obsChemicalProducts { get; set; }
        public List<ObsPhysicalProducts> obsPhysicalProducts { get; set; }
        public List<ObsDamage> obsDamages { get; set; }
        public List<ObsBreakage> obsBreakages { get; set; }
        public List<ObsSplitting> obsSplittings { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }

    public class LeftStrand
    {
        public string LeftPhoto { get; set; }
        public string LeftStrandDiameter { get; set; }
        public string LeftHealthText { get; set; }
        public Observation Observation { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }

    public class BottomStrand
    {
        public string BottomPhoto { get; set; }
        public string BottomStrandDiameter { get; set; }
        public string BottomHealthText { get; set; }
        public Observation Observation { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }

    public class RightStrand
    {
        public string RightPhoto { get; set; }
        public string RightStrandDiameter { get; set; }
        public string RightHealthText { get; set; }
        public Observation Observation { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }

    public class TopStrand
    {
        public string TopPhoto { get; set; }
        public string TopStrandDiameter { get; set; }
        public string TopHealthText { get; set; }
        public Observation Observation { get; set; }
        public Pororsity Pororsity { get; set; }
        public Elasticity Elasticity { get; set; }
    }


    public class Elasticity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsTopLeft { get; set; }
        public bool IsTopRight { get; set; }
        public bool IsBottomLeft { get; set; }
        public bool IsBottomRight { get; set; }
        public bool IsCrown { get; set; }
    }

    public class Pororsity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsTopLeft { get; set; }
        public bool IsTopRight { get; set; }
        public bool IsBottomLeft { get; set; }
        public bool IsBottomRight { get; set; }
        public bool IsCrown { get; set; }
    }

    public class Observation
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsTopLeft { get; set; }
        public bool IsTopRight { get; set; }
        public bool IsBottomLeft { get; set; }
        public bool IsBottomRight { get; set; }
        public bool IsCrown { get; set; }
    }

    public class ObsElasticity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsTopLeft { get; set; }
        public bool IsTopRight { get; set; }
        public bool IsBottomLeft { get; set; }
        public bool IsBottomRight { get; set; }
        public bool IsCrown { get; set; }
    }

    public class ObsChemicalProducts
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsTopLeft { get; set; }
        public bool IsTopRight { get; set; }
        public bool IsBottomLeft { get; set; }
        public bool IsBottomRight { get; set; }
        public bool IsCrown { get; set; }
    }

    public class ObsPhysicalProducts
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsTopLeft { get; set; }
        public bool IsTopRight { get; set; }
        public bool IsBottomLeft { get; set; }
        public bool IsBottomRight { get; set; }
        public bool IsCrown { get; set; }
    }

    public class ObsDamage
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsTopLeft { get; set; }
        public bool IsTopRight { get; set; }
        public bool IsBottomLeft { get; set; }
        public bool IsBottomRight { get; set; }
        public bool IsCrown { get; set; }
    }

    public class ObsBreakage
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsTopLeft { get; set; }
        public bool IsTopRight { get; set; }
        public bool IsBottomLeft { get; set; }
        public bool IsBottomRight { get; set; }
        public bool IsCrown { get; set; }
    }

    public class ObsSplitting
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsTopLeft { get; set; }
        public bool IsTopRight { get; set; }
        public bool IsBottomLeft { get; set; }
        public bool IsBottomRight { get; set; }
        public bool IsCrown { get; set; }
    }

    public class RecommendedProducts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }

    }

    public class RecommendedProductsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public int ProductId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }
    }

    public class RecommendedIngredients
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IngredientId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }

    }

    public class RecommendedRegimens
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegimenId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }

    }

    public class RecommendedVideos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MediaLinkEntityId { get; set; }
        public string ThumbNail { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }

        public int HairProfileId { get; set; }

    }

    public class HairProfileCustomerModel
    {
        public string UserId { get; set; }
        public string ConsultantNotes { get; set; }
        public string RecommendationNotes { get; set; }
        public string HairId { get; set; }
        public string UserName { get; set; }
        public bool IsBasicHHCP { get; set; }
        public string HealthSummary { get; set; }
        public string UserUploadedImage { get; set; }
        public TopLeftAdmin TopLeft { get; set; }
        public TopRightAdmin TopRight { get; set; }
        public BottomLeftAdmin BottomLeft { get; set; }
        public BottomRightAdmin BottomRight { get; set; }
        public CrownStrandAdmin CrownStrand { get; set; }
        public List<RecommendedVideosCustomer> RecommendedVideos { get; set; }
        public List<RecommendedProductsCustomer> RecommendedProducts { get; set; }
        public List<RecommendedProductsStylingModel> RecommendedProductsStyling { get; set; }
        public List<RecommendedIngredientsCustomer> RecommendedIngredients { get; set; }
        public List<RecommendedToolsCustomer> RecommendedTools { get; set; }
        public List<RecommendedRegimensCustomer> RecommendedRegimens { get; set; }
        public List<RecommendedStylistCustomer> recommendedStylistCustomers { get; set; }
        public List<RecommendedVideosCustomer> RecommendedStyleRecipeVideos { get; set; }
        public List<RecommendedProductsCustomer> RecommendedProductsStyleRecipe { get; set; }
        public QuestionaireSelectedAnswer SelectedAnswers { get; set; }
        public JObject AIResultDecoded { get; set; }
        public string AIResult { get; set; }
        public string AIResultNew { get; set; }
        public JObject AIResultNewDecoded { get; set; }
        public string HairTextureLabelAIResult { get; set; }
        public string HairTypeLabelAIResult { get; set; }
        public string LabelAIResult { get; set; }
        public int HairProfileId { get; set; }
        public string HairAnalysisNotes { get; set; }
        public int? CountAIResults { get; set; }
        public bool? IsVersion2 { get; set; }
        public JObject AIResultTextureDecoded { get; set; }
        public bool? IsAIV2Mobile { get; set; }
        public string UserAIImage { get; set; }
        public bool? IsViewEnabled { get; set; }
        public bool? IsRequestedFromCustomer { get; set; }
        public string HairStyle { get; set; }
        public bool IsAlreadyShareHHCP { get; set; }
        public bool HasHHCPSharedWithMe { get; set; }
        public HairProfileAnayst HairAnalyst { get; set; }
    }
    public class HairProfileAnayst
    {
        public int HairAnalystId { get; set; }
        public string AnalystName { get; set; }
        public string AnalystImage { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class RecommendedProductsStyle
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public int ProductId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }
        public string ImageName { get; set; }
        public string ProductLink { get; set; }
        public string ProductDetails { get; set; }
    }
    public class QuestionaireSelectedAnswer
    {
        public string UserEmail { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string TypeId { get; set; }
        public string TypeDescription { get; set; }
        public string TextureId { get; set; }
        public string TextureDescription { get; set; }
        public string HealthId { get; set; }
        public string HealthDescription { get; set; }
        public string PorosityId { get; set; }
        public string PorosityDescription { get; set; }
        public string ElasticityId { get; set; }
        public string ElasticityDescription { get; set; }
        public string DensityId { get; set; }
        public string DensityDescription { get; set; }
        public List<string> Goals { get; set; }
        public List<string> Challenges { get; set; }
    }

    public class RecommendedStylistCustomer
    {
        public int Id { get; set; }
        public int StylistId { get; set; }
        public int HairProfileId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public List<StylistCustomerModel> Stylist { get; set; }
    }

    public class StylistCustomerModel
    {
        public string StylistName { get; set; }
        public string Salon { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Instagram { get; set; }
    }

    public class RecommendedProductsStylingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public int ProductId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }
        public string ProductParentName { get; set; }
        public List<ProductsTypesStylingModels> ProductsTypes { get; set; }
    }

    public class ProductsTypesStylingModels
    {
        public string ProductTypeName { get; set; }
        public int? ProductId { get; set; }
        public List<ProductsStylingModels> Products { get; set; }
    }
    public class ProductsStylingModels
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string ProductLink { get; set; }
        public string BrandName { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductType { get; set; }
        public string ProductDetails { get; set; }
        public string ProductName { get; set; }
        public List<HairChallengesModel> HairChallenge { get; set; }
        public List<HairGoalsModel> HairGoals { get; set; }
    }

    public class RecommendedVideosCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MediaLinkEntityId { get; set; }
        public string ThumbNail { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }
        public List<string> Videos { get; set; }
    }
	public class RecommendedProductsCustomer
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string BrandName { get; set; }
		public int ProductId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public bool? IsActive { get; set; }
		public int HairProfileId { get; set; }
		public string ProductParentName { get; set; }
		public List<ProductsTypesModels> ProductsTypes { get; set; }
	}
	public class ProductsTypesModels
	{
		public string ProductTypeName { get; set; }
		public int? ProductId { get; set; }
		public List<ProductsModels> Products { get; set; }

	}
	public class RecommendedIngredientsCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IngredientId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }
        public List<IngredientsModels> Ingredients { get; set; }
    }
    public class RecommendedRegimensCustomer
    {
        public int RegimenId { get; set; }
        public string RegimenName { get; set; }
        public string RegimenTitle { get; set; }
        public string Description { get; set; }
        public List<RegimenStepsModel> RegimenSteps { get; set; }
    }
    public class RegimenStepsModel
    {
        public string RegimenStepPhoto { get; set; }
        public string RegimenStepInstruction { get; set; }
    }
    public class ProductsModels
    {
		public string ImageName { get; set; }
		public string ProductLink { get; set; }
		public string BrandName { get; set; }
		public string ProductTypeName { get; set; }
		public string ProductType { get; set; }
		public string ProductDetails { get; set; }
		public string ProductName { get; set; }
        public List<HairChallengesModel> HairChallenge { get; set; }
        public List<HairGoalsModel> HairGoals { get; set; }
    }
    public class HairChallengesModel
    {
        [JsonProperty(PropertyName = "HairChallengeId")]
        public int HairChallengeId { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "IsActive")]
        public bool? IsActive { get; set; }
        [JsonProperty(PropertyName = "CreatedOn")]
        public DateTime? CreatedOn { get; set; }
    }

    public class HairGoalsModel
    {
        [JsonProperty(PropertyName = "HairGoalId")]
        public int HairGoalId { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "IsActive")]
        public bool? IsActive { get; set; }
        [JsonProperty(PropertyName = "CreatedOn")]
        public DateTime? CreatedOn { get; set; }
        [JsonProperty(PropertyName = "HairGoalsId")]
        public int HairGoalsId { get; set; }
    }
    public class IngredientsModels
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
    }

    public class QuestionaireModel
    {
        public string Userid { get; set; }
        public bool? IsExist { get; set; }
        public int? QA { get; set; }
        public int? HHCPCount { get; set; }
        public Guid? PaymentId { get; set; }

        public string ProviderName { get; set; }
        public string ExpiredMessage { get; set; }
        public int? QuestionAnswerCount { get; set; }
    }

    public class HealthModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsTopLeft { get; set; }
        public bool IsTopRight { get; set; }
        public bool IsBottomLeft { get; set; }
        public bool IsBottomRight { get; set; }
        public bool IsCrown { get; set; }
    }


    public class QuestionAnswerModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int? QA { get; set; }
        public bool? IsDraft { get; set; }
        public DateTime? CreatedOn { get; set; }
        public ICollection<QuestionModels> questionModel { get; set; }
    }

    public class QuestionModels
    {
        public int? QuestionId { get; set; }
        public string Question { get; set; }
        public int SerialNo { get; set; }
        public string QAType { get; set; }
        public int? QA { get; set; }
        public ICollection<AnswerModel> AnswerList { get; set; }
    }

    public class AnswerModel
    {
        public int? AnswerId { get; set; }
        public string Answer { get; set; }
        public string Description { get; set; }
        public int QA { get; set; }
        public DateTime? QAdate { get; set; }
    }
    public class ToolsModels
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string ToolDetail { get; set; }


    }
    public class RecommendedToolsCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ToolId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }

        public List<ToolsModels> ToolList { get; set; }

    }

    public class Imagerequest
    {
        public string fileData { get; set; }
        public string email { get; set; }

    }
    public class DigitalAssessmentModel
    {
        public string Userid { get; set; }
        public string AIResult { get; set; }
        public string HairType { get; set; }
        public Guid? PaymentId { get; set; }
    }

    public class DigitalAssessmenRequesttModel
    {
        public string AIResult { get; set; }
        public string HairType { get; set; }
        public string ImageData { get; set; }
        public Guid? PaymentId { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Root
    {
        public string label { get; set; }
        public string labelValue { get; set; }
        public string hairTypeLabel { get; set; }
        public string hairTextureLabel { get; set; }
        public string imageLink { get; set; }
        public Score score { get; set; }
        public bool isError { get; set; }
        public object errorMessage { get; set; }
    }

    public class Score
    {
        [JsonProperty("Type 1")]
        public double Type1 { get; set; }

        [JsonProperty("Type 4b")]
        public double Type4b { get; set; }

        [JsonProperty("Type 2a")]
        public double Type2a { get; set; }
    }
    public class QuestionaireImage
    {
        public int QuestionaireId { get; set; }
        public string UserId { get; set; }
        public int? QuestionId { get; set; }
        public string Question { get; set; }
        public int? AnswerId { get; set; }
        public string Answer { get; set; }
        public string DescriptiveAnswer { get; set; }
        public int QA { get; set; }
        public string QADb { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }

    }

    public class HairProfileSelectModel
    {
        public int HairProfileId { get; set; }
        public string HairProfile { get; set; }
    }

    public class DigitalAssessmentMarketModel
    {
        public string userId { get; set; }
        public bool? isTrialOn { get; set; }
    }

    public class ProfileImagerequest
    {
        public string fileData { get; set; }

    }

    public class fileData
    {
        public string access_token { get; set; }
        public string ImageURL { get; set; }
        public string user_name { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string AccountNo { get; set; }
        public bool TwoFactor { get; set; }
        public string HairType { get; set; }
    }
    public class SharedHHCPModel
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "HairProfileId")]
        public int HairProfileId { get; set; }
        [JsonProperty(PropertyName = "HHCPName")]
        public string HHCPName { get; set; }
        [JsonProperty(PropertyName = "SharedBy")]
        public Guid SharedBy { get; set; }
        [JsonProperty(PropertyName = "SharedByUser")]
        public string SharedByUser { get; set; }
        [JsonProperty(PropertyName = "SharedWith")]
        public Guid SharedWith { get; set; }
        [JsonProperty(PropertyName = "SharedWithUser")]
        public string SharedWithUser { get; set; }
        [JsonProperty(PropertyName = "SharedOn")]
        public string SharedOn { get; set; }
        [JsonProperty(PropertyName = "RevokedOn")]
        public string RevokedOn { get; set; }
        [JsonProperty(PropertyName = "IsRevoked")]
        public bool IsRevoked { get; set; }
        [JsonProperty(PropertyName = "UserEmail")]
        public string UserEmail { get; set; }
    }

}
