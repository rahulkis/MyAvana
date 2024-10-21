using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyAvana.Models.Entities;
using MyAvanaApi.Models.Entities;
using System;

namespace MyAvana.DAL.Auth
{
    public class AvanaContext : IdentityDbContext<UserEntity, UserRoleEntity, Guid>
    {
        public AvanaContext(DbContextOptions<AvanaContext> options)
           : base(options)
        {
            // Database.EnsureCreated();
            //Database.SetCommandTimeout(9000);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GenericSetting>()
                .HasKey(c => new { c.SettingID, c.AdminAccountId, c.SettingName, c.SubSettingName });
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<UserEmails> UserEmails { get; set; }
        public virtual DbSet<UsersCode> Codes { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<GenericSetting> GenericSettings { get; set; }
        public virtual DbSet<PromoCode> PromoCodes { get; set; }
        public virtual DbSet<UserHistory> UserHistories { get; set; }
        public virtual DbSet<ProductEntity> ProductEntities { get; set; }
        public virtual DbSet<SubscriptionsEntity> SubscriptionsEntities { get; set; }
        public virtual DbSet<PaymentEntity> PaymentEntities { get; set; }
        public virtual DbSet<CodeEntity> CodeEntities { get; set; }
        public virtual DbSet<MediaLinkEntity> MediaLinkEntities { get; set; }
        public virtual DbSet<UsersTicketsEntity> UsersTicketsEntities { get; set; }
        public virtual DbSet<BlogArticle> BlogArticles { get; set; }
        public virtual DbSet<WebLogin> WebLogins { get; set; }
        public virtual DbSet<IngedientsEntity> IngedientsEntities { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<Regimens> Regimens { get; set; }
        public virtual DbSet<RegimenSteps> RegimenSteps { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Questionaire> Questionaires { get; set; }
        public virtual DbSet<Elasticity> Elasticities { get; set; }
        public virtual DbSet<HairElasticity> HairElasticities { get; set; }
        public virtual DbSet<HairObservation> HairObservations { get; set; }
        public virtual DbSet<HairPorosity> HairPorosities { get; set; }
        public virtual DbSet<HairProfile> HairProfiles { get; set; }
        public virtual DbSet<HairStrands> HairStrands { get; set; }
        public virtual DbSet<Observation> Observations { get; set; }
        public virtual DbSet<Pororsity> Pororsities { get; set; }
        public virtual DbSet<RecommendedIngredients> RecommendedIngredients { get; set; }
        public virtual DbSet<RecommendedProducts> RecommendedProducts { get; set; }
        public virtual DbSet<RecommendedRegimens> RecommendedRegimens { get; set; }
        public virtual DbSet<RecommendedVideos> RecommendedVideos { get; set; }
        public virtual DbSet<Stylist> Stylists { get; set; }
        public virtual DbSet<StylistSpecialty> StylistSpecialties { get; set; }
        public virtual DbSet<StylishCommon> StylishCommons { get; set; }
        public virtual DbSet<HairStrandsImages> HairStrandsImages { get; set; }
        public virtual DbSet<Health> Healths { get; set; }
        public virtual DbSet<HairHealth> HairHealths { get; set; }
        public virtual DbSet<ObsElasticity> ObsElasticities { get; set; }
        public virtual DbSet<ObsChemicalProducts> ObsChemicalProducts { get; set; }
        public virtual DbSet<ObsPhysicalProducts> ObsPhysicalProducts { get; set; }
        public virtual DbSet<ObsDamage> ObsDamage { get; set; }
        public virtual DbSet<ObsBreakage> ObsBreakage { get; set; }
        public virtual DbSet<ObsSplitting> ObsSplitting { get; set; }
        public virtual DbSet<HairType> HairTypes { get; set; }
        public virtual DbSet<HairChallenges> HairChallenges { get; set; }
        public virtual DbSet<ProductIndicator> ProductIndicator { get; set; }
        public virtual DbSet<ProductTags> ProductTags { get; set; }
        public virtual DbSet<ProductClassification> ProductClassification { get; set; }
        public virtual DbSet<ProductCommon> ProductCommons { get; set; }
        public virtual DbSet<ProductTypeCategory> ProductTypeCategories { get; set; }
        public virtual DbSet<RecommendedProductsStylingRegimen> RecommendedProductsStyleRegimens { get; set; }
        public virtual DbSet<RecommendedStylist> RecommendedStylists { get; set; }
        public virtual DbSet<GroupPost> GroupPosts { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<LikePosts> LikePosts { get; set; }
        public virtual DbSet<VideoCategory> VideoCategories { get; set; }
        public virtual DbSet<CustomerHairGoals> CustomerHairGoals { get; set; }
        public virtual DbSet<AdditionalHairInfo> AdditionalHairInfo { get; set; }
        public virtual DbSet<CustomerHairChallenge> CustomerHairChallenge { get; set; }
        public virtual DbSet<DailyRoutineTracker> DailyRoutineTracker { get; set; }
        public virtual DbSet<Tools> Tools { get; set; }
        public virtual DbSet<TrackingDetails> TrackingDetails { get; set; }
        public virtual DbSet<HairStyles> HairStyles { get; set; }
        public virtual DbSet<DailyRoutineProducts> DailyRoutineProducts { get; set; }
        public virtual DbSet<DailyRoutineIngredients> DailyRoutineIngredients { get; set; }
        public virtual DbSet<DailyRoutineRegimens> DailyRoutineRegimens { get; set; }
        public virtual DbSet<DailyRoutineHairStyles> DailyRoutineHairStyles { get; set; }
        public virtual DbSet<RecommendedTools> RecommendedTools { get; set; }
        public virtual DbSet<RecommendedCategory> RecommendedCategories { get; set; }
        public virtual DbSet<RecommendedProductTypes> RecommendedProductTypes { get; set; }
        public virtual DbSet<CustomerMessage> CustomerMessage { get; set; }
        public virtual DbSet<PrePopulateTypes> PrePopulateTypes { get; set; }
        public virtual DbSet<LiveConsultationStatus> LiveConsultationStatus { get; set; }
        public virtual DbSet<LiveConsultationUserDetails> LiveConsultationUserDetails { get; set; }
        public virtual DbSet<LiveConsultationCustomer> LiveConsultationCustomer { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<ScheduleTime> ScheduleTimes { get; set; }
        public virtual DbSet<TimeZones> TimeZones { get; set; }
        public virtual DbSet<UserEntity> UserEntity { get; set; }
        public virtual DbSet<Salons> Salons { get; set; }
        public virtual DbSet<SalonsOwner> SalonsOwners { get; set; }
        public virtual DbSet<SalonsStylist> SalonsStylists { get; set; }
        public virtual DbSet<SalonVideo> SalonVideos { get; set; }
        public virtual DbSet<AlexaFAQ> AlexaFAQs { get; set; }
        public virtual DbSet<ReportPosts> ReportPosts { get; set; }
        public virtual DbSet<StreakCountTracker> StreakCountTrackers { get; set; }
        public virtual DbSet<CustomerAIResult> CustomerAIResults { get; set; }
        public virtual DbSet<EducationTip> EducationTips { get; set; }
        public virtual DbSet<MappingHairGoalAndProductTag> MappingHairGoalAndProductTags { get; set; }
        public virtual DbSet<HairGoal> HairGoals { get; set; }
        public virtual DbSet<CustomerType> CustomerTypes { get; set; }
        public virtual DbSet<CustomerAuthentication> CustomerAuthentications { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<ProductTypeHairChallenge> ProductTypeHairChallenges { get; set; }
        public virtual DbSet<ProductTypeHairGoal> ProductTypeHairGoals { get; set; }
        public virtual DbSet<BrandClassification> BrandClassifications { get; set; }
        public virtual DbSet<ArticleHairStyle> ArticleHairStyles { get; set; }
        public virtual DbSet<ArticleProduct> ArticleProducts { get; set; }
        public virtual DbSet<StylistNotesHHCP> StylistNotesHHCPs { get; set; }
        public virtual DbSet<ShopifySampleData> ShopifySampleDatas { get; set; }
        public virtual DbSet<CustomerTypeHistory> CustomerTypeHistory { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<DigitalAnalysisTrial> DigitalAnalysisTrial { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<ClassificationBrand> ClassificationBrands { get; set; }
        public virtual DbSet<BrandHairChallenge> BrandHairChallenges { get; set; }
        public virtual DbSet<BrandHairGoal> BrandHairGoals { get; set; }
        public virtual DbSet<BrandHairType> BrandHairTypes { get; set; }
        public virtual DbSet<Brands> Brands { get; set; }
        public virtual DbSet<BrandTag> BrandTags { get; set; }
        public virtual DbSet<MessageTemplate> MessageTemplates { get; set; }
        public virtual DbSet<UserCodes> UserCodes { get; set; }
        public virtual DbSet<SalonNotesHHCP> SalonNotesHHCP { get; set; }
        public virtual DbSet<CustomerPreference> CustomerPreference { get; set; }
        public virtual DbSet<HairAnalysisStatus> HairAnalysisStatus { get; set; }
        public virtual DbSet<StatusTracker> StatusTracker { get; set; }
        public virtual DbSet<HairAnalysisStatusHistory> HairAnalysisStatusHistory { get; set; }
        public virtual DbSet<AdminAuthentication> AdminAuthentications { get; set; }

        public virtual DbSet<HairStrandUploadNotification> HairStrandUploadNotification { get; set; }

        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<DiscountCodes> DiscountCodes { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<Moods> Moods { get; set; }
        public virtual DbSet<Guidances> Guidances { get; set; }
        public virtual DbSet<ArticleMoods> ArticleMoods { get; set; }
        public virtual DbSet<ArticleGuidances> ArticleGuidances { get; set; }
        public virtual DbSet<ShopifyRequest> ShopifyRequest { get; set; }
        public virtual DbSet<CustomerSubscriptionHistory> CustomerSubscriptionHistory { get; set; }
        //public virtual DbSet<MappingHairStyleandProductType> MappingHairStyleandProductType { get; set; }
        public virtual DbSet<MobileHelpFAQ> MobileHelpFAQ { get; set; }

        public virtual DbSet<HairState> HairState { get; set; }
        public virtual DbSet<MolecularWeight> MolecularWeight { get; set; }
        public virtual DbSet<BrandRecommendationStatus> BrandRecommendationStatus { get; set; }
        public virtual DbSet<BrandHairState> BrandHairState { get; set; }
        public virtual DbSet<BrandsBrandRecommendationStatus> BrandsBrandRecommendationStatus { get; set; }
        public virtual DbSet<BrandMolecularWeight> BrandMolecularWeight { get; set; }
        public virtual DbSet<ProductRecommendationStatus> ProductRecommendationStatus { get; set; }
        public virtual DbSet<ProductTypeHairStyles> ProductTypeHairStyles { get; set; }
        public virtual DbSet<HairScope> HairScope { get; set; }
        public virtual DbSet<TaggedUsers> TaggedUsers { get; set; }
        public virtual DbSet<ProductTypeScalpChallenge> ProductTypeScalpChallenge { get; set; }
        public virtual DbSet<RecommendedProductsStyleRecipe> RecommendedProductsStyleRecipe { get; set; }
        public virtual DbSet<RecommendedStyleRecipeVideos> RecommendedStyleRecipeVideos { get; set; }
        public virtual DbSet<StyleRecipeHairStyle> StyleRecipeHairStyle { get; set; }

        public virtual DbSet<MobileHelpTopic> MobileHelpTopic { get; set; }
        public virtual DbSet<MobileNotifications> MobileNotifications { get; set; }
        public virtual DbSet<SharedHHCP> SharedHHCP { get; set; }
        public virtual DbSet<HairAnalyst> HairAnalyst { get; set; }
        public virtual DbSet<InAppPayload> InAppPayload { get; set; }
        public virtual DbSet<HairChallengeVideoMapping> HairChallengeVideoMapping { get; set; }
        public virtual DbSet<HairGoalVideoMapping> HairGoalVideoMapping { get; set; }
    }

}