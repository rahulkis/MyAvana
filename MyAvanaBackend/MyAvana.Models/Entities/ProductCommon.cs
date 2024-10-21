using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ProductCommon
    {
        [Key]
        public int ProductCommonId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("HairTypeId")]
        public int? HairTypeId { get; set; }

        public virtual HairType HairType { get; set; }

        [ForeignKey("HairChallengeId")]
        public int? HairChallengeId { get; set; }

        public virtual HairChallenges HairChallenges { get; set; }

        [ForeignKey("ProductIndicatorId")]
        public int? ProductIndicatorId { get; set; }

        public virtual ProductIndicator ProductIndicator { get; set; }

        [ForeignKey("ProductTagsId")]
        public int? ProductTagsId { get; set; }

        public virtual ProductTags ProductTags { get; set; }

        [ForeignKey("ProductClassificationId")]
        public int? ProductClassificationId { get; set; }

        public virtual ProductClassification ProductClassification { get; set; }

        [ForeignKey("Id")]
        public int? ProductEntityId { get; set; }

        public virtual ProductEntity ProductEntity { get; set; }

        [ForeignKey("Id")]
        public int? ProductTypeId { get; set; }

        public virtual ProductType ProductType { get; set; }

        [ForeignKey("HairGoalId")]
        public int? HairGoalId { get; set; }
        public virtual HairGoal HairGoal { get; set; }


        [ForeignKey("BrandClassificationId")]
        public int? BrandClassificationId { get; set; }

        public virtual BrandClassification BrandClassification { get; set; }

        [ForeignKey("CustomerPreferenceId")]
        public int? CustomerPreferenceId { get; set; }

        public virtual CustomerPreference CustomerPreference { get; set; }

        [ForeignKey("Id")]
        public int? HairStylesId { get; set; }
        public virtual HairStyles HairStyles { get; set; }

        [ForeignKey("ProductRecommendationStatusId")]
        public int? ProductRecommendationStatusId { get; set; }

        public virtual ProductRecommendationStatus ProductRecommendationStatus { get; set; }

        [ForeignKey("MolecularWeightId")]
        public int? MolecularWeightId { get; set; }

        public virtual MolecularWeight MolecularWeight { get; set; }
        //[ForeignKey("IngedientsEntityId")]
        //public int? IngedientsEntityId { get; set; }

        //public virtual IngedientsEntity IngedientsEntity { get; set; }
    }
}
