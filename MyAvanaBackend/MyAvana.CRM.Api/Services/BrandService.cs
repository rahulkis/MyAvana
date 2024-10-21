using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class BrandService:IBrandService
    {
        private readonly AvanaContext _context;
        private readonly IConfiguration configuration;
        private readonly Logger.Contract.ILogger _logger;
        public BrandService(AvanaContext avanaContext, IConfiguration configuration,Logger.Contract.ILogger logger)
        {
            _context = avanaContext;
            _logger = logger;
            this.configuration = configuration;
        }
        public Tags GetTagById(Tags Tag)
        {
            try
            {
                Tags objTag = _context.Tags.Where(x => x.TagId == Tag.TagId).FirstOrDefault();
                return objTag;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetTagById, TagId:" + Tag.TagId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public Tags SaveTag(Tags Tag)
        {
            try
            {
                var TagModel = _context.Tags.FirstOrDefault(x => x.TagId == Tag.TagId);
                if (TagModel != null)
                {
                    TagModel.Description = Tag.Description;
                }
                else
                {
                    Tags objType = new Tags();
                    objType.Description = Tag.Description;
                    objType.CreatedOn = DateTime.Now;
                    objType.IsActive = true;

                    _context.Add(objType);
                }
                _context.SaveChanges();
                return Tag;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveTag, TagId:" + Tag.TagId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<Tags> GetBrandTagList()
        {
            try
            {
                List<Tags> lstTags = _context.Tags.Where(x => x.IsActive == true).ToList();
                return lstTags;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetBrandTagList , Error: " + ex.Message, ex);
                return null;
            }
        }
        public bool DeleteTag(Tags Tag)
        {
            try
            {
                var objTag = _context.Tags.FirstOrDefault(x => x.TagId == Tag.TagId);
                {
                    if (objTag != null)
                    {
                        objTag.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteTag, TagId:" + Tag.TagId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public BrandsEntityModel SaveBrand(BrandsEntityModel brandEntity)
        {
            try
            {
                List<HairType> listHairType = JsonConvert.DeserializeObject<List<HairType>>(brandEntity.HairTypes);
                List<HairChallenges> listHairChallenges = JsonConvert.DeserializeObject<List<HairChallenges>>(brandEntity.HairChallenges);
                List<HairGoal> listHairGoals = JsonConvert.DeserializeObject<List<HairGoal>>(brandEntity.HairGoals);
                List<BrandTag> listProductTags = JsonConvert.DeserializeObject<List<BrandTag>>(brandEntity.BrandTags);
                List<BrandClassification> listBrandClassification = JsonConvert.DeserializeObject<List<BrandClassification>>(brandEntity.BrandClassification);
                List<HairState> listHairStates = JsonConvert.DeserializeObject<List<HairState>>(brandEntity.HairStates);
                List<MolecularWeight> listMolecularWeights = JsonConvert.DeserializeObject<List<MolecularWeight>>(brandEntity.MolecularWeights);
                List<BrandRecommendationStatus> listBrandRecommendationStatuses = JsonConvert.DeserializeObject<List<BrandRecommendationStatus>>(brandEntity.RecommendationStatuses);

                Brands objBrand = new Brands();
                if (brandEntity.BrandId != null && brandEntity.BrandId > 0)
                {
                    objBrand = _context.Brands.Where(x => x.BrandId == brandEntity.BrandId).FirstOrDefault();
                    objBrand.BrandName = brandEntity.BrandName;
                    objBrand.FeaturedIngredients = brandEntity.FeaturedIngredients;
                    objBrand.Rank = brandEntity.Rank;

                    List<BrandHairType> brandHairTypes = _context.BrandHairTypes.Where(x => x.BrandId == brandEntity.BrandId).ToList();
                    _context.BrandHairTypes.RemoveRange(brandHairTypes);

                    List<BrandHairChallenge> brandHairChallenge = _context.BrandHairChallenges.Where(x => x.BrandId == brandEntity.BrandId).ToList();
                    _context.BrandHairChallenges.RemoveRange(brandHairChallenge);

                    List<BrandHairGoal> brandHairGoal = _context.BrandHairGoals.Where(x => x.BrandId == brandEntity.BrandId).ToList();
                    _context.BrandHairGoals.RemoveRange(brandHairGoal);

                    List<ClassificationBrand> classificationBrand = _context.ClassificationBrands.Where(x => x.BrandId == brandEntity.BrandId).ToList();
                    _context.ClassificationBrands.RemoveRange(classificationBrand);

                    List<BrandTag> tags = _context.BrandTags.Where(x => x.BrandId == brandEntity.BrandId).ToList();
                    _context.BrandTags.RemoveRange(tags);

                    List<BrandHairState> hairstates = _context.BrandHairState.Where(x => x.BrandId == brandEntity.BrandId).ToList();
                    _context.BrandHairState.RemoveRange(hairstates);

                    List<BrandMolecularWeight> molecularWeights = _context.BrandMolecularWeight.Where(x => x.BrandId == brandEntity.BrandId).ToList();
                    _context.BrandMolecularWeight.RemoveRange(molecularWeights);
                    
                    List<BrandsBrandRecommendationStatus> recommendationStatuses = _context.BrandsBrandRecommendationStatus.Where(x => x.BrandId == brandEntity.BrandId).ToList();
                    _context.BrandsBrandRecommendationStatus.RemoveRange(recommendationStatuses);
                    
                    _context.SaveChanges();
                }
                else
                {
                    objBrand.BrandName = brandEntity.BrandName;
                    objBrand.IsActive = true;
                    objBrand.CreatedOn = DateTime.UtcNow;
                    objBrand.FeaturedIngredients = brandEntity.FeaturedIngredients;
                    objBrand.Rank = brandEntity.Rank;
                    _context.Add(objBrand);
                    _context.SaveChanges();
                }
                


                foreach (var spec in listHairType)
                {
                    BrandHairType objcommon = new BrandHairType();
                    objcommon.HairTypeId = spec.HairTypeId;
                    objcommon.IsActive = true;
                    objcommon.BrandId = objBrand.BrandId;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listHairChallenges)
                {
                    BrandHairChallenge objcommon = new BrandHairChallenge();
                    objcommon.HairChallengeId = spec.HairChallengeId;
                    objcommon.IsActive = true;
                    objcommon.BrandId = objBrand.BrandId;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listHairGoals)
                {
                    BrandHairGoal objcommon = new BrandHairGoal();
                    objcommon.HairGoalId = spec.HairGoalId;

                    objcommon.IsActive = true;
                    objcommon.BrandId = objBrand.BrandId;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listBrandClassification)
                {
                    ClassificationBrand objcommon = new ClassificationBrand();
                    objcommon.BrandClassificationId = spec.BrandClassificationId;
                    objcommon.IsActive = true;
                    objcommon.BrandId = objBrand.BrandId;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listProductTags)
                {
                    BrandTag objcommon = new BrandTag();
                    objcommon.TagsId = spec.TagsId;
                    objcommon.IsActive = true;
                    objcommon.BrandId = objBrand.BrandId;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }
                foreach (var spec in listHairStates)
                {
                    BrandHairState objcommon = new BrandHairState();
                    objcommon.HairStateId = spec.HairStateId;
                    objcommon.IsActive = true;
                    objcommon.BrandId = objBrand.BrandId;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }
                foreach (var spec in listMolecularWeights)
                {
                    BrandMolecularWeight objcommon = new BrandMolecularWeight();
                    objcommon.MolecularWeightId = spec.MolecularWeightId;
                    objcommon.IsActive = true;
                    objcommon.BrandId = objBrand.BrandId;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }
                foreach (var spec in listBrandRecommendationStatuses)
                {
                    BrandsBrandRecommendationStatus objcommon = new BrandsBrandRecommendationStatus();
                    objcommon.BrandRecommendationStatusId = spec.BrandRecommendationStatusId;
                    objcommon.IsActive = true;
                    objcommon.BrandId = objBrand.BrandId;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }
                _context.SaveChanges();
                return brandEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveBrand, Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool DeleteBrand(BrandsEntityModel brandEntity)
        {
            try
            {
                var objBrand = _context.Brands.FirstOrDefault(x => x.BrandId == brandEntity.BrandId);
                {
                    if (objBrand != null)
                    {
                        objBrand.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteBrand, Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public List<BrandModelList> GetBrandsList()
        {
            try
            {
                var result = _context.Brands.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();

                List<BrandModelList> lstBrandModel = result.Select(x => new BrandModelList
                {
                    BrandId = x.BrandId,
                    BrandName = x.BrandName,
                    FeaturedIngredients = x.FeaturedIngredients,
                    Rank = x.Rank,
                    HairTypes = string.Join(",", _context.BrandHairTypes.Where(p => p.BrandId == x.BrandId)
                     .Select(d => d.HairType.Description
                    ).ToArray()),
                    HairGoalsDes = string.Join(",", _context.BrandHairGoals.Where(p => p.BrandId == x.BrandId)
                     .Select(d => d.HairGoal.Description
                    ).ToArray()),
                    IsActive = x.IsActive,
                    HideInSearch = x.HideInSearch != null ? x.HideInSearch : false,
                    CreatedOn = x.CreatedOn,
                    HairChallenges = string.Join(",", _context.BrandHairChallenges.Where(p => p.BrandId == x.BrandId)
                    .Select(d => d.HairChallenges.Description
                    ).ToArray()),
                    Tags = string.Join(",", _context.BrandTags.Where(p => p.BrandId == x.BrandId)
                    .Select(d => d.Tags.Description
                    ).ToArray()),
                    BrandClassifications = string.Join(",", _context.ClassificationBrands.Where(p => p.BrandId == x.BrandId).Select(d => d.BrandClassification.Description
                    ).ToArray()),
                    HairStates = string.Join(",", _context.BrandHairState.Where(p => p.BrandId == x.BrandId)
                    .Select(d => d.HairState.Description
                    ).ToArray()),
                    RecommendationStatuses = string.Join(",", _context.BrandsBrandRecommendationStatus.Where(p => p.BrandId == x.BrandId)
                    .Select(d => d.BrandRecommendationStatus.Description
                    ).ToArray()),
                    MolecularWeights = string.Join(",", _context.BrandMolecularWeight.Where(p => p.BrandId == x.BrandId)
                    .Select(d => d.MolecularWeight.Description
                    ).ToArray()),
                }).ToList();

                return lstBrandModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetBrandsList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public BrandsEntityModel GetBrandDetailsById(BrandsEntityModel brandEntity)
        {
            try
            {
                BrandsEntityModel product = (from brand in _context.Brands
                                                  where brand.BrandId == brandEntity.BrandId
                                                  && brand.IsActive == true
                                                  select new BrandsEntityModel()
                                                  {
                                                      BrandId = brand.BrandId,                                                     
                                                      BrandName = brand.BrandName,
                                                      FeaturedIngredients = brand.FeaturedIngredients,
                                                      Rank = brand.Rank,
                                                      CreatedOn = brand.CreatedOn,
                                                      HairType = _context.BrandHairTypes.Where(x => x.BrandId == brandEntity.BrandId).ToList(),
                                                      HairChallenge = _context.BrandHairChallenges.Where(x => x.BrandId == brandEntity.BrandId).ToList(),
                                                      HairGoal = _context.BrandHairGoals.Where(x => x.BrandId == brandEntity.BrandId).ToList(),
                                                      BrandTag = _context.BrandTags.Where(x => x.BrandId == brandEntity.BrandId).ToList(),
                                                      BrandClassifications = _context.ClassificationBrands.Where(x => x.BrandId == brandEntity.BrandId).ToList(),
                                                      HairState = _context.BrandHairState.Where(x => x.BrandId == brandEntity.BrandId).ToList(),
                                                      BrandRecommendationStatus = _context.BrandsBrandRecommendationStatus.Where(x => x.BrandId == brandEntity.BrandId).ToList(),
                                                      MolecularWeight = _context.BrandMolecularWeight.Where(x => x.BrandId == brandEntity.BrandId).ToList(),
                                                  }).FirstOrDefault();

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetBrandDetailsById, Error: " + ex.Message, ex);
                return null;
            }
        }

        public Brands ShowHideBrand(Brands brandEntity)
        {

            var objBrand = _context.Brands.Where(x => x.BrandId == brandEntity.BrandId).FirstOrDefault();

            try
            {
                if (objBrand != null)
                {
                    if (objBrand.HideInSearch == true)
                    {
                        objBrand.HideInSearch = false;
                    }
                    else
                    {
                        objBrand.HideInSearch = true;
                    }
                }
                _context.Update(objBrand);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: ShowHideBrand, Error: " + ex.Message, ex);
            }

            return objBrand;
        }
        public List<BrandList> GetAllBrandsList()
        {
            try
            {
                List<BrandList> lstBrandModel = _context.Brands.Where(x => x.IsActive == true).GroupBy(x => x.BrandName).Select(g => new BrandList
                {
                    BrandName = g.FirstOrDefault().BrandName,
                    BrandId = g.FirstOrDefault().BrandId
                }).ToList();


                return lstBrandModel;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetAllBrandsList, Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public List<HairState> GetBrandHairStateList()
        {
            try
            {
                List<HairState> hairState = _context.HairState.Where(x => x.IsActive == true).ToList();
                return hairState;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetBrandHairStateList , Error: " + ex.Message, ex);
                return null;
            }
        }
        public HairState SaveHairState(HairState hairState)
        {
            try
            {
                var TagModel = _context.HairState.FirstOrDefault(x => x.HairStateId == hairState.HairStateId);
                if (TagModel != null)
                {
                    TagModel.Description = hairState.Description;
                }
                else
                {
                    HairState hairStatemodel = new HairState();
                    hairStatemodel.Description = hairState.Description;
                    hairStatemodel.CreatedOn = DateTime.Now;
                    hairStatemodel.IsActive = true;

                    _context.Add(hairStatemodel);
                }
                _context.SaveChanges();
                return hairState;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveHairState, HairStateId:" + hairState.HairStateId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public HairState GetHairStateById(HairState hairState)
        {
            try
            {
                HairState hairStatemodel = _context.HairState.Where(x => x.HairStateId == hairState.HairStateId).FirstOrDefault();
                return hairStatemodel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairStateById, HairStateId:" + hairState.HairStateId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public bool DeleteHairState(HairState hairState)
        {
            try
            {
                var hairStatemodel = _context.HairState.FirstOrDefault(x => x.HairStateId == hairState.HairStateId);
                {
                    if (hairStatemodel != null)
                    {
                        hairStatemodel.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteHairState, HairStateId:" + hairState.HairStateId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public List<BrandRecommendationStatus> GetBrandRecommendationStatusList()
        {
            try
            {
                List<BrandRecommendationStatus> statuses = _context.BrandRecommendationStatus.Where(x => x.IsActive == true).ToList();
                return statuses;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetBrandRecommendationStatusList , Error: " + ex.Message, ex);
                return null;
            }
        }
        public BrandRecommendationStatus SaveBrandRecommendationStatus(BrandRecommendationStatus recommendationStatus)
        {
            try
            {
                var statusModel = _context.BrandRecommendationStatus.FirstOrDefault(x => x.BrandRecommendationStatusId == recommendationStatus.BrandRecommendationStatusId);
                if (statusModel != null)
                {
                    statusModel.Description = recommendationStatus.Description;
                }
                else
                {
                    BrandRecommendationStatus BrandRecommendationStatusmodel = new BrandRecommendationStatus();
                    BrandRecommendationStatusmodel.Description = recommendationStatus.Description;
                    BrandRecommendationStatusmodel.CreatedOn = DateTime.Now;
                    BrandRecommendationStatusmodel.IsActive = true;

                    _context.Add(BrandRecommendationStatusmodel);
                }
                _context.SaveChanges();
                return recommendationStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveBrandRecommendationStatus, BrandRecommendationStatusId:" + recommendationStatus.BrandRecommendationStatusId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public BrandRecommendationStatus GetBrandRecommmendationStatusById(BrandRecommendationStatus recommendationStatus)
        {
            try
            {
                BrandRecommendationStatus statusModel = _context.BrandRecommendationStatus.Where(x => x.BrandRecommendationStatusId == recommendationStatus.BrandRecommendationStatusId).FirstOrDefault();
                return statusModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetBrandRecommmendationStatusById, BrandRecommmendationStatusId:" + recommendationStatus.BrandRecommendationStatusId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public bool DeleteBrandRecommendationStatus(BrandRecommendationStatus recommendationStatus)
        {
            try
            {
                var statusModel = _context.BrandRecommendationStatus.FirstOrDefault(x => x.BrandRecommendationStatusId == recommendationStatus.BrandRecommendationStatusId);
                {
                    if (statusModel != null)
                    {
                        statusModel.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteBrandRecommendationStatus, BrandRecommendationStatusId:" + recommendationStatus.BrandRecommendationStatusId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public List<MolecularWeight> GetBrandMolecularWeightList()
        {
            try
            {
                List<MolecularWeight> weights = _context.MolecularWeight.Where(x => x.IsActive == true).ToList();
                return weights;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetBrandMolecularWeightList , Error: " + ex.Message, ex);
                return null;
            }
        }
        public MolecularWeight SaveMolecularWeight(MolecularWeight weight)
        {
            try
            {
                var WeightModel = _context.MolecularWeight.FirstOrDefault(x => x.MolecularWeightId == weight.MolecularWeightId);
                if (WeightModel != null)
                {
                    WeightModel.Description = weight.Description;
                }
                else
                {
                    MolecularWeight molecularWeightmodel = new MolecularWeight();
                    molecularWeightmodel.Description = weight.Description;
                    molecularWeightmodel.CreatedOn = DateTime.Now;
                    molecularWeightmodel.IsActive = true;

                    _context.Add(molecularWeightmodel);
                }
                _context.SaveChanges();
                return weight;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveMolecularWeight, MolecularWeightId:" + weight.MolecularWeightId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public MolecularWeight GetMolecularWeightById(MolecularWeight weight)
        {
            try
            {
                MolecularWeight weightmodel = _context.MolecularWeight.Where(x => x.MolecularWeightId == weight.MolecularWeightId).FirstOrDefault();
                return weightmodel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetMolecularWeightById, MolecularWeightId:" + weight.MolecularWeightId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public bool DeleteMolecularWeight(MolecularWeight weight)
        {
            try
            {
                var weightmodel = _context.MolecularWeight.FirstOrDefault(x => x.MolecularWeightId == weight.MolecularWeightId);
                {
                    if (weightmodel != null)
                    {
                        weightmodel.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteMolecularWeight, MolecularWeightId:" + weight.MolecularWeightId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
    }
}
