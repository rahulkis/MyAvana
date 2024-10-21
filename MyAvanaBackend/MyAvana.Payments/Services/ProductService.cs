using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyAvana.DAL.Auth;
using MyAvana.Logger.Contract;
using MyAvana.Models.ViewModels;
using MyAvana.Payments.Api.Contract;
using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyAvana.Payments.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly AvanaContext _avanaContext;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration configuration;





        public ProductService(AvanaContext avanaContext, IHostingEnvironment hostingEnvironment, ILogger logger, IConfiguration configuration)
        {
            _avanaContext = avanaContext;
            _env = hostingEnvironment;
            _logger = logger;
            this.configuration = configuration;
        }

        public (JsonResult result, bool success, string error) GetProductDetails(string id)
        {
            try
            {
                var result = _avanaContext.ProductEntities.Where(s => s.guid.ToString() == id).ToList();
                return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK }, true, "");
            }
            catch (Exception Ex)
            {
                return (new JsonResult(""), false, Ex.Message);
            }
        }

        public (JsonResult result, bool success, string error) GetSuggestions(string hairType, string productType, string hairChallenge, int pageNumber, string userId)
        {
            try
            {
                string[] hairGoalArray = null;
                string[] hairChallengeArray = null;
                int latestQA = _avanaContext.Questionaires.Where(x => x.UserId == userId && x.IsActive == true).Max(x => x.QA);
                var QuestionList = _avanaContext.Questionaires.Include(x => x.Answer).Where(x => x.UserId == userId && x.IsActive == true && x.QA == latestQA).ToList();

                string[] hairChallengeIds = QuestionList.Where(x => x.QuestionId == 16).ToList().Select(x => x.Answer.Description.ToLower()).ToArray();
                string[] hairGoals = QuestionList.Where(x => x.QuestionId == 25).ToList().Select(x => x.Answer.Description.ToLower()).ToArray();
                if (string.IsNullOrEmpty(hairChallenge))
                {
                    List<string> hairChallengeIdsRename = new List<string>();
                    foreach (var item in hairChallengeIds)
                    {
                        if (item == "dryness")
                        {
                            hairChallengeIdsRename.Add("dry");
                        }
                        else if(item == "weak edges")
                        {
                            hairChallengeIdsRename.Add("weak");
                        }
                        else
                        {
                            hairChallengeIdsRename.Add(item);
                        }
                    }
                    hairChallengeIds = hairChallengeIdsRename.ToArray();
                    var ids = _avanaContext.HairChallenges.Where(x => x.IsActive == true && hairChallengeIds.Contains(x.Description.ToLower())).ToList().Select(x => x.HairChallengeId).ToArray();
                    hairChallenge = string.Join(",", ids);
                    hairChallengeArray = hairChallenge.Split(',');

                }
                else
                {
                    hairChallengeArray = hairChallenge.Split(',');
                }

                if (hairGoals != null)
                {
                    hairGoalArray = _avanaContext.MappingHairGoalAndProductTags.Where(x => hairGoals.Contains(x.GoalDescription.ToLower())).ToList().Select(x => x.ProductTagsId.ToString()).ToArray();
                }

                String type1 = "1";

                List<ProductEntity> result = new List<ProductEntity>();
                List<ProductEntity> resultToReturn = new List<ProductEntity>();
                string[] productTypeArray = null;
                string[] hairTypeArray = null;
               
                //string[] hairChallengeArray = hairChallenge.Split(',');
                string hairTypes = "";

                hairType = hairType.ToLower().Replace("type", "").Trim();

                if (hairType == type1)
                {
                    hairTypes = "1a,1b,1c,all types";
                    hairTypeArray = hairTypes.Split(',');
                }
                else
                {
                    hairTypes = hairType + "," + "all types";
                    hairTypeArray = hairTypes.Split(',');
                }

                var brandsList = _avanaContext.Brands.Where(x => x.HideInSearch == true && x.IsActive == true).Select(x => x.BrandName).ToList();

                if (productType == null || productType == "")
                {
                    result = (from products in _avanaContext.ProductEntities
                              join common in _avanaContext.ProductCommons.Where(x=>x.HairTypeId !=null)
                              on products.Id equals common.ProductEntityId
                              join common2 in _avanaContext.ProductCommons.Where(x => x.HairChallengeId != null)
                              on common.ProductEntityId equals common2.ProductEntityId
                              join common3 in _avanaContext.ProductCommons.Where(x => x.ProductTagsId != null)
                              on common.ProductEntityId equals common3.ProductEntityId
                              join pType in _avanaContext.ProductTypes
                              on products.ProductTypesId equals pType.Id
                              join types in _avanaContext.HairTypes
                              on common.HairTypeId equals types.HairTypeId
                              where hairTypeArray.Contains(types.Description) 
                              && hairChallengeArray.Contains(common2.HairChallengeId.ToString())
                              && hairGoalArray.Contains(common3.ProductTagsId.ToString())
                              && products.IsActive == true && products.HideInSearch != true
                              && !brandsList.Contains(products.BrandName)
                              select products).Distinct().ToList();

                }
                else
                {
                    if (productType != null)
                    {
                        productTypeArray = productType.Split(',');
                    }
                    result = (from products in _avanaContext.ProductEntities
                              join common in _avanaContext.ProductCommons
                              on products.Id equals common.ProductEntityId
                              join common2 in _avanaContext.ProductCommons.Where(x => x.HairChallengeId != null)
                              on common.ProductEntityId equals common2.ProductEntityId
                              join common3 in _avanaContext.ProductCommons.Where(x => x.ProductTagsId != null)
                              on common.ProductEntityId equals common3.ProductEntityId
                              join pType in _avanaContext.ProductTypes
                              on products.ProductTypesId equals pType.Id
                              join types in _avanaContext.HairTypes
                              on common.HairTypeId equals types.HairTypeId
                              join pTypeCat in _avanaContext.ProductTypeCategories
                              on pType.ParentId equals pTypeCat.Id
                              where hairTypeArray.Contains(types.Description)
                              && hairChallengeArray.Contains(common2.HairChallengeId.ToString())
                              && hairGoalArray.Contains(common3.ProductTagsId.ToString())
                              && products.IsActive == true && products.HideInSearch != true
                              && productTypeArray.Contains(pTypeCat.Id.ToString())
                              && !brandsList.Contains(products.BrandName)
                              select products).Distinct().ToList();

                  
                }
                return (new JsonResult(result), true, "");
            }
            catch (Exception Ex)
            {
                return (new JsonResult(""), false, Ex.Message);
            }
        }


        public (JsonResult result, bool success, string error) GetAllHairTypes()
        {
            try
            {
                var result = _avanaContext.ProductEntities.Where(i => i.IsActive == true).Select(x => new { x.guid, x.TypeFor }).Distinct().ToList();
                return (new JsonResult(result), true, "");
            }
            catch (Exception Ex)
            {
                return (new JsonResult(""), false, Ex.Message);
            }
        }

        public (JsonResult result, bool success, string error) GetAllProductTypes(string hairType)
        {
            try
            {
                String type1 = "1";

                string[] hairTypeArray = null;
                string hairTypes = "";

                hairType = hairType.ToLower().Replace("type", "").Trim();

                if (hairType == type1)
                {
                    hairTypes = "1a,1b,1c,all types";
                    hairTypeArray = hairTypes.Split(',');
                }
                else
                {
                    hairTypes = hairType + "," + "all types";
                    hairTypeArray = hairTypes.Split(',');
                }

                List<int?> parentids = (from products in _avanaContext.ProductEntities
                                        join common in _avanaContext.ProductCommons
                                        on products.Id equals common.ProductEntityId
                                        join pType in _avanaContext.ProductTypes
                                        on products.ProductTypesId equals pType.Id
                                        join types in _avanaContext.HairTypes
                                        on common.HairTypeId equals types.HairTypeId
                                        where hairTypeArray.Contains(types.Description) && products.IsActive == true
                                        select products.ProductTypes.ParentId).Distinct().ToList();

                var result = _avanaContext.ProductTypeCategories.Where(x => parentids.Contains(x.Id)).Select(x => new { x.CategoryName, x.Id, x.CreatedOn }).Distinct().ToList();
                return (new JsonResult(result), true, "");
            }
            catch (Exception Ex)
            {
                return (new JsonResult(""), false, Ex.Message);
            }
        }


        public (JsonResult result, bool success, string error) GetProductsByTypes(string hairTypes)
        {
            try
            {
                string[] productTypeArray = null;
                if (hairTypes != null)
                {
                    productTypeArray = hairTypes.Split(',');
                }
                var result = _avanaContext.ProductEntities.Include(x => x.ProductTypes).Where(s => productTypeArray.Contains(s.TypeFor)).ToList();
                return (new JsonResult(result), true, "");
            }
            catch (Exception Ex)
            {
                return (new JsonResult(""), false, Ex.Message);

            }
        }

        public (JsonResult result, bool success, string error) GetSuggestionsSP(string hairType, string productType, string hairChallenge, int pageNumber, string userId)
        {
            try
            {
                List<ProductEntity> res = new List<ProductEntity>();
                var dp_params = new DynamicParameters();
                dp_params.Add("UserID", userId, DbType.String);
                dp_params.Add("HairType", hairType, DbType.String);
                dp_params.Add("QA", null, DbType.Int32);
                using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result =  connection.QueryAsync<ProductEntity>("sp_GetMatchingProducts", dp_params, commandType: CommandType.StoredProcedure).GetAwaiter().GetResult();
                  
                    return (new JsonResult(result), true, "");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            throw new NotImplementedException();
        }
    }
}
