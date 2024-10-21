using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class HairProfile
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string ConsultantNotes { get; set; }
        public string RecommendationNotes { get; set; }
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

        public List<Health> Health { get; set; }
        public List<Observation> Observation { get; set; }
        public List<Pororsity> Pororsity { get; set; }
        public List<Elasticity> Elasticity { get; set; }

        public string TempHealth { get; set; }

        public string TempObservation { get; set; }
        public string TempObservationElasticity { get; set; }
        public string TempObservationChemicalProduct { get; set; }
        public string TempObservationPhysicalProduct { get; set; }
        //public string TempObservationDamage { get; set; }
        public string TempObservationBreakage { get; set; }
        public string TempObservationSplitting { get; set; }

        public string TempPororsity { get; set; }
        public string TempElasticity { get; set; }

        public List<IFormFile> TopLeftFile { get; set; }
        public List<string> TopLeftFile1 { get; set; }
        public List<IFormFile> TopRightFile { get; set; }
        public List<IFormFile> BottomLeftFile { get; set; }
        public List<IFormFile> BottomRightFile { get; set; }
        public List<IFormFile> CrownFile { get; set; }

        public string TopLeftPhoto { get; set; }
        public string TopRightPhoto { get; set; }
        public string BottomLeftPhoto { get; set; }
        public string BottomRightPhoto { get; set; }
        public string CrownPhoto { get; set; }

        public List<RecommendedProducts> RecommendedProducts { get; set; }
        public List<RecommendedIngredients> RecommendedIngredients { get; set; }
        public List<RecommendedCategory> RecommendedCategories { get; set; }
        public List<RecommendedProductTypes> RecommendedProductTypes { get; set; }
        public List<RecommendedTools> RecommendedTools { get; set; }
        public List<RecommendedRegimens> RecommendedRegimens { get; set; }
        public List<RecommendedVideos> RecommendedVideos { get; set; }
        //public List<RecommendedStyleRecipeVideos> RecommendedStyleRecipeVideos { get; set; }
        public QuestionaireSelectedAnswer SelectedAnswer { get; set; }
        public string TempRecommendedProducts { get; set; }
        public string TempAllRecommendedProductsEssential { get; set; }
        public string TempAllRecommendedProductsStyling { get; set; }
        public string TempRecommendedCategories { get; set; }
        public string TempRecommendedProductTypes { get; set; }
        public string TempRecommendedProductsStylings { get; set; }
        public string TempRecommendedIngredients { get; set; }

        public string TempRecommendedTools { get; set; }
        public string TempRecommendedRegimens { get; set; }
        public string TempRecommendedVideos { get; set; }
        public string TempRecommendedStylist { get; set; }
        public string TempSelectedAnswer { get; set; }
        public string TempRecommendedStyleRecipeVideos { get; set; }
        public string TempRecommendedProductsStyleRecipe { get; set; }
        public string HairStyleId { get; set; }
        public string SaveType { get; set; }
        public string TabNo { get; set; }
        public bool NotifyUser { get; set; }
        public int? IsNewHHCP { get; set; }
        public string HairAnalysisNotes { get; set; }
        public string MyNotes { get; set; }
        public string LoginUserId { get; set; }
        public int CreatedBy { get; set; }
        public string HairProfileId { get; set; }
        public int? QA { get; set; }
        public int? ModifiedBy { get; set; }
    }
    public class HairProfileAnayst
    {
        public int HairAnalystId { get; set; }
        public string AnalystName { get; set; }
        public string AnalystImage { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class HairProfileAdminModel
    {
        public string UserId { get; set; }
        public string ConsultantNotes { get; set; }
        public string RecommendationNotes { get; set; }
        public string HairId { get; set; }
        public string HealthSummary { get; set; }
        public TopLeftAdmin TopLeft { get; set; }
        public TopRightAdmin TopRight { get; set; }
        public BottomLeftAdmin BottomLeft { get; set; }
        public BottomRightAdmin BottomRight { get; set; }
        public CrownStrandAdmin CrownStrand { get; set; }
        public List<RecommendedVideos> RecommendedVideos { get; set; }
        public List<RecommendedProductsModel> RecommendedProducts { get; set; }
        public List<RecommendedProductsStylingModel> RecommendedProductsStyling { get; set; }
        public List<RecommendedProductsModel> AllRecommendedProductsEssential { get; set; }
        public List<RecommendedProductsStylingModel> AllRecommendedProductsStyling { get; set; }
        public List<RecommendedIngredients> RecommendedIngredients { get; set; }

        public List<RecommendedTools> RecommendedTools { get; set; }
        public List<RecommendedRegimens> RecommendedRegimens { get; set; }
        public List<RecommendedStylist> RecommendedStylist { get; set; }
        public QuestionaireSelectedAnswer SelectedAnswers { get; set; }
        public List<RecommendedStyleRecipeVideos> RecommendedStyleRecipeVideos { get; set; }
        public List<RecommendedProductsStyleRecipe> RecommendedProductsStyleRecipe { get; set; }
        public string HairStyleId { get; set; }
        public string TabNo { get; set; }
        public int? IsNewHHCP { get; set; }
        public string HairAnalysisNotes { get; set; }
        public int[] VideoCategoryIds { get; set; }
        public int[] StyleRecipeVideoCategoryIds { get; set; }
        public string LoginUserId { get; set; }
        public string MyNotes { get; set; }
        public int? CreatedBy { get; set; }
        public string HairProfileId { get; set; }
        //  public List<int> MatchingProductTypeIds { get; set; }
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
        public List<int?> ProductClassifications { get; set; }
        public List<int?> HairChallenges { get; set; }
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

    public class Health
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
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
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
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
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
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
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
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
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
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
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
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsTopLeft { get; set; }
        public bool IsTopRight { get; set; }
        public bool IsBottomLeft { get; set; }
        public bool IsBottomRight { get; set; }
        public bool IsCrown { get; set; }
    }

    public class RecommendedCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public bool IsHealthyRegimen { get; set; }
        public bool IsStylingRegimen { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }
    }

    public class RecommendedProductTypes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }
        public bool IsHealthyRegimen { get; set; }
        public bool IsStylingRegimen { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }
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
        public string ProductParentName { get; set; }
        public List<ProductsTypesModels> ProductsTypes { get; set; }
    }

    public class ProductsTypesModels
    {
        public string ProductTypeName { get; set; }
        public int? ProductId { get; set; }
        public List<ProductsModels> Products { get; set; }

    }

    public class ProductsModels
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string ProductLink { get; set; }
        public string BrandName { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductType { get; set; }
        public string ProductDetails { get; set; }
        public string ProductName { get; set; }
        public List<int?> ProductClassifications { get; set; }
        public List<int?> HairChallenges { get; set; }
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

    public class RecommendedTools
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ToolId { get; set; }
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

    public class RecommendedStylist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StylistId { get; set; }
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
    public class RecommendedStyleRecipeVideos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MediaLinkEntityId { get; set; }
        public string ThumbNail { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }

    }
    public class RecommendedProductsStyleRecipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int HairProfileId { get; set; }
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

    public class HairProfileCustomersModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? CustomerType { get; set; }
        public string CustomerTypeDesc { get; set; }
    }

    public class HairProfileCustomerModel
    {
        public int? SalonId { get; set; }
        public int HairProfileId { get; set; }
        public string UserId { get; set; }
        public string ConsultantNotes { get; set; }
        public string RecommendationNotes { get; set; }
        public string HairId { get; set; }
        public string UserName { get; set; }
        public string UserUploadedImage { get; set; }
        public string HealthSummary { get; set; }
        public string AIResult { get; set; }
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
        public string AIResultNew { get; set; }
        public JObject AIResultNewDecoded { get; set; }
        public string HairTextureLabelAIResult { get; set; }
        public string HairTypeLabelAIResult { get; set; }
        public string LabelAIResult { get; set; }
        public int? CustomerTypeId { get; set; }
        public string HairAnalysisNotes { get; set; }
        public string CustomerTypeDesc { get; set; }
        public string LoginUserId { get; set; }
        public string MyNotes { get; set; }
        public string UserAIImage { get; set; }
        public bool? IsVersion2 { get; set; }
        public JObject AIResultTextureDecoded { get; set; }
        public bool? IsAIV2Mobile { get; set; }

        public string SalonNotes { get; set; }
        public bool? IsPublicSalonNote { get; set; }
        public List<SalonNotesModel> SalonNotesModel { get; set; }
        public bool? IsViewEnabled { get; set; }
        public int? CountAIResults { get; set; }
        public string HairStyle { get; set; }
        public HairProfileAnayst HairAnalyst { get; set; }
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
    public class Item2
    {
        [JsonProperty("Type 3c")]
        public double Type3c { get; set; }

        [JsonProperty("Type 1")]
        public double Type1 { get; set; }

        [JsonProperty("Type 4b")]
        public double Type4b { get; set; }
    }

    public class RootAI
    {
        public string item1 { get; set; }
        public Item2 item2 { get; set; }
        public string item3 { get; set; }
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


    public class IngredientsModels
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
    }

    public class ToolsModels
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string ToolDetail { get; set; }


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

    public class HairProfileSelectModel
    {
        public int HairProfileId { get; set; }
        public string HairProfile { get; set; }
        public string QA { get; set; }
    }

    public class CreateHHCPModel
    {
        public string Userid { get; set; }
    }
    public class HHCPParam
    {
        public string Userid { get; set; }
        public Guid? PaymentId { get; set; }
        public int? HairProfileId { get; set; }
        public bool? IsIPad { get; set; }
    }
    public class MessageTemplateModel
    {
        public int MessageTemplateId { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateSubject { get; set; }
        public string TemplateBody { get; set; }
    }

    public class EnableDisableProfileModel
    {
        public int HairProfileId { get; set; }
        public bool? IsViewEnabled { get; set; }
    }
    public class HairStrandUploadNotificationModel
    {
        public int Id { get; set; }
        public string SalonName { get; set; }

        public bool? IsRead { get; set; }

        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string  CreatedDate { get; set; }
        public string UserId { get; set; }
        public int HairProfileId { get; set; }
    }

    public class DailyRoutineTrackerNotificationModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string HairStyle { get; set; }
        public string ProfileImage { get; set; }
        public string Notes { get; set; }
        public DateTime TrackTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        public string CurrentMood { get; set; }
        public string GuidanceNeeded { get; set; }
        public bool? IsRead { get; set; }
        public string CreatedDate { get; set; }
    }
    public class DigitalAssessmentModel
    {
        public string Userid { get; set; }
        public string AIResult { get; set; }
        public string HairType { get; set; }
        public Guid? PaymentId { get; set; }       
        public string ImageData { get; set; }
    }
    public class DigitalAssessmenRequesttModel
    {
        public string AIResult { get; set; }
        public string HairType { get; set; }
        public string ImageData { get; set; }
        public Guid? PaymentId { get; set; }
    }

}
