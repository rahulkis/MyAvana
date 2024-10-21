using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Contract;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace MyAvana.CRM.Api.Services
{
    public class HairProfileService : IHairProfileService
    {
        private AvanaContext context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration configuration;
        private readonly Logger.Contract.ILogger _logger;
        private readonly string _adminStagingUrl;
        private readonly string _adminUrl;

        public HairProfileService(AvanaContext _context, UserManager<UserEntity> userManager, IEmailService emailService, IConfiguration configuration, Logger.Contract.ILogger logger)
        {
            context = _context;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
            this.configuration = configuration;
             _adminStagingUrl = configuration["AdminStagingUrl"];
            _adminUrl = configuration["AdminUrl"];
        }

        public HairProfileModel GetHairProfile(string userId)
        {
            try
            {
                int id = context.HairProfiles.Where(x => x.UserId == userId && x.IsActive == true).Select(x => x.Id).FirstOrDefault();

                HairProfileModel profile = (from hr in context.HairProfiles
                                            join st in context.HairStrands
                                            on hr.Id equals st.HairProfileId
                                            where hr.UserId == userId
                                            && hr.IsActive == true && hr.IsDrafted == false
                                            select new HairProfileModel()
                                            {
                                                UserId = hr.UserId,
                                                ProfileId = hr.Id,
                                                HairId = hr.HairId,
                                                HealthSummary = hr.HealthSummary,
                                                TopLeft = new TopLeftMobile()
                                                {
                                                    TopLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopLeftImage != null && x.TopLeftImage != "")
                                                    .Select(x => _adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.TopLeftImage).ToList(),

                                                    TopLeftHealthText = st.TopLeftHealthText,
                                                    TopLeftStrandDiameter = st.TopLeftStrandDiameter,
                                                    Health = (from hb in context.HairHealths
                                                              join ob in context.Healths
                                                              on hb.HealthId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                              select new HealthModel()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),

                                                },
                                                TopRight = new TopRightMobile()
                                                {
                                                    TopRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopRightImage != null && x.TopRightImage != "")
                                                    .Select(x => _adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.TopRightImage).ToList(),

                                                    TopRightHealthText = st.TopRightHealthText,
                                                    TopRightStrandDiameter = st.TopRightStrandDiameter,
                                                    Health = (from hb in context.HairHealths
                                                              join ob in context.Healths
                                                              on hb.HealthId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                              select new HealthModel()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                                },
                                                BottomLeft = new BottomLeftMobile()
                                                {
                                                    BottomLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomLeftImage != null && x.BottomLeftImage != "")
                                                    .Select(x => _adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.BottomLeftImage).ToList(),

                                                    BottomLeftHealthText = st.BottomLeftHealthText,
                                                    BottomLeftStrandDiameter = st.BottomLeftStrandDiameter,
                                                    Health = (from hb in context.HairHealths
                                                              join ob in context.Healths
                                                              on hb.HealthId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                              select new HealthModel()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),

                                                },
                                                BottomRight = new BottomRightMobile()
                                                {
                                                    BottomRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomRightImage != null && x.BottomRightImage != "")
                                                    .Select(x => _adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.BottomRightImage).ToList(),

                                                    BottomRightHealthText = st.BottomRightHealthText,
                                                    BottomRightStrandDiameter = st.BottomRightStrandDiameter,
                                                    Health = (from hb in context.HairHealths
                                                              join ob in context.Healths
                                                              on hb.HealthId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                              select new HealthModel()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                                },
                                                CrownStrand = new CrownStrandMobile()
                                                {
                                                    CrownHealthText = st.CrownHealthText,
                                                    CrownPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.CrownImage != null && x.CrownImage != "")
                                                    .Select(x => _adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.CrownImage).ToList(),

                                                    CrownStrandDiameter = st.CrownStrandDiameter,
                                                    Health = (from hb in context.HairHealths
                                                              join ob in context.Healths
                                                              on hb.HealthId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                              select new HealthModel()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                                },
                                                RecommendedVideos = (from rv in context.RecommendedVideos
                                                                     join ml in context.MediaLinkEntities
                                                                     on rv.MediaLinkEntityId equals ml.MediaLinkEntityId
                                                                     where rv.HairProfileId == hr.Id
                                                                     select new RecommendedVideos()
                                                                     {
                                                                         MediaLinkEntityId = ml.MediaLinkEntityId,
                                                                         Name = ml.VideoId
                                                                     }).ToList()

                                                //RecommendedVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hr.Id).ToList()
                                            }).FirstOrDefault();
                if (id != 0)
                {
                    profile.TopLeft.ObservationValues = GetData(id, "topLeft");
                    profile.TopRight.ObservationValues = GetData(id, "topRight");
                    profile.BottomLeft.ObservationValues = GetData(id, "bottomLeft");
                    profile.BottomRight.ObservationValues = GetData(id, "bottomRight");
                    profile.CrownStrand.ObservationValues = GetData(id, "crown");

                    foreach (var abc in profile.RecommendedVideos)
                    {
                        if (abc.Name.Contains("instagram"))
                        {
                            abc.ThumbNail = "http://admin.myavana.com/images/instagram.jpg";
                        }
                    }
                }
                return profile;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetHairProfile, UserId:" + userId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public HairProfileModel2 GetHairProfile2(string userId,int  hairProfileId)
        {
            try
            {
                int id;
                string WebApiUrl = configuration["WebApiUrl"];
                if (!string.IsNullOrEmpty(userId) && hairProfileId==0)
                {
                     id = context.HairProfiles.Where(x => x.UserId == userId && x.IsActive == true).Select(x => x.Id).LastOrDefault();
                }
                else
                {
                    id = hairProfileId;
                }

                HairProfileModel2 profile = (from hr in context.HairProfiles
                                             join sts in context.HairStrands
                                             on hr.Id equals sts.HairProfileId into hs
                                             from st in hs.DefaultIfEmpty()
                                             where hr.Id == id
                                             && hr.IsActive == true && hr.IsDrafted == false
                                             select new HairProfileModel2()
                                             {
                                                 UserId = hr.UserId,
                                                 ProfileId = hr.Id,
                                                 HairId = hr.HairId,
                                                 HealthSummary = hr.HealthSummary,
                                                 ConsultantNotes = hr.ConsultantNotes,
                                                 RecommendationNotes = hr.RecommendationNotes,
                                                 IsBasicHHCP = hr.IsBasicHHCP,
                                                 HairAnalyst = context.HairAnalyst.Where(a => a.HairAnalystId == hr.CreatedBy).FirstOrDefault(),
                                                 //HairAnalyst = context.HairAnalyst.Where(a => a.HairAnalystId == hr.CreatedBy).ToList(),
                                                 TopLeft = st != null ? new TopLeftMobile()
                                                 {
                                                     TopLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopLeftImage != null && x.TopLeftImage != "")
                                                     .Select(x => x.TopLeftImage.Contains(WebApiUrl)
                                                        ? x.TopLeftImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.TopLeftImage).Replace(" ", "")).ToList(),

                                                     TopLeftHealthText = st.TopLeftHealthText,
                                                     TopLeftStrandDiameter = st.TopLeftStrandDiameter,
                                                     Health = (from hb in context.HairHealths
                                                               join ob in context.Healths
                                                               on hb.HealthId equals ob.Id
                                                               where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                               select new HealthModel()
                                                               {
                                                                   Id = ob.Id,
                                                                   Description = ob.Description
                                                               }).ToList(),

                                                 } : new TopLeftMobile(),
                                                 TopRight = st != null ? new TopRightMobile()
                                                 {
                                                     TopRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopRightImage != null && x.TopRightImage != "")
                                                     .Select(x => x.TopRightImage.Contains(WebApiUrl)
                                                        ? x.TopRightImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.TopRightImage).Replace(" ", "")
                                                      ).ToList(),

                                                     TopRightHealthText = st.TopRightHealthText,
                                                     TopRightStrandDiameter = st.TopRightStrandDiameter,
                                                     Health = (from hb in context.HairHealths
                                                               join ob in context.Healths
                                                               on hb.HealthId equals ob.Id
                                                               where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                               select new HealthModel()
                                                               {
                                                                   Id = ob.Id,
                                                                   Description = ob.Description
                                                               }).ToList(),
                                                 } : new TopRightMobile(),
                                                 BottomLeft = st != null ? new BottomLeftMobile()
                                                 {
                                                     BottomLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomLeftImage != null && x.BottomLeftImage != "")
                                                     .Select(x => x.BottomLeftImage.Contains(WebApiUrl)
                                                        ? x.BottomLeftImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.BottomLeftImage).Replace(" ", "")
                                                      ).ToList(),
                                                     BottomLeftHealthText = st.BottomLeftHealthText,
                                                     BottomLeftStrandDiameter = st.BottomLeftStrandDiameter,
                                                     Health = (from hb in context.HairHealths
                                                               join ob in context.Healths
                                                               on hb.HealthId equals ob.Id
                                                               where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                               select new HealthModel()
                                                               {
                                                                   Id = ob.Id,
                                                                   Description = ob.Description
                                                               }).ToList(),

                                                 } : new BottomLeftMobile(),
                                                 BottomRight = st != null ? new BottomRightMobile()
                                                 {
                                                     BottomRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomRightImage != null && x.BottomRightImage != "")
                                                     .Select(x => x.BottomRightImage.Contains(WebApiUrl)
                                                        ? x.BottomRightImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.BottomRightImage).Replace(" ", "")
                                                      ).ToList(),
                                                     
                                                     BottomRightHealthText = st.BottomRightHealthText,
                                                     BottomRightStrandDiameter = st.BottomRightStrandDiameter,
                                                     Health = (from hb in context.HairHealths
                                                               join ob in context.Healths
                                                               on hb.HealthId equals ob.Id
                                                               where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                               select new HealthModel()
                                                               {
                                                                   Id = ob.Id,
                                                                   Description = ob.Description
                                                               }).ToList(),
                                                 } : new BottomRightMobile(),
                                                 CrownStrand = st != null ? new CrownStrandMobile()
                                                 {
                                                     CrownHealthText = st.CrownHealthText,
                                                     CrownPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.CrownImage != null && x.CrownImage != "")
                                                     .Select(x => x.CrownImage.Contains(WebApiUrl)
                                                        ? x.CrownImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.CrownImage).Replace(" ", "")
                                                      ).ToList(),

                                                     CrownStrandDiameter = st.CrownStrandDiameter,
                                                     Health = (from hb in context.HairHealths
                                                               join ob in context.Healths
                                                               on hb.HealthId equals ob.Id
                                                               where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                               select new HealthModel()
                                                               {
                                                                   Id = ob.Id,
                                                                   Description = ob.Description
                                                               }).ToList(),
                                                 } : new CrownStrandMobile(),
                                                 RecommendedVideos = (from rv in context.RecommendedVideos
                                                                      join ml in context.MediaLinkEntities
                                                                      on rv.MediaLinkEntityId equals ml.MediaLinkEntityId
                                                                      where rv.HairProfileId == hr.Id
                                                                      select new RecommendedVideosModel()
                                                                      {
                                                                          MediaLinkEntityId = ml.MediaLinkEntityId,
                                                                          Name = ml.VideoId,
                                                                          ThumbNail = ml.ImageLink,
                                                                          Title = ml.Title
                                                                      }).ToList(),
                                                 RecommendedStylist = (from rs in context.RecommendedStylists
                                                                       join ml in context.Stylists
                                                                       on rs.StylistId equals ml.StylistId
                                                                       where rs.HairProfileId == hr.Id
                                                                       select new StylistCustomerModel()
                                                                       {
                                                                           StylistName = ml.StylistName,
                                                                           Salon = ml.SalonName,
                                                                           Email = ml.Email,
                                                                           Phone = ml.PhoneNumber,
                                                                           Website = ml.Website,
                                                                           Instagram = ml.Instagram
                                                                       }).ToList()
                                                 //RecommendedVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hr.Id).ToList()
                                             }).FirstOrDefault();
                if (id != 0)
                {
                    profile.TopLeft.ObservationValues = GetData2(id, "topLeft");
                    profile.TopRight.ObservationValues = GetData2(id, "topRight");
                    profile.BottomLeft.ObservationValues = GetData2(id, "bottomLeft");
                    profile.BottomRight.ObservationValues = GetData2(id, "bottomRight");
                    profile.CrownStrand.ObservationValues = GetData2(id, "crown");

                    //foreach (var abc in profile.RecommendedVideos)
                    //{
                    //    if (abc.Name.Contains("instagram"))
                    //    {
                    //        abc.ThumbNail = "http://admin.myavana.com/images/instagram.jpg";
                    //    }
                    //}
                }
                return profile;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetHairProfile2, UserId:" + userId + ", Error: " + Ex.Message, Ex);
                return null;
            }

        }

        public CollaboratedDetailModel CollaboratedDetails(string hairProfileId)
        {
            try
            {
                int profileId = Convert.ToInt32(hairProfileId);
                CollaboratedDetailModel collaboratedDetailModel = new CollaboratedDetailModel();

                List<int> productIds = context.RecommendedProducts.Where(x => x.HairProfileId == profileId).Select(x => x.ProductId).ToList();
                List<int?> types = context.ProductEntities.Where(x => productIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                List<int?> parentIds = context.ProductTypes.Where(x => types.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                var parents = context.ProductTypeCategories.Where(x => parentIds.Contains(x.Id)).ToList();
                List<RecommendedProductsModel> productsTypesList = new List<RecommendedProductsModel>();
                List<RecommendedProductsModel> styleproductsTypesList = new List<RecommendedProductsModel>();
                foreach (var parentProduct in parents)
                {
                    RecommendedProductsModel productsTypes = new RecommendedProductsModel();
                    productsTypes.ProductParentName = parentProduct.CategoryName;
                    productsTypes.ProductId = parentProduct.Id;
                    List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                    List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id))
                        .Select(x => x.ProductTypesId).Distinct().ToList();

                    foreach (var type in productByTypes)
                    {
                        if (type != null)
                        {
                            ProductsTypesModels productsTypesModel = new ProductsTypesModels();
                            var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id)).ToList();
                            if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                            {
                                productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                productsTypesModel.ProductId = type;
                            }

                            productsTypesModel.Products = products.Select(x => new ProductsModels
                            {
                                Id = x.Id,
                                BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                ImageName = x.ImageName,
                                ProductLink = x.ProductLink,
                                ProductDetails = x.ProductDetails,
                                ProductName = x.ProductName,
                                ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault(),
                                HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                {
                                    Description = p.HairChallenges.Description,
                                    HairChallengeId = p.HairChallenges.HairChallengeId
                                }).ToList(),
                                HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                {
                                    Description = p.HairGoal.Description,
                                    HairGoalId = p.HairGoal.HairGoalId
                                }).ToList()
                            }).ToList();
                            productsTypesModels.Add(productsTypesModel);
                        }
                    }
                    productsTypes.ProductsTypes = productsTypesModels;
                    productsTypesList.Add(productsTypes);
                }
                //collaboratedDetailModel.ProductDetailModel = productsTypesList;

                //--style recipe products
                var styleProds = (from s in context.ProductCommons
                                  join srecomm in context.RecommendedProductsStyleRecipe
                                  on s.ProductEntityId equals srecomm.ProductId
                                  where srecomm.HairProfileId == profileId && s.ProductTypeId != null && s.IsActive == true
                                  select s).Distinct().ToList();
                List<int?> styletypesNew = styleProds.Select(x => x.ProductTypeId).Distinct().ToList();
                List<int?> styleproductIdsNew = styleProds.Where(x => x.ProductTypeId != null).Select(x => x.ProductEntityId).Distinct().ToList();

                List<int?> styleparentIdsNew = context.ProductTypes.Where(x => styletypesNew.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                var styleparentsNew = context.ProductTypeCategories.Where(x => styleparentIdsNew.Contains(x.Id)).ToList();

                foreach (var parentProduct in styleparentsNew)
                {

                    List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                    List<int?> productByTypes = (from s in styleProds
                                                 join pType in context.ProductTypes
                                                 on s.ProductTypeId equals pType.Id
                                                 where pType.ParentId == parentProduct.Id
                                                 select s.ProductTypeId).Distinct().ToList();

                    var existProdType = productsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id);
                    if (existProdType != null)
                    {
                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                var existType = existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type);
                                if (existType != null)
                                {
                                    var products = (from prod in context.ProductEntities
                                                    join pCom in context.ProductCommons
                                                    on prod.Id equals pCom.ProductEntityId
                                                    join pType in context.ProductTypes
                                                    on pCom.ProductTypeId equals pType.Id
                                                    where pCom.ProductTypeId != null && styleproductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                    select prod).Distinct().ToList();
                                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();


                                    existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type).Products.AddRange(products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductType = productType.ProductName,
                                        HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                        {
                                            Description = p.HairChallenges.Description,
                                            HairChallengeId = p.HairChallenges.HairChallengeId
                                        }).ToList(),
                                        HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                        {
                                            Description = p.HairGoal.Description,
                                            HairGoalId = p.HairGoal.HairGoalId
                                        }).ToList()
                                    }).ToList());
                                }
                                else
                                {
                                    ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                                    var products = (from prod in context.ProductEntities
                                                    join pCom in context.ProductCommons
                                                    on prod.Id equals pCom.ProductEntityId
                                                    join pType in context.ProductTypes
                                                    on pCom.ProductTypeId equals pType.Id
                                                    where pCom.ProductTypeId != null && styleproductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                    select prod).Distinct().ToList();
                                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                    if (productType != null)
                                    {
                                        productsTypesNew.ProductTypeName = productType.ProductName;
                                        productsTypesNew.ProductId = type;
                                    }

                                    productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductType = productType.ProductName,
                                        HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                        {
                                            Description = p.HairChallenges.Description,
                                            HairChallengeId = p.HairChallenges.HairChallengeId
                                        }).ToList(),
                                        HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                        {
                                            Description = p.HairGoal.Description,
                                            HairGoalId = p.HairGoal.HairGoalId
                                        }).ToList()
                                    }).ToList();
                                    productsTypesModels.Add(productsTypesNew);
                                }

                            }
                        }
                        styleproductsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id).ProductsTypes.AddRange(productsTypesModels);
                    }
                    else
                    {
                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                                var products = (from prod in context.ProductEntities
                                                join pCom in context.ProductCommons
                                                on prod.Id equals pCom.ProductEntityId
                                                join pType in context.ProductTypes
                                                on pCom.ProductTypeId equals pType.Id
                                                where pCom.ProductTypeId != null && styleproductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                select prod).Distinct().ToList();
                                var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                if (productType != null)
                                {
                                    productsTypesNew.ProductTypeName = productType.ProductName;
                                    productsTypesNew.ProductId = type;
                                }

                                productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                {
                                    Id = x.Id,
                                    BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                    ImageName = x.ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductType = productType.ProductName,
                                    HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                    {
                                        Description = p.HairChallenges.Description,
                                        HairChallengeId = p.HairChallenges.HairChallengeId
                                    }).ToList(),
                                    HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                    {
                                        Description = p.HairGoal.Description,
                                        HairGoalId = p.HairGoal.HairGoalId
                                    }).ToList()
                                }).ToList();
                                productsTypesModels.Add(productsTypesNew);
                            }
                        }
                        RecommendedProductsModel productsTypes = new RecommendedProductsModel();
                        productsTypes.ProductParentName = parentProduct.CategoryName;
                        productsTypes.ProductId = parentProduct.Id;
                        productsTypes.ProductsTypes = productsTypesModels;
                        styleproductsTypesList.Add(productsTypes);
                    }

                }
                collaboratedDetailModel.RecommendedProductsStyleRecipe = styleproductsTypesList;
                //---
                collaboratedDetailModel.RecommendedStyleRecipeVideos = context.RecommendedStyleRecipeVideos.Where(x => x.HairProfileId == profileId).OrderByDescending(x => x.CreatedOn)
                              .Select(s => new RecommendedVideosCustomer
                              {
                                  Id = s.Id,
                                  MediaLinkEntityId = s.MediaLinkEntityId,
                                  HairProfileId = s.HairProfileId,
                                  Name = s.Name,
                                  // Videos = context.MediaLinkEntities.Where(x => x.MediaLinkEntityId == s.MediaLinkEntityId).Select(x => x.VideoId).ToList().ToString().Replace("watch","embed")
                                  Videos = (from media in context.MediaLinkEntities
                                            where media.MediaLinkEntityId == s.MediaLinkEntityId
                                            select media.VideoId.ToString().Replace("watch?v=", "embed/")).ToList()
                              }).ToList();
                //Start Essential Products Code after multiple Product Type functionality merge recommended products
                var newEssProds = (from s in context.ProductCommons
                                   join srecomm in context.RecommendedProducts
                                   on s.ProductEntityId equals srecomm.ProductId
                                   where srecomm.HairProfileId == profileId && s.ProductTypeId != null && s.IsActive == true
                                   select s).Distinct().ToList();
                List<int?> typesNew = newEssProds.Select(x => x.ProductTypeId).Distinct().ToList();
                List<int?> productIdsNew = newEssProds.Where(x => x.ProductTypeId != null).Select(x => x.ProductEntityId).Distinct().ToList();

                List<int?> parentIdsNew = context.ProductTypes.Where(x => typesNew.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                var parentsNew = context.ProductTypeCategories.Where(x => parentIdsNew.Contains(x.Id)).ToList();

                foreach (var parentProduct in parentsNew)
                {

                    List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                    List<int?> productByTypes = (from s in newEssProds
                                                 join pType in context.ProductTypes
                                                 on s.ProductTypeId equals pType.Id
                                                 where pType.ParentId == parentProduct.Id
                                                 select s.ProductTypeId).Distinct().ToList();

                    var existProdType = productsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id);
                    if (existProdType != null)
                    {
                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                var existType = existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type);
                                if (existType != null)
                                {
                                    var products = (from prod in context.ProductEntities
                                                    join pCom in context.ProductCommons
                                                    on prod.Id equals pCom.ProductEntityId
                                                    join pType in context.ProductTypes
                                                    on pCom.ProductTypeId equals pType.Id
                                                    where pCom.ProductTypeId != null && productIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                    select prod).Distinct().ToList();
                                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();


                                    existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type).Products.AddRange(products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductType = productType.ProductName,
                                        HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                        {
                                            Description = p.HairChallenges.Description,
                                            HairChallengeId = p.HairChallenges.HairChallengeId
                                        }).ToList(),
                                        HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                        {
                                            Description = p.HairGoal.Description,
                                            HairGoalId = p.HairGoal.HairGoalId
                                        }).ToList()
                                    }).ToList());
                                }
                                else
                                {
                                    ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                                    var products = (from prod in context.ProductEntities
                                                    join pCom in context.ProductCommons
                                                    on prod.Id equals pCom.ProductEntityId
                                                    join pType in context.ProductTypes
                                                    on pCom.ProductTypeId equals pType.Id
                                                    where pCom.ProductTypeId != null && productIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                    select prod).Distinct().ToList();
                                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                    if (productType != null)
                                    {
                                        productsTypesNew.ProductTypeName = productType.ProductName;
                                        productsTypesNew.ProductId = type;
                                    }

                                    productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductType = productType.ProductName,
                                        HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                        {
                                            Description = p.HairChallenges.Description,
                                            HairChallengeId = p.HairChallenges.HairChallengeId
                                        }).ToList(),
                                        HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                        {
                                            Description = p.HairGoal.Description,
                                            HairGoalId = p.HairGoal.HairGoalId
                                        }).ToList()
                                    }).ToList();
                                    productsTypesModels.Add(productsTypesNew);
                                }

                            }
                        }
                        productsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id).ProductsTypes.AddRange(productsTypesModels);
                    }
                    else
                    {
                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                                var products = (from prod in context.ProductEntities
                                                join pCom in context.ProductCommons
                                                on prod.Id equals pCom.ProductEntityId
                                                join pType in context.ProductTypes
                                                on pCom.ProductTypeId equals pType.Id
                                                where pCom.ProductTypeId != null && productIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                select prod).Distinct().ToList();
                                var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                if (productType != null)
                                {
                                    productsTypesNew.ProductTypeName = productType.ProductName;
                                    productsTypesNew.ProductId = type;
                                }

                                productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                {
                                    Id = x.Id,
                                    BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                    ImageName = x.ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductType = productType.ProductName,
                                    HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                    {
                                        Description = p.HairChallenges.Description,
                                        HairChallengeId = p.HairChallenges.HairChallengeId
                                    }).ToList(),
                                    HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                    {
                                        Description = p.HairGoal.Description,
                                        HairGoalId = p.HairGoal.HairGoalId
                                    }).ToList()
                                }).ToList();
                                productsTypesModels.Add(productsTypesNew);
                            }
                        }
                        RecommendedProductsModel productsTypes = new RecommendedProductsModel();
                        productsTypes.ProductParentName = parentProduct.CategoryName;
                        productsTypes.ProductId = parentProduct.Id;
                        productsTypes.ProductsTypes = productsTypesModels;
                        productsTypesList.Add(productsTypes);
                    }

                }
                collaboratedDetailModel.ProductDetailModel = productsTypesList;


                //Styling Regimens Code
                List<int> rProductIds = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == profileId).Select(x => x.ProductId).ToList();
                List<int?> pTypes = context.ProductEntities.Where(x => rProductIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                List<int?> pParentIds = context.ProductTypes.Where(x => pTypes.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                var pParents = context.ProductTypeCategories.Where(x => pParentIds.Contains(x.Id)).ToList();
                List<RecommendedProductsStylingModel> productsTypesStylingList = new List<RecommendedProductsStylingModel>();
                foreach (var parentProduct in pParents)
                {
                    RecommendedProductsStylingModel productsTypes = new RecommendedProductsStylingModel();
                    productsTypes.ProductParentName = parentProduct.CategoryName;
                    productsTypes.ProductId = parentProduct.Id;
                    List<ProductsTypesStylingModels> productsTypesModels = new List<ProductsTypesStylingModels>();
                    List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id))
                        .Select(x => x.ProductTypesId).Distinct().ToList();

                    foreach (var type in productByTypes)
                    {
                        if (type != null)
                        {
                            ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                            var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id)).ToList();
                            if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                            {
                                productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                productsTypesModel.ProductId = type;
                            }

                            productsTypesModel.Products = products.Select(x => new ProductsStylingModels
                            {
                                Id = x.Id,
                                BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                ImageName = x.ImageName,
                                ProductLink = x.ProductLink,
                                ProductDetails = x.ProductDetails,
                                ProductName = x.ProductName,
                                ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault(),
                                HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                {
                                    Description = p.HairChallenges.Description,
                                    HairChallengeId = p.HairChallenges.HairChallengeId
                                }).ToList(),
                                HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                {
                                    Description = p.HairGoal.Description,
                                    HairGoalId = p.HairGoal.HairGoalId
                                }).ToList()
                            }).ToList();
                            productsTypesModels.Add(productsTypesModel);
                        }
                    }
                    productsTypes.ProductsTypes = productsTypesModels;
                    productsTypesStylingList.Add(productsTypes);
                }

                //Start Styling Regimens Code after multiple Product Type functionality merge recommended products
                var newProds = (from s in context.ProductCommons
                                join srecomm in context.RecommendedProductsStyleRegimens
                                on s.ProductEntityId equals srecomm.ProductId
                                where srecomm.HairProfileId == profileId && s.ProductTypeId != null && s.IsActive == true
                                select s).Distinct().ToList();
                List<int?> pTypesNew = newProds.Select(x => x.ProductTypeId).Distinct().ToList();
                List<int?> rProductIdsNew = newProds.Where(x => x.ProductTypeId != null).Select(x => x.ProductEntityId).Distinct().ToList();
                List<int?> pParentIdsNew = context.ProductTypes.Where(x => pTypesNew.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                var pParentsNew = context.ProductTypeCategories.Where(x => pParentIdsNew.Contains(x.Id)).ToList();

                foreach (var parentProduct in pParentsNew)
                {

                    List<ProductsTypesStylingModels> productsTypesModels = new List<ProductsTypesStylingModels>();
                    List<int?> productByTypes = (from s in newProds
                                                 join pType in context.ProductTypes
                                                 on s.ProductTypeId equals pType.Id
                                                 where pType.ParentId == parentProduct.Id
                                                 select s.ProductTypeId).Distinct().ToList();

                    var existProdType = productsTypesStylingList.FirstOrDefault(x => x.ProductId == parentProduct.Id);
                    if (existProdType != null)
                    {
                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                var existType = existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type);
                                if (existType != null)
                                {
                                    //ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                    var products = (from prod in context.ProductEntities
                                                    join pCom in context.ProductCommons
                                                    on prod.Id equals pCom.ProductEntityId
                                                    join pType in context.ProductTypes
                                                    on pCom.ProductTypeId equals pType.Id
                                                    where pCom.ProductTypeId != null && rProductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                    select prod).Distinct().ToList();
                                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();


                                    existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type).Products.AddRange(products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductType = productType.ProductName,
                                        HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                        {
                                            Description = p.HairChallenges.Description,
                                            HairChallengeId = p.HairChallenges.HairChallengeId
                                        }).ToList(),
                                        HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                        {
                                            Description = p.HairGoal.Description,
                                            HairGoalId = p.HairGoal.HairGoalId
                                        }).ToList()
                                    }).ToList());
                                }
                                else
                                {
                                    ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                    var products = (from prod in context.ProductEntities
                                                    join pCom in context.ProductCommons
                                                    on prod.Id equals pCom.ProductEntityId
                                                    join pType in context.ProductTypes
                                                    on pCom.ProductTypeId equals pType.Id
                                                    where pCom.ProductTypeId != null && rProductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                    select prod).Distinct().ToList();
                                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                    if (productType != null)
                                    {
                                        productsTypesModel.ProductTypeName = productType.ProductName;
                                        productsTypesModel.ProductId = type;
                                    }

                                    productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductType = productType.ProductName,
                                        HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                        {
                                            Description = p.HairChallenges.Description,
                                            HairChallengeId = p.HairChallenges.HairChallengeId
                                        }).ToList(),
                                        HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                        {
                                            Description = p.HairGoal.Description,
                                            HairGoalId = p.HairGoal.HairGoalId
                                        }).ToList()
                                    }).ToList();
                                    productsTypesModels.Add(productsTypesModel);
                                }

                            }
                        }
                        productsTypesStylingList.FirstOrDefault(x => x.ProductId == parentProduct.Id).ProductsTypes.AddRange(productsTypesModels);
                    }
                    else
                    {
                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                var products = (from prod in context.ProductEntities
                                                join pCom in context.ProductCommons
                                                on prod.Id equals pCom.ProductEntityId
                                                join pType in context.ProductTypes
                                                on pCom.ProductTypeId equals pType.Id
                                                where pCom.ProductTypeId != null && rProductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                select prod).Distinct().ToList();
                                var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                if (productType != null)
                                {
                                    productsTypesModel.ProductTypeName = productType.ProductName;
                                    productsTypesModel.ProductId = type;
                                }

                                productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                {
                                    Id = x.Id,
                                    BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                    ImageName = x.ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductType = productType.ProductName,
                                    HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                    {
                                        Description = p.HairChallenges.Description,
                                        HairChallengeId = p.HairChallenges.HairChallengeId
                                    }).ToList(),
                                    HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                    {
                                        Description = p.HairGoal.Description,
                                        HairGoalId = p.HairGoal.HairGoalId
                                    }).ToList()
                                }).ToList();
                                productsTypesModels.Add(productsTypesModel);
                            }
                        }
                        RecommendedProductsStylingModel productsTypes = new RecommendedProductsStylingModel();
                        productsTypes.ProductParentName = parentProduct.CategoryName;
                        productsTypes.ProductId = parentProduct.Id;
                        productsTypes.ProductsTypes = productsTypesModels;
                        productsTypesStylingList.Add(productsTypes);
                    }

                }

                // End  Styling Regimens Code after multiple Product Type functionality merge recommended products
                collaboratedDetailModel.RecommendedProductsStyling = productsTypesStylingList;

                collaboratedDetailModel.IngredientDetailModel = (from rprod in context.RecommendedIngredients
                                                                 join ing in context.IngedientsEntities
                                                                 on rprod.IngredientId equals ing.IngedientsEntityId
                                                                 where rprod.HairProfileId == profileId
                                                                 select new IngredientDetailModel()
                                                                 {
                                                                     IngredientId = ing.IngedientsEntityId,
                                                                     Name = ing.Name,
                                                                     ImageUrl = "http://admin.myavana.com/Ingredients/" + ing.Image,
                                                                     IngredientDescription = ing.Description
                                                                 }).ToList();
                collaboratedDetailModel.RegimenDetailModel = (from rprod in context.RecommendedRegimens
                                                              join reg in context.Regimens
                                                              on rprod.RegimenId equals reg.RegimensId
                                                              where rprod.HairProfileId == profileId
                                                              select new RegimenDetailModel()
                                                              {
                                                                  RegimenId = reg.RegimensId,
                                                                  RegimenName = reg.Name,
                                                                  RegimenDescription = reg.Description
                                                              }).ToList();
                collaboratedDetailModel.HairStyle = (from sr in context.StyleRecipeHairStyle
                                                     join hr in context.HairStyles
                                                     on sr.HairStyleId equals hr.Id
                                                     where sr.HairProfileId == profileId
                                                     select hr.Style
                                       ).FirstOrDefault();
                return collaboratedDetailModel;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CollaboratedDetails, HairProfileId:" + hairProfileId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public RecommendedRegimensModel RecommendedRegimens(int regimenId)
        {
            try
            {
                Regimens regimens = context.Regimens.Where(x => x.RegimensId == regimenId).FirstOrDefault();
                RegimenSteps regimenList = context.RegimenSteps.Where(z => z.RegimenStepsId == regimens.RegimenStepsId).FirstOrDefault();
                RegimenSteps regimenSteps = regimenList;

                List<RegimenStepsModel> regimenStepsModels = GetRegimenSteps(regimenSteps);

                RecommendedRegimensModel recommendedRegimensModel = new RecommendedRegimensModel();
                recommendedRegimensModel.RegimenId = regimens.RegimensId;
                recommendedRegimensModel.RegimenName = regimens.Name;
                recommendedRegimensModel.RegimenSteps = regimenStepsModels;
                recommendedRegimensModel.RegimenTitle = regimens.Title;
                recommendedRegimensModel.Description = regimens.Description;

                return recommendedRegimensModel;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: RecommendedRegimens, RegimenId:" + regimenId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public RecommendedProductModel RecommendedProducts(int productId)
        {
            try
            {
                String[] Ingredient = { "Almond Oil", "Aloe Vera", "AMMONIUM LAURETH SULFATE", "Apple Cider vinegar", "Argan Oil", "Avocado Oil", "Beeswax", "BEHENTRIMONIUM CHLORIDE", "BEHENTRIMONIUM METHOSULFATE", "Biotin", "Boabab Oil", "Castor Oil" };

                var productList = (from prod in context.ProductEntities
                                   where prod.Id == productId
                                   select new RecommendedProductModel()
                                   {
                                       ProductId = prod.Id,
                                       ProductName = prod.ProductName,
                                       ActualName = prod.ActualName,
                                       ImageUrl = prod.ImageName,
                                       PurchaseLink = prod.ProductLink,
                                       ProductDetails = prod.ProductDetails,
                                       BrandName = context.Brands.Where(y => y.BrandId == prod.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                       //TypeFor = prod.TypeFor,
                                       TypeFor = (from prdc in context.ProductCommons
                                                  where prdc.ProductEntityId == productId && prdc.HairTypeId != null && prdc.IsActive == true
                                                  select new HairTypes()
                                                  {
                                                      HairType = prdc.HairType.Description
                                                  }).ToList(),
                                       Ingredient = (from ing in Ingredient
                                                     select new Ingredients()
                                                     {
                                                         Name = ing,
                                                     }).ToList()
                                   }).FirstOrDefault();
                return productList;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: RecommendedProducts, ProductId:" + productId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public RecommendedProductModel2 RecommendedProducts2(int productId)
        {
            try
            {
                String[] Ingredient = { "Almond Oil", "Aloe Vera", "AMMONIUM LAURETH SULFATE", "Apple Cider vinegar", "Argan Oil", "Avocado Oil", "Beeswax", "BEHENTRIMONIUM CHLORIDE", "BEHENTRIMONIUM METHOSULFATE", "Biotin", "Boabab Oil", "Castor Oil" };
                var productList = (from prod in context.ProductEntities
                                   where prod.Id == productId
                                   select new RecommendedProductModel2()
                                   {
                                       ProductId = prod.Id,
                                       ProductName = prod.ProductName,
                                       ActualName = prod.ActualName,
                                       ImageUrl = prod.ImageName,
                                       PurchaseLink = prod.ProductLink,
                                       ProductDetails = prod.ProductDetails,
                                       BrandName = context.Brands.Where(y => y.BrandId == prod.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                       TypeFor = prod.TypeFor,
                                       Ingredients = (from ing in Ingredient
                                                      select new Ingredients()
                                                      {
                                                          Name = ing,
                                                      }).ToList()
                                   }).FirstOrDefault();
                return productList;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: RecommendedProducts2, ProductId:" + productId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        private List<RegimenStepsModel> GetRegimenSteps(RegimenSteps regimenSteps)
        {
            List<RegimenStepsModel> regimenStepsModels = new List<RegimenStepsModel>();

            try
            {
                for (int i = 1; i <= 20; i++)
                {
                    if (i == 1 && regimenSteps.Step1Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();

                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step1Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step1Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 2 && regimenSteps.Step2Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();

                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step2Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step2Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 3 && regimenSteps.Step3Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();

                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step3Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step3Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 4 && regimenSteps.Step4Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();

                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step4Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step4Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 5 && regimenSteps.Step5Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step5Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step5Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 6 && regimenSteps.Step6Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step6Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step6Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 7 && regimenSteps.Step7Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step7Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step7Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 8 && regimenSteps.Step8Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step8Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step8Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 9 && regimenSteps.Step9Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step9Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step9Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 10 && regimenSteps.Step10Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step10Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step10Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 11 && regimenSteps.Step11Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step11Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step11Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 12 && regimenSteps.Step12Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step12Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step12Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 13 && regimenSteps.Step13Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step13Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step13Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 14 && regimenSteps.Step14Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step14Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step14Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 15 && regimenSteps.Step15Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step15Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step15Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 16 && regimenSteps.Step16Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();

                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step16Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step16Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 17 && regimenSteps.Step17Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step17Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step17Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 18 && regimenSteps.Step18Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step18Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step18Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 19 && regimenSteps.Step19Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step19Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step19Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                    if (i == 20 && regimenSteps.Step20Photo != null)
                    {
                        RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                        regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step20Photo;
                        regimenStepsModel.RegimenStepInstruction = regimenSteps.Step20Instruction;
                        regimenStepsModels.Add(regimenStepsModel);
                    }
                }

            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetRegimenSteps, Error: " + Ex.Message, Ex);
            }
            return regimenStepsModels;
        }

        public Models.ViewModels.HairProfile SaveProfile(Models.ViewModels.HairProfile hairProfile)
        {
            try
            {
                string TabNo = hairProfile.TabNo;


                List<HairStrandsImages> hairStrandsImages = new List<HairStrandsImages>();
                Models.Entities.HairProfile hair = new Models.Entities.HairProfile();

                hairProfile.Health = JsonConvert.DeserializeObject<List<HealthModel>>(hairProfile.TempHealth);

                hairProfile.Observation = JsonConvert.DeserializeObject<List<ObservationModel>>(hairProfile.TempObservation);
                hairProfile.observationElasticityModels = JsonConvert.DeserializeObject<List<ObservationElasticityModel>>(hairProfile.TempObservationElasticity);
                hairProfile.observationChemicalModels = JsonConvert.DeserializeObject<List<ObservationChemicalModel>>(hairProfile.TempObservationChemicalProduct);
                hairProfile.observationPhysicalModels = JsonConvert.DeserializeObject<List<ObservationPhysicalModel>>(hairProfile.TempObservationPhysicalProduct);
                //hairProfile.observationDamageModels = JsonConvert.DeserializeObject<List<ObservationDamageModel>>(hairProfile.TempObservationDamage);
                hairProfile.observationBreakageModels = JsonConvert.DeserializeObject<List<ObservationBreakageModel>>(hairProfile.TempObservationBreakage);
                hairProfile.observationSplittingModels = JsonConvert.DeserializeObject<List<ObservationSplittingModel>>(hairProfile.TempObservationSplitting);
                hairProfile.SelectedAnswer = JsonConvert.DeserializeObject<QuestionaireSelectedAnswer>(hairProfile.TempSelectedAnswer);

                hairProfile.Pororsity = JsonConvert.DeserializeObject<List<PororsityModel>>(hairProfile.TempPororsity);
                hairProfile.Elasticity = JsonConvert.DeserializeObject<List<ElasticityModel>>(hairProfile.TempElasticity);

                hairProfile.RecommendedIngredients = JsonConvert.DeserializeObject<List<RecommendedIngredients>>(hairProfile.TempRecommendedIngredients);
                hairProfile.RecommendedVideos = JsonConvert.DeserializeObject<List<RecommendedVideos>>(hairProfile.TempRecommendedVideos);

                hairProfile.RecommendedTools = JsonConvert.DeserializeObject<List<RecommendedTools>>(hairProfile.TempRecommendedTools);
                hairProfile.RecommendedCategories = JsonConvert.DeserializeObject<List<RecommendedCategory>>(hairProfile.TempRecommendedCategories);
                hairProfile.RecommendedProductTypes = JsonConvert.DeserializeObject<List<RecommendedProductTypes>>(hairProfile.TempRecommendedProductTypes);

                hairProfile.RecommendedProducts = JsonConvert.DeserializeObject<List<RecommendedProducts>>(hairProfile.TempRecommendedProducts);
                hairProfile.AllRecommendedProductsEssential = JsonConvert.DeserializeObject<List<RecommendedProducts>>(hairProfile.TempAllRecommendedProductsEssential);
                hairProfile.AllRecommendedProductsStyling = JsonConvert.DeserializeObject<List<RecommendedProductsStylingRegimen>>(hairProfile.TempAllRecommendedProductsStyling);
                hairProfile.RecommendedProductsStylings = JsonConvert.DeserializeObject<List<RecommendedProductsStylingRegimen>>(hairProfile.TempRecommendedProductsStylings);
                hairProfile.RecommendedRegimens = JsonConvert.DeserializeObject<List<RecommendedRegimens>>(hairProfile.TempRecommendedRegimens);
                hairProfile.RecommendedStylists = JsonConvert.DeserializeObject<List<RecommendedStylist>>(hairProfile.TempRecommendedStylist);

                hairProfile.RecommendedStyleRecipeVideos = JsonConvert.DeserializeObject<List<RecommendedStyleRecipeVideos>>(hairProfile.TempRecommendedStyleRecipeVideos);
                hairProfile.RecommendedProductsStyleRecipe = JsonConvert.DeserializeObject<List<RecommendedProductsStyleRecipe>>(hairProfile.TempRecommendedProductsStyleRecipe);

                string[] TopLeftImage = null;
                if (hairProfile.TopLeftPhoto != null)
                {
                    TopLeftImage = hairProfile.TopLeftPhoto.Split(',');
                }
                string[] TopRightImage = null;
                if (hairProfile.TopRightPhoto != null)
                {
                    TopRightImage = hairProfile.TopRightPhoto.Split(',');
                }
                string[] BottomLeftImage = null;
                if (hairProfile.BottomLeftPhoto != null)
                {
                    BottomLeftImage = hairProfile.BottomLeftPhoto.Split(',');
                }
                string[] BottomRightImage = null;
                if (hairProfile.BottomRightPhoto != null)
                {
                    BottomRightImage = hairProfile.BottomRightPhoto.Split(',');
                }
                string[] CrownImage = null;
                if (hairProfile.CrownPhoto != null)
                {
                    CrownImage = hairProfile.CrownPhoto.Split(',');
                }
                if(hairProfile.UserId != null)
                {
                    if (hairProfile.HairProfileId != null)
                    {
                        hair = context.HairProfiles.Where(x => x.Id.ToString() == hairProfile.HairProfileId && x.IsActive == true).LastOrDefault();
                    }
                    else
                    {
                        hair = context.HairProfiles.Where(x => x.UserId == hairProfile.UserId && x.IsActive == true).LastOrDefault();
                    }
                    var user = context.Users.Where(x => x.Email.ToLower() == hairProfile.UserId.ToLower()).FirstOrDefault();
                    if (hair != null && hairProfile.IsNewHHCP != 1)
                    {

                        if (TabNo.Equals("Tab1"))
                        {
                            if (hairProfile.SaveType == "draft")
                            {

                                hair.UserId = hairProfile.UserId;
                                hair.HairId = hairProfile.HairId;
                                hair.HealthSummary = hairProfile.HealthSummary;
                                hair.ConsultantNotes = hairProfile.ConsultantNotes;
                                hair.RecommendationNotes = hairProfile.RecommendationNotes;
                                hair.IsActive = true;
                                hair.IsDrafted = true;
                                hair.CreatedBy = hairProfile.CreatedBy;
                                hair.ModifiedOn = DateTime.Now;
                                hair.ModifiedBy = hairProfile.ModifiedBy;
                                hair.AttachedQA = hairProfile.QA;
                                // context.Add(hair);
                                context.SaveChanges();
                            }
                            else
                            {
                                hair.UserId = hairProfile.UserId;
                                hair.HairId = hairProfile.HairId;
                                hair.HealthSummary = hairProfile.HealthSummary;
                                hair.ConsultantNotes = hairProfile.ConsultantNotes;
                                hair.RecommendationNotes = hairProfile.RecommendationNotes;
                                hair.IsActive = true;
                                hair.ModifiedOn = DateTime.Now;
                                hair.ModifiedBy = hairProfile.ModifiedBy;
                                hair.IsDrafted = false;
                                hair.CreatedBy = hairProfile.CreatedBy;
                                hair.AttachedQA = hairProfile.QA;
                                var stylistNotes = context.StylistNotesHHCPs.FirstOrDefault(x => x.UserId == Convert.ToInt16(hairProfile.LoginUserId) && x.HairProfileId == hair.Id);
                                if (stylistNotes != null)
                                {
                                    stylistNotes.Notes = hairProfile.MyNotes;
                                    context.StylistNotesHHCPs.Update(stylistNotes);
                                }
                                else
                                {
                                    var obj = new StylistNotesHHCP();
                                    obj.Notes = hairProfile.MyNotes;
                                    obj.HairProfileId = hair.Id;
                                    obj.UserId = Convert.ToInt16(hairProfile.LoginUserId);
                                    obj.CreatedOn = DateTime.Now;
                                    context.StylistNotesHHCPs.Add(obj);
                                }
                                //context.Add(hair);
                                context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        //if (TabNo.Equals("Tab1"))
                        //{
                        if (hairProfile.SaveType == "draft")
                        {
                            hair = new Models.Entities.HairProfile();

                            hair.UserId = hairProfile.UserId;
                            hair.HairId = hairProfile.HairId;
                            hair.HealthSummary = hairProfile.HealthSummary;
                            hair.ConsultantNotes = hairProfile.ConsultantNotes;
                            hair.RecommendationNotes = hairProfile.RecommendationNotes;
                            hair.IsActive = true;
                            hair.IsDrafted = true;
                            hair.CreatedOn = DateTime.Now;
                            hair.IsViewEnabled = true;
                            hair.CreatedBy = hairProfile.CreatedBy;
                            hair.AttachedQA = hairProfile.QA;
                            context.Add(hair);
                            context.SaveChanges();
                        }
                        else
                        {
                            hair = new Models.Entities.HairProfile();

                            hair.UserId = hairProfile.UserId;
                            hair.HairId = hairProfile.HairId;
                            hair.HealthSummary = hairProfile.HealthSummary;
                            hair.ConsultantNotes = hairProfile.ConsultantNotes;
                            hair.RecommendationNotes = hairProfile.RecommendationNotes;
                            hair.HairAnalysisNotes = hairProfile.HairAnalysisNotes;
                            hair.IsActive = true;
                            hair.CreatedOn = DateTime.Now;
                            hair.IsDrafted = false;
                            hair.IsViewEnabled = true;
                            hair.CreatedBy = hairProfile.CreatedBy;
                            hair.AttachedQA = hairProfile.QA;
                            context.Add(hair);
                            context.SaveChanges();

                            var obj = new StylistNotesHHCP();
                            obj.Notes = hairProfile.MyNotes;
                            obj.HairProfileId = hair.Id;
                            obj.UserId = Convert.ToInt16(hairProfile.LoginUserId);
                            obj.CreatedOn = DateTime.Now;
                            context.StylistNotesHHCPs.Add(obj);
                        }
                        //}


                    }
                    if (TabNo.Equals("Tab2"))
                    {
                        hair.HairAnalysisNotes = hairProfile.HairAnalysisNotes;
                        HairStrands strands = new HairStrands();
                        strands = context.HairStrands.Where(x => x.HairProfileId == hair.Id).FirstOrDefault();
                        if (strands != null)
                        {
                            //HairStrands strands = new HairStrands();
                            strands.TopLeftPhoto = hairProfile.TopLeftPhoto;
                            strands.TopLeftStrandDiameter = hairProfile.TopLeftStrandDiameter;
                            strands.TopLeftHealthText = hairProfile.TopLeftHealthText;

                            strands.TopRightPhoto = hairProfile.TopRightPhoto;
                            strands.TopRightStrandDiameter = hairProfile.TopRightStrandDiameter;
                            strands.TopRightHealthText = hairProfile.TopRightHealthText;

                            strands.BottomLeftPhoto = hairProfile.BottomLeftPhoto;
                            strands.BottomLeftStrandDiameter = hairProfile.BottomLeftStrandDiameter;
                            strands.BottomLeftHealthText = hairProfile.BottomLeftHealthText;

                            strands.BottomRightPhoto = hairProfile.BottomRightPhoto;
                            strands.BottomRightHealthText = hairProfile.BottomRightHealthText;
                            strands.BottomRightStrandDiameter = hairProfile.BottoRightStrandDiameter;

                            strands.CrownPhoto = hairProfile.CrownPhoto;
                            strands.CrownHealthText = hairProfile.CrownHealthText;
                            strands.CrownStrandDiameter = hairProfile.CrownStrandDiameter;

                            strands.HairProfileId = hair.Id;

                            //context.Add(strands);
                            context.SaveChanges();
                        }
                        else
                        {
                            strands = new HairStrands();
                            strands.TopLeftPhoto = hairProfile.TopLeftPhoto;
                            strands.TopLeftStrandDiameter = hairProfile.TopLeftStrandDiameter;
                            strands.TopLeftHealthText = hairProfile.TopLeftHealthText;

                            strands.TopRightPhoto = hairProfile.TopRightPhoto;
                            strands.TopRightStrandDiameter = hairProfile.TopRightStrandDiameter;
                            strands.TopRightHealthText = hairProfile.TopRightHealthText;

                            strands.BottomLeftPhoto = hairProfile.BottomLeftPhoto;
                            strands.BottomLeftStrandDiameter = hairProfile.BottomLeftStrandDiameter;
                            strands.BottomLeftHealthText = hairProfile.BottomLeftHealthText;

                            strands.BottomRightPhoto = hairProfile.BottomRightPhoto;
                            strands.BottomRightHealthText = hairProfile.BottomRightHealthText;
                            strands.BottomRightStrandDiameter = hairProfile.BottoRightStrandDiameter;

                            strands.CrownPhoto = hairProfile.CrownPhoto;
                            strands.CrownHealthText = hairProfile.CrownHealthText;
                            strands.CrownStrandDiameter = hairProfile.CrownStrandDiameter;

                            strands.HairProfileId = hair.Id;

                            context.Add(strands);
                            context.SaveChanges();

                        }


                        //-------------
                        if (TopLeftImage == null)
                        {

                            List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.TopLeftImage != null).ToList();
                            context.RemoveRange(lstHairStrandsImages);
                            context.SaveChanges();
                        }

                        if (TopLeftImage != null)
                        {
                            List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.TopLeftImage != null).ToList();
                            context.RemoveRange(lstHairStrandsImages);
                            context.SaveChanges();

                            List<HairStrandsImages> lsthairStrands = new List<HairStrandsImages>();
                            foreach (var tLeft in TopLeftImage)
                            {
                                if (tLeft != "")
                                {
                                    HairStrandsImages hairStrandsImag = new HairStrandsImages();
                                    hairStrandsImag.TopLeftImage = tLeft;
                                    hairStrandsImag.IsActive = true;
                                    hairStrandsImag.CreatedOn = DateTime.Now;
                                    hairStrandsImag.Id = strands.Id;
                                    lsthairStrands.Add(hairStrandsImag);

                                }
                            }
                            context.AddRange(lsthairStrands);
                            context.SaveChanges();
                        }




                        if (TopRightImage == null)
                        {

                            List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.TopRightImage != null).ToList();
                            context.RemoveRange(lstHairStrandsImages);
                            context.SaveChanges();
                        }
                        if (TopRightImage != null)
                        {
                            List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.TopRightImage != null).ToList();
                            context.RemoveRange(lstHairStrandsImages);
                            context.SaveChanges();

                            List<HairStrandsImages> lsthairStrands = new List<HairStrandsImages>();
                            foreach (var tRight in TopRightImage)
                            {

                                if (tRight != "")
                                {
                                    HairStrandsImages hairStrands = new HairStrandsImages();

                                    hairStrands.TopRightImage = tRight;
                                    hairStrands.IsActive = true;
                                    hairStrands.CreatedOn = DateTime.Now;
                                    hairStrands.Id = strands.Id;
                                    lsthairStrands.Add(hairStrands);

                                }
                            }
                            context.AddRange(lsthairStrands);
                            context.SaveChanges();
                        }
                        if (BottomLeftImage == null)
                        {

                            List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.BottomLeftImage != null).ToList();
                            context.RemoveRange(lstHairStrandsImages);
                            context.SaveChanges();
                        }
                        if (BottomLeftImage != null)
                        {
                            List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.BottomLeftImage != null).ToList();
                            context.RemoveRange(lstHairStrandsImages);
                            context.SaveChanges();

                            List<HairStrandsImages> lsthairStrands = new List<HairStrandsImages>();
                            foreach (var bLeft in BottomLeftImage)
                            {
                                if (bLeft != "")
                                {
                                    HairStrandsImages hairStrands = new HairStrandsImages();

                                    hairStrands.BottomLeftImage = bLeft;
                                    hairStrands.IsActive = true;
                                    hairStrands.CreatedOn = DateTime.Now;
                                    hairStrands.Id = strands.Id;
                                    lsthairStrands.Add(hairStrands);

                                }
                            }
                            context.AddRange(lsthairStrands);
                            context.SaveChanges();
                        }
                        if (BottomRightImage == null)
                        {

                            List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.BottomRightImage != null).ToList();
                            context.RemoveRange(lstHairStrandsImages);
                            context.SaveChanges();
                        }
                        if (BottomRightImage != null)
                        {
                            List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.BottomRightImage != null).ToList();
                            context.RemoveRange(lstHairStrandsImages);
                            context.SaveChanges();

                            List<HairStrandsImages> lsthairStrands = new List<HairStrandsImages>();
                            foreach (var bRight in BottomRightImage)
                            {
                                if (bRight != "")
                                {
                                    HairStrandsImages hairStrands = new HairStrandsImages();

                                    hairStrands.BottomRightImage = bRight;
                                    hairStrands.IsActive = true;
                                    hairStrands.CreatedOn = DateTime.Now;
                                    hairStrands.Id = strands.Id;
                                    lsthairStrands.Add(hairStrands);

                                }
                            }
                            context.AddRange(lsthairStrands);
                            context.SaveChanges();
                        }
                        if (CrownImage == null)
                        {

                            List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.CrownImage != null).ToList();
                            context.RemoveRange(lstHairStrandsImages);
                            context.SaveChanges();
                        }

                        if (CrownImage != null)
                        {
                            List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.CrownImage != null).ToList();
                            context.RemoveRange(lstHairStrandsImages);
                            context.SaveChanges();

                            List<HairStrandsImages> lsthairStrands = new List<HairStrandsImages>();
                            foreach (var crown in CrownImage)
                            {
                                if (crown != "")
                                {
                                    HairStrandsImages hairStrands = new HairStrandsImages();

                                    hairStrands.CrownImage = crown;
                                    hairStrands.IsActive = true;
                                    hairStrands.CreatedOn = DateTime.Now;
                                    hairStrands.Id = strands.Id;
                                    lsthairStrands.Add(hairStrands);
                                }
                            }
                            context.AddRange(lsthairStrands);
                            context.SaveChanges();
                        }

                        if (hairProfile.Health.Count() > 0 || hairProfile.Health.Count() == 0)
                        {
                            List<HairHealth> lst = context.HairHealths.Where(x => x.HairProfileId == hair.Id).ToList();
                            context.RemoveRange(lst);
                            context.SaveChanges();
                            if (hairProfile.Health.Count() > 0)
                            {
                                List<HairHealth> lstHairHealth = new List<HairHealth>();
                                foreach (var health in hairProfile.Health)
                                {
                                    HairHealth hairHealth = new HairHealth();
                                    hairHealth.HairProfileId = hair.Id;
                                    hairHealth.HealthId = health.Id;
                                    hairHealth.IsTopLeft = health.IsTopLeft;
                                    hairHealth.IsTopRight = health.IsTopRight;
                                    hairHealth.IsBottomLeft = health.IsBottomLeft;
                                    hairHealth.IsBottomRight = health.IsBottomRight;
                                    hairHealth.IsCrown = health.IsCrown;
                                    lstHairHealth.Add(hairHealth);

                                }
                                context.AddRange(lstHairHealth);
                                context.SaveChanges();
                            }
                        }

                        if (hairProfile.Observation.Count() > 0 || hairProfile.Observation.Count() == 0)
                        {
                            List<HairObservation> lst = context.HairObservations.Where(x => x.HairProfileId == hair.Id && x.ObservationId != null).ToList();
                            context.RemoveRange(lst);
                            context.SaveChanges();
                            if (hairProfile.Observation.Count() > 0)
                            {
                                List<HairObservation> lstHairObservation = new List<HairObservation>();
                                foreach (var obser in hairProfile.Observation)
                                {
                                    HairObservation hairObservation = new HairObservation();
                                    hairObservation.HairProfileId = hair.Id;
                                    hairObservation.ObservationId = obser.Id;
                                    hairObservation.IsTopLeft = obser.IsTopLeft;
                                    hairObservation.IsTopRight = obser.IsTopRight;
                                    hairObservation.IsBottomLeft = obser.IsBottomLeft;
                                    hairObservation.IsBottomRight = obser.IsBottomRight;
                                    hairObservation.IsCrown = obser.IsCrown;
                                    lstHairObservation.Add(hairObservation);

                                }
                                context.AddRange(lstHairObservation);
                                context.SaveChanges();
                            }
                        }


                        if (hairProfile.observationElasticityModels.Count() > 0)
                        {
                            List<HairObservation> lst = context.HairObservations.Where(x => x.HairProfileId == hair.Id && x.ObsElasticityId != null).ToList();
                            context.RemoveRange(lst);
                            context.SaveChanges();

                            List<HairObservation> lstHairObservation = new List<HairObservation>();
                            foreach (var obser in hairProfile.observationElasticityModels)
                            {
                                HairObservation hairObservation = new HairObservation();
                                hairObservation.HairProfileId = hair.Id;
                                hairObservation.ObsElasticityId = obser.Id;
                                hairObservation.IsTopLeft = obser.IsTopLeft;
                                hairObservation.IsTopRight = obser.IsTopRight;
                                hairObservation.IsBottomLeft = obser.IsBottomLeft;
                                hairObservation.IsBottomRight = obser.IsBottomRight;
                                hairObservation.IsCrown = obser.IsCrown;

                                lstHairObservation.Add(hairObservation);

                            }
                            context.AddRange(lstHairObservation);
                            context.SaveChanges();
                        }

                        if (hairProfile.observationChemicalModels.Count() > 0)
                        {
                            List<HairObservation> lst = context.HairObservations.Where(x => x.HairProfileId == hair.Id && x.ObsChemicalProductId != null).ToList();
                            context.RemoveRange(lst);
                            context.SaveChanges();

                            List<HairObservation> lstHairObservation = new List<HairObservation>();
                            foreach (var obser in hairProfile.observationChemicalModels)
                            {
                                HairObservation hairObservation = new HairObservation();
                                hairObservation.HairProfileId = hair.Id;
                                hairObservation.ObsChemicalProductId = obser.Id;
                                hairObservation.IsTopLeft = obser.IsTopLeft;
                                hairObservation.IsTopRight = obser.IsTopRight;
                                hairObservation.IsBottomLeft = obser.IsBottomLeft;
                                hairObservation.IsBottomRight = obser.IsBottomRight;
                                hairObservation.IsCrown = obser.IsCrown;

                                lstHairObservation.Add(hairObservation);
                            }
                            context.AddRange(lstHairObservation);
                            context.SaveChanges();
                        }

                        if (hairProfile.observationPhysicalModels.Count() > 0)
                        {
                            List<HairObservation> lst = context.HairObservations.Where(x => x.HairProfileId == hair.Id && x.ObsPhysicalProductId != null).ToList();
                            context.RemoveRange(lst);
                            context.SaveChanges();

                            List<HairObservation> lstHairObservation = new List<HairObservation>();
                            foreach (var obser in hairProfile.observationPhysicalModels)
                            {
                                HairObservation hairObservation = new HairObservation();
                                hairObservation.HairProfileId = hair.Id;
                                hairObservation.ObsPhysicalProductId = obser.Id;
                                hairObservation.IsTopLeft = obser.IsTopLeft;
                                hairObservation.IsTopRight = obser.IsTopRight;
                                hairObservation.IsBottomLeft = obser.IsBottomLeft;
                                hairObservation.IsBottomRight = obser.IsBottomRight;
                                hairObservation.IsCrown = obser.IsCrown;

                                lstHairObservation.Add(hairObservation);
                            }
                            context.AddRange(lstHairObservation);
                            context.SaveChanges();
                        }


                        if (hairProfile.observationBreakageModels.Count() > 0)
                        {
                            List<HairObservation> lst = context.HairObservations.Where(x => x.HairProfileId == hair.Id && x.ObsBreakageId != null).ToList();
                            context.RemoveRange(lst);
                            context.SaveChanges();

                            List<HairObservation> lstHairObservation = new List<HairObservation>();
                            foreach (var obser in hairProfile.observationBreakageModels)
                            {
                                HairObservation hairObservation = new HairObservation();
                                hairObservation.HairProfileId = hair.Id;
                                hairObservation.ObsBreakageId = obser.Id;
                                hairObservation.IsTopLeft = obser.IsTopLeft;
                                hairObservation.IsTopRight = obser.IsTopRight;
                                hairObservation.IsBottomLeft = obser.IsBottomLeft;
                                hairObservation.IsBottomRight = obser.IsBottomRight;
                                hairObservation.IsCrown = obser.IsCrown;
                                lstHairObservation.Add(hairObservation);

                            }
                            context.AddRange(lstHairObservation);
                            context.SaveChanges();
                        }

                        if (hairProfile.observationSplittingModels.Count() > 0)
                        {
                            List<HairObservation> lst = context.HairObservations.Where(x => x.HairProfileId == hair.Id && x.ObsSplittingId != null).ToList();
                            context.RemoveRange(lst);
                            context.SaveChanges();

                            List<HairObservation> lstHairObservation = new List<HairObservation>();
                            foreach (var obser in hairProfile.observationSplittingModels)
                            {
                                HairObservation hairObservation = new HairObservation();
                                hairObservation.HairProfileId = hair.Id;
                                hairObservation.ObsSplittingId = obser.Id;
                                hairObservation.IsTopLeft = obser.IsTopLeft;
                                hairObservation.IsTopRight = obser.IsTopRight;
                                hairObservation.IsBottomLeft = obser.IsBottomLeft;
                                hairObservation.IsBottomRight = obser.IsBottomRight;
                                hairObservation.IsCrown = obser.IsCrown;
                                lstHairObservation.Add(hairObservation);

                            }
                            context.AddRange(lstHairObservation);
                            context.SaveChanges();
                        }

                        if (hairProfile.Pororsity.Count() > 0)
                        {
                            List<HairPorosity> lst = context.HairPorosities.Where(x => x.HairProfileId == hair.Id).ToList();
                            context.RemoveRange(lst);
                            context.SaveChanges();

                            List<HairPorosity> lstHairPorosity = new List<HairPorosity>();
                            foreach (var poro in hairProfile.Pororsity)
                            {
                                HairPorosity hairPorosity = new HairPorosity();
                                hairPorosity.HairProfileId = hair.Id;
                                hairPorosity.PorosityId = poro.Id;
                                hairPorosity.IsTopLeft = poro.IsTopLeft;
                                hairPorosity.IsTopRight = poro.IsTopRight;
                                hairPorosity.IsBottomLeft = poro.IsBottomLeft;
                                hairPorosity.IsBottomRight = poro.IsBottomRight;
                                hairPorosity.IsCrown = poro.IsCrown;
                                lstHairPorosity.Add(hairPorosity);

                            }
                            context.AddRange(lstHairPorosity);
                            context.SaveChanges();
                        }

                        if (hairProfile.Elasticity.Count() > 0)
                        {
                            List<HairElasticity> lst = context.HairElasticities.Where(x => x.HairProfileId == hair.Id).ToList();
                            context.RemoveRange(lst);
                            context.SaveChanges();

                            List<HairElasticity> lstHairElasticity = new List<HairElasticity>();
                            foreach (var elas in hairProfile.Elasticity)
                            {
                                HairElasticity hairElasticity = new HairElasticity();
                                hairElasticity.HairProfileId = hair.Id;
                                hairElasticity.ElasticityId = elas.Id;
                                hairElasticity.IsTopLeft = elas.IsTopLeft;
                                hairElasticity.IsTopRight = elas.IsTopRight;
                                hairElasticity.IsBottomLeft = elas.IsBottomLeft;
                                hairElasticity.IsBottomRight = elas.IsBottomRight;
                                hairElasticity.IsCrown = elas.IsCrown;
                                lstHairElasticity.Add(hairElasticity);
                            }
                            context.AddRange(lstHairElasticity);
                            context.SaveChanges();
                        }

                    }
                    if (TabNo.Equals("Tab1"))
                    {
                        if (hairProfile.SelectedAnswer != null)
                        {
                            var ansResponse = context.AdditionalHairInfo.Where(x => x.HairId == hair.Id).FirstOrDefault();
                            int AdditonalHairInfoID = 0;
                            if (ansResponse != null)
                            {
                                AdditonalHairInfoID = ansResponse.Id;
                                ansResponse.HairId = hair.Id;
                                ansResponse.TypeId = hairProfile.SelectedAnswer.TypeId;
                                ansResponse.TypeDescription = hairProfile.SelectedAnswer.TypeDescription;
                                ansResponse.TextureId = hairProfile.SelectedAnswer.TextureId;
                                ansResponse.TextureDescription = hairProfile.SelectedAnswer.TextureDescription;
                                ansResponse.PorosityId = hairProfile.SelectedAnswer.PorosityId;
                                ansResponse.PorosityDescription = hairProfile.SelectedAnswer.PorosityDescription;
                                ansResponse.HealthId = hairProfile.SelectedAnswer.HealthId;
                                ansResponse.HealthDescription = hairProfile.SelectedAnswer.HealthDescription;
                                ansResponse.DensityId = hairProfile.SelectedAnswer.DensityId;
                                ansResponse.DensityDescription = hairProfile.SelectedAnswer.DensityDescription;
                                ansResponse.ElasticityId = hairProfile.SelectedAnswer.ElasticityId;
                                ansResponse.ElasticityDescription = hairProfile.SelectedAnswer.ElasticityDescription;
                                context.SaveChanges();

                                List<CustomerHairChallenge> hairChallenges = context.CustomerHairChallenge.Where(x => x.HairInfoId == ansResponse.Id).ToList();
                                List<CustomerHairGoals> hairGoals = context.CustomerHairGoals.Where(x => x.HairInfoId == ansResponse.Id).ToList();
                                context.RemoveRange(hairChallenges);
                                context.RemoveRange(hairGoals);
                                //context.Remove(ansResponse);
                                context.SaveChanges();
                            }
                            else
                            {
                                AdditionalHairInfo additionalHairInfo = new AdditionalHairInfo();
                                additionalHairInfo.HairId = hair.Id;
                                additionalHairInfo.TypeId = hairProfile.SelectedAnswer.TypeId;
                                additionalHairInfo.TypeDescription = hairProfile.SelectedAnswer.TypeDescription;
                                additionalHairInfo.TextureId = hairProfile.SelectedAnswer.TextureId;
                                additionalHairInfo.TextureDescription = hairProfile.SelectedAnswer.TextureDescription;
                                additionalHairInfo.PorosityId = hairProfile.SelectedAnswer.PorosityId;
                                additionalHairInfo.PorosityDescription = hairProfile.SelectedAnswer.PorosityDescription;
                                additionalHairInfo.HealthId = hairProfile.SelectedAnswer.HealthId;
                                additionalHairInfo.HealthDescription = hairProfile.SelectedAnswer.HealthDescription;
                                additionalHairInfo.DensityId = hairProfile.SelectedAnswer.DensityId;
                                additionalHairInfo.DensityDescription = hairProfile.SelectedAnswer.DensityDescription;
                                additionalHairInfo.ElasticityId = hairProfile.SelectedAnswer.ElasticityId;
                                additionalHairInfo.ElasticityDescription = hairProfile.SelectedAnswer.ElasticityDescription;
                                context.Add(additionalHairInfo);
                                context.SaveChanges();
                                AdditonalHairInfoID = additionalHairInfo.Id;
                            }

                            List<CustomerHairGoals> customerHairGoals = new List<CustomerHairGoals>();
                            foreach (var hairGoal in hairProfile.SelectedAnswer.Goals)
                            {
                                if (hairGoal != "")
                                {
                                    CustomerHairGoals customerHairGoal = new CustomerHairGoals();
                                    customerHairGoal.HairInfoId = AdditonalHairInfoID;//additionalHairInfo.Id;
                                    customerHairGoal.Description = hairGoal;
                                    customerHairGoal.CreatedOn = DateTime.Now;
                                    customerHairGoal.IsActive = true;
                                    customerHairGoals.Add(customerHairGoal);
                                }
                            }
                            context.AddRange(customerHairGoals);
                            List<CustomerHairChallenge> customerHairChallenges = new List<CustomerHairChallenge>();
                            foreach (var challenge in hairProfile.SelectedAnswer.Challenges)
                            {
                                if (challenge != "")
                                {
                                    CustomerHairChallenge customerHairChallenge = new CustomerHairChallenge();
                                    customerHairChallenge.HairInfoId = AdditonalHairInfoID;//additionalHairInfo.Id;
                                    customerHairChallenge.Description = challenge;
                                    customerHairChallenge.CreatedOn = DateTime.Now;
                                    customerHairChallenge.IsActive = true;
                                    customerHairChallenges.Add(customerHairChallenge);
                                }
                            }
                            context.AddRange(customerHairChallenges);
                            context.SaveChanges();
                        }


                    }

                    if (TabNo.Equals("Tab3"))
                    {
                        if (hairProfile.RecommendedProducts.Count() == 0)
                        {
                            List<RecommendedProducts> OldProducts = context.RecommendedProducts.Where(x => x.HairProfileId == hair.Id && x.IsAllEssential != true).ToList();
                            context.RemoveRange(OldProducts);
                            context.SaveChanges();
                        }
                        if (hairProfile.RecommendedProducts.Count() > 0)
                        {

                            List<RecommendedProducts> OldProducts = context.RecommendedProducts.Where(x => x.HairProfileId == hair.Id && x.IsAllEssential != true).ToList();

                            List<RecommendedProducts> SelectedProducts = hairProfile.RecommendedProducts.ToList();
                            List<RecommendedProducts> NewProductsToAdd = new List<RecommendedProducts>();
                            NewProductsToAdd = SelectedProducts.Where(item1 => OldProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();

                            List<RecommendedProducts> DeselectedProducts = new List<RecommendedProducts>();
                            DeselectedProducts = OldProducts.Where(item1 => SelectedProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();
                            if (DeselectedProducts.Count() > 0)
                            {
                                context.RemoveRange(DeselectedProducts);
                                context.SaveChanges();
                            }
                            if (NewProductsToAdd.Count() > 0)
                            {
                                foreach (var prod in NewProductsToAdd)
                                {

                                    prod.IsActive = true;
                                    prod.CreatedOn = DateTime.Now;
                                    prod.HairProfileId = hair.Id;
                                    prod.IsAllEssential = false;

                                }
                                context.AddRange(NewProductsToAdd);
                                context.SaveChanges();
                            }

                        }

                        if (hairProfile.RecommendedProductsStylings.Count() == 0)
                        {
                            List<RecommendedProductsStylingRegimen> OldStylingProducts = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hair.Id && x.IsAllStyling != true).ToList();
                            context.RemoveRange(OldStylingProducts);
                            context.SaveChanges();
                        }
                        if (hairProfile.RecommendedProductsStylings.Count() > 0)
                        {

                            List<RecommendedProductsStylingRegimen> OldStylingProducts = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hair.Id && x.IsAllStyling != true).ToList();

                            List<RecommendedProductsStylingRegimen> SelectedStylingProducts = hairProfile.RecommendedProductsStylings.ToList();
                            List<RecommendedProductsStylingRegimen> NewStylingProductsToAdd = new List<RecommendedProductsStylingRegimen>();
                            NewStylingProductsToAdd = SelectedStylingProducts.Where(item1 => OldStylingProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();

                            List<RecommendedProductsStylingRegimen> DeselectedStylingProducts = new List<RecommendedProductsStylingRegimen>();
                            DeselectedStylingProducts = OldStylingProducts.Where(item1 => SelectedStylingProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();
                            if (DeselectedStylingProducts.Count() > 0)
                            {
                                context.RemoveRange(DeselectedStylingProducts);
                                context.SaveChanges();
                            }
                            if (NewStylingProductsToAdd.Count() > 0)
                            {
                                foreach (var prod in NewStylingProductsToAdd)
                                {

                                    prod.IsActive = true;
                                    prod.CreatedOn = DateTime.Now;
                                    prod.HairProfileId = hair.Id;
                                    prod.IsAllStyling = false;

                                }
                                context.AddRange(NewStylingProductsToAdd);
                                context.SaveChanges();
                            }


                        }


                        //--------------All Products Essential------------
                        if (hairProfile.AllRecommendedProductsEssential.Count() == 0)
                        {

                            List<RecommendedProducts> OldAllProductsEssen = context.RecommendedProducts.Where(x => x.HairProfileId == hair.Id && x.IsAllEssential == true).ToList();
                            context.RemoveRange(OldAllProductsEssen);
                            context.SaveChanges();
                        }
                        if (hairProfile.AllRecommendedProductsEssential.Count() > 0)
                        {

                            List<RecommendedProducts> OldAllProductsEssen = context.RecommendedProducts.Where(x => x.HairProfileId == hair.Id && x.IsAllEssential == true).ToList();

                            List<RecommendedProducts> SelectedProducts = hairProfile.AllRecommendedProductsEssential.ToList();
                            List<RecommendedProducts> NewEssenProductsToAdd = new List<RecommendedProducts>();
                            NewEssenProductsToAdd = SelectedProducts.Where(item1 => OldAllProductsEssen.All(item2 => item1.ProductId != item2.ProductId)).ToList();

                            List<RecommendedProducts> DeselectedEssenProducts = new List<RecommendedProducts>();
                            DeselectedEssenProducts = OldAllProductsEssen.Where(item1 => SelectedProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();
                            if (DeselectedEssenProducts.Count() > 0)
                            {
                                context.RemoveRange(DeselectedEssenProducts);
                                context.SaveChanges();
                            }
                            if (NewEssenProductsToAdd.Count() > 0)
                            {
                                foreach (var prod in NewEssenProductsToAdd)
                                {

                                    prod.IsActive = true;
                                    prod.CreatedOn = DateTime.Now;
                                    prod.HairProfileId = hair.Id;
                                    prod.IsAllEssential = true;

                                }
                                context.AddRange(NewEssenProductsToAdd);
                                context.SaveChanges();
                            }

                        }

                        //---All Products styling
                        if (hairProfile.AllRecommendedProductsStyling.Count() == 0)
                        {

                            List<RecommendedProductsStylingRegimen> OldAllStylingProducts = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hair.Id && x.IsAllStyling == true).ToList();
                            context.RemoveRange(OldAllStylingProducts);
                            context.SaveChanges();
                        }
                        if (hairProfile.AllRecommendedProductsStyling.Count() > 0)
                        {

                            List<RecommendedProductsStylingRegimen> OldAllStylingProducts = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hair.Id && x.IsAllStyling == true).ToList();

                            List<RecommendedProductsStylingRegimen> SelectedStylingProducts = hairProfile.AllRecommendedProductsStyling.ToList();
                            List<RecommendedProductsStylingRegimen> NewAllStylingProductsToAdd = new List<RecommendedProductsStylingRegimen>();
                            NewAllStylingProductsToAdd = SelectedStylingProducts.Where(item1 => OldAllStylingProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();

                            List<RecommendedProductsStylingRegimen> DeselectedAllStylingProducts = new List<RecommendedProductsStylingRegimen>();
                            DeselectedAllStylingProducts = OldAllStylingProducts.Where(item1 => SelectedStylingProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();
                            if (DeselectedAllStylingProducts.Count() > 0)
                            {
                                context.RemoveRange(DeselectedAllStylingProducts);
                                context.SaveChanges();
                            }
                            if (NewAllStylingProductsToAdd.Count() > 0)
                            {
                                foreach (var prod in NewAllStylingProductsToAdd)
                                {

                                    prod.IsActive = true;
                                    prod.CreatedOn = DateTime.Now;
                                    prod.HairProfileId = hair.Id;
                                    prod.IsAllStyling = true;

                                }
                                context.AddRange(NewAllStylingProductsToAdd);
                                context.SaveChanges();
                            }


                        }
                    }

                    if (TabNo.Equals("Tab4"))
                    {
                        if (hairProfile.RecommendedIngredients.Count() == 0)
                        {
                            List<RecommendedIngredients> OldIngredients = context.RecommendedIngredients.Where(x => x.HairProfileId == hair.Id).ToList();
                            context.RemoveRange(OldIngredients);
                            context.SaveChanges();
                        }
                        if (hairProfile.RecommendedIngredients.Count() > 0)
                        {
                            List<RecommendedIngredients> OldIngredients = context.RecommendedIngredients.Where(x => x.HairProfileId == hair.Id).ToList();

                            List<RecommendedIngredients> SelectedIngredients = hairProfile.RecommendedIngredients.ToList();
                            List<RecommendedIngredients> NewIngredientsToAdd = new List<RecommendedIngredients>();
                            NewIngredientsToAdd = SelectedIngredients.Where(item1 => OldIngredients.All(item2 => item1.IngredientId != item2.IngredientId)).ToList();

                            List<RecommendedIngredients> DeselectedIngredients = new List<RecommendedIngredients>();
                            DeselectedIngredients = OldIngredients.Where(item1 => SelectedIngredients.All(item2 => item1.IngredientId != item2.IngredientId)).ToList();
                            if (DeselectedIngredients.Count() > 0)
                            {
                                context.RemoveRange(DeselectedIngredients);
                                context.SaveChanges();
                            }
                            if (NewIngredientsToAdd.Count() > 0)
                            {
                                foreach (var ing in NewIngredientsToAdd)
                                {
                                    ing.IsActive = true;
                                    ing.CreatedOn = DateTime.Now;
                                    ing.HairProfileId = hair.Id;
                                    // objIngredient.IngredientId = ing.IngredientId;
                                    //objIngredients.Add(objIngredient);
                                }
                                context.AddRange(NewIngredientsToAdd);
                                context.SaveChanges();
                            }
                        }


                        if (hairProfile.RecommendedTools.Count() == 0)
                        {
                            List<RecommendedTools> OldTools = context.RecommendedTools.Where(x => x.HairProfileId == hair.Id).ToList();
                            context.RemoveRange(OldTools);
                            context.SaveChanges();
                        }
                        if (hairProfile.RecommendedTools.Count() > 0)
                        {
                            //------------------------------------------------------


                            List<RecommendedTools> OldTools = context.RecommendedTools.Where(x => x.HairProfileId == hair.Id).ToList();
                            List<RecommendedTools> SelectedTools = hairProfile.RecommendedTools.ToList();

                            List<RecommendedTools> NewToolsToAdd = new List<RecommendedTools>();
                            NewToolsToAdd = SelectedTools.Where(item1 => OldTools.All(item2 => item1.Id != item2.ToolId)).ToList();

                            List<RecommendedTools> DeselectedTools = new List<RecommendedTools>();
                            DeselectedTools = OldTools.Where(item1 => SelectedTools.All(item2 => item1.ToolId != item2.Id)).ToList();
                            if (DeselectedTools.Count() > 0)
                            {
                                context.RemoveRange(DeselectedTools);
                                context.SaveChanges();
                            }
                            if (NewToolsToAdd.Count() > 0)
                            {
                                List<RecommendedTools> lstTools = new List<RecommendedTools>();
                                foreach (var tool in NewToolsToAdd)
                                {

                                    RecommendedTools objTools = new RecommendedTools();
                                    objTools.Name = tool.Name;
                                    objTools.IsActive = true;
                                    objTools.CreatedOn = DateTime.Now;
                                    objTools.HairProfileId = hair.Id;
                                    objTools.ToolId = tool.Id;
                                    lstTools.Add(objTools);
                                }
                                context.AddRange(lstTools);
                                context.SaveChanges();
                            }
                            //-----------------------------------------

                        }

                        if (hairProfile.RecommendedRegimens.Count() == 0)
                        {
                            List<RecommendedRegimens> OldRegimens = context.RecommendedRegimens.Where(x => x.HairProfileId == hair.Id).ToList();
                            context.RemoveRange(OldRegimens);
                            context.SaveChanges();
                        }
                        if (hairProfile.RecommendedRegimens.Count() > 0)
                        {
                            List<RecommendedRegimens> OldRegimens = context.RecommendedRegimens.Where(x => x.HairProfileId == hair.Id).ToList();
                            List<RecommendedRegimens> SelectedRegimens = hairProfile.RecommendedRegimens.ToList();

                            List<RecommendedRegimens> NewRegimensToAdd = new List<RecommendedRegimens>();
                            NewRegimensToAdd = SelectedRegimens.Where(item1 => OldRegimens.All(item2 => item1.RegimenId != item2.RegimenId)).ToList();

                            List<RecommendedRegimens> DeselectedRegimens = new List<RecommendedRegimens>();
                            DeselectedRegimens = OldRegimens.Where(item1 => SelectedRegimens.All(item2 => item1.RegimenId != item2.RegimenId)).ToList();

                            if (DeselectedRegimens.Count() > 0)
                            {
                                context.RemoveRange(DeselectedRegimens);
                                context.SaveChanges();
                            }

                            if (NewRegimensToAdd.Count() > 0)
                            {
                                foreach (var regim in NewRegimensToAdd)
                                {
                                    regim.IsActive = true;
                                    regim.CreatedOn = DateTime.Now;
                                    regim.HairProfileId = hair.Id;
                                    // regim.RegimenId = regim.RegimenId;

                                }
                                context.AddRange(NewRegimensToAdd);
                                context.SaveChanges();
                            }


                        }

                        if (hairProfile.RecommendedVideos.Count() == 0)
                        {
                            List<RecommendedVideos> OldVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hair.Id).ToList();
                            context.RemoveRange(OldVideos);
                            context.SaveChanges();
                        }
                        if (hairProfile.RecommendedVideos.Count() > 0)
                        {
                            List<RecommendedVideos> OldVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hair.Id).ToList();
                            List<RecommendedVideos> SelectedVideos = hairProfile.RecommendedVideos.ToList();

                            List<RecommendedVideos> NewVideosToAdd = new List<RecommendedVideos>();
                            NewVideosToAdd = SelectedVideos.Where(item1 => OldVideos.All(item2 => item1.MediaLinkEntityId != item2.MediaLinkEntityId)).ToList();

                            List<RecommendedVideos> DeselectedVideos = new List<RecommendedVideos>();
                            DeselectedVideos = OldVideos.Where(item1 => SelectedVideos.All(item2 => item1.MediaLinkEntityId != item2.MediaLinkEntityId)).ToList();

                            if (DeselectedVideos.Count() > 0)
                            {
                                context.RemoveRange(DeselectedVideos);
                                context.SaveChanges();
                            }

                            if (NewVideosToAdd.Count() > 0)
                            {
                                foreach (var vid in NewVideosToAdd)
                                {

                                    vid.IsActive = true;
                                    vid.CreatedOn = DateTime.Now;
                                    vid.HairProfileId = hair.Id;
                                    //  vid.MediaLinkEntityId = vid.MediaLinkEntityId;

                                }
                                context.AddRange(NewVideosToAdd);
                                context.SaveChanges();
                            }

                        }

                        if (hairProfile.RecommendedStylists.Count() == 0)
                        {
                            List<RecommendedStylist> OldStylists = context.RecommendedStylists.Where(x => x.HairProfileId == hair.Id).ToList();
                            context.RemoveRange(OldStylists);
                            context.SaveChanges();
                        }
                        if (hairProfile.RecommendedStylists.Count() > 0)
                        {
                            List<RecommendedStylist> OldStylists = context.RecommendedStylists.Where(x => x.HairProfileId == hair.Id).ToList();
                            List<RecommendedStylist> SelectedStylists = hairProfile.RecommendedStylists.ToList();

                            List<RecommendedStylist> NewStylistsToAdd = new List<RecommendedStylist>();
                            NewStylistsToAdd = SelectedStylists.Where(item1 => OldStylists.All(item2 => item1.StylistId != item2.StylistId)).ToList();

                            List<RecommendedStylist> DeselectedStylists = new List<RecommendedStylist>();
                            DeselectedStylists = OldStylists.Where(item1 => SelectedStylists.All(item2 => item1.StylistId != item2.StylistId)).ToList();

                            if (DeselectedStylists.Count() > 0)
                            {
                                context.RemoveRange(DeselectedStylists);
                                context.SaveChanges();
                            }

                            if (NewStylistsToAdd.Count() > 0)
                            {
                                foreach (var styl in NewStylistsToAdd)
                                {

                                    styl.IsActive = true;
                                    styl.CreatedOn = DateTime.Now;
                                    styl.HairProfileId = hair.Id;


                                }
                                context.AddRange(NewStylistsToAdd);
                                context.SaveChanges();
                            }

                        }
                    }
                    //----------------------------------
                    if (TabNo.Equals("Tab5"))
                    {
                        var existRecord = context.StyleRecipeHairStyle
                            .FirstOrDefault(sr => sr.HairProfileId == hair.Id);
                        if (existRecord != null)
                        {

                            existRecord.CreatedOn = DateTime.Now;
                            existRecord.HairStyleId = Convert.ToInt32(hairProfile.HairStyleId);
                            context.StyleRecipeHairStyle.Update(existRecord);
                            context.SaveChanges();
                        }
                        else
                        {
                            var obj = new StyleRecipeHairStyle();
                            obj.HairStyleId = Convert.ToInt32(hairProfile.HairStyleId);
                            obj.HairProfileId = hair.Id;
                            obj.CreatedOn = DateTime.Now;
                            context.StyleRecipeHairStyle.Add(obj);
                            context.SaveChanges();
                        }
                        if (hairProfile.RecommendedStyleRecipeVideos.Count() == 0)
                        {
                            List<RecommendedStyleRecipeVideos> OldStyleVideos = context.RecommendedStyleRecipeVideos.Where(x => x.HairProfileId == hair.Id).ToList();
                            context.RemoveRange(OldStyleVideos);
                            context.SaveChanges();
                        }
                        if (hairProfile.RecommendedStyleRecipeVideos.Count() > 0)
                        {
                            List<RecommendedStyleRecipeVideos> OldVideos = context.RecommendedStyleRecipeVideos.Where(x => x.HairProfileId == hair.Id).ToList();
                            List<RecommendedStyleRecipeVideos> SelectedVideos = hairProfile.RecommendedStyleRecipeVideos.ToList();

                            List<RecommendedStyleRecipeVideos> NewVideosToAdd = new List<RecommendedStyleRecipeVideos>();
                            NewVideosToAdd = SelectedVideos.Where(item1 => OldVideos.All(item2 => item1.MediaLinkEntityId != item2.MediaLinkEntityId)).ToList();

                            List<RecommendedStyleRecipeVideos> DeselectedVideos = new List<RecommendedStyleRecipeVideos>();
                            DeselectedVideos = OldVideos.Where(item1 => SelectedVideos.All(item2 => item1.MediaLinkEntityId != item2.MediaLinkEntityId)).ToList();

                            if (DeselectedVideos.Count() > 0)
                            {
                                context.RemoveRange(DeselectedVideos);
                                context.SaveChanges();
                            }

                            if (NewVideosToAdd.Count() > 0)
                            {
                                foreach (var vid in NewVideosToAdd)
                                {

                                    vid.IsActive = true;
                                    vid.CreatedOn = DateTime.Now;
                                    vid.HairProfileId = hair.Id;
                                    //  vid.MediaLinkEntityId = vid.MediaLinkEntityId;

                                }
                                context.AddRange(NewVideosToAdd);
                                context.SaveChanges();
                            }

                        }

                        //---Products style recipe
                        if (hairProfile.RecommendedProductsStyleRecipe.Count() == 0)
                        {

                            List<RecommendedProductsStyleRecipe> OldStylingProducts = context.RecommendedProductsStyleRecipe.Where(x => x.HairProfileId == hair.Id).ToList();
                            context.RemoveRange(OldStylingProducts);
                            context.SaveChanges();
                        }
                        if (hairProfile.RecommendedProductsStyleRecipe.Count() > 0)
                        {

                            List<RecommendedProductsStyleRecipe> OldStylingProducts = context.RecommendedProductsStyleRecipe.Where(x => x.HairProfileId == hair.Id).ToList();

                            List<RecommendedProductsStyleRecipe> SelectedStylingProducts = hairProfile.RecommendedProductsStyleRecipe.ToList();
                            List<RecommendedProductsStyleRecipe> NewStylingProductsToAdd = new List<RecommendedProductsStyleRecipe>();
                            NewStylingProductsToAdd = SelectedStylingProducts.Where(item1 => OldStylingProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();

                            List<RecommendedProductsStyleRecipe> DeselectedStylingProducts = new List<RecommendedProductsStyleRecipe>();
                            DeselectedStylingProducts = OldStylingProducts.Where(item1 => SelectedStylingProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();
                            if (DeselectedStylingProducts.Count() > 0)
                            {
                                context.RemoveRange(DeselectedStylingProducts);
                                context.SaveChanges();
                            }
                            if (NewStylingProductsToAdd.Count() > 0)
                            {
                                foreach (var prod in NewStylingProductsToAdd)
                                {
                                    prod.IsActive = true;
                                    prod.CreatedOn = DateTime.Now;
                                    prod.HairProfileId = hair.Id;
                                }
                                context.AddRange(NewStylingProductsToAdd);
                                context.SaveChanges();
                            }
                        }

                    }
                    //----------------------------------------
                    if (hairProfile.NotifyUser.ToLower() == "true")
                    {


                        EmailInformation emailInformation = new EmailInformation
                        {
                            //Code = activationCode,
                            Email = user.Email,
                            Name = user.FirstName + " " + user.LastName,

                        };

                        var emailRes = _emailService.SendEmail("HHCPUPDT", emailInformation);
                    }
                }
                hairProfile.HairProfileId = hair.Id.ToString();
                return hairProfile;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SaveProfile, UserId:" + hairProfile.UserId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }


        public HairProfileAdminModel GetHairProfileAdmin(HairProfileAdminModel hairProfileModel)
        {
            try
            {
                HairProfileAdminModel profile = new HairProfileAdminModel();
                //int hairId = context.HairProfiles.Where(x => x.UserId == hairProfileModel.UserId && x.IsActive == true).Select(x => x.Id).LastOrDefault();
                int hairId;
                if (hairProfileModel.HairProfileId == null)
                {
                    hairId = context.HairProfiles.Where(x => x.UserId == hairProfileModel.UserId && x.IsActive == true).Select(x => x.Id).LastOrDefault();
                }
                else
                {
                    hairId = Convert.ToInt32(hairProfileModel.HairProfileId);
                }
                string TabNo = hairProfileModel.TabNo;
                if (TabNo.Equals("Tab1"))
                {
                    profile = new HairProfileAdminModel();
                    profile = (from hr in context.HairProfiles
                                   //where hr.UserId == hairProfileModel.UserId
                               where hr.Id == hairId
                               && hr.IsActive == true
                               select new HairProfileAdminModel()
                               {
                                   UserId = hr.UserId,
                                   HairId = hr.HairId,
                                   HealthSummary = hr.HealthSummary,
                                   ConsultantNotes = hr.ConsultantNotes,
                                   RecommendationNotes = hr.RecommendationNotes,
                                   CreatedBy = hr.CreatedBy,
                               }).LastOrDefault();
                }
                if (TabNo.Equals("Tab2"))
                {
                    profile = new HairProfileAdminModel();
                    profile = (from hr in context.HairProfiles
                               join st in context.HairStrands
                               on hr.Id equals st.HairProfileId
                               //where hr.UserId == hairProfileModel.UserId
                               where hr.Id == hairId 
                               && hr.IsActive == true
                               select new HairProfileAdminModel()
                               {
                                   HairAnalysisNotes = hr.HairAnalysisNotes,
                                   TopLeft = new TopLeftAdmin()
                                   {
                                       //TopLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopLeftImage != null && x.TopLeftImage != "").Select(x => x.TopLeftImage).ToList(),
                                       TopLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopLeftImage != null && x.TopLeftImage != "" && x.IsActive == true)
                                       .Select(x => new HairStrandImageInfo
                                       {
                                           StrandImage = x.TopLeftImage,
                                           StrandImageId = x.StrandsImagesId
                                       }).ToList(),
                                       TopLeftHealthText = st.TopLeftHealthText,
                                       TopLeftStrandDiameter = st.TopLeftStrandDiameter,
                                       Health = (from hb in context.HairHealths
                                                 join ob in context.Healths
                                                 on hb.HealthId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                 select new HealthModel()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).ToList(),
                                       Observation = (from hb in context.HairObservations
                                                      join ob in context.Observations
                                                      on hb.ObservationId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                      select new Observation()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                       obsElasticities = (from hb in context.HairObservations
                                                          join ob in context.ObsElasticities
                                                          on hb.ObsElasticityId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                          select new ObsElasticity()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                       obsChemicalProducts = (from hb in context.HairObservations
                                                              join ob in context.ObsChemicalProducts
                                                              on hb.ObsChemicalProductId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                              select new ObsChemicalProducts()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                       obsPhysicalProducts = (from hb in context.HairObservations
                                                              join ob in context.ObsPhysicalProducts
                                                              on hb.ObsPhysicalProductId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                              select new ObsPhysicalProducts()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                       obsDamages = (from hb in context.HairObservations
                                                     join ob in context.ObsDamage
                                                     on hb.ObsDamageId equals ob.Id
                                                     where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                     select new ObsDamage()
                                                     {
                                                         Id = ob.Id,
                                                         Description = ob.Description
                                                     }).ToList(),
                                       obsBreakages = (from hb in context.HairObservations
                                                       join ob in context.ObsBreakage
                                                       on hb.ObsBreakageId equals ob.Id
                                                       where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                       select new ObsBreakage()
                                                       {
                                                           Id = ob.Id,
                                                           Description = ob.Description
                                                       }).ToList(),
                                       obsSplittings = (from hb in context.HairObservations
                                                        join ob in context.ObsSplitting
                                                        on hb.ObsSplittingId equals ob.Id
                                                        where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                        select new ObsSplitting()
                                                        {
                                                            Id = ob.Id,
                                                            Description = ob.Description
                                                        }).ToList(),
                                       Pororsity = (from hb in context.HairPorosities
                                                    join ob in context.Pororsities
                                                    on hb.PorosityId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                    select new Pororsity()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).FirstOrDefault(),
                                       Elasticity = (from hb in context.HairElasticities
                                                     join ob in context.Elasticities
                                                     on hb.ElasticityId equals ob.Id
                                                     where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                     select new Elasticity()
                                                     {
                                                         Id = ob.Id,
                                                         Description = ob.Description
                                                     }).FirstOrDefault()

                                   },
                                   TopRight = new TopRightAdmin()
                                   {
                                       TopRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopRightImage != null && x.TopRightImage != "" && x.IsActive == true)
                                        .Select(x => new HairStrandImageInfo
                                        {
                                            StrandImage = x.TopRightImage,
                                            StrandImageId = x.StrandsImagesId
                                        }).ToList(),
                                       TopRightHealthText = st.TopRightHealthText,
                                       TopRightStrandDiameter = st.TopRightStrandDiameter,
                                       Health = (from hb in context.HairHealths
                                                 join ob in context.Healths
                                                 on hb.HealthId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                 select new HealthModel()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).ToList(),
                                       Observation = (from hb in context.HairObservations
                                                      join ob in context.Observations
                                                      on hb.ObservationId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                      select new Observation()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                       obsElasticities = (from hb in context.HairObservations
                                                          join ob in context.ObsElasticities
                                                          on hb.ObsElasticityId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                          select new ObsElasticity()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                       obsChemicalProducts = (from hb in context.HairObservations
                                                              join ob in context.ObsChemicalProducts
                                                              on hb.ObsChemicalProductId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                              select new ObsChemicalProducts()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                       obsPhysicalProducts = (from hb in context.HairObservations
                                                              join ob in context.ObsPhysicalProducts
                                                              on hb.ObsPhysicalProductId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                              select new ObsPhysicalProducts()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                       obsDamages = (from hb in context.HairObservations
                                                     join ob in context.ObsDamage
                                                     on hb.ObsDamageId equals ob.Id
                                                     where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                     select new ObsDamage()
                                                     {
                                                         Id = ob.Id,
                                                         Description = ob.Description
                                                     }).ToList(),
                                       obsBreakages = (from hb in context.HairObservations
                                                       join ob in context.ObsBreakage
                                                       on hb.ObsBreakageId equals ob.Id
                                                       where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                       select new ObsBreakage()
                                                       {
                                                           Id = ob.Id,
                                                           Description = ob.Description
                                                       }).ToList(),
                                       obsSplittings = (from hb in context.HairObservations
                                                        join ob in context.ObsSplitting
                                                        on hb.ObsSplittingId equals ob.Id
                                                        where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                        select new ObsSplitting()
                                                        {
                                                            Id = ob.Id,
                                                            Description = ob.Description
                                                        }).ToList(),
                                       Pororsity = (from hb in context.HairPorosities
                                                    join ob in context.Pororsities
                                                    on hb.PorosityId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                    select new Pororsity()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).FirstOrDefault(),
                                       Elasticity = (from hb in context.HairElasticities
                                                     join ob in context.Elasticities
                                                     on hb.ElasticityId equals ob.Id
                                                     where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                     select new Elasticity()
                                                     {
                                                         Id = ob.Id,
                                                         Description = ob.Description
                                                     }).FirstOrDefault()
                                   },
                                   BottomLeft = new BottomLeftAdmin()
                                   {
                                       BottomLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomLeftImage != null && x.BottomLeftImage != "" && x.IsActive == true)
                                        .Select(x => new HairStrandImageInfo
                                        {
                                            StrandImage = x.BottomLeftImage,
                                            StrandImageId = x.StrandsImagesId
                                        }).ToList(),
                                       BottomLeftHealthText = st.BottomLeftHealthText,
                                       BottomLeftStrandDiameter = st.BottomLeftStrandDiameter,
                                       Health = (from hb in context.HairHealths
                                                 join ob in context.Healths
                                                 on hb.HealthId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                 select new HealthModel()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).ToList(),
                                       Observation = (from hb in context.HairObservations
                                                      join ob in context.Observations
                                                      on hb.ObservationId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                      select new Observation()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                       obsElasticities = (from hb in context.HairObservations
                                                          join ob in context.ObsElasticities
                                                          on hb.ObsElasticityId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                          select new ObsElasticity()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                       obsChemicalProducts = (from hb in context.HairObservations
                                                              join ob in context.ObsChemicalProducts
                                                              on hb.ObsChemicalProductId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                              select new ObsChemicalProducts()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                       obsPhysicalProducts = (from hb in context.HairObservations
                                                              join ob in context.ObsPhysicalProducts
                                                              on hb.ObsPhysicalProductId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                              select new ObsPhysicalProducts()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                       obsDamages = (from hb in context.HairObservations
                                                     join ob in context.ObsDamage
                                                     on hb.ObsDamageId equals ob.Id
                                                     where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                     select new ObsDamage()
                                                     {
                                                         Id = ob.Id,
                                                         Description = ob.Description
                                                     }).ToList(),
                                       obsBreakages = (from hb in context.HairObservations
                                                       join ob in context.ObsBreakage
                                                       on hb.ObsBreakageId equals ob.Id
                                                       where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                       select new ObsBreakage()
                                                       {
                                                           Id = ob.Id,
                                                           Description = ob.Description
                                                       }).ToList(),
                                       obsSplittings = (from hb in context.HairObservations
                                                        join ob in context.ObsSplitting
                                                        on hb.ObsSplittingId equals ob.Id
                                                        where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                        select new ObsSplitting()
                                                        {
                                                            Id = ob.Id,
                                                            Description = ob.Description
                                                        }).ToList(),
                                       Pororsity = (from hb in context.HairPorosities
                                                    join ob in context.Pororsities
                                                    on hb.PorosityId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                    select new Pororsity()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).FirstOrDefault(),
                                       Elasticity = (from hb in context.HairElasticities
                                                     join ob in context.Elasticities
                                                     on hb.ElasticityId equals ob.Id
                                                     where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                     select new Elasticity()
                                                     {
                                                         Id = ob.Id,
                                                         Description = ob.Description
                                                     }).FirstOrDefault()
                                   },
                                   BottomRight = new BottomRightAdmin()
                                   {
                                       BottomRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomRightImage != null && x.BottomRightImage != "" && x.IsActive == true)
                                       .Select(x => new HairStrandImageInfo
                                       {
                                           StrandImage = x.BottomRightImage,
                                           StrandImageId = x.StrandsImagesId
                                       }).ToList(),
                                       BottomRightHealthText = st.BottomRightHealthText,
                                       BottomRightStrandDiameter = st.BottomRightStrandDiameter,
                                       Health = (from hb in context.HairHealths
                                                 join ob in context.Healths
                                                 on hb.HealthId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                 select new HealthModel()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).ToList(),
                                       Observation = (from hb in context.HairObservations
                                                      join ob in context.Observations
                                                      on hb.ObservationId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                      select new Observation()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                       obsElasticities = (from hb in context.HairObservations
                                                          join ob in context.ObsElasticities
                                                          on hb.ObsElasticityId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                          select new ObsElasticity()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                       obsChemicalProducts = (from hb in context.HairObservations
                                                              join ob in context.ObsChemicalProducts
                                                              on hb.ObsChemicalProductId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                              select new ObsChemicalProducts()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                       obsPhysicalProducts = (from hb in context.HairObservations
                                                              join ob in context.ObsPhysicalProducts
                                                              on hb.ObsPhysicalProductId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                              select new ObsPhysicalProducts()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                       obsDamages = (from hb in context.HairObservations
                                                     join ob in context.ObsDamage
                                                     on hb.ObsDamageId equals ob.Id
                                                     where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                     select new ObsDamage()
                                                     {
                                                         Id = ob.Id,
                                                         Description = ob.Description
                                                     }).ToList(),
                                       obsBreakages = (from hb in context.HairObservations
                                                       join ob in context.ObsBreakage
                                                       on hb.ObsBreakageId equals ob.Id
                                                       where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                       select new ObsBreakage()
                                                       {
                                                           Id = ob.Id,
                                                           Description = ob.Description
                                                       }).ToList(),
                                       obsSplittings = (from hb in context.HairObservations
                                                        join ob in context.ObsSplitting
                                                        on hb.ObsSplittingId equals ob.Id
                                                        where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                        select new ObsSplitting()
                                                        {
                                                            Id = ob.Id,
                                                            Description = ob.Description
                                                        }).ToList(),
                                       Pororsity = (from hb in context.HairPorosities
                                                    join ob in context.Pororsities
                                                    on hb.PorosityId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                    select new Pororsity()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).FirstOrDefault(),
                                       Elasticity = (from hb in context.HairElasticities
                                                     join ob in context.Elasticities
                                                     on hb.ElasticityId equals ob.Id
                                                     where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                     select new Elasticity()
                                                     {
                                                         Id = ob.Id,
                                                         Description = ob.Description
                                                     }).FirstOrDefault()
                                   },
                                   CrownStrand = new CrownStrandAdmin()
                                   {
                                       CrownHealthText = st.CrownHealthText,
                                       CrownPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.CrownImage != null && x.CrownImage != "" && x.IsActive == true)
                                      .Select(x => new HairStrandImageInfo
                                      {
                                          StrandImage = x.CrownImage,
                                          StrandImageId = x.StrandsImagesId
                                      }).ToList(),
                                       CrownStrandDiameter = st.CrownStrandDiameter,
                                       Health = (from hb in context.HairHealths
                                                 join ob in context.Healths
                                                 on hb.HealthId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                 select new HealthModel()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).ToList(),
                                       Observation = (from hb in context.HairObservations
                                                      join ob in context.Observations
                                                      on hb.ObservationId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                      select new Observation()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                       obsElasticities = (from hb in context.HairObservations
                                                          join ob in context.ObsElasticities
                                                          on hb.ObsElasticityId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                          select new ObsElasticity()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                       obsChemicalProducts = (from hb in context.HairObservations
                                                              join ob in context.ObsChemicalProducts
                                                              on hb.ObsChemicalProductId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                              select new ObsChemicalProducts()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                       obsPhysicalProducts = (from hb in context.HairObservations
                                                              join ob in context.ObsPhysicalProducts
                                                              on hb.ObsPhysicalProductId equals ob.Id
                                                              where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                              select new ObsPhysicalProducts()
                                                              {
                                                                  Id = ob.Id,
                                                                  Description = ob.Description
                                                              }).ToList(),
                                       obsDamages = (from hb in context.HairObservations
                                                     join ob in context.ObsDamage
                                                     on hb.ObsDamageId equals ob.Id
                                                     where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                     select new ObsDamage()
                                                     {
                                                         Id = ob.Id,
                                                         Description = ob.Description
                                                     }).ToList(),
                                       obsBreakages = (from hb in context.HairObservations
                                                       join ob in context.ObsBreakage
                                                       on hb.ObsBreakageId equals ob.Id
                                                       where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                       select new ObsBreakage()
                                                       {
                                                           Id = ob.Id,
                                                           Description = ob.Description
                                                       }).ToList(),
                                       obsSplittings = (from hb in context.HairObservations
                                                        join ob in context.ObsSplitting
                                                        on hb.ObsSplittingId equals ob.Id
                                                        where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                        select new ObsSplitting()
                                                        {
                                                            Id = ob.Id,
                                                            Description = ob.Description
                                                        }).ToList(),
                                       Pororsity = (from hb in context.HairPorosities
                                                    join ob in context.Pororsities
                                                    on hb.PorosityId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                    select new Pororsity()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).FirstOrDefault(),
                                       Elasticity = (from hb in context.HairElasticities
                                                     join ob in context.Elasticities
                                                     on hb.ElasticityId equals ob.Id
                                                     where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                     select new Elasticity()
                                                     {
                                                         Id = ob.Id,
                                                         Description = ob.Description
                                                     }).FirstOrDefault()
                                   },

                               }).LastOrDefault();

                }
                if (TabNo.Equals("Tab4"))
                {
                    profile = new HairProfileAdminModel();
                    profile.RecommendedVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hairId).ToList();
                    profile.RecommendedIngredients = context.RecommendedIngredients.Where(x => x.HairProfileId == hairId).ToList();
                    profile.RecommendedTools = context.RecommendedTools.Where(x => x.HairProfileId == hairId).ToList();
                    profile.RecommendedRegimens = context.RecommendedRegimens.Where(x => x.HairProfileId == hairId).ToList();
                    profile.RecommendedStylist = context.RecommendedStylists.Where(x => x.HairProfileId == hairId).ToList();
                    profile.VideoCategoryIds = context.MediaLinkEntities.Where(x => profile.RecommendedVideos.Select(y => y.MediaLinkEntityId).Contains(x.MediaLinkEntityId)).Select(z => z.VideoCategoryId).Distinct().ToArray();
                }
                if (TabNo.Equals("Tab5"))
                {
                    profile = new HairProfileAdminModel();
                    profile.RecommendedStyleRecipeVideos = context.RecommendedStyleRecipeVideos.Where(x => x.HairProfileId == hairId).ToList();
                    profile.RecommendedProductsStyleRecipe = context.RecommendedProductsStyleRecipe.Where(x => x.HairProfileId == hairId).ToList();

                    // --style recipe products
                    //List<RecommendedProductsModel> styleproductsTypesList = new List<RecommendedProductsModel>();
                    //var styleProds = (from s in context.ProductCommons
                    //                  join srecomm in context.RecommendedProductsStyleRecipe
                    //                  on s.ProductEntityId equals srecomm.ProductId
                    //                  where srecomm.HairProfileId == hairId && s.ProductTypeId != null && s.IsActive == true
                    //                  select s).Distinct().ToList();
                    //List<int?> styletypesNew = styleProds.Select(x => x.ProductTypeId).Distinct().ToList();
                    //List<int?> styleproductIdsNew = styleProds.Where(x => x.ProductTypeId != null).Select(x => x.ProductEntityId).Distinct().ToList();

                    //List<int?> styleparentIdsNew = context.ProductTypes.Where(x => styletypesNew.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    //var styleparentsNew = context.ProductTypeCategories.Where(x => styleparentIdsNew.Contains(x.Id)).ToList();

                    //foreach (var parentProduct in styleparentsNew)
                    //{

                    //    List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                    //    List<int?> productByTypes = (from s in styleProds
                    //                                 join pType in context.ProductTypes
                    //                                 on s.ProductTypeId equals pType.Id
                    //                                 where pType.ParentId == parentProduct.Id
                    //                                 select s.ProductTypeId).Distinct().ToList();

                    //    var existProdType = styleproductsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id);
                    //    if (existProdType != null)
                    //    {
                    //        foreach (var type in productByTypes)
                    //        {
                    //            if (type != null)
                    //            {
                    //                var existType = existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type);
                    //                if (existType != null)
                    //                {
                    //                    var products = (from prod in context.ProductEntities
                    //                                    join pCom in context.ProductCommons
                    //                                    on prod.Id equals pCom.ProductEntityId
                    //                                    join pType in context.ProductTypes
                    //                                    on pCom.ProductTypeId equals pType.Id
                    //                                    where pCom.ProductTypeId != null && styleproductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                    //                                    select prod).Distinct().ToList();
                    //                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();


                    //                    existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type).Products.AddRange(products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                    //                    {
                    //                        Id = x.Id,
                    //                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                    //                        ImageName = x.ImageName,
                    //                        ProductLink = x.ProductLink,
                    //                        ProductDetails = x.ProductDetails,
                    //                        ProductName = x.ProductName,
                    //                        ProductType = productType.ProductName
                    //                    }).ToList());
                    //                }
                    //                else
                    //                {
                    //                    ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                    //                    var products = (from prod in context.ProductEntities
                    //                                    join pCom in context.ProductCommons
                    //                                    on prod.Id equals pCom.ProductEntityId
                    //                                    join pType in context.ProductTypes
                    //                                    on pCom.ProductTypeId equals pType.Id
                    //                                    where pCom.ProductTypeId != null && styleproductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                    //                                    select prod).Distinct().ToList();
                    //                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                    //                    if (productType != null)
                    //                    {
                    //                        productsTypesNew.ProductTypeName = productType.ProductName;
                    //                        productsTypesNew.ProductId = type;
                    //                    }

                    //                    productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                    //                    {
                    //                        Id = x.Id,
                    //                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                    //                        ImageName = x.ImageName,
                    //                        ProductLink = x.ProductLink,
                    //                        ProductDetails = x.ProductDetails,
                    //                        ProductName = x.ProductName,
                    //                        ProductType = productType.ProductName
                    //                    }).ToList();
                    //                    productsTypesModels.Add(productsTypesNew);
                    //                }

                    //            }
                    //        }
                    //        styleproductsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id).ProductsTypes.AddRange(productsTypesModels);
                    //    }
                    //    else
                    //    {
                    //        foreach (var type in productByTypes)
                    //        {
                    //            if (type != null)
                    //            {
                    //                ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                    //                var products = (from prod in context.ProductEntities
                    //                                join pCom in context.ProductCommons
                    //                                on prod.Id equals pCom.ProductEntityId
                    //                                join pType in context.ProductTypes
                    //                                on pCom.ProductTypeId equals pType.Id
                    //                                where pCom.ProductTypeId != null && styleproductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                    //                                select prod).Distinct().ToList();
                    //                var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                    //                if (productType != null)
                    //                {
                    //                    productsTypesNew.ProductTypeName = productType.ProductName;
                    //                    productsTypesNew.ProductId = type;
                    //                }

                    //                productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                    //                {
                    //                    Id = x.Id,
                    //                    BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                    //                    ImageName = x.ImageName,
                    //                    ProductLink = x.ProductLink,
                    //                    ProductDetails = x.ProductDetails,
                    //                    ProductName = x.ProductName,
                    //                    ProductType = productType.ProductName
                    //                }).ToList();
                    //                productsTypesModels.Add(productsTypesNew);
                    //            }
                    //        }
                    //        RecommendedProductsModel productsTypes = new RecommendedProductsModel();
                    //        productsTypes.ProductParentName = parentProduct.CategoryName;
                    //        productsTypes.ProductId = parentProduct.Id;
                    //        productsTypes.ProductsTypes = productsTypesModels;
                    //        styleproductsTypesList.Add(productsTypes);
                    //    }

                    //}
                    //profile.RecommendedProductsStyleRecipe = styleproductsTypesList;
                    //---
                    // profile.RecommendedProductsStyleRecipe = context.RecommendedProductsStyleRecipe.Where(x => x.HairProfileId == hairId).ToList();
                    profile.HairStyleId = context.StyleRecipeHairStyle.Where(x => x.HairProfileId == hairId).Select(x => x.HairStyleId).FirstOrDefault().ToString();
                    profile.StyleRecipeVideoCategoryIds = context.MediaLinkEntities.Where(x => profile.RecommendedStyleRecipeVideos.Select(y => y.MediaLinkEntityId).Contains(x.MediaLinkEntityId)).Select(z => z.VideoCategoryId).Distinct().ToArray();
                }
                if (hairId != 0)
                {
                    if (TabNo.Equals("Tab3"))
                    {
                        profile = new HairProfileAdminModel();

                        //var user_id = context.Users.Where(x => x.UserName == hairProfileModel.UserId).Select(y => y.Id).FirstOrDefault();
                        //int? latestQA = context.Questionaires.Where(x => x.UserId == user_id.ToString() && x.IsActive == true).OrderByDescending(x => x.QA).FirstOrDefault()?.QA;

                        ////QuestionaireSelectedAnswer selectedAnswer = new QuestionaireSelectedAnswer();
                        ////var result = context.Questionaires.Include(x => x.Answer).Where(x => (x.QuestionId == 16 || x.QuestionId == 25) && x.UserId == hairProfileModel.UserId.ToString() && x.QA == latestQA).ToList();
                        ////selectedAnswer.Goals = result.Where(y => y.QuestionId == 25).Select(x => x.Answer.Description).ToList();
                        ////selectedAnswer.Challenges = result.Where(y => y.QuestionId == 16).Select(x => x.Answer.Description).ToList();

                        //int[] HairStyleIDs = null;
                        //int[] ProductTypeIds = null;

                        //var hairStyleAnswer = context.Questionaires.Include(x => x.Answer).Where(x => (x.QuestionId == 14 || x.QuestionId == 15) && x.UserId == user_id.ToString() && x.QA == latestQA).ToList();
                        //HairStyleIDs = hairStyleAnswer.Select(x => x.Answer.AnswerId).ToArray();
                        //ProductTypeIds = context.MappingHairStyleandProductType.Where(x => HairStyleIDs.Contains(x.HairStyleAnswerId)).Select(x => x.ProductTypeId).Distinct().ToArray();
                        //profile.MatchingProductTypeIds = ProductTypeIds.ToList();

                        //Healthy hair regimens
                        List<int> productIds = context.RecommendedProducts.Where(x => x.HairProfileId == hairId).Select(x => x.ProductId).ToList();
                        List<int?> types = context.ProductEntities.Where(x => productIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                        List<int?> parentIds = context.ProductTypes.Where(x => types.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                        var parents = context.ProductTypeCategories.Where(x => parentIds.Contains(x.Id)).ToList();
                        List<RecommendedProductsModel> productsTypesList = new List<RecommendedProductsModel>();
                        foreach (var parentProduct in parents)
                        {
                            RecommendedProductsModel productsTypes = new RecommendedProductsModel();
                            productsTypes.ProductParentName = parentProduct.CategoryName;
                            productsTypes.ProductId = parentProduct.Id;
                            List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                            List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id))
                                .Select(x => x.ProductTypesId).Distinct().ToList();

                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    ProductsTypesModels productsTypesModel = new ProductsTypesModels();
                                    var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id)).ToList();
                                    if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                                    {
                                        productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                        productsTypesModel.ProductId = type;
                                    }

                                    productsTypesModel.Products = products.Select(x => new ProductsModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                        HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                        ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                                    }).ToList();
                                    productsTypesModels.Add(productsTypesModel);
                                }
                            }
                            productsTypes.ProductsTypes = productsTypesModels;
                            productsTypesList.Add(productsTypes);
                        }

                        //profile.RecommendedProducts = productsTypesList;
                        //profile.AllRecommendedProductsEssential = context.RecommendedProducts.Where(x => x.HairProfileId == hairId && x.IsAllEssential == true).ToList();
                        //profile.AllRecommendedProductsStyling = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hairId && x.IsAllStyling == true).ToList();

                        //Start Essential Products Code after multiple Product Type functionality merge recommended products
                        var newEssProds = (from s in context.ProductCommons
                                           join srecomm in context.RecommendedProducts
                                           on s.ProductEntityId equals srecomm.ProductId
                                           where srecomm.HairProfileId == hairId && s.ProductTypeId != null && s.IsActive == true
                                           select s).Distinct().ToList();
                        List<int?> typesNew = newEssProds.Select(x => x.ProductTypeId).Distinct().ToList();
                        List<int?> productIdsNew = newEssProds.Where(x => x.ProductTypeId != null).Select(x => x.ProductEntityId).Distinct().ToList();

                        List<int?> parentIdsNew = context.ProductTypes.Where(x => typesNew.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                        var parentsNew = context.ProductTypeCategories.Where(x => parentIdsNew.Contains(x.Id)).ToList();

                        foreach (var parentProduct in parentsNew)
                        {
                            RecommendedProductsModel productsTypesNew = new RecommendedProductsModel();
                            List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                            List<int?> productByTypes = (from s in newEssProds
                                                         join pType in context.ProductTypes
                                                         on s.ProductTypeId equals pType.Id
                                                         where pType.ParentId == parentProduct.Id
                                                         select s.ProductTypeId).Distinct().ToList();

                            var existProdType = productsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id);
                            if (existProdType != null)
                            {
                                foreach (var type in productByTypes)
                                {
                                    if (type != null)
                                    {
                                        var existType = existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type);
                                        if (existType != null)
                                        {
                                            var products = (from prod in context.ProductEntities
                                                            join pCom in context.ProductCommons
                                                            on prod.Id equals pCom.ProductEntityId
                                                            join pType in context.ProductTypes
                                                            on pCom.ProductTypeId equals pType.Id
                                                            where pCom.ProductTypeId != null && productIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                            select prod).Distinct().ToList();
                                            var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();


                                            existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type).Products.AddRange(products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                            {
                                                Id = x.Id,
                                                BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                                ImageName = x.ImageName,
                                                ProductLink = x.ProductLink,
                                                ProductDetails = x.ProductDetails,
                                                ProductName = x.ProductName,
                                                ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                                HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                                ProductType = productType.ProductName
                                            }).ToList());
                                        }
                                        else
                                        {
                                            ProductsTypesModels productsTypesModel = new ProductsTypesModels();
                                            var products = (from prod in context.ProductEntities
                                                            join pCom in context.ProductCommons
                                                            on prod.Id equals pCom.ProductEntityId
                                                            join pType in context.ProductTypes
                                                            on pCom.ProductTypeId equals pType.Id
                                                            where pCom.ProductTypeId != null && productIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                            select prod).Distinct().ToList();
                                            var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                            if (productType != null)
                                            {
                                                productsTypesModel.ProductTypeName = productType.ProductName;
                                                productsTypesModel.ProductId = type;
                                            }

                                            productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                            {
                                                Id = x.Id,
                                                BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                                ImageName = x.ImageName,
                                                ProductLink = x.ProductLink,
                                                ProductDetails = x.ProductDetails,
                                                ProductName = x.ProductName,
                                                ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                                HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                                ProductType = productType.ProductName
                                            }).ToList();
                                            productsTypesModels.Add(productsTypesModel);
                                        }

                                    }
                                }
                                productsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id).ProductsTypes.AddRange(productsTypesModels);
                            }
                            else
                            {
                                foreach (var type in productByTypes)
                                {
                                    if (type != null)
                                    {
                                        ProductsTypesModels productsTypesModel = new ProductsTypesModels();
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && productIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                        if (productType != null)
                                        {
                                            productsTypesModel.ProductTypeName = productType.ProductName;
                                            productsTypesModel.ProductId = type;
                                        }

                                        productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                            HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                            ProductType = productType.ProductName
                                        }).ToList();
                                        productsTypesModels.Add(productsTypesModel);
                                    }
                                }
                                productsTypesNew.ProductParentName = parentProduct.CategoryName;
                                productsTypesNew.ProductId = parentProduct.Id;
                                productsTypesNew.ProductsTypes = productsTypesModels;
                                productsTypesList.Add(productsTypesNew);
                            }

                        }
                        profile.RecommendedProducts = productsTypesList;
                        profile.AllRecommendedProductsEssential = context.RecommendedProducts.Where(x => x.HairProfileId == hairId && x.IsAllEssential == true).ToList();
                        profile.AllRecommendedProductsStyling = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hairId && x.IsAllStyling == true).ToList();

                        //End Essential Products Code after multiple Product Type functionality merge recommended products

                        //Styling Regimens Code
                        List<int> rProductIds = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hairId).Select(x => x.ProductId).ToList();
                        List<int?> pTypes = context.ProductEntities.Where(x => rProductIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                        List<int?> pParentIds = context.ProductTypes.Where(x => pTypes.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                        var pParents = context.ProductTypeCategories.Where(x => pParentIds.Contains(x.Id)).ToList();
                        List<RecommendedProductsStylingModel> productsTypesStylingList = new List<RecommendedProductsStylingModel>();
                        foreach (var parentProduct in pParents)
                        {
                            RecommendedProductsStylingModel productsTypes = new RecommendedProductsStylingModel();
                            productsTypes.ProductParentName = parentProduct.CategoryName;
                            productsTypes.ProductId = parentProduct.Id;
                            List<ProductsTypesStylingModels> productsTypesModels = new List<ProductsTypesStylingModels>();
                            List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id))
                                .Select(x => x.ProductTypesId).Distinct().ToList();

                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                    var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id)).ToList();
                                    if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                                    {
                                        productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                        productsTypesModel.ProductId = type;
                                    }

                                    productsTypesModel.Products = products.Select(x => new ProductsStylingModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                        HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                        ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                                    }).ToList();
                                    productsTypesModels.Add(productsTypesModel);
                                }
                            }
                            productsTypes.ProductsTypes = productsTypesModels;
                            productsTypesStylingList.Add(productsTypes);
                        }
                        //profile.RecommendedProductsStyling = productsTypesStylingList;

                        //Start Styling Regimens Code after multiple Product Type functionality merge recommended products
                        var newProds = (from s in context.ProductCommons
                                        join srecomm in context.RecommendedProductsStyleRegimens
                                        on s.ProductEntityId equals srecomm.ProductId
                                        where srecomm.HairProfileId == hairId && s.ProductTypeId != null && s.IsActive == true
                                        select s).Distinct().ToList();
                        List<int?> pTypesNew = newProds.Select(x => x.ProductTypeId).Distinct().ToList();
                        List<int?> rProductIdsNew = newProds.Where(x => x.ProductTypeId != null).Select(x => x.ProductEntityId).Distinct().ToList();
                        List<int?> pParentIdsNew = context.ProductTypes.Where(x => pTypesNew.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                        var pParentsNew = context.ProductTypeCategories.Where(x => pParentIdsNew.Contains(x.Id)).ToList();

                        foreach (var parentProduct in pParentsNew)
                        {

                            List<ProductsTypesStylingModels> productsTypesModels = new List<ProductsTypesStylingModels>();
                            List<int?> productByTypes = (from s in newProds
                                                         join pType in context.ProductTypes
                                                         on s.ProductTypeId equals pType.Id
                                                         where pType.ParentId == parentProduct.Id
                                                         select s.ProductTypeId).Distinct().ToList();

                            var existProdType = productsTypesStylingList.FirstOrDefault(x => x.ProductId == parentProduct.Id);
                            if (existProdType != null)
                            {
                                foreach (var type in productByTypes)
                                {
                                    if (type != null)
                                    {
                                        var existType = existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type);
                                        if (existType != null)
                                        {
                                            //ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                            var products = (from prod in context.ProductEntities
                                                            join pCom in context.ProductCommons
                                                            on prod.Id equals pCom.ProductEntityId
                                                            join pType in context.ProductTypes
                                                            on pCom.ProductTypeId equals pType.Id
                                                            where pCom.ProductTypeId != null && rProductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                            select prod).Distinct().ToList();
                                            var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();


                                            existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type).Products.AddRange(products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                            {
                                                Id = x.Id,
                                                BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                                ImageName = x.ImageName,
                                                ProductLink = x.ProductLink,
                                                ProductDetails = x.ProductDetails,
                                                ProductName = x.ProductName,
                                                ProductType = productType.ProductName
                                            }).ToList());
                                        }
                                        else
                                        {
                                            ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                            var products = (from prod in context.ProductEntities
                                                            join pCom in context.ProductCommons
                                                            on prod.Id equals pCom.ProductEntityId
                                                            join pType in context.ProductTypes
                                                            on pCom.ProductTypeId equals pType.Id
                                                            where pCom.ProductTypeId != null && rProductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                            select prod).Distinct().ToList();
                                            var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                            if (productType != null)
                                            {
                                                productsTypesModel.ProductTypeName = productType.ProductName;
                                                productsTypesModel.ProductId = type;
                                            }

                                            productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                            {
                                                Id = x.Id,
                                                BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                                ImageName = x.ImageName,
                                                ProductLink = x.ProductLink,
                                                ProductDetails = x.ProductDetails,
                                                ProductName = x.ProductName,
                                                ProductType = productType.ProductName
                                            }).ToList();
                                            productsTypesModels.Add(productsTypesModel);
                                        }

                                    }
                                }
                                productsTypesStylingList.FirstOrDefault(x => x.ProductId == parentProduct.Id).ProductsTypes.AddRange(productsTypesModels);
                            }
                            else
                            {
                                foreach (var type in productByTypes)
                                {
                                    if (type != null)
                                    {
                                        ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && rProductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                        if (productType != null)
                                        {
                                            productsTypesModel.ProductTypeName = productType.ProductName;
                                            productsTypesModel.ProductId = type;
                                        }

                                        productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductType = productType.ProductName
                                        }).ToList();
                                        productsTypesModels.Add(productsTypesModel);
                                    }
                                }
                                RecommendedProductsStylingModel productsTypes = new RecommendedProductsStylingModel();
                                productsTypes.ProductParentName = parentProduct.CategoryName;
                                productsTypes.ProductId = parentProduct.Id;
                                productsTypes.ProductsTypes = productsTypesModels;
                                productsTypesStylingList.Add(productsTypes);
                            }

                        }

                        profile.RecommendedProductsStyling = productsTypesStylingList;
                        // End  Styling Regimens Code after multiple Product Type functionality merge recommended products
                    }
                    if (TabNo.Equals("Tab1"))
                    {
                        QuestionaireSelectedAnswer additionalHairInfo = new QuestionaireSelectedAnswer();
                        AdditionalHairInfo hairInfo = context.AdditionalHairInfo.Where(x => x.HairId == hairId).FirstOrDefault();
                        if (hairInfo != null)
                        {
                            additionalHairInfo.TypeId = hairInfo.TypeId;
                            additionalHairInfo.TypeDescription = hairInfo.TypeDescription;
                            additionalHairInfo.TextureId = hairInfo.TextureId;
                            additionalHairInfo.TextureDescription = hairInfo.TextureDescription;
                            additionalHairInfo.PorosityId = hairInfo.PorosityId;
                            additionalHairInfo.PorosityDescription = hairInfo.PorosityDescription;
                            additionalHairInfo.HealthId = hairInfo.HealthId;
                            additionalHairInfo.HealthDescription = hairInfo.HealthDescription;
                            additionalHairInfo.DensityId = hairInfo.DensityId;
                            additionalHairInfo.DensityDescription = hairInfo.DensityDescription;
                            additionalHairInfo.ElasticityId = hairInfo.ElasticityId;
                            additionalHairInfo.ElasticityDescription = hairInfo.ElasticityDescription;
                            additionalHairInfo.Goals = context.CustomerHairGoals.Where(x => x.HairInfoId == hairInfo.Id).Select(y => y.Description).ToList();
                            additionalHairInfo.Challenges = context.CustomerHairChallenge.Where(x => x.HairInfoId == hairInfo.Id).Select(y => y.Description).ToList();
                            profile.SelectedAnswers = additionalHairInfo;
                        }
                        if (!string.IsNullOrEmpty(hairProfileModel.LoginUserId))
                        {
                            profile.MyNotes = context.StylistNotesHHCPs.FirstOrDefault(x => x.HairProfileId == hairId && x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId))?.Notes;
                        }
                    }
                }
                var Data = context.HairProfiles.Where(a => a.Id == hairId).Select(a => a.CreatedBy).FirstOrDefault();
                if (hairProfileModel.CreatedBy == null || hairProfileModel.CreatedBy == 0)
                {
                    if (profile != null)
                    {
                        profile.CreatedBy = (Data == null?2: Data);
                    }
                }
                return profile;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetHairProfileAdmin, UserId:" + hairProfileModel.UserId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public QuestionaireSelectedAnswer GetQuestionaireAnswer(QuestionaireSelectedAnswer hairProfileModel)
        {
            try
            {
                var userid = context.Users.Where(x => x.UserName == hairProfileModel.UserEmail).Select(y => y.Id).FirstOrDefault();
                //int latestQA = context.Questionaires.Where(x => x.UserId == userid.ToString() && x.IsActive == true).Max(x => x.QA);
                int? latestQA = context.Questionaires.Where(x => x.UserId == userid.ToString() && x.IsActive == true).OrderByDescending(x => x.QA).FirstOrDefault()?.QA;

                QuestionaireSelectedAnswer selectedAnswer = new QuestionaireSelectedAnswer();
                var result = context.Questionaires.Include(x => x.Answer).Where(x => (x.QuestionId == 16 || x.QuestionId == 25) && x.UserId == userid.ToString() && x.QA == latestQA).ToList();
                selectedAnswer.Goals = result.Where(y => y.QuestionId == 25).Select(x => x.Answer.Description).ToList();
                selectedAnswer.Challenges = result.Where(y => y.QuestionId == 16).Select(x => x.Answer.Description).ToList();


                return selectedAnswer;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetQuestionaireAnswer, Email:" + hairProfileModel.UserEmail + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public List<RecommendedRegimensCustomer> RecommendedRegimensCustomer(int hairId)
        {
            List<RecommendedRegimensCustomer> recommendedRegimensModelList = new List<RecommendedRegimensCustomer>();
            try
            {
                List<int> regimenIdList = context.RecommendedRegimens.Where(x => x.HairProfileId == hairId).OrderByDescending(x => x.CreatedOn).Select(x => x.RegimenId).ToList();
                foreach (int regimenId in regimenIdList)
                {
                    Regimens regimens = context.Regimens.Where(x => x.RegimensId == regimenId).FirstOrDefault();
                    if (regimens != null)
                    {
                        RegimenSteps regimenList = context.RegimenSteps.Where(z => z.RegimenStepsId == regimens.RegimenStepsId).FirstOrDefault();

                        RegimenSteps regimenSteps = regimenList;

                        List<RegimenStepsModel> regimenStepsModels = GetRegimenSteps(regimenSteps);

                        RecommendedRegimensCustomer recommendedRegimensModel = new RecommendedRegimensCustomer();
                        recommendedRegimensModel.RegimenId = regimens.RegimensId;
                        recommendedRegimensModel.RegimenName = regimens.Name;
                        recommendedRegimensModel.RegimenSteps = regimenStepsModels;
                        recommendedRegimensModel.RegimenTitle = regimens.Title;
                        recommendedRegimensModel.Description = regimens.Description;

                        recommendedRegimensModelList.Add(recommendedRegimensModel);
                    }
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: RecommendedRegimensCustomer, HairProfileId:" + hairId + ", Error: " + Ex.Message, Ex);
            }

            return recommendedRegimensModelList;
        }

        public async Task<HairProfileCustomerAlexaModel> GetHairProfileCustomerAlexa(string userId)
        {
            HairProfileCustomerAlexaModel profile = new HairProfileCustomerAlexaModel();
            //var userName = await _userManager.FindByIdAsync(userId);

            int hairId = context.HairProfiles.Where(x => x.UserId == userId && x.IsActive == true && x.IsDrafted == false).Select(x => x.Id).FirstOrDefault();
            if (hairId != 0)
            {
                try
                {
                    RecommendedProductsAlexa recommendedProductsAlexa = new RecommendedProductsAlexa();
                    List<int> productIds = context.RecommendedProducts.Where(x => x.HairProfileId == hairId).OrderByDescending(x => x.CreatedOn).Select(x => x.ProductId).ToList();
                    List<int?> types = context.ProductEntities.Where(x => productIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                    List<int?> parentIds = context.ProductTypes.Where(x => types.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var parents = context.ProductTypeCategories.Where(x => parentIds.Contains(x.Id)).ToList();
                    List<ProductsTypesAlexaModels> productsTypesModels = new List<ProductsTypesAlexaModels>();
                    foreach (var parentProduct in parents)
                    {
                        List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id))
                            .Select(x => x.ProductTypesId).Distinct().ToList();

                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesAlexaModels productsTypesModel = new ProductsTypesAlexaModels();
                                var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id)).ToList();
                                if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                                {
                                    productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                }

                                productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModelsAlexa
                                {
                                    BrandName = x.BrandName,
                                    ImageName = x.ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                                }).ToList();
                                productsTypesModels.Add(productsTypesModel);
                            }
                        }
                    }
                    recommendedProductsAlexa.RecommendedEssentialProducts = productsTypesModels;


                    //Styling Regimens Code
                    List<int> rProductIds = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hairId).OrderByDescending(x => x.CreatedOn).Select(x => x.ProductId).ToList();
                    List<int?> pTypes = context.ProductEntities.Where(x => rProductIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                    List<int?> pParentIds = context.ProductTypes.Where(x => pTypes.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var pParents = context.ProductTypeCategories.Where(x => pParentIds.Contains(x.Id)).ToList();
                    List<RecommendedProductsStylingAlexaModel> productsTypesStylingList = new List<RecommendedProductsStylingAlexaModel>();
                    foreach (var parentProduct in pParents)
                    {
                        RecommendedProductsStylingAlexaModel productsTypes = new RecommendedProductsStylingAlexaModel();
                        productsTypes.ProductParentName = parentProduct.CategoryName;
                        List<ProductsTypesStylingAlexaModels> productsStylingTypesModels = new List<ProductsTypesStylingAlexaModels>();
                        List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id))
                            .Select(x => x.ProductTypesId).Distinct().ToList();

                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesStylingAlexaModels productsTypesModel = new ProductsTypesStylingAlexaModels();
                                var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id)).ToList();

                                productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsStylingAlexaModels
                                {
                                    BrandName = x.BrandName,
                                    ImageName = x.ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                                }).ToList();
                                productsStylingTypesModels.Add(productsTypesModel);
                            }
                        }
                        recommendedProductsAlexa.RecommendedStylingProducts = productsStylingTypesModels;
                    }

                    profile.products = recommendedProductsAlexa;
                    profile.nextProductUseTime = DateTime.Now;
                    profile.plan = context.CustomerHairGoals.Where(x => x.HairInfoId == hairId).OrderByDescending(x => x.CreatedOn).Select(y => y.Description).FirstOrDefault();
                }
                catch (Exception Ex)
                {
                    _logger.LogError("Method: GetHairProfileCustomerAlexa, UserId:" + userId + ", Error: " + Ex.Message, Ex);
                }
            }

            return profile;
        }
        public async Task<HairProfileCustomerModel> GetHairProfileCustomer(HairProfileCustomerModel hairProfileModel)
        {
            string WebApiUrl = configuration["WebApiUrl"];
            HairProfileCustomerModel profile = new HairProfileCustomerModel();
            var userName = await _userManager.FindByIdAsync(hairProfileModel.UserId);
            var latesthhcpId = context.HairProfiles.LastOrDefault(x => x.UserId == userName.UserName && x.IsActive == true && x.IsDrafted != true)?.Id;
            if (hairProfileModel.IsRequestedFromCustomer == true)
            {
                latesthhcpId = context.HairProfiles.LastOrDefault(x => x.UserId == userName.UserName && x.IsActive == true && x.IsDrafted != true && x.IsViewEnabled == true)?.Id;
            }
            if (hairProfileModel.HairProfileId != 0)
            {
                latesthhcpId = hairProfileModel.HairProfileId;
            }
            if (latesthhcpId == null)
            {
                return null;
            }
            try
            {
                profile = (from hr in context.HairProfiles
                           join sts in context.HairStrands
                           on hr.Id equals sts.HairProfileId into hs
                           from st in hs.DefaultIfEmpty()
                           where hr.Id == latesthhcpId
                           && hr.IsActive == true && hr.IsDrafted == false
                           select new HairProfileCustomerModel()
                           {
                               HairProfileId = hr.Id,
                               UserId = hr.UserId,
                               HairId = hr.HairId,
                               UserName = userName.FirstName + " " + userName.LastName,
                               AIResult = userName.AIResult,
                               CustomerTypeId = userName.CustomerTypeId,
                               IsAIV2Mobile = userName.IsAIV2Mobile,
                               HairAnalyst = context.HairAnalyst.Where(a => a.HairAnalystId == hr.CreatedBy).FirstOrDefault(),
                               //HairAnalyst = context.HairAnalyst.Where(a => a.HairAnalystId == hr.CreatedBy).ToList(),
                               //AIResultNew = context.CustomerAIResults.Where(x => x.HairProfileId == hr.Id).Select(x => x.AIResult).FirstOrDefault(),
                               AIResultNew = context.CustomerAIResults.Where(x => x.HairProfileId == hr.Id).FirstOrDefault() != null ? context.CustomerAIResults.Where(x => x.HairProfileId == hr.Id).Select(x => x.AIResult).FirstOrDefault() : context.CustomerAIResults.Where(x => x.UserId.ToString() == hairProfileModel.UserId).OrderBy(x => x.CreatedOn).Select(x => x.AIResult).FirstOrDefault(),
                               CountAIResults = (context.CustomerAIResults.Where(x => x.UserId.ToString() == hairProfileModel.UserId)).Count(),
                               IsVersion2 = context.CustomerAIResults.FirstOrDefault(x => x.HairProfileId == hr.Id && x.IsVersion2 == true) != null ? true : false,
                               HealthSummary = hr.HealthSummary,
                               ConsultantNotes = hr.ConsultantNotes,
                               RecommendationNotes = hr.RecommendationNotes,
                               HairAnalysisNotes = hr.HairAnalysisNotes,
                               SalonId = userName.SalonId,
                               IsViewEnabled = hr.IsViewEnabled,
                               CustomerTypeDesc = context.CustomerTypes.FirstOrDefault(x => x.CustomerTypeId == userName.CustomerTypeId).Description,
                               TopLeft = st != null ? new TopLeftAdmin()
                               {
                                   //TopLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopLeftImage != null && x.TopLeftImage != "")
                                   // .Select(x =>x.TopLeftImage.Contains("https://api.myavana.com")? x.TopLeftImage: ("http://admin.myavana.com/HairProfile/" + x.TopLeftImage).Replace(" ", "")).ToList(),

                                   TopLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopLeftImage != null && x.TopLeftImage != "" && x.IsActive == true)
                                   .Select(x => new HairStrandImageInfo
                                   {
                                       StrandImage = x.TopLeftImage.Contains(WebApiUrl)
                                                        ? x.TopLeftImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.TopLeftImage).Replace(" ", ""),
                                       StrandImageId = x.StrandsImagesId
                                   }).ToList(),
                                   TopLeftHealthText = st.TopLeftHealthText,
                                   TopLeftStrandDiameter = st.TopLeftStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()

                               } : new TopLeftAdmin(),
                               TopRight = st != null ? new TopRightAdmin()
                               {
                                   TopRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopRightImage != null && x.TopRightImage != "" && x.IsActive == true)
                                    .Select(x => new HairStrandImageInfo
                                    {
                                        StrandImage = x.TopRightImage.Contains(WebApiUrl)
                                                        ? x.TopRightImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.TopRightImage).Replace(" ", ""),
                                        StrandImageId = x.StrandsImagesId
                                    }).ToList(),
                                   TopRightHealthText = st.TopRightHealthText,
                                   TopRightStrandDiameter = st.TopRightStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()
                               } : new TopRightAdmin(),
                               BottomLeft = st != null ? new BottomLeftAdmin()
                               {
                                   BottomLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomLeftImage != null && x.BottomLeftImage != "" && x.IsActive == true)
                                  .Select(x => new HairStrandImageInfo
                                  {
                                      StrandImage = x.BottomLeftImage.Contains(WebApiUrl)
                                                        ? x.BottomLeftImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.BottomLeftImage).Replace(" ", ""),
                                      StrandImageId = x.StrandsImagesId
                                  }).ToList(),
                                   BottomLeftHealthText = st.BottomLeftHealthText,
                                   BottomLeftStrandDiameter = st.BottomLeftStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()
                               } : new BottomLeftAdmin(),
                               BottomRight = st != null ? new BottomRightAdmin()
                               {
                                   BottomRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomRightImage != null && x.BottomRightImage != "" && x.IsActive == true)
                                  .Select(x => new HairStrandImageInfo
                                  {
                                      StrandImage = x.BottomRightImage.Contains(WebApiUrl)
                                                        ? x.BottomRightImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.BottomRightImage).Replace(" ", ""),
                                      StrandImageId = x.StrandsImagesId
                                  }).ToList(),
                                   BottomRightHealthText = st.BottomRightHealthText,
                                   BottomRightStrandDiameter = st.BottomRightStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()
                               } : new BottomRightAdmin(),
                               CrownStrand = st != null ? new CrownStrandAdmin()
                               {
                                   CrownHealthText = st.CrownHealthText,
                                   CrownPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.CrownImage != null && x.CrownImage != "" && x.IsActive == true)
                                    .Select(x => new HairStrandImageInfo
                                    {
                                        StrandImage = x.CrownImage.Contains(WebApiUrl)
                                                        ? x.CrownImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.CrownImage).Replace(" ", ""),
                                        StrandImageId = x.StrandsImagesId
                                    }).ToList(),
                                   CrownStrandDiameter = st.CrownStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()
                               } : new CrownStrandAdmin(),
                               RecommendedVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedVideosCustomer
                               {
                                   Id = s.Id,
                                   MediaLinkEntityId = s.MediaLinkEntityId,
                                   HairProfileId = s.HairProfileId,
                                   Name = s.Name,
                                   // Videos = context.MediaLinkEntities.Where(x => x.MediaLinkEntityId == s.MediaLinkEntityId).Select(x => x.VideoId).ToList().ToString().Replace("watch","embed")
                                   Videos = (from media in context.MediaLinkEntities
                                             where media.MediaLinkEntityId == s.MediaLinkEntityId
                                             select media.VideoId.ToString().Replace("watch?v=", "embed/")).ToList()
                               }).ToList(),
                               RecommendedStyleRecipeVideos = context.RecommendedStyleRecipeVideos.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedVideosCustomer
                               {
                                   Id = s.Id,
                                   MediaLinkEntityId = s.MediaLinkEntityId,
                                   HairProfileId = s.HairProfileId,
                                   Name = s.Name,
                                   // Videos = context.MediaLinkEntities.Where(x => x.MediaLinkEntityId == s.MediaLinkEntityId).Select(x => x.VideoId).ToList().ToString().Replace("watch","embed")
                                   Videos = (from media in context.MediaLinkEntities
                                             where media.MediaLinkEntityId == s.MediaLinkEntityId
                                             select media.VideoId.ToString().Replace("watch?v=", "embed/")).ToList()
                               }).ToList(),
                               RecommendedIngredients = context.RecommendedIngredients.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedIngredientsCustomer
                               {
                                   Id = s.Id,
                                   IngredientId = s.IngredientId,
                                   HairProfileId = s.HairProfileId,
                                   Ingredients = context.IngedientsEntities.Where(x => x.IngedientsEntityId == s.IngredientId).Select(x => new IngredientsModels
                                   {
                                       Name = x.Name,
                                       ImageName = "http://admin.myavana.com/Ingredients/" + x.Image,
                                       Description = x.Description,
                                   }).ToList()
                               }).ToList(),
                               //--------------------------------------------
                               RecommendedTools = context.RecommendedTools.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedToolsCustomer
                               {
                                   Id = s.Id,
                                   ToolId = s.ToolId,
                                   HairProfileId = s.HairProfileId,
                                   ToolList = context.Tools.Where(x => x.Id == s.ToolId).Select(x => new ToolsModels
                                   {
                                       Name = x.ToolName,
                                       ImageName = x.Image,
                                       ToolDetail = x.ToolDetails
                                   }).ToList()
                               }).ToList(),



                               //---------------------------------------------
                               recommendedStylistCustomers = context.RecommendedStylists.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedStylistCustomer
                               {
                                   Id = s.Id,
                                   StylistId = s.StylistId,
                                   HairProfileId = s.HairProfileId,
                                   Stylist = context.Stylists.Where(x => x.StylistId == s.StylistId).Select(x => new StylistCustomerModel
                                   {
                                       StylistName = x.StylistName,
                                       Salon = x.SalonName,
                                       Email = x.Email,
                                       Phone = x.PhoneNumber,
                                       Website = x.Website,
                                       Instagram = x.Instagram
                                   }).ToList()
                               }).ToList()

                           }).FirstOrDefault();
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetHairProfileCustomer, UserId:" + hairProfileModel.UserId + ", Error: " + Ex.Message, Ex);
            }

            int hairId = context.HairProfiles.Where(x => x.UserId == userName.UserName && x.IsActive == true && x.IsDrafted == false).Select(x => x.Id).LastOrDefault();
            if (hairProfileModel.HairProfileId != 0)
            {
                hairId = hairProfileModel.HairProfileId;
            }
            if (hairId != 0)
            {
                try
                {
                    profile.RecommendedRegimens = RecommendedRegimensCustomer(hairId);

                    List<int> productIds = context.RecommendedProducts.Where(x => x.HairProfileId == hairId).OrderByDescending(x => x.CreatedOn).Select(x => x.ProductId).ToList();
                    List<int?> types = context.ProductEntities.Where(x => productIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                    List<int?> parentIds = context.ProductTypes.Where(x => types.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var parents = context.ProductTypeCategories.Where(x => parentIds.Contains(x.Id)).ToList();
                    List<RecommendedProductsCustomer> productsTypesList = new List<RecommendedProductsCustomer>();
                    List<RecommendedProductsCustomer> styleproductsTypesList = new List<RecommendedProductsCustomer>();
                    var brandsList = context.Brands.Where(x => x.HideInSearch == true && x.IsActive == true).Select(x => x.BrandName).ToList();

                    foreach (var parentProduct in parents)
                    {
                        RecommendedProductsCustomer productsTypes = new RecommendedProductsCustomer();
                        productsTypes.ProductParentName = parentProduct.CategoryName;
                        productsTypes.ProductId = parentProduct.Id;
                        List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                        List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id))
                            .Select(x => x.ProductTypesId).Distinct().ToList();

                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesModels productsTypesModel = new ProductsTypesModels();
                                var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type
                                && x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id) && x.HideInSearch != true && !brandsList.Contains(x.BrandName)).ToList();
                                if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                                {
                                    productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                    productsTypesModel.ProductId = type;
                                }

                                productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                {
                                    Id = x.Id,
                                    BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                    ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                    HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                    ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault(),
                                    HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                    {
                                        Description = p.HairChallenges.Description,
                                        HairChallengeId = p.HairChallenges.HairChallengeId
                                    }).ToList(),
                                    HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                    {
                                        Description = p.HairGoal.Description,
                                        HairGoalId = p.HairGoal.HairGoalId
                                    }).ToList()
                                }).ToList();
                                productsTypesModels.Add(productsTypesModel);
                            }
                        }
                        productsTypes.ProductsTypes = productsTypesModels;
                        productsTypesList.Add(productsTypes);
                    }

                    //profile.RecommendedProducts = productsTypesList;

                    //Start Essential Products Code after multiple Product Type functionality merge recommended products
                    var newEssProds = (from s in context.ProductCommons
                                       join srecomm in context.RecommendedProducts
                                       on s.ProductEntityId equals srecomm.ProductId
                                       where srecomm.HairProfileId == hairId && s.ProductTypeId != null && s.IsActive == true
                                       select s).Distinct().ToList();
                    List<int?> typesNew = newEssProds.Select(x => x.ProductTypeId).Distinct().ToList();
                    List<int?> productIdsNew = newEssProds.Where(x => x.ProductTypeId != null).Select(x => x.ProductEntityId).Distinct().ToList();

                    List<int?> parentIdsNew = context.ProductTypes.Where(x => typesNew.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var parentsNew = context.ProductTypeCategories.Where(x => parentIdsNew.Contains(x.Id)).ToList();

                    foreach (var parentProduct in parentsNew)
                    {

                        List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                        List<int?> productByTypes = (from s in newEssProds
                                                     join pType in context.ProductTypes
                                                     on s.ProductTypeId equals pType.Id
                                                     where pType.ParentId == parentProduct.Id
                                                     select s.ProductTypeId).Distinct().ToList();

                        var existProdType = productsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id);
                        if (existProdType != null)
                        {
                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    var existType = existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type);
                                    if (existType != null)
                                    {
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && pCom.IsActive == true
                                                        && productIdsNew.Contains(prod.Id) && pType.Id == type
                                                        && pType.ParentId == parentProduct.Id
                                                        && prod.HideInSearch != true
                                                        && !brandsList.Contains(prod.BrandName)
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();


                                        existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type).Products.AddRange(products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                            HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                            ProductType = productType.ProductName,
                                            HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                            {
                                                Description = p.HairChallenges.Description,
                                                HairChallengeId = p.HairChallenges.HairChallengeId
                                            }).ToList(),
                                            HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                            {
                                                Description = p.HairGoal.Description,
                                                HairGoalId = p.HairGoal.HairGoalId
                                            }).ToList()
                                        }).ToList());
                                    }
                                    else
                                    {
                                        ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && pCom.IsActive == true
                                                        && productIdsNew.Contains(prod.Id)
                                                        && pType.Id == type && pType.ParentId == parentProduct.Id
                                                        && prod.HideInSearch != true
                                                        && !brandsList.Contains(prod.BrandName)
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                        if (productType != null)
                                        {
                                            productsTypesNew.ProductTypeName = productType.ProductName;
                                            productsTypesNew.ProductId = type;
                                        }

                                        productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                            HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                            ProductType = productType.ProductName,
                                            HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                            {
                                                Description = p.HairChallenges.Description,
                                                HairChallengeId = p.HairChallenges.HairChallengeId
                                            }).ToList(),
                                            HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                            {
                                                Description = p.HairGoal.Description,
                                                HairGoalId = p.HairGoal.HairGoalId
                                            }).ToList()
                                        }).ToList();
                                        productsTypesModels.Add(productsTypesNew);
                                    }

                                }
                            }
                            productsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id).ProductsTypes.AddRange(productsTypesModels);
                        }
                        else
                        {
                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                                    var products = (from prod in context.ProductEntities
                                                    join pCom in context.ProductCommons
                                                    on prod.Id equals pCom.ProductEntityId
                                                    join pType in context.ProductTypes
                                                    on pCom.ProductTypeId equals pType.Id
                                                    where pCom.ProductTypeId != null && pCom.IsActive == true
                                                    && productIdsNew.Contains(prod.Id)
                                                    && pType.Id == type && pType.ParentId == parentProduct.Id
                                                    && prod.HideInSearch != true
                                                    && !brandsList.Contains(prod.BrandName)
                                                    select prod).Distinct().ToList();
                                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                    if (productType != null)
                                    {
                                        productsTypesNew.ProductTypeName = productType.ProductName;
                                        productsTypesNew.ProductId = type;
                                    }

                                    productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                        HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                        ProductType = productType.ProductName,
										HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                        {
                                            Description = p.HairChallenges.Description,
                                            HairChallengeId = p.HairChallenges.HairChallengeId
                                        }).ToList(),
                                        HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                        {
                                            Description = p.HairGoal.Description,
                                            HairGoalId = p.HairGoal.HairGoalId
                                        }).ToList()
                                    }).ToList();
                                    productsTypesModels.Add(productsTypesNew);
                                }
                            }
                            RecommendedProductsCustomer productsTypes = new RecommendedProductsCustomer();
                            productsTypes.ProductParentName = parentProduct.CategoryName;
                            productsTypes.ProductId = parentProduct.Id;
                            productsTypes.ProductsTypes = productsTypesModels;
                            productsTypesList.Add(productsTypes);
                        }

                    }
                    profile.RecommendedProducts = productsTypesList;
                    //End Essential Products Code after multiple Product Type functionality merge recommended products
                    //--style recipe products
                    var styleProds = (from s in context.ProductCommons
                                      join srecomm in context.RecommendedProductsStyleRecipe
                                      on s.ProductEntityId equals srecomm.ProductId
                                      where srecomm.HairProfileId == hairId && s.ProductTypeId != null && s.IsActive == true
                                      select s).Distinct().ToList();
                    List<int?> styletypesNew = styleProds.Select(x => x.ProductTypeId).Distinct().ToList();
                    List<int?> styleproductIdsNew = styleProds.Where(x => x.ProductTypeId != null).Select(x => x.ProductEntityId).Distinct().ToList();

                    List<int?> styleparentIdsNew = context.ProductTypes.Where(x => styletypesNew.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var styleparentsNew = context.ProductTypeCategories.Where(x => styleparentIdsNew.Contains(x.Id)).ToList();

                    foreach (var parentProduct in styleparentsNew)
                    {

                        List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                        List<int?> productByTypes = (from s in styleProds
                                                     join pType in context.ProductTypes
                                                     on s.ProductTypeId equals pType.Id
                                                     where pType.ParentId == parentProduct.Id
                                                     select s.ProductTypeId).Distinct().ToList();

                        var existProdType = styleproductsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id);
                        if (existProdType != null)
                        {
                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    var existType = existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type);
                                    if (existType != null)
                                    {
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && styleproductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();


                                        existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type).Products.AddRange(products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductType = productType.ProductName,
                                            HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                            {
                                                Description = p.HairChallenges.Description,
                                                HairChallengeId = p.HairChallenges.HairChallengeId
                                            }).ToList(),
                                            HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                            {
                                                Description = p.HairGoal.Description,
                                                HairGoalId = p.HairGoal.HairGoalId
                                            }).ToList()
                                        }).ToList());
                                    }
                                    else
                                    {
                                        ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && styleproductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                        if (productType != null)
                                        {
                                            productsTypesNew.ProductTypeName = productType.ProductName;
                                            productsTypesNew.ProductId = type;
                                        }

                                        productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductType = productType.ProductName,
                                            HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                            {
                                                Description = p.HairChallenges.Description,
                                                HairChallengeId = p.HairChallenges.HairChallengeId
                                            }).ToList(),
                                            HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                            {
                                                Description = p.HairGoal.Description,
                                                HairGoalId = p.HairGoal.HairGoalId
                                            }).ToList()
                                        }).ToList();
                                        productsTypesModels.Add(productsTypesNew);
                                    }

                                }
                            }
                            styleproductsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id).ProductsTypes.AddRange(productsTypesModels);
                        }
                        else
                        {
                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                                    var products = (from prod in context.ProductEntities
                                                    join pCom in context.ProductCommons
                                                    on prod.Id equals pCom.ProductEntityId
                                                    join pType in context.ProductTypes
                                                    on pCom.ProductTypeId equals pType.Id
                                                    where pCom.ProductTypeId != null && styleproductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                    select prod).Distinct().ToList();
                                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                    if (productType != null)
                                    {
                                        productsTypesNew.ProductTypeName = productType.ProductName;
                                        productsTypesNew.ProductId = type;
                                    }

                                    productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductType = productType.ProductName,
                                        HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                        {
                                            Description = p.HairChallenges.Description,
                                            HairChallengeId = p.HairChallenges.HairChallengeId
                                        }).ToList(),
                                        HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                        {
                                            Description = p.HairGoal.Description,
                                            HairGoalId = p.HairGoal.HairGoalId
                                        }).ToList()
                                    }).ToList();
                                    productsTypesModels.Add(productsTypesNew);
                                }
                            }
                            RecommendedProductsCustomer productsTypes = new RecommendedProductsCustomer();
                            productsTypes.ProductParentName = parentProduct.CategoryName;
                            productsTypes.ProductId = parentProduct.Id;
                            productsTypes.ProductsTypes = productsTypesModels;
                            styleproductsTypesList.Add(productsTypes);
                        }

                    }
                    profile.RecommendedProductsStyleRecipe = styleproductsTypesList;
                    //---
                    //Styling Regimens Code before multiple Product Type functionality
                    List<int> rProductIds = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hairId).OrderByDescending(x => x.CreatedOn).Select(x => x.ProductId).ToList();
                    List<int?> pTypes = context.ProductEntities.Where(x => rProductIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();

                    List<int?> pParentIds = context.ProductTypes.Where(x => pTypes.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var pParents = context.ProductTypeCategories.Where(x => pParentIds.Contains(x.Id)).ToList();
                    List<RecommendedProductsStylingModel> productsTypesStylingList = new List<RecommendedProductsStylingModel>();
                    foreach (var parentProduct in pParents)
                    {
                        RecommendedProductsStylingModel productsTypes = new RecommendedProductsStylingModel();
                        productsTypes.ProductParentName = parentProduct.CategoryName;
                        productsTypes.ProductId = parentProduct.Id;
                        List<ProductsTypesStylingModels> productsTypesModels = new List<ProductsTypesStylingModels>();
                        List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id))
                            .Select(x => x.ProductTypesId).Distinct().ToList();


                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type
                                && x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id) && x.HideInSearch != true && !brandsList.Contains(x.BrandName)).ToList();
                                if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                                {
                                    productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                    productsTypesModel.ProductId = type;
                                }

                                productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                {
                                    Id = x.Id,
                                    BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                    ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                                }).ToList();
                                productsTypesModels.Add(productsTypesModel);
                            }
                        }
                        productsTypes.ProductsTypes = productsTypesModels;
                        productsTypesStylingList.Add(productsTypes);
                    }
                    //profile.RecommendedProductsStyling = productsTypesStylingList;

                    //Start Styling Regimens Code after multiple Product Type functionality merge recommended products
                    var newProds = (from s in context.ProductCommons
                                    join srecomm in context.RecommendedProductsStyleRegimens
                                    on s.ProductEntityId equals srecomm.ProductId
                                    where srecomm.HairProfileId == hairId && s.ProductTypeId != null && s.IsActive == true
                                    select s).Distinct().ToList();
                    List<int?> pTypesNew = newProds.Select(x => x.ProductTypeId).Distinct().ToList();
                    List<int?> rProductIdsNew = newProds.Where(x => x.ProductTypeId != null).Select(x => x.ProductEntityId).Distinct().ToList();
                    List<int?> pParentIdsNew = context.ProductTypes.Where(x => pTypesNew.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var pParentsNew = context.ProductTypeCategories.Where(x => pParentIdsNew.Contains(x.Id)).ToList();

                    foreach (var parentProduct in pParentsNew)
                    {

                        List<ProductsTypesStylingModels> productsTypesModels = new List<ProductsTypesStylingModels>();
                        List<int?> productByTypes = (from s in newProds
                                                     join pType in context.ProductTypes
                                                     on s.ProductTypeId equals pType.Id
                                                     where pType.ParentId == parentProduct.Id
                                                     select s.ProductTypeId).Distinct().ToList();

                        var existProdType = productsTypesStylingList.FirstOrDefault(x => x.ProductId == parentProduct.Id);
                        if (existProdType != null)
                        {
                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    var existType = existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type);
                                    if (existType != null)
                                    {
                                        //ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && pCom.IsActive == true
                                                        && rProductIdsNew.Contains(prod.Id)
                                                        && pType.Id == type && pType.ParentId == parentProduct.Id
                                                        && prod.HideInSearch != true
                                                        && !brandsList.Contains(prod.BrandName)
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();


                                        existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type).Products.AddRange(products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductType = productType.ProductName
                                        }).ToList());
                                    }
                                    else
                                    {
                                        ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && pCom.IsActive == true
                                                        && rProductIdsNew.Contains(prod.Id)
                                                        && pType.Id == type && pType.ParentId == parentProduct.Id
                                                        && prod.HideInSearch != true
                                                        && !brandsList.Contains(prod.BrandName)
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                        if (productType != null)
                                        {
                                            productsTypesModel.ProductTypeName = productType.ProductName;
                                            productsTypesModel.ProductId = type;
                                        }

                                        productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductType = productType.ProductName
                                        }).ToList();
                                        productsTypesModels.Add(productsTypesModel);
                                    }

                                }
                            }
                            productsTypesStylingList.FirstOrDefault(x => x.ProductId == parentProduct.Id).ProductsTypes.AddRange(productsTypesModels);
                        }
                        else
                        {
                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                    var products = (from prod in context.ProductEntities
                                                    join pCom in context.ProductCommons
                                                    on prod.Id equals pCom.ProductEntityId
                                                    join pType in context.ProductTypes
                                                    on pCom.ProductTypeId equals pType.Id
                                                    where pCom.ProductTypeId != null && pCom.IsActive == true
                                                    && rProductIdsNew.Contains(prod.Id)
                                                    && pType.Id == type && pType.ParentId == parentProduct.Id
                                                    && prod.HideInSearch != true
                                                    && !brandsList.Contains(prod.BrandName)
                                                    select prod).Distinct().ToList();
                                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                    if (productType != null)
                                    {
                                        productsTypesModel.ProductTypeName = productType.ProductName;
                                        productsTypesModel.ProductId = type;
                                    }

                                    productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductType = productType.ProductName
                                    }).ToList();
                                    productsTypesModels.Add(productsTypesModel);
                                }
                            }
                            RecommendedProductsStylingModel productsTypes = new RecommendedProductsStylingModel();
                            productsTypes.ProductParentName = parentProduct.CategoryName;
                            productsTypes.ProductId = parentProduct.Id;
                            productsTypes.ProductsTypes = productsTypesModels;
                            productsTypesStylingList.Add(productsTypes);
                        }

                    }

                    profile.RecommendedProductsStyling = productsTypesStylingList;
                    // End  Styling Regimens Code after multiple Product Type functionality merge recommended products
                    profile.HairStyle = (from sr in context.StyleRecipeHairStyle
                                         join hr in context.HairStyles
                                         on sr.HairStyleId equals hr.Id
                                         where sr.HairProfileId == hairId
                                         select hr.Style
                                      ).FirstOrDefault();
                    QuestionaireSelectedAnswer additionalHairInfo = new QuestionaireSelectedAnswer();
                    AdditionalHairInfo hairInfo = context.AdditionalHairInfo.Where(x => x.HairId == hairId).FirstOrDefault();
                    if (hairInfo != null)
                    {
                        additionalHairInfo.TypeId = hairInfo.TypeId;
                        additionalHairInfo.TypeDescription = hairInfo.TypeDescription;
                        additionalHairInfo.TextureId = hairInfo.TextureId;
                        additionalHairInfo.TextureDescription = hairInfo.TextureDescription;
                        additionalHairInfo.PorosityId = hairInfo.PorosityId;
                        additionalHairInfo.PorosityDescription = hairInfo.PorosityDescription;
                        additionalHairInfo.HealthId = hairInfo.HealthId;
                        additionalHairInfo.HealthDescription = hairInfo.HealthDescription;
                        additionalHairInfo.DensityId = hairInfo.DensityId;
                        additionalHairInfo.DensityDescription = hairInfo.DensityDescription;
                        additionalHairInfo.ElasticityId = hairInfo.ElasticityId;
                        additionalHairInfo.ElasticityDescription = hairInfo.ElasticityDescription;
                        additionalHairInfo.Goals = context.CustomerHairGoals.Where(x => x.HairInfoId == hairInfo.Id).Select(y => y.Description).ToList();
                        additionalHairInfo.Challenges = context.CustomerHairChallenge.Where(x => x.HairInfoId == hairInfo.Id).Select(y => y.Description).ToList();
                        profile.SelectedAnswers = additionalHairInfo;
                    }
                    if (!string.IsNullOrEmpty(hairProfileModel.LoginUserId))
                    {
                        profile.MyNotes = context.StylistNotesHHCPs.FirstOrDefault(x => x.HairProfileId == hairId && x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId))?.Notes;
                    }
                    if (!string.IsNullOrEmpty(hairProfileModel.LoginUserId))
                    {
                        var salonNotes = context.SalonNotesHHCP.Include(x => x.WebLogin).Where(x => x.HairProfileId == hairId);
                        profile.SalonNotes = salonNotes.FirstOrDefault(x => x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId))?.Notes;

                        var userDetail = context.WebLogins.FirstOrDefault(x => x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId));

                        TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("US/Eastern");
                        if (userDetail?.UserTypeId == (int)UserTypeEnum.B2B && salonNotes != null)

                        {
                            var salonId = context.SalonsOwners.FirstOrDefault(x => x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId) && x.IsActive == true)?.SalonId;
                            if (salonId > 0)
                            {
                                var salonDet = context.Salons.FirstOrDefault(x => x.SalonId == salonId);
                                if (salonDet.IsPublicNotes == true)
                                {
                                    profile.SalonNotesModel = salonNotes.OrderByDescending(x => x.CreatedOn).Select(x => new SalonNotesModel
                                    {
                                        SalonNotes = x.Notes,
                                        HairProfileId = x.HairProfileId,
                                        LoginUserId = x.UserId.ToString(),
                                        CreatedBy = x.WebLogin.UserEmail,
                                        CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(x.CreatedOn, easternZone),
                                    }).ToList();
                                    profile.IsPublicSalonNote = true;
                                }
                                else
                                {
                                    profile.SalonNotesModel = salonNotes.OrderByDescending(x => x.CreatedOn).Where(x => x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId)).Select(x => new SalonNotesModel
                                    {
                                        SalonNotes = x.Notes,
                                        HairProfileId = x.HairProfileId,
                                        LoginUserId = x.UserId.ToString(),
                                        CreatedBy = x.WebLogin.UserEmail,
                                        CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(x.CreatedOn, easternZone),
                                    }).ToList();

                                }
                            }
                        }
                        else
                        {
                            profile.SalonNotesModel = salonNotes.OrderByDescending(x => x.CreatedOn).Select(x => new SalonNotesModel
                            {
                                SalonNotes = x.Notes,
                                HairProfileId = x.HairProfileId,
                                LoginUserId = x.UserId.ToString(),
                                CreatedBy = x.WebLogin.UserEmail,
                                CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(x.CreatedOn, easternZone),
                            }).ToList();
                            profile.IsPublicSalonNote = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Method: GetHairProfileCustomer, HairProfileId:" + hairId + ", Error: " + ex.Message, ex);
                }
            }
            if (profile != null)
            {
                int? latestQA = context.Questionaires.Where(x => x.UserId == hairProfileModel.UserId && x.IsActive == true).OrderByDescending(x => x.QA).FirstOrDefault()?.QA;

                //int latestQA = context.Questionaires.Where(x => x.UserId == hairProfileModel.UserId && x.IsActive == true).Max(x => x.QA);
                string uploadedImage = context.Questionaires.Where(x => x.UserId == hairProfileModel.UserId && x.QuestionId == 22 && x.QA == latestQA).Select(x => x.DescriptiveAnswer).LastOrDefault();
                profile.UserUploadedImage = uploadedImage;
            }
            return profile;
        }

        public QuestionaireModel GetQuestionaireDetails(QuestionaireModel questionaire)
        {
            try
            {
                var entity = _userManager.FindByIdAsync(questionaire.Userid).GetAwaiter().GetResult();
                var objCode = context.Questionaires.FirstOrDefault(x => x.UserId == questionaire.Userid && x.IsActive == true);
                var maxQA = context.Questionaires.Where(x => x.UserId == questionaire.Userid && x.IsActive == true).Max(x => (int?)x.QA) ?? 0;
                var paymententity = context.PaymentEntities.Where(x => x.EmailAddress == entity.Email).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                var payment = paymententity != null &&
                                ((paymententity.ProviderName != "OneTime" && paymententity.ExpirationDate.HasValue && paymententity.ExpirationDate.Value.Date >= DateTime.Now.Date) ||
                                 (paymententity.ProviderName == "OneTime" && paymententity.ExpirationDate.HasValue && paymententity.ExpirationDate.Value.Date >= DateTime.Now.Date && paymententity.HairAIAvailDate == null))
                                ? paymententity
                                : null;    //unexpired payment
                if (objCode != null)
                {
                    questionaire.IsExist = true;
                    questionaire.HHCPCount = context.HairProfiles.Where(x => x.UserId == entity.Email && x.IsActive == true).Count();
                    questionaire.QuestionAnswerCount = context.Questionaires.Where(x => x.UserId == questionaire.Userid && x.IsActive == true && x.QA == maxQA).Select(x => x.QuestionId).Distinct().Count();
                    questionaire.PaymentId = payment?.PaymentId;
                    questionaire.ProviderName = paymententity?.ProviderName;
                    if (payment == null)  //if no unexpired payment in that case send message and provider name of the last payment done.
                    {
                        if (paymententity != null)
                        {
                            if (paymententity.ProviderName == "OneTime")
                            {
                                questionaire.ExpiredMessage = "You have used your one-time subscription. If you would like to perform another Hair Analysis, please buy a new subscription.";
                            }
                            else
                            {
                                questionaire.ExpiredMessage = "Your subscription has expired. If you would like to perform another Hair Analysis, please buy a new subscription.";
                            }
                        }
                        else
                        {
                            questionaire.ExpiredMessage = "";
                        }
                    }
                }
                else
                {
                    questionaire.IsExist = false;
                    questionaire.PaymentId = payment?.PaymentId;
                    questionaire.ProviderName = paymententity?.ProviderName;
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetQuestionaireDetails, UserId:" + questionaire.Userid + ", Error: " + Ex.Message, Ex);
            }
            return questionaire;
        }


        public async Task<QuestionaireModel> GetQuestionaireDetailsMobile(string userId)
        {
            QuestionaireModel questionaire = new QuestionaireModel();

            try
            {
                var user = await _userManager.FindByEmailAsync(userId);

                var objCode = context.Questionaires.FirstOrDefault(x => x.UserId == user.Id.ToString() && x.IsActive == true && x.QuestionId == 22);
                var paymententity = context.PaymentEntities.Where(x => x.EmailAddress == user.Email).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                var payment = paymententity != null &&
                                ((paymententity.ProviderName != "OneTime" && paymententity.ExpirationDate.HasValue && paymententity.ExpirationDate.Value.Date >= DateTime.Now.Date) ||
                                 (paymententity.ProviderName == "OneTime" && paymententity.ExpirationDate.HasValue && paymententity.ExpirationDate.Value.Date >= DateTime.Now.Date && paymententity.HairAIAvailDate == null))
                                ? paymententity
                                : null;
                if (objCode != null)
                {
                    questionaire.Userid = user.Id.ToString();
                    questionaire.IsExist = true;
                    questionaire.HHCPCount = context.HairProfiles.Where(x => x.UserId == user.Email && x.IsActive == true).Count();
                    questionaire.PaymentId = payment?.PaymentId;
                    questionaire.ProviderName = paymententity?.ProviderName;
                    questionaire.IsAlreadyShareHHCP = (context.SharedHHCP.Where(a => a.SharedBy == user.Id).Any() == true ? true : false);
                    questionaire.HasHHCPSharedWithMe = (context.SharedHHCP.Where(a => a.SharedWith == user.Id).Any() == true ? true : false);
                    if (payment == null)  //if no unexpired payment in that case send message and provider name of the last payment done.
                    {
                        if (paymententity != null)
                        {
                            if (paymententity.ProviderName == "OneTime")
                            {
                                questionaire.ExpiredMessage = "You have used your one-time subscription. If you would like to perform another Hair Analysis, please buy a new subscription.";
                            }
                            else
                            {
                                questionaire.ExpiredMessage = "Your subscription has expired. If you would like to perform another Hair Analysis, please buy a new subscription.";
                            }
                        }
                        else
                        {
                            questionaire.ExpiredMessage = "";
                        }
                    }
                }
                else
                {
                    questionaire.Userid = user.Id.ToString();
                    questionaire.IsExist = false;
                    questionaire.PaymentId = payment?.PaymentId;
                    questionaire.ProviderName = paymententity?.ProviderName;
                    questionaire.IsAlreadyShareHHCP = (context.SharedHHCP.Where(a => a.SharedBy == user.Id).Any() == true ? true : false);
                    questionaire.HasHHCPSharedWithMe = (context.SharedHHCP.Where(a => a.SharedWith == user.Id).Any() == true ? true : false);
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetQuestionaireDetailsMobile, UserId:" + userId + ", Error: " + Ex.Message, Ex);
            }
            return questionaire;
        }

        public CollaboratedDetailModelLocal CollaboratedDetailsLocal(string hairProfileId)
        {

            int profileId = Convert.ToInt32(hairProfileId);
            CollaboratedDetailModelLocal collaboratedDetailModel = new CollaboratedDetailModelLocal();
            try
            {
                List<int> productIds = context.RecommendedProducts.Where(x => x.HairProfileId == profileId).Select(x => x.ProductId).ToList();
                List<int?> types = context.ProductEntities.Where(x => productIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                List<int?> parentIds = context.ProductTypes.Where(x => types.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                var parents = context.ProductTypeCategories.Where(x => parentIds.Contains(x.Id)).ToList();
                List<RecommendedProductsModel> productsTypesList = new List<RecommendedProductsModel>();
                foreach (var parentProduct in parents)
                {
                    RecommendedProductsModel productsTypes = new RecommendedProductsModel();
                    productsTypes.ProductParentName = parentProduct.CategoryName;
                    productsTypes.ProductId = parentProduct.Id;
                    List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                    List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id))
                        .Select(x => x.ProductTypesId).Distinct().ToList();

                    foreach (var type in productByTypes)
                    {
                        if (type != null)
                        {
                            ProductsTypesModels productsTypesModel = new ProductsTypesModels();
                            var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id)).ToList();
                            if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                            {
                                productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                productsTypesModel.ProductId = type;
                            }

                            productsTypesModel.Products = products.Select(x => new ProductsModels
                            {
                                Id = x.Id,
                                BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                ImageName = x.ImageName,
                                ProductLink = x.ProductLink,
                                ProductDetails = x.ProductDetails,
                                ProductName = x.ProductName,
                                ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                            }).ToList();
                            productsTypesModels.Add(productsTypesModel);
                        }
                    }
                    productsTypes.ProductsTypes = productsTypesModels;
                    productsTypesList.Add(productsTypes);
                }

                collaboratedDetailModel.ProductDetailModel = productsTypesList;

                collaboratedDetailModel.IngredientDetailModel = (from rprod in context.RecommendedIngredients
                                                                 join ing in context.IngedientsEntities
                                                                 on rprod.IngredientId equals ing.IngedientsEntityId
                                                                 where rprod.HairProfileId == profileId
                                                                 select new IngredientDetailModel()
                                                                 {
                                                                     IngredientId = ing.IngedientsEntityId,
                                                                     Name = ing.Name,
                                                                     ImageUrl = "http://admin.myavana.com/Ingredients/" + ing.Image
                                                                 }).ToList();
                collaboratedDetailModel.RegimenDetailModel = (from rprod in context.RecommendedRegimens
                                                              join reg in context.Regimens
                                                              on rprod.RegimenId equals reg.RegimensId
                                                              where rprod.HairProfileId == profileId
                                                              select new RegimenDetailModel()
                                                              {
                                                                  RegimenId = reg.RegimensId,
                                                                  RegimenName = reg.Name
                                                              }).ToList();

            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CollaboratedDetailsLocal, HairProfileId:" + hairProfileId + ", Error: " + Ex.Message, Ex);
            }
            return collaboratedDetailModel;
        }

        public List<HairProfileCustomersModel> GetHairProfileCustomerList(int userId)
        {
            try
            {
                List<HairProfileCustomersModel> list = (from hr in context.HairProfiles
                                                        join usr in context.Users
                                                        on hr.UserId equals usr.UserName
                                                        join custType in context.CustomerTypes
                                                        on usr.CustomerTypeId equals custType.CustomerTypeId
                                                        where hr.IsActive == true && hr.IsDrafted == false
                                                        select new HairProfileCustomersModel()
                                                        {
                                                            UserId = usr.Id,
                                                            UserName = usr.FirstName + " " + usr.LastName,
                                                            UserEmail = usr.Email,
                                                            CreatedOn = hr.CreatedOn,
                                                            CustomerType = usr.CustomerType,
                                                            SalonId = usr.SalonId,
                                                            CustomerTypeDesc = custType.Description
                                                        }).ToList();

                var userType = context.WebLogins.FirstOrDefault(x => x.UserId == userId).UserTypeId;
                if (userType == (int)UserTypeEnum.B2B)
                {
                    var salonIds = context.SalonsOwners.Where(x => x.UserId == userId && x.IsActive == true).Select(x => x.SalonId).ToArray();
                    list = list.Where(u => salonIds.Any(x => x == u.SalonId)).ToList();
                }
                return list;

            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairProfileCustomerList, UserId:" + userId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<ObsCommonData> GetData(int hairId, string type)
        {
            try
            {
                List<ObsCommonData> commonData = new List<ObsCommonData>();

                if (type == "topLeft")
                {
                    //var observation = (from hb in context.HairObservations
                    //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                    //                   where hb.HairProfileId == hairId && hb.IsTopLeft == true
                    //                   select new ObsCommonData()
                    //                   {
                    //                       Id = ob.Id,
                    //                       Name = "Observation",
                    //                       Description = ob.Description
                    //                   }).FirstOrDefault();
                    //commonData.Add(observation);

                    var breakage = (from hb in context.HairObservations
                                    join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Breakage",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (breakage != null)
                    {
                        commonData.Add(breakage);
                    }

                    var damage = (from hb in context.HairObservations
                                  join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Damage",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                    if (damage != null)
                    {
                        commonData.Add(damage);
                    }

                    var chemical = (from hb in context.HairObservations
                                    join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Chemical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (chemical != null)
                    {
                        commonData.Add(chemical);
                    }

                    var physical = (from hb in context.HairObservations
                                    join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Physical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (physical != null)
                    {
                        commonData.Add(physical);
                    }

                    var splitting = (from hb in context.HairObservations
                                     join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                     where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                     select new ObsCommonData()
                                     {
                                         Id = ob.Id,
                                         Name = "Splitting",
                                         Description = ob.Description
                                     }).FirstOrDefault();
                    if (splitting != null)
                    {
                        commonData.Add(splitting);
                    }

                    var elasticity = (from hb in context.HairObservations
                                      join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                      where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                      select new ObsCommonData()
                                      {
                                          Id = ob.Id,
                                          Name = "Elasticitiy",
                                          Description = ob.Description
                                      }).FirstOrDefault();
                    if (elasticity != null)
                    {
                        commonData.Add(elasticity);
                    }

                    var porosity = (from hp in context.HairPorosities
                                    join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                    where hp.HairProfileId == hairId && hp.IsTopLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Porosity",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (porosity != null)
                    {
                        commonData.Add(porosity);
                    }

                    var density = (from he in context.HairElasticities
                                   join ob in context.Elasticities on he.ElasticityId equals ob.Id
                                   where he.HairProfileId == hairId && he.IsTopLeft == true
                                   select new ObsCommonData()
                                   {
                                       Id = ob.Id,
                                       Name = "Density",
                                       Description = ob.Description
                                   }).FirstOrDefault();
                    if (density != null)
                    {
                        commonData.Add(density);
                    }
                }
                if (type == "topRight")
                {
                    //var observation = (from hb in context.HairObservations
                    //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                    //                   where hb.HairProfileId == hairId && hb.IsTopRight == true
                    //                   select new ObsCommonData()
                    //                   {
                    //                       Id = ob.Id,
                    //                       Name = "Observation",
                    //                       Description = ob.Description
                    //                   }).FirstOrDefault();
                    //commonData.Add(observation);

                    var breakage = (from hb in context.HairObservations
                                    join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsTopRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Breakage",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (breakage != null)
                    {
                        commonData.Add(breakage);
                    }

                    var damage = (from hb in context.HairObservations
                                  join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsTopRight == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Damage",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                    if (damage != null)
                    {
                        commonData.Add(damage);
                    }

                    var chemical = (from hb in context.HairObservations
                                    join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsTopRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Chemical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (chemical != null)
                    {
                        commonData.Add(chemical);
                    }

                    var physical = (from hb in context.HairObservations
                                    join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsTopRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Physical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (physical != null)
                    {
                        commonData.Add(physical);
                    }

                    var splitting = (from hb in context.HairObservations
                                     join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                     where hb.HairProfileId == hairId && hb.IsTopRight == true
                                     select new ObsCommonData()
                                     {
                                         Id = ob.Id,
                                         Name = "Splitting",
                                         Description = ob.Description
                                     }).FirstOrDefault();
                    if (splitting != null)
                    {
                        commonData.Add(splitting);
                    }

                    var elasticity = (from hb in context.HairObservations
                                      join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                      where hb.HairProfileId == hairId && hb.IsTopRight == true
                                      select new ObsCommonData()
                                      {
                                          Id = ob.Id,
                                          Name = "Elasticitiy",
                                          Description = ob.Description
                                      }).FirstOrDefault();
                    if (elasticity != null)
                    {
                        commonData.Add(elasticity);
                    }

                    var porosity = (from hp in context.HairPorosities
                                    join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                    where hp.HairProfileId == hairId && hp.IsTopRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Porosity",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (porosity != null)
                    {
                        commonData.Add(porosity);
                    }

                    var density = (from he in context.HairElasticities
                                   join ob in context.Elasticities on he.ElasticityId equals ob.Id
                                   where he.HairProfileId == hairId && he.IsTopRight == true
                                   select new ObsCommonData()
                                   {
                                       Id = ob.Id,
                                       Name = "Density",
                                       Description = ob.Description
                                   }).FirstOrDefault();
                    if (density != null)
                    {
                        commonData.Add(density);
                    }
                }
                if (type == "bottomLeft")
                {
                    //var observation = (from hb in context.HairObservations
                    //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                    //                   where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                    //                   select new ObsCommonData()
                    //                   {
                    //                       Id = ob.Id,
                    //                       Name = "Observation",
                    //                       Description = ob.Description
                    //                   }).FirstOrDefault();
                    //commonData.Add(observation);

                    var breakage = (from hb in context.HairObservations
                                    join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Breakage",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (breakage != null)
                    {
                        commonData.Add(breakage);
                    }

                    var damage = (from hb in context.HairObservations
                                  join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Damage",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                    if (damage != null)
                    {
                        commonData.Add(damage);
                    }

                    var chemical = (from hb in context.HairObservations
                                    join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Chemical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (chemical != null)
                    {
                        commonData.Add(chemical);
                    }

                    var physical = (from hb in context.HairObservations
                                    join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Physical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (physical != null)
                    {
                        commonData.Add(physical);
                    }

                    var splitting = (from hb in context.HairObservations
                                     join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                     where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                     select new ObsCommonData()
                                     {
                                         Id = ob.Id,
                                         Name = "Splitting",
                                         Description = ob.Description
                                     }).FirstOrDefault();
                    if (splitting != null)
                    {
                        commonData.Add(splitting);
                    }

                    var elasticity = (from hb in context.HairObservations
                                      join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                      where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                      select new ObsCommonData()
                                      {
                                          Id = ob.Id,
                                          Name = "Elasticitiy",
                                          Description = ob.Description
                                      }).FirstOrDefault();
                    if (elasticity != null)
                    {
                        commonData.Add(elasticity);
                    }

                    var porosity = (from hp in context.HairPorosities
                                    join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                    where hp.HairProfileId == hairId && hp.IsBottomLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Porosity",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (porosity != null)
                    {
                        commonData.Add(porosity);
                    }

                    var density = (from he in context.HairElasticities
                                   join ob in context.Elasticities on he.ElasticityId equals ob.Id
                                   where he.HairProfileId == hairId && he.IsBottomLeft == true
                                   select new ObsCommonData()
                                   {
                                       Id = ob.Id,
                                       Name = "Density",
                                       Description = ob.Description
                                   }).FirstOrDefault();
                    if (density != null)
                    {
                        commonData.Add(density);
                    }
                }
                if (type == "bottomRight")
                {
                    //var observation = (from hb in context.HairObservations
                    //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                    //                   where hb.HairProfileId == hairId && hb.IsBottomRight == true
                    //                   select new ObsCommonData()
                    //                   {
                    //                       Id = ob.Id,
                    //                       Name = "Observation",
                    //                       Description = ob.Description
                    //                   }).FirstOrDefault();
                    //commonData.Add(observation);

                    var breakage = (from hb in context.HairObservations
                                    join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Breakage",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (breakage != null)
                    {
                        commonData.Add(breakage);
                    }

                    var damage = (from hb in context.HairObservations
                                  join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Damage",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                    if (damage != null)
                    {
                        commonData.Add(damage);
                    }

                    var chemical = (from hb in context.HairObservations
                                    join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Chemical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (chemical != null)
                    {
                        commonData.Add(chemical);
                    }

                    var physical = (from hb in context.HairObservations
                                    join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Physical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (physical != null)
                    {
                        commonData.Add(physical);
                    }

                    var splitting = (from hb in context.HairObservations
                                     join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                     where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                     select new ObsCommonData()
                                     {
                                         Id = ob.Id,
                                         Name = "Splitting",
                                         Description = ob.Description
                                     }).FirstOrDefault();
                    if (splitting != null)
                    {
                        commonData.Add(splitting);
                    }

                    var elasticity = (from hb in context.HairObservations
                                      join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                      where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                      select new ObsCommonData()
                                      {
                                          Id = ob.Id,
                                          Name = "Elasticitiy",
                                          Description = ob.Description
                                      }).FirstOrDefault();
                    if (elasticity != null)
                    {
                        commonData.Add(elasticity);
                    }

                    var porosity = (from hp in context.HairPorosities
                                    join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                    where hp.HairProfileId == hairId && hp.IsBottomRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Porosity",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (porosity != null)
                    {
                        commonData.Add(porosity);
                    }

                    var density = (from he in context.HairElasticities
                                   join ob in context.Elasticities on he.ElasticityId equals ob.Id
                                   where he.HairProfileId == hairId && he.IsBottomRight == true
                                   select new ObsCommonData()
                                   {
                                       Id = ob.Id,
                                       Name = "Density",
                                       Description = ob.Description
                                   }).FirstOrDefault();
                    if (density != null)
                    {
                        commonData.Add(density);
                    }
                }
                if (type == "crown")
                {
                    //var observation = (from hb in context.HairObservations
                    //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                    //                   where hb.HairProfileId == hairId && hb.IsCrown == true
                    //                   select new ObsCommonData()
                    //                   {
                    //                       Id = ob.Id,
                    //                       Name = "Observation",
                    //                       Description = ob.Description
                    //                   }).FirstOrDefault();
                    //commonData.Add(observation);

                    var breakage = (from hb in context.HairObservations
                                    join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsCrown == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Breakage",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (breakage != null)
                    {
                        commonData.Add(breakage);
                    }

                    var damage = (from hb in context.HairObservations
                                  join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsCrown == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Damage",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                    if (damage != null)
                    {
                        commonData.Add(damage);
                    }

                    var chemical = (from hb in context.HairObservations
                                    join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsCrown == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Chemical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (chemical != null)
                    {
                        commonData.Add(chemical);
                    }

                    var physical = (from hb in context.HairObservations
                                    join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsCrown == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Physical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (physical != null)
                    {
                        commonData.Add(physical);
                    }

                    var splitting = (from hb in context.HairObservations
                                     join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                     where hb.HairProfileId == hairId && hb.IsCrown == true
                                     select new ObsCommonData()
                                     {
                                         Id = ob.Id,
                                         Name = "Splitting",
                                         Description = ob.Description
                                     }).FirstOrDefault();
                    if (splitting != null)
                    {
                        commonData.Add(splitting);
                    }

                    var elasticity = (from hb in context.HairObservations
                                      join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                      where hb.HairProfileId == hairId && hb.IsCrown == true
                                      select new ObsCommonData()
                                      {
                                          Id = ob.Id,
                                          Name = "Elasticitiy",
                                          Description = ob.Description
                                      }).FirstOrDefault();
                    if (elasticity != null)
                    {
                        commonData.Add(elasticity);
                    }

                    var porosity = (from hp in context.HairPorosities
                                    join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                    where hp.HairProfileId == hairId && hp.IsCrown == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Porosity",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (porosity != null)
                    {
                        commonData.Add(porosity);
                    }

                    var density = (from he in context.HairElasticities
                                   join ob in context.Elasticities on he.ElasticityId equals ob.Id
                                   where he.HairProfileId == hairId && he.IsCrown == true
                                   select new ObsCommonData()
                                   {
                                       Id = ob.Id,
                                       Name = "Density",
                                       Description = ob.Description
                                   }).FirstOrDefault();
                    if (density != null)
                    {
                        commonData.Add(density);
                    }
                }
                return commonData;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetData, HairProfileId:" + hairId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }


        public List<ObsCommonData> GetData2(int hairId, string type)
        {
            try
            {
                List<ObsCommonData> commonData = new List<ObsCommonData>();

                if (type == "topLeft")
                {
                    //var observation = (from hb in context.HairObservations
                    //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                    //                   where hb.HairProfileId == hairId && hb.IsTopLeft == true
                    //                   select new ObsCommonData()
                    //                   {
                    //                       Id = ob.Id,
                    //                       Name = "Observation",
                    //                       Description = ob.Description
                    //                   }).FirstOrDefault();
                    //commonData.Add(observation);

                    var breakage = (from hb in context.HairObservations
                                    join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Breakage",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (breakage != null)
                    {
                        commonData.Add(breakage);
                    }

                    //var damage = (from hb in context.HairObservations
                    //              join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                    //              where hb.HairProfileId == hairId && hb.IsTopLeft == true
                    //              select new ObsCommonData()
                    //              {
                    //                  Id = ob.Id,
                    //                  Name = "Damage",
                    //                  Description = ob.Description
                    //              }).FirstOrDefault();
                    //if (damage != null)
                    //{
                    //    commonData.Add(damage);
                    //}

                    var chemical = (from hb in context.HairObservations
                                    join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Chemical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (chemical != null)
                    {
                        commonData.Add(chemical);
                    }

                    var physical = (from hb in context.HairObservations
                                    join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Physical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (physical != null)
                    {
                        commonData.Add(physical);
                    }

                    var splitting = (from hb in context.HairObservations
                                     join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                     where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                     select new ObsCommonData()
                                     {
                                         Id = ob.Id,
                                         Name = "Splitting",
                                         Description = ob.Description
                                     }).FirstOrDefault();
                    if (splitting != null)
                    {
                        commonData.Add(splitting);
                    }

                    var elasticity = (from hb in context.HairObservations
                                      join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                      where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                      select new ObsCommonData()
                                      {
                                          Id = ob.Id,
                                          Name = "Elasticitiy",
                                          Description = ob.Description
                                      }).FirstOrDefault();
                    if (elasticity != null)
                    {
                        commonData.Add(elasticity);
                    }

                    var porosity = (from hp in context.HairPorosities
                                    join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                    where hp.HairProfileId == hairId && hp.IsTopLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Porosity",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (porosity != null)
                    {
                        commonData.Add(porosity);
                    }

                    var density = (from he in context.HairElasticities
                                   join ob in context.Elasticities on he.ElasticityId equals ob.Id
                                   where he.HairProfileId == hairId && he.IsTopLeft == true
                                   select new ObsCommonData()
                                   {
                                       Id = ob.Id,
                                       Name = "Density",
                                       Description = ob.Description
                                   }).FirstOrDefault();
                    if (density != null)
                    {
                        commonData.Add(density);
                    }
                }
                if (type == "topRight")
                {
                    //var observation = (from hb in context.HairObservations
                    //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                    //                   where hb.HairProfileId == hairId && hb.IsTopRight == true
                    //                   select new ObsCommonData()
                    //                   {
                    //                       Id = ob.Id,
                    //                       Name = "Observation",
                    //                       Description = ob.Description
                    //                   }).FirstOrDefault();
                    //commonData.Add(observation);

                    var breakage = (from hb in context.HairObservations
                                    join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsTopRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Breakage",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (breakage != null)
                    {
                        commonData.Add(breakage);
                    }

                    var damage = (from hb in context.HairObservations
                                  join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsTopRight == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Damage",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                    if (damage != null)
                    {
                        commonData.Add(damage);
                    }

                    var chemical = (from hb in context.HairObservations
                                    join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsTopRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Chemical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (chemical != null)
                    {
                        commonData.Add(chemical);
                    }

                    var physical = (from hb in context.HairObservations
                                    join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsTopRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Physical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (physical != null)
                    {
                        commonData.Add(physical);
                    }

                    var splitting = (from hb in context.HairObservations
                                     join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                     where hb.HairProfileId == hairId && hb.IsTopRight == true
                                     select new ObsCommonData()
                                     {
                                         Id = ob.Id,
                                         Name = "Splitting",
                                         Description = ob.Description
                                     }).FirstOrDefault();
                    if (splitting != null)
                    {
                        commonData.Add(splitting);
                    }

                    var elasticity = (from hb in context.HairObservations
                                      join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                      where hb.HairProfileId == hairId && hb.IsTopRight == true
                                      select new ObsCommonData()
                                      {
                                          Id = ob.Id,
                                          Name = "Elasticitiy",
                                          Description = ob.Description
                                      }).FirstOrDefault();
                    if (elasticity != null)
                    {
                        commonData.Add(elasticity);
                    }

                    var porosity = (from hp in context.HairPorosities
                                    join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                    where hp.HairProfileId == hairId && hp.IsTopRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Porosity",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (porosity != null)
                    {
                        commonData.Add(porosity);
                    }

                    var density = (from he in context.HairElasticities
                                   join ob in context.Elasticities on he.ElasticityId equals ob.Id
                                   where he.HairProfileId == hairId && he.IsTopRight == true
                                   select new ObsCommonData()
                                   {
                                       Id = ob.Id,
                                       Name = "Density",
                                       Description = ob.Description
                                   }).FirstOrDefault();
                    if (density != null)
                    {
                        commonData.Add(density);
                    }
                }
                if (type == "bottomLeft")
                {
                    //var observation = (from hb in context.HairObservations
                    //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                    //                   where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                    //                   select new ObsCommonData()
                    //                   {
                    //                       Id = ob.Id,
                    //                       Name = "Observation",
                    //                       Description = ob.Description
                    //                   }).FirstOrDefault();
                    //commonData.Add(observation);

                    var breakage = (from hb in context.HairObservations
                                    join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Breakage",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (breakage != null)
                    {
                        commonData.Add(breakage);
                    }

                    var damage = (from hb in context.HairObservations
                                  join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Damage",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                    if (damage != null)
                    {
                        commonData.Add(damage);
                    }

                    var chemical = (from hb in context.HairObservations
                                    join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Chemical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (chemical != null)
                    {
                        commonData.Add(chemical);
                    }

                    var physical = (from hb in context.HairObservations
                                    join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Physical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (physical != null)
                    {
                        commonData.Add(physical);
                    }

                    var splitting = (from hb in context.HairObservations
                                     join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                     where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                     select new ObsCommonData()
                                     {
                                         Id = ob.Id,
                                         Name = "Splitting",
                                         Description = ob.Description
                                     }).FirstOrDefault();
                    if (splitting != null)
                    {
                        commonData.Add(splitting);
                    }

                    var elasticity = (from hb in context.HairObservations
                                      join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                      where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                      select new ObsCommonData()
                                      {
                                          Id = ob.Id,
                                          Name = "Elasticitiy",
                                          Description = ob.Description
                                      }).FirstOrDefault();
                    if (elasticity != null)
                    {
                        commonData.Add(elasticity);
                    }

                    var porosity = (from hp in context.HairPorosities
                                    join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                    where hp.HairProfileId == hairId && hp.IsBottomLeft == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Porosity",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (porosity != null)
                    {
                        commonData.Add(porosity);
                    }

                    var density = (from he in context.HairElasticities
                                   join ob in context.Elasticities on he.ElasticityId equals ob.Id
                                   where he.HairProfileId == hairId && he.IsBottomLeft == true
                                   select new ObsCommonData()
                                   {
                                       Id = ob.Id,
                                       Name = "Density",
                                       Description = ob.Description
                                   }).FirstOrDefault();
                    if (density != null)
                    {
                        commonData.Add(density);
                    }
                }
                if (type == "bottomRight")
                {
                    //var observation = (from hb in context.HairObservations
                    //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                    //                   where hb.HairProfileId == hairId && hb.IsBottomRight == true
                    //                   select new ObsCommonData()
                    //                   {
                    //                       Id = ob.Id,
                    //                       Name = "Observation",
                    //                       Description = ob.Description
                    //                   }).FirstOrDefault();
                    //commonData.Add(observation);

                    var breakage = (from hb in context.HairObservations
                                    join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Breakage",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (breakage != null)
                    {
                        commonData.Add(breakage);
                    }

                    var damage = (from hb in context.HairObservations
                                  join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Damage",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                    if (damage != null)
                    {
                        commonData.Add(damage);
                    }

                    var chemical = (from hb in context.HairObservations
                                    join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Chemical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (chemical != null)
                    {
                        commonData.Add(chemical);
                    }

                    var physical = (from hb in context.HairObservations
                                    join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Physical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (physical != null)
                    {
                        commonData.Add(physical);
                    }

                    var splitting = (from hb in context.HairObservations
                                     join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                     where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                     select new ObsCommonData()
                                     {
                                         Id = ob.Id,
                                         Name = "Splitting",
                                         Description = ob.Description
                                     }).FirstOrDefault();
                    if (splitting != null)
                    {
                        commonData.Add(splitting);
                    }

                    var elasticity = (from hb in context.HairObservations
                                      join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                      where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                      select new ObsCommonData()
                                      {
                                          Id = ob.Id,
                                          Name = "Elasticitiy",
                                          Description = ob.Description
                                      }).FirstOrDefault();
                    if (elasticity != null)
                    {
                        commonData.Add(elasticity);
                    }

                    var porosity = (from hp in context.HairPorosities
                                    join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                    where hp.HairProfileId == hairId && hp.IsBottomRight == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Porosity",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (porosity != null)
                    {
                        commonData.Add(porosity);
                    }

                    var density = (from he in context.HairElasticities
                                   join ob in context.Elasticities on he.ElasticityId equals ob.Id
                                   where he.HairProfileId == hairId && he.IsBottomRight == true
                                   select new ObsCommonData()
                                   {
                                       Id = ob.Id,
                                       Name = "Density",
                                       Description = ob.Description
                                   }).FirstOrDefault();
                    if (density != null)
                    {
                        commonData.Add(density);
                    }
                }
                if (type == "crown")
                {
                    //var observation = (from hb in context.HairObservations
                    //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                    //                   where hb.HairProfileId == hairId && hb.IsCrown == true
                    //                   select new ObsCommonData()
                    //                   {
                    //                       Id = ob.Id,
                    //                       Name = "Observation",
                    //                       Description = ob.Description
                    //                   }).FirstOrDefault();
                    //commonData.Add(observation);

                    var breakage = (from hb in context.HairObservations
                                    join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsCrown == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Breakage",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (breakage != null)
                    {
                        commonData.Add(breakage);
                    }

                    var damage = (from hb in context.HairObservations
                                  join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsCrown == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Damage",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                    if (damage != null)
                    {
                        commonData.Add(damage);
                    }

                    var chemical = (from hb in context.HairObservations
                                    join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsCrown == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Chemical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (chemical != null)
                    {
                        commonData.Add(chemical);
                    }

                    var physical = (from hb in context.HairObservations
                                    join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                    where hb.HairProfileId == hairId && hb.IsCrown == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Physical",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (physical != null)
                    {
                        commonData.Add(physical);
                    }

                    var splitting = (from hb in context.HairObservations
                                     join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                     where hb.HairProfileId == hairId && hb.IsCrown == true
                                     select new ObsCommonData()
                                     {
                                         Id = ob.Id,
                                         Name = "Splitting",
                                         Description = ob.Description
                                     }).FirstOrDefault();
                    if (splitting != null)
                    {
                        commonData.Add(splitting);
                    }

                    var elasticity = (from hb in context.HairObservations
                                      join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                      where hb.HairProfileId == hairId && hb.IsCrown == true
                                      select new ObsCommonData()
                                      {
                                          Id = ob.Id,
                                          Name = "Elasticitiy",
                                          Description = ob.Description
                                      }).FirstOrDefault();
                    if (elasticity != null)
                    {
                        commonData.Add(elasticity);
                    }

                    var porosity = (from hp in context.HairPorosities
                                    join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                    where hp.HairProfileId == hairId && hp.IsCrown == true
                                    select new ObsCommonData()
                                    {
                                        Id = ob.Id,
                                        Name = "Porosity",
                                        Description = ob.Description
                                    }).FirstOrDefault();
                    if (porosity != null)
                    {
                        commonData.Add(porosity);
                    }

                    var density = (from he in context.HairElasticities
                                   join ob in context.Elasticities on he.ElasticityId equals ob.Id
                                   where he.HairProfileId == hairId && he.IsCrown == true
                                   select new ObsCommonData()
                                   {
                                       Id = ob.Id,
                                       Name = "Density",
                                       Description = ob.Description
                                   }).FirstOrDefault();
                    if (density != null)
                    {
                        commonData.Add(density);
                    }
                }
                return commonData;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetData2, HairProfileId:" + hairId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public DigitalAssessmentModel CreateHHCPByDigitalAssessment(DigitalAssessmentModel digitalAssessmentModel)
        {
            try
            {
                DigitalAssessmentModel ret = new DigitalAssessmentModel();
                var entity = _userManager.FindByIdAsync(digitalAssessmentModel.Userid).GetAwaiter().GetResult();
                if (entity != null)
                {
                    UserEntity us = context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();
                    var latestQA = context.Questionaires.Where(x => x.UserId == digitalAssessmentModel.Userid && x.IsActive == true).Max(x => x.QA);
                    //var hair = context.HairProfiles.Where(x => x.UserId == us.Email && x.IsActive == true).FirstOrDefault();
                    //if (hair != null)
                    //{
                    //    hair.CreatedOn = DateTime.Now;
                    //    hair.HealthSummary = "Hi " + us.FirstName + " ! " + "\r\nYour Digital Hair Profile includes your introductory product recommendations based on a quick analysis of your hair. We incorporated your Hair Goals, Hair Challenges, Product Recommendations, and % breakdown of your Unique Hair Type Combination. To get a comprehensive healthy hair care plan, make sure to get your hair analysis kit in the menu to your left. This should get you started in the meantime! If you have any questions, please email us at support@myavana.com\r\nLove, \r\nMYAVANA ";
                    //    context.SaveChanges();
                    //    if (!string.IsNullOrEmpty(digitalAssessmentModel.HairType))
                    //    {
                    //        var recPRods = UpdateRecommendedProducts(digitalAssessmentModel.HairType, hair.Id);
                    //    }
                    //}
                    //else
                    //{
                    Models.Entities.HairProfile hairProfile = new Models.Entities.HairProfile();
                    hairProfile.UserId = us.Email;
                    hairProfile.HairType = digitalAssessmentModel.HairType;
                    hairProfile.AttachedQA = latestQA;
                    hairProfile.HealthSummary = "Hi " + us.FirstName + " ! " + "\r\nYour Digital Hair Profile includes your introductory product recommendations based on a quick analysis of your hair. We incorporated your Hair Goals, Hair Challenges, Product Recommendations, and % breakdown of your Unique Hair Type Combination. To get a comprehensive healthy hair care plan, make sure to get your hair analysis kit in the menu to your left. This should get you started in the meantime! If you have any questions, please email us at support@myavana.com\r\nLove, \r\nMYAVANA ";
                    int res = CreateHairProfile(hairProfile);
                    if (res > 0)
                    {
                        CustomerAIResult objCustomerAIResult = new CustomerAIResult();
                        objCustomerAIResult.UserId = entity.Id;
                        objCustomerAIResult.AIResult = digitalAssessmentModel.AIResult;
                        objCustomerAIResult.IsActive = true;
                        objCustomerAIResult.CreatedOn = DateTime.Now;
                        objCustomerAIResult.HairProfileId = res;
                        objCustomerAIResult.IsVersion2 = true;
                        context.Add(objCustomerAIResult);

                        us.HairType = digitalAssessmentModel.HairType;

                        //if (us.SubscriptionType == (int)SubscriptionTypeEnum.OneTime)
                        //{
                        /*set IsHairAIAllowed to false, when hair profile is created after shopify one time payment so as to allow 
                    customer to use hair ai only once */
                        //us.IsHairAIAllowed = false;

                        var Payment = context.PaymentEntities.Where(x => x.PaymentId == digitalAssessmentModel.PaymentId && x.HairAIAvailDate == null && x.ProviderName == "OneTime").FirstOrDefault();
                        if (Payment != null)
                        {
                            Payment.IsHairAIAvailed = true;
                            Payment.HairAIAvailDate = DateTime.Now;
                            //Payment.ExpirationDate = DateTime.Now;
                            context.SaveChanges();
                        }

                        //}

                        //us.AIResult = digitalAssessmentModel.AIResult;
                        context.SaveChanges();


                        // var recPRods = UpdateRecommendedProducts(digitalAssessmentModel.HairType, res, digitalAssessmentModel.Userid);
                        var recPRods = UpdateRecommendedProductsSP(digitalAssessmentModel.HairType, res, digitalAssessmentModel.Userid,null);
                        AdditionalHairInfo additionalHairInfo = new AdditionalHairInfo();
                        additionalHairInfo.HairId = res;
                        context.Add(additionalHairInfo);
                        context.SaveChanges();
                        var selectedGoals = context.Questionaires.Where(x => x.UserId == entity.Id.ToString() && x.QuestionId == 25).Select(x => x.AnswerId).ToList();
                        List<CustomerHairGoals> customerHairGoals = new List<CustomerHairGoals>();
                        foreach (var hairGoal in selectedGoals)
                        {

                            string description = context.Answers.Where(x => x.AnswerId == hairGoal).Select(x => x.Description).FirstOrDefault();
                            CustomerHairGoals customerHairGoal = new CustomerHairGoals();
                            customerHairGoal.HairInfoId = additionalHairInfo.Id;
                            customerHairGoal.Description = description;
                            customerHairGoal.CreatedOn = DateTime.Now;
                            customerHairGoal.IsActive = true;
                            customerHairGoals.Add(customerHairGoal);

                        }
                        context.AddRange(customerHairGoals);

                        var selectedChallenges = context.Questionaires.Where(x => x.UserId == entity.Id.ToString() && x.QuestionId == 16).Select(x => x.AnswerId).ToList();
                        List<CustomerHairChallenge> customerHairChallenges = new List<CustomerHairChallenge>();
                        foreach (var challenge in selectedChallenges)
                        {
                            string description = context.Answers.Where(x => x.AnswerId == challenge).Select(x => x.Description).FirstOrDefault();
                            CustomerHairChallenge customerHairChallenge = new CustomerHairChallenge();
                            customerHairChallenge.HairInfoId = additionalHairInfo.Id;
                            customerHairChallenge.Description = description;
                            customerHairChallenge.CreatedOn = DateTime.Now;
                            customerHairChallenge.IsActive = true;
                            customerHairChallenges.Add(customerHairChallenge);

                        }
                        context.AddRange(customerHairChallenges);
                        context.SaveChanges();
                    }
                    //}


                }

                return ret;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateHHCPByDigitalAssessment, UserId:" + digitalAssessmentModel.Userid + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public DigitalAssessmentModel CreateHHCPByDigitalAssessmentForMobile(DigitalAssessmentModel digitalAssessmentModel)
        {
            try
            {
                var latestQA = context.Questionaires.Where(x => x.UserId == digitalAssessmentModel.Userid && x.IsActive == true).Max(x => x.QA);
                DigitalAssessmentModel ret = new DigitalAssessmentModel();
                var entity = _userManager.FindByIdAsync(digitalAssessmentModel.Userid).GetAwaiter().GetResult();
                if (entity != null)
                {
                    UserEntity us = context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();




                    Models.Entities.HairProfile hairProfile = new Models.Entities.HairProfile();
                    hairProfile.UserId = us.Email;
                    hairProfile.HairType = digitalAssessmentModel.HairType;
                    hairProfile.AttachedQA = latestQA;
                    hairProfile.HealthSummary = "Hi " + us.FirstName + " ! " + "\r\nYour Digital Hair Profile includes your introductory product recommendations based on a quick analysis of your hair. We incorporated your Hair Goals, Hair Challenges, Product Recommendations, and % breakdown of your Unique Hair Type Combination. To get a comprehensive healthy hair care plan, make sure to get your hair analysis kit in the menu to your left. This should get you started in the meantime! If you have any questions, please email us at support@myavana.com\r\nLove, \r\nMYAVANA ";
                    int res = CreateHairProfile(hairProfile);
                    if (res > 0)
                    {
                        CustomerAIResult objCustomerAIResult = new CustomerAIResult();
                        objCustomerAIResult.UserId = entity.Id;
                        objCustomerAIResult.AIResult = JsonConvert.SerializeObject(digitalAssessmentModel.AIResult);
                        objCustomerAIResult.IsActive = true;
                        objCustomerAIResult.CreatedOn = DateTime.Now;
                        objCustomerAIResult.HairProfileId = res;
                        objCustomerAIResult.IsVersion2 = true;
                        context.Add(objCustomerAIResult);

                        us.HairType = digitalAssessmentModel.HairType;



                        var Payment = context.PaymentEntities.Where(x => x.PaymentId == digitalAssessmentModel.PaymentId && x.HairAIAvailDate == null && x.ProviderName == "OneTime").FirstOrDefault();
                        if (Payment != null)
                        {
                            Payment.IsHairAIAvailed = true;
                            Payment.HairAIAvailDate = DateTime.Now;
                            //Payment.ExpirationDate = DateTime.Now;
                            context.SaveChanges();
                        }


                        context.SaveChanges();


                        //var recPRods = UpdateRecommendedProducts(digitalAssessmentModel.HairType, res, digitalAssessmentModel.Userid);
                        var recPRods = UpdateRecommendedProductsSP(digitalAssessmentModel.HairType, res, digitalAssessmentModel.Userid, null);
                        AdditionalHairInfo additionalHairInfo = new AdditionalHairInfo();
                        additionalHairInfo.HairId = res;
                        context.Add(additionalHairInfo);
                        context.SaveChanges();
                        var selectedGoals = context.Questionaires.Where(x => x.UserId == entity.Id.ToString() && x.QuestionId == 25).Select(x => x.AnswerId).ToList();
                        List<CustomerHairGoals> customerHairGoals = new List<CustomerHairGoals>();
                        foreach (var hairGoal in selectedGoals)
                        {

                            string description = context.Answers.Where(x => x.AnswerId == hairGoal).Select(x => x.Description).FirstOrDefault();
                            CustomerHairGoals customerHairGoal = new CustomerHairGoals();
                            customerHairGoal.HairInfoId = additionalHairInfo.Id;
                            customerHairGoal.Description = description;
                            customerHairGoal.CreatedOn = DateTime.Now;
                            customerHairGoal.IsActive = true;
                            customerHairGoals.Add(customerHairGoal);

                        }
                        context.AddRange(customerHairGoals);

                        var selectedChallenges = context.Questionaires.Where(x => x.UserId == entity.Id.ToString() && x.QuestionId == 16).Select(x => x.AnswerId).ToList();
                        List<CustomerHairChallenge> customerHairChallenges = new List<CustomerHairChallenge>();
                        foreach (var challenge in selectedChallenges)
                        {
                            string description = context.Answers.Where(x => x.AnswerId == challenge).Select(x => x.Description).FirstOrDefault();
                            CustomerHairChallenge customerHairChallenge = new CustomerHairChallenge();
                            customerHairChallenge.HairInfoId = additionalHairInfo.Id;
                            customerHairChallenge.Description = description;
                            customerHairChallenge.CreatedOn = DateTime.Now;
                            customerHairChallenge.IsActive = true;
                            customerHairChallenges.Add(customerHairChallenge);

                        }
                        context.AddRange(customerHairChallenges);
                        context.SaveChanges();
                    }



                }

                return ret;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateHHCPByDigitalAssessmentForMobile, UserId:" + digitalAssessmentModel.Userid + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public DigitalAssessmentModel SaveCustomerAIResultForMobile(DigitalAssessmentModel digitalAssessmentModel)
        {
            try
            {
                DigitalAssessmentModel ret = new DigitalAssessmentModel();
                var entity = _userManager.FindByIdAsync(digitalAssessmentModel.Userid).GetAwaiter().GetResult();
                if (entity != null)
                {
                    UserEntity us = context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();

                    CustomerAIResult objCustomerAIResult = new CustomerAIResult();
                    objCustomerAIResult.UserId = entity.Id;
                    objCustomerAIResult.AIResult = JsonConvert.SerializeObject(digitalAssessmentModel.AIResult);
                    objCustomerAIResult.IsActive = true;
                    objCustomerAIResult.CreatedOn = DateTime.Now;
                    //objCustomerAIResult.HairProfileId = res;
                    objCustomerAIResult.IsVersion2 = true;
                    context.Add(objCustomerAIResult);
                    context.SaveChanges();

                    us.HairType = digitalAssessmentModel.HairType;
                    context.SaveChanges();

                }
                return ret;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SaveCustomerAIResultForMobile, UserId:" + digitalAssessmentModel.Userid + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }
        public int CreateHairProfile(Models.Entities.HairProfile hairProfile)
        {
            try
            {
                var hair = new Models.Entities.HairProfile();
                hair.UserId = hairProfile.UserId;
                hair.HairId = hairProfile.HairId;
                hair.HealthSummary = hairProfile.HealthSummary;
                hair.ConsultantNotes = hairProfile.ConsultantNotes;
                hair.RecommendationNotes = hairProfile.RecommendationNotes;
                hair.IsActive = true;
                hair.CreatedOn = DateTime.Now;
                hair.IsDrafted = false;
                hair.IsBasicHHCP = true;
                hair.HairType = hairProfile.HairType;
                hair.AttachedQA = hairProfile.AttachedQA;
                hair.IsViewEnabled = true;
                context.Add(hair);
                context.SaveChanges();
                return hair.Id;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateHairProfile, UserId:" + hairProfile.UserId + ", Error: " + Ex.Message, Ex);
                return 0;
            }

        }
        public bool UpdateRecommendedProducts(string hairType, int hairProfileId, string userId)
        {
            try
            {
                string[] hairGoalArray = null;
                string[] hairChallengeArray = null;
                int latestQA = context.Questionaires.Where(x => x.UserId == userId && x.IsActive == true).Max(x => x.QA);
                var QuestionList = context.Questionaires.Include(x => x.Answer).Where(x => x.UserId == userId && x.IsActive == true && x.QA == latestQA).ToList();

                string[] hairChallengeIds = QuestionList.Where(x => x.QuestionId == 16).ToList().Select(x => x.Answer.Description.ToLower()).ToArray();
                string[] hairGoals = QuestionList.Where(x => x.QuestionId == 25).ToList().Select(x => x.Answer.Description.ToLower()).ToArray();

                if (hairChallengeIds != null)
                {
                    var ids = context.HairChallenges.Where(x => x.IsActive == true && hairChallengeIds.Contains(x.Description.ToLower())).ToList().Select(x => x.HairChallengeId).ToArray();
                    var hairChallenge = string.Join(",", ids);
                    hairChallengeArray = hairChallenge.Split(',');
                }
                if (hairGoals != null)
                {
                    hairGoalArray = context.MappingHairGoalAndProductTags.Where(x => hairGoals.Contains(x.GoalDescription.ToLower())).ToList().Select(x => x.ProductTagsId.ToString()).ToArray();
                }

                String type1 = "1";

                //ProductResponse productResponse = new ProductResponse();
                List<ProductEntity> result = new List<ProductEntity>();
                //List<ProductEntity> resultToReturn = new List<ProductEntity>();
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
                    hairTypeArray = hairTypes.Split(',').Select(s => s.Trim()).ToArray();
                }

                var brandsList = context.Brands.Where(x => x.HideInSearch == true && x.IsActive == true).Select(x => x.BrandName).ToList();

                //result = (from products in context.ProductEntities
                //          join common in context.ProductCommons
                //          on products.Id equals common.ProductEntityId
                //          join pType in context.ProductTypes
                //          on products.ProductTypesId equals pType.Id
                //          join types in context.HairTypes
                //          on common.HairTypeId equals types.HairTypeId
                //          where hairTypeArray.Contains(types.Description) && products.IsActive == true
                //          select products).OrderByDescending(x => x.CreatedOn).Distinct().ToList();
                result = (from products in context.ProductEntities
                          join common in context.ProductCommons.Where(x => x.HairTypeId != null && x.IsActive == true)
                          on products.Id equals common.ProductEntityId
                          join common2 in context.ProductCommons.Where(x => x.HairChallengeId != null && x.IsActive == true)
                          on common.ProductEntityId equals common2.ProductEntityId
                          join common3 in context.ProductCommons.Where(x => x.ProductTagsId != null && x.IsActive == true)
                          on common.ProductEntityId equals common3.ProductEntityId
                          join pType in context.ProductTypes
                          on products.ProductTypesId equals pType.Id
                          join types in context.HairTypes
                          on common.HairTypeId equals types.HairTypeId
                          where hairTypeArray.Contains(types.Description)
                          && hairChallengeArray.Contains(common2.HairChallengeId.ToString())
                          && hairGoalArray.Contains(common3.ProductTagsId.ToString())
                          && products.IsActive == true && products.HideInSearch != true
                          && !brandsList.Contains(products.BrandName)
                          select products).OrderByDescending(x => x.CreatedOn).Distinct().Take(20).ToList();
                //result = result.Take(20).ToList();
                // resultToReturn = result;

                //productResponse.Data = resultToReturn;

                List<RecommendedProducts> recommendedProducts = new List<RecommendedProducts>();
                foreach (var item in result)
                {
                    var obj = new RecommendedProducts();
                    obj.Name = item.ProductName;
                    obj.ProductId = item.Id;
                    obj.IsActive = true;
                    obj.CreatedOn = DateTime.Now;
                    obj.HairProfileId = hairProfileId;
                    recommendedProducts.Add(obj);
                }
                AddRecommendedProducts(recommendedProducts, hairProfileId);
                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: UpdateRecommendedProducts, UserId:" + userId + ", HairProfileId:" + hairProfileId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public bool UpdateRecommendedProductsSP(string hairType, int hairProfileId, string userId, int? QA)
        {
            try
            {
                var dp_params = new DynamicParameters();
                dp_params.Add("UserID", userId, DbType.String);
                dp_params.Add("HairType", hairType, DbType.String);
                dp_params.Add("QA", QA, DbType.Int32);
                using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = connection.QueryAsync<ProductEntity>("sp_GetMatchingProducts", dp_params, commandType: CommandType.StoredProcedure).GetAwaiter().GetResult();

                    List<RecommendedProducts> recommendedProducts = new List<RecommendedProducts>();
                    foreach (var item in result)
                    {
                        var obj = new RecommendedProducts();
                        obj.Name = item.ProductName;
                        obj.ProductId = item.Id;
                        obj.IsActive = true;
                        obj.CreatedOn = DateTime.Now;
                        obj.HairProfileId = hairProfileId;
                        recommendedProducts.Add(obj);
                    }
                    AddRecommendedProducts(recommendedProducts, hairProfileId);
                    AddStyleRecipeProducts(hairType, hairProfileId, userId);
                    AutoRecommendVideos(QA, hairProfileId, userId);

                }


                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: UpdateRecommendedProductsSP, UserId:" + userId + ", HairProfileId:" + hairProfileId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public bool UpdateRecommendedProductsSPForMobile(string hairType, int hairProfileId, string userId)
        {
            try
            {
                var dp_params = new DynamicParameters();
                dp_params.Add("UserID", userId, DbType.String);
                dp_params.Add("HairType", hairType, DbType.String);

                using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = connection.QueryAsync<ProductEntity>("sp_GetMatchingProducts_MyavanaPro", dp_params, commandType: CommandType.StoredProcedure).GetAwaiter().GetResult();

                    List<RecommendedProducts> recommendedProducts = new List<RecommendedProducts>();
                    foreach (var item in result)
                    {
                        var obj = new RecommendedProducts();
                        obj.Name = item.ProductName;
                        obj.ProductId = item.Id;
                        obj.IsActive = true;
                        obj.CreatedOn = DateTime.Now;
                        obj.HairProfileId = hairProfileId;
                        recommendedProducts.Add(obj);
                    }
                    AddRecommendedProducts(recommendedProducts, hairProfileId);
                    AddStyleRecipeProducts(hairType, hairProfileId, userId);
                }


                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: UpdateRecommendedProductsSPForMobile, UserId:" + userId + ", HairProfileId:" + hairProfileId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public bool AddStyleRecipeProducts(string hairType, int hairProfileId, string userId)
        {
            try
            {
                var dp_params = new DynamicParameters();
                dp_params.Add("UserID", userId, DbType.String);
                dp_params.Add("HairType", hairType, DbType.String);

                using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = connection.QueryAsync<ProductEntity>("sp_GetStyleRecipeProducts", dp_params, commandType: CommandType.StoredProcedure).GetAwaiter().GetResult();

                    List<RecommendedProductsStyleRecipe> recommendedStyleRecipeProducts = new List<RecommendedProductsStyleRecipe>();
                    foreach (var item in result)
                    {
                        var obj = new RecommendedProductsStyleRecipe();
                        obj.Name = item.ProductName;
                        obj.ProductId = item.Id;
                        obj.IsActive = true;
                        obj.CreatedOn = DateTime.Now;
                        obj.HairProfileId = hairProfileId;
                        recommendedStyleRecipeProducts.Add(obj);
                    }
                    AddRecommendedProductsStyleRecipe(recommendedStyleRecipeProducts, hairProfileId, userId);
                }


                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: UpdateRecommendedProductsSP, UserId:" + userId + ", HairProfileId:" + hairProfileId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public bool AddRecommendedProducts(List<RecommendedProducts> recommendedProducts, int hairProfileId)
        {
            try
            {
                if (recommendedProducts.Count() == 0)
                {
                    List<RecommendedProducts> OldProducts = context.RecommendedProducts.Where(x => x.HairProfileId == hairProfileId && x.IsAllEssential != true).ToList();
                    context.RemoveRange(OldProducts);
                    context.SaveChanges();
                }
                if (recommendedProducts.Count() > 0)
                {

                    List<RecommendedProducts> OldProducts = context.RecommendedProducts.Where(x => x.HairProfileId == hairProfileId && x.IsAllEssential != true).ToList();

                    List<RecommendedProducts> SelectedProducts = recommendedProducts.ToList();
                    List<RecommendedProducts> NewProductsToAdd = new List<RecommendedProducts>();
                    NewProductsToAdd = SelectedProducts.Where(item1 => OldProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();

                    List<RecommendedProducts> DeselectedProducts = new List<RecommendedProducts>();
                    DeselectedProducts = OldProducts.Where(item1 => SelectedProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();
                    if (DeselectedProducts.Count() > 0)
                    {
                        context.RemoveRange(DeselectedProducts);
                        context.SaveChanges();
                    }
                    if (NewProductsToAdd.Count() > 0)
                    {
                        foreach (var prod in NewProductsToAdd)
                        {

                            prod.IsActive = true;
                            prod.CreatedOn = DateTime.Now;
                            prod.HairProfileId = hairProfileId;
                            prod.IsAllEssential = false;

                        }
                        context.AddRange(NewProductsToAdd);
                        context.SaveChanges();
                    }

                }
                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: AddRecommendedProducts, HairProfileId:" + hairProfileId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public bool AddRecommendedProductsStyleRecipe(List<RecommendedProductsStyleRecipe> recommendedStyleRecipeProducts, int hairProfileId, string userId)
        {
            try
            {
                if (recommendedStyleRecipeProducts.Count() == 0)
                {
                    List<RecommendedProductsStyleRecipe> OldProducts = context.RecommendedProductsStyleRecipe.Where(x => x.HairProfileId == hairProfileId).ToList();
                    context.RemoveRange(OldProducts);
                    context.SaveChanges();
                }
                if (recommendedStyleRecipeProducts.Count() > 0)
                {

                    List<RecommendedProductsStyleRecipe> OldProducts = context.RecommendedProductsStyleRecipe.Where(x => x.HairProfileId == hairProfileId).ToList();

                    List<RecommendedProductsStyleRecipe> SelectedProducts = recommendedStyleRecipeProducts.ToList();
                    List<RecommendedProductsStyleRecipe> NewProductsToAdd = new List<RecommendedProductsStyleRecipe>();
                    NewProductsToAdd = SelectedProducts.Where(item1 => OldProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();

                    List<RecommendedProductsStyleRecipe> DeselectedProducts = new List<RecommendedProductsStyleRecipe>();
                    DeselectedProducts = OldProducts.Where(item1 => SelectedProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();
                    if (DeselectedProducts.Count() > 0)
                    {
                        context.RemoveRange(DeselectedProducts);
                        context.SaveChanges();
                    }
                    if (NewProductsToAdd.Count() > 0)
                    {
                        foreach (var prod in NewProductsToAdd)
                        {

                            prod.IsActive = true;
                            prod.CreatedOn = DateTime.Now;
                            prod.HairProfileId = hairProfileId;
                            //prod.IsAllEssential = false;
                        }
                        context.AddRange(NewProductsToAdd);
                        context.SaveChanges();
                    }

                }
                int? latestQA = context.Questionaires.Where(x => x.UserId == userId && x.IsActive == true).OrderByDescending(x => x.QA).FirstOrDefault()?.QA;
                var hairStyleAnswer = context.Questionaires.Include(x => x.Answer).Where(x => x.QuestionId == 15 && x.UserId == userId && x.QA == latestQA).FirstOrDefault();

                int nextHairStyleId = context.HairStyles.Where(x => x.Style == hairStyleAnswer.Answer.Description).Select(x => x.Id).LastOrDefault();
                var existRecord = context.StyleRecipeHairStyle
                            .FirstOrDefault(sr => sr.HairProfileId == hairProfileId);
                if (existRecord != null)
                {

                    existRecord.CreatedOn = DateTime.Now;
                    existRecord.HairStyleId = nextHairStyleId;
                    context.StyleRecipeHairStyle.Update(existRecord);
                    context.SaveChanges();
                }
                else
                {
                    var obj = new StyleRecipeHairStyle();
                    obj.HairStyleId = nextHairStyleId;
                    obj.HairProfileId = hairProfileId;
                    obj.CreatedOn = DateTime.Now;
                    context.StyleRecipeHairStyle.Add(obj);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: AddRecommendedProductsStyleRecipe, HairProfileId:" + hairProfileId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public List<HairProfileSelectModel> GetHHCPList(string userId, bool? isRequestedFromCustomer)
        {
            try
            {
                var emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}";
                var UserEmail = Regex.IsMatch(userId, emailPattern);
                if(!UserEmail)
                {
                    userId = context.Users.Where(x => x.Id.ToString() == userId).Select(x => x.Email).FirstOrDefault();
                }
                if (isRequestedFromCustomer == true)
                {
                    var hhcpList = (from hr in context.HairProfiles
                                    where hr.UserId.ToLower() == userId.ToLower() && hr.IsActive == true && hr.IsDrafted != true
                                    && hr.IsViewEnabled == true
                                    select new HairProfileSelectModel
                                    {
                                        HairProfileId = hr.Id,
                                        HairProfile = hr.IsBasicHHCP == true ? "Hair AI Results - " + hr.CreatedOn.ToString("MMM dd,yyyy | HH:mm") : "Hair SI Results - " + hr.CreatedOn.ToString("MMM dd,yyyy HH:mm")
                                    }).OrderByDescending(x => x.HairProfileId).ToList();
                    return hhcpList;
                }
                else
                {
                    var hhcpList = (from hr in context.HairProfiles
                                    where hr.UserId.ToLower() == userId.ToLower() && hr.IsActive == true && hr.IsDrafted != true
                                    select new HairProfileSelectModel
                                    {
                                        HairProfileId = hr.Id,
                                        HairProfile = hr.IsBasicHHCP == true ? "Hair AI Results - " + hr.CreatedOn.ToString("MMM dd,yyyy | HH:mm") : "Hair SI Results - " + hr.CreatedOn.ToString("MMM dd,yyyy HH:mm"),
                                        QA = hr.AttachedQA == null ? "" : " - QA " + Convert.ToInt32(hr.AttachedQA + 1).ToString()
                                    }).OrderByDescending(x => x.HairProfileId).ToList();
                    return hhcpList;
                }

            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetHHCPList, UserId:" + userId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public DigitalAssessmentModel CreateHHCPHairKitUser(DigitalAssessmentModel digitalAssessmentModel)
        {
            try
            {
                DigitalAssessmentModel ret = new DigitalAssessmentModel();
                var entity = _userManager.FindByEmailAsync(digitalAssessmentModel.Userid).GetAwaiter().GetResult();
                if (entity != null)
                {
                    UserEntity us = context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();

                    var hairProfileLatest = context.HairProfiles.LastOrDefault(x => x.UserId == us.Email && x.IsActive == true);

                    Models.Entities.HairProfile hairProfile = new Models.Entities.HairProfile();
                    hairProfile.UserId = us.Email;
                    hairProfile.HealthSummary = hairProfileLatest.HealthSummary;
                    hairProfile.HairId = hairProfileLatest.HairId;
                    hairProfile.AttachedQA = digitalAssessmentModel.QA;
                    int res = CreateHairProfileHairKit(hairProfile);
                    if (res > 0)
                    {
                        var ExistAdditionalHairInfo = context.AdditionalHairInfo.FirstOrDefault(x => x.HairId == hairProfileLatest.Id);
                        if (ExistAdditionalHairInfo != null)
                        {
                            AdditionalHairInfo additionalHairInfo = new AdditionalHairInfo();
                            additionalHairInfo.TypeId = ExistAdditionalHairInfo.TypeId;
                            additionalHairInfo.TypeDescription = ExistAdditionalHairInfo.TypeDescription;
                            additionalHairInfo.TextureId = ExistAdditionalHairInfo.TextureId;
                            additionalHairInfo.TextureDescription = ExistAdditionalHairInfo.TextureDescription;
                            additionalHairInfo.PorosityId = ExistAdditionalHairInfo.PorosityId;
                            additionalHairInfo.PorosityDescription = ExistAdditionalHairInfo.PorosityDescription;
                            additionalHairInfo.HealthId = ExistAdditionalHairInfo.HealthId;
                            additionalHairInfo.HealthDescription = ExistAdditionalHairInfo.HealthDescription;
                            additionalHairInfo.DensityId = ExistAdditionalHairInfo.DensityId;
                            additionalHairInfo.DensityDescription = ExistAdditionalHairInfo.DensityDescription;
                            additionalHairInfo.ElasticityId = ExistAdditionalHairInfo.ElasticityId;
                            additionalHairInfo.ElasticityDescription = ExistAdditionalHairInfo.ElasticityDescription;
                            additionalHairInfo.HairId = res;
                            context.Add(additionalHairInfo);
                            context.SaveChanges();
                            var selectedGoals = context.CustomerHairGoals.Where(x => x.HairInfoId == ExistAdditionalHairInfo.Id);
                            List<CustomerHairGoals> customerHairGoals = new List<CustomerHairGoals>();
                            foreach (var hairGoal in selectedGoals)
                            {
                                CustomerHairGoals customerHairGoal = new CustomerHairGoals();
                                customerHairGoal.HairInfoId = additionalHairInfo.Id;
                                customerHairGoal.Description = hairGoal.Description;
                                customerHairGoal.CreatedOn = DateTime.Now;
                                customerHairGoal.IsActive = true;
                                customerHairGoals.Add(customerHairGoal);

                            }
                            context.AddRange(customerHairGoals);

                            var selectedChallenges = context.CustomerHairChallenge.Where(x => x.HairInfoId == ExistAdditionalHairInfo.Id);
                            List<CustomerHairChallenge> customerHairChallenges = new List<CustomerHairChallenge>();
                            foreach (var challenge in selectedChallenges)
                            {
                                CustomerHairChallenge customerHairChallenge = new CustomerHairChallenge();
                                customerHairChallenge.HairInfoId = additionalHairInfo.Id;
                                customerHairChallenge.Description = challenge.Description;
                                customerHairChallenge.CreatedOn = DateTime.Now;
                                customerHairChallenge.IsActive = true;
                                customerHairChallenges.Add(customerHairChallenge);

                            }
                            context.AddRange(customerHairChallenges);
                            context.SaveChanges();
                        }

                    }
                }

                return ret;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateHHCPHairKitUser, UserId:" + digitalAssessmentModel.Userid + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public int CreateHairProfileHairKit(Models.Entities.HairProfile hairProfile)
        {
            try
            {
                var hair = new Models.Entities.HairProfile();
                hair.UserId = hairProfile.UserId;
                hair.HairId = hairProfile.HairId;
                hair.HealthSummary = hairProfile.HealthSummary;
                hair.ConsultantNotes = hairProfile.ConsultantNotes;
                hair.RecommendationNotes = hairProfile.RecommendationNotes;
                hair.IsActive = true;
                hair.CreatedOn = DateTime.Now;
                hair.IsDrafted = false;
                hair.IsBasicHHCP = false;
                hair.IsViewEnabled = true;
                hair.AttachedQA = hairProfile.AttachedQA;
                context.Add(hair);
                context.SaveChanges();
                return hair.Id;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateHairProfileHairKit, UserId:" + hairProfile.UserId + ", Error: " + Ex.Message, Ex);
                return 0;
            }

        }

        public List<MessageTemplateModel> GetMessageTempleteList()
        {
            try
            {
                var tempList = (from messTemp in context.MessageTemplates
                                where messTemp.IsActive == true
                                select new MessageTemplateModel
                                {
                                    MessageTemplateId = messTemp.MessageTemplateId,
                                    TemplateCode = messTemp.TemplateCode,
                                    TemplateSubject = messTemp.TemplateSubject,
                                    TemplateBody = messTemp.TemplateBody
                                }).ToList();
                return tempList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetMessageTempleteList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public SalonNotesModel SaveSalonNotes(SalonNotesModel salonNotesModel)
        {
            try
            {
                //var existNotes = context.SalonNotesHHCP.FirstOrDefault(x => x.HairProfileId == salonNotesModel.HairProfileId && x.UserId == Convert.ToInt32(salonNotesModel.LoginUserId));
                //if (existNotes != null)
                //{
                //    existNotes.Notes = salonNotesModel.SalonNotes;
                //}
                //else
                //{
                var obj = new SalonNotesHHCP();
                obj.Notes = salonNotesModel.SalonNotes;
                obj.HairProfileId = salonNotesModel.HairProfileId;
                obj.UserId = Convert.ToInt16(salonNotesModel.LoginUserId);
                obj.CreatedOn = DateTime.UtcNow;
                context.SalonNotesHHCP.Add(obj);
                //}
                context.SaveChanges();
                return salonNotesModel;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SaveSalonNotes, UserId:" + salonNotesModel.LoginUserId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public HairStrandsImagesModel UploadHairAnalysisImages(HairStrandsImagesModel hairProfile)
        {
            string WebApiUrl = configuration["WebApiUrl"];
            var hairProfileId = hairProfile.HairProfileId;
            if (hairProfile.HairProfileId == 0)
            {
                var entity = _userManager.FindByIdAsync(hairProfile.UserId).GetAwaiter().GetResult();
                if (entity != null)
                {
                    var latestQA = context.Questionaires.Where(x => x.UserId == hairProfile.UserId && x.IsActive == true).Max(x => x.QA);
                    UserEntity us = context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();
                    var hair = new Models.Entities.HairProfile();

                    hair.UserId = us.Email;
                    if (us.CustomerTypeId == (int)CustomerTypeEnum.DigitalAnalysis)
                    {
                        hair.HealthSummary = "Hi " + us.FirstName + " ! " + "\r\nYour Digital Hair Profile includes your introductory product recommendations based on a quick analysis of your hair. We incorporated your Hair Goals, Hair Challenges, Product Recommendations, and % breakdown of your Unique Hair Type Combination. To get a comprehensive healthy hair care plan, make sure to get your hair analysis kit in the menu to your left. This should get you started in the meantime! If you have any questions, please email us at support@myavana.com\r\nLove, \r\nMYAVANA ";
                    }
                    else
                    {
                        hair.HealthSummary = "Hi " + us.FirstName + " ! " + "\r\nYour Healthy Hair Care Plan includes your Hair ID, Hair Goals and Challenges, Hair Strand Analysis, Product Recommendations, Ingredients, Regimens, and Education. We have also included some personal notes from your MYAVANA lab analyst and hair consultant.\r\nLove, \r\nTanisha \r\nHair Analyst";
                    }
                    hair.IsActive = true;
                    hair.CreatedOn = DateTime.Now;
                    hair.IsDrafted = false;
                    hair.IsViewEnabled = true;
                    hair.AttachedQA = latestQA;
                    context.Add(hair);
                    context.SaveChanges();
                    hairProfileId = hair.Id;
                }

            }
            HairStrands strands = new HairStrands();
            string[] HairStrandImage = null;
            if (hairProfile.HairStrand != null)
            {
                HairStrandImage = hairProfile.HairStrand.Split(',');
            }
            try
            {

                strands = context.HairStrands.Where(x => x.HairProfileId == Convert.ToInt32(hairProfileId)).FirstOrDefault();
                if (strands != null)
                {
                    //HairStrands strands = new HairStrands();
                    if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.TopLeft)
                    {
                        strands.TopLeftPhoto = (strands.TopLeftPhoto?.TrimEnd(',') ?? string.Empty) + "," + hairProfile.HairStrand;
                    }
                    else if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.TopRight)
                    {
                        strands.TopRightPhoto = (strands.TopRightPhoto?.TrimEnd(',') ?? string.Empty) + "," + hairProfile.HairStrand;
                    }
                    else if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.BottomLeft)
                    {
                        strands.BottomLeftPhoto = (strands.BottomLeftPhoto?.TrimEnd(',') ?? string.Empty) + "," + hairProfile.HairStrand;
                    }
                    else if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.BottomRight)
                    {
                        strands.BottomRightPhoto = (strands.BottomRightPhoto?.TrimEnd(',') ?? string.Empty) + "," + hairProfile.HairStrand;
                    }
                    else if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.Crown)
                    {
                        strands.CrownPhoto = (strands.CrownPhoto?.TrimEnd(',') ?? string.Empty) + "," + hairProfile.HairStrand;
                    }
                    //strands.HairProfileId = Convert.ToInt32(hairProfile.HairProfileId);
                    context.SaveChanges();
                }
                else
                {
                    strands = new HairStrands();
                    if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.TopLeft)
                    {
                        strands.TopLeftPhoto = strands.TopLeftPhoto + "," + hairProfile.HairStrand;
                    }
                    else if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.TopRight)
                    {
                        strands.TopRightPhoto = strands.TopRightPhoto + "," + hairProfile.HairStrand;
                    }
                    else if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.BottomLeft)
                    {
                        strands.BottomLeftPhoto = strands.BottomLeftPhoto + "," + hairProfile.HairStrand;
                    }
                    else if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.BottomRight)
                    {
                        strands.BottomRightPhoto = strands.BottomRightPhoto + "," + hairProfile.HairStrand;
                    }
                    else if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.Crown)
                    {
                        strands.CrownPhoto = strands.CrownPhoto + "," + hairProfile.HairStrand;
                    }
                    strands.HairProfileId = hairProfileId;
                    context.Add(strands);
                    context.SaveChanges();
                }
                HairStrandUploadNotification notification = new HairStrandUploadNotification();
                notification.HairProfileId = hairProfileId;
                notification.SalonId = hairProfile.SalonId;
                notification.CreatedOn = DateTime.Now;
                notification.IsRead = false;
                context.Add(notification);
                context.SaveChanges();

                if (HairStrandImage != null)
                {
                    //List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.TopLeftImage != null).ToList();
                    //context.RemoveRange(lstHairStrandsImages);
                    //context.SaveChanges();

                    List<HairStrandsImages> lsthairStrands = new List<HairStrandsImages>();
                    foreach (var t in HairStrandImage)
                    {
                        if (t != "")
                        {
                            HairStrandsImages hairStrandsImag = new HairStrandsImages();
                            if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.TopLeft)
                            {
                                hairStrandsImag.TopLeftImage = WebApiUrl + "/HairProfile/GetHairAnalysisImage?imageName=" + t;
                            }
                            else if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.TopRight)
                            {
                                hairStrandsImag.TopRightImage = WebApiUrl + "/HairProfile/GetHairAnalysisImage?imageName=" + t;
                            }
                            else if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.BottomLeft)
                            {
                                hairStrandsImag.BottomLeftImage = WebApiUrl + "/HairProfile/GetHairAnalysisImage?imageName=" + t;
                            }
                            else if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.BottomRight)
                            {
                                hairStrandsImag.BottomRightImage = WebApiUrl + "/HairProfile/GetHairAnalysisImage?imageName=" + t;
                            }
                            else if (hairProfile.HairAnalysisImageType == (int)HairAnalysisImageTypeEnum.Crown)
                            {
                                hairStrandsImag.CrownImage = WebApiUrl + "/HairProfile/GetHairAnalysisImage?imageName=" + t;
                            }
                            hairStrandsImag.IsActive = true;
                            hairStrandsImag.CreatedOn = DateTime.Now;
                            hairStrandsImag.Id = strands.Id;
                            lsthairStrands.Add(hairStrandsImag);

                        }
                    }
                    context.AddRange(lsthairStrands);
                    context.SaveChanges();
                }
                return hairProfile;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: UploadHairAnalysisImages, HairProfileId:" + hairProfile.HairProfileId + ", Error: " + ex.Message, ex);
                return null;
            }

        }

        public bool DeleteHairStrandImage(HairStrandImageInfo strandImageInfo)
        {
            try
            {
                var objCode = context.HairStrandsImages.FirstOrDefault(x => x.StrandsImagesId == strandImageInfo.StrandImageId);
                {
                    if (objCode != null)
                    {
                        objCode.IsActive = false;
                    }
                }
                context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteHairStrandImage, HairStrandImageId:" + strandImageInfo.StrandImageId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public EnableDisableProfileModel EnableDisableProfileView(EnableDisableProfileModel enableDisableProfileModel)
        {
            try
            {
                var hairProfile = context.HairProfiles.FirstOrDefault(x => x.Id == enableDisableProfileModel.HairProfileId);
                if (hairProfile != null)
                {
                    hairProfile.IsViewEnabled = enableDisableProfileModel.IsViewEnabled;
                }
                context.SaveChanges();
                return enableDisableProfileModel;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: EnableDisableProfileView, HairProfileId:" + enableDisableProfileModel.HairProfileId + ", Error: " + Ex.Message, Ex);
                return null;
            }

        }

        public async Task<HairProfileCustomerModel> GetHairProfileCustomerTab2(HairProfileCustomerModel hairProfileModel)
        {
            string WebApiUrl = configuration["WebApiUrl"];
            HairProfileCustomerModel profile = new HairProfileCustomerModel();
            var userName = await _userManager.FindByIdAsync(hairProfileModel.UserId);
            var latesthhcpId = hairProfileModel.HairProfileId;
            if (latesthhcpId == null)
            {
                return null;
            }
            try
            {
                profile = (from hr in context.HairProfiles
                           join sts in context.HairStrands
                           on hr.Id equals sts.HairProfileId into hs
                           from st in hs.DefaultIfEmpty()
                           where hr.Id == latesthhcpId
                           && hr.IsActive == true && hr.IsDrafted == false
                           select new HairProfileCustomerModel()
                           {
                               HairProfileId = hr.Id,
                               UserId = hr.UserId,
                               HairId = hr.HairId,
                               HairAnalysisNotes = hr.HairAnalysisNotes,
                               TopLeft = st != null ? new TopLeftAdmin()
                               {

                                   TopLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopLeftImage != null && x.TopLeftImage != "" && x.IsActive == true)
                                   .Select(x => new HairStrandImageInfo
                                   {
                                       StrandImage = x.TopLeftImage.Contains(WebApiUrl)
                                                        ? x.TopLeftImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.TopLeftImage).Replace(" ", ""),
                                       StrandImageId = x.StrandsImagesId
                                   }).ToList(),
                                   TopLeftHealthText = st.TopLeftHealthText,
                                   TopLeftStrandDiameter = st.TopLeftStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()

                               } : new TopLeftAdmin(),
                               TopRight = st != null ? new TopRightAdmin()
                               {
                                   TopRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopRightImage != null && x.TopRightImage != "" && x.IsActive == true)
                                    .Select(x => new HairStrandImageInfo
                                    {
                                        StrandImage = x.TopRightImage.Contains(WebApiUrl)
                                                        ? x.TopRightImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.TopRightImage).Replace(" ", ""),
                                        StrandImageId = x.StrandsImagesId
                                    }).ToList(),
                                   TopRightHealthText = st.TopRightHealthText,
                                   TopRightStrandDiameter = st.TopRightStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()
                               } : new TopRightAdmin(),
                               BottomLeft = st != null ? new BottomLeftAdmin()
                               {
                                   BottomLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomLeftImage != null && x.BottomLeftImage != "" && x.IsActive == true)
                                  .Select(x => new HairStrandImageInfo
                                  {
                                      StrandImage = x.BottomLeftImage.Contains(WebApiUrl)
                                                        ? x.BottomLeftImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.BottomLeftImage).Replace(" ", ""),
                                      StrandImageId = x.StrandsImagesId
                                  }).ToList(),
                                   BottomLeftHealthText = st.BottomLeftHealthText,
                                   BottomLeftStrandDiameter = st.BottomLeftStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()
                               } : new BottomLeftAdmin(),
                               BottomRight = st != null ? new BottomRightAdmin()
                               {
                                   BottomRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomRightImage != null && x.BottomRightImage != "" && x.IsActive == true)
                                  .Select(x => new HairStrandImageInfo
                                  {
                                      StrandImage = x.BottomRightImage.Contains(WebApiUrl)
                                                        ? x.BottomRightImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.BottomRightImage).Replace(" ", ""),
                                      StrandImageId = x.StrandsImagesId
                                  }).ToList(),
                                   BottomRightHealthText = st.BottomRightHealthText,
                                   BottomRightStrandDiameter = st.BottomRightStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()
                               } : new BottomRightAdmin(),
                               CrownStrand = st != null ? new CrownStrandAdmin()
                               {
                                   CrownHealthText = st.CrownHealthText,
                                   CrownPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.CrownImage != null && x.CrownImage != "" && x.IsActive == true)
                                    .Select(x => new HairStrandImageInfo
                                    {
                                        StrandImage = x.CrownImage.Contains(WebApiUrl)
                                                        ? x.CrownImage
                                                        : (_adminStagingUrl + "HairProfile/HairProfileThumbnails/" + x.CrownImage).Replace(" ", ""),
                                        StrandImageId = x.StrandsImagesId
                                    }).ToList(),
                                   CrownStrandDiameter = st.CrownStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()
                               } : new CrownStrandAdmin()
                           }).FirstOrDefault();

                if (!string.IsNullOrEmpty(hairProfileModel.LoginUserId))
                {
                    var salonNotes = context.SalonNotesHHCP.Include(x => x.WebLogin).Where(x => x.HairProfileId == profile.HairProfileId);
                    profile.SalonNotes = salonNotes.FirstOrDefault(x => x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId))?.Notes;

                    var userDetail = context.WebLogins.FirstOrDefault(x => x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId));

                    TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("US/Eastern");
                    if (userDetail?.UserTypeId == (int)UserTypeEnum.B2B && salonNotes != null)
                    {
                        var salonId = context.SalonsOwners.FirstOrDefault(x => x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId) && x.IsActive == true)?.SalonId;
                        if (salonId > 0)
                        {
                            var salonDet = context.Salons.FirstOrDefault(x => x.SalonId == salonId);
                            if (salonDet.IsPublicNotes == true)
                            {
                                profile.SalonNotesModel = salonNotes.OrderByDescending(x => x.CreatedOn).Select(x => new SalonNotesModel
                                {
                                    SalonNotes = x.Notes,
                                    HairProfileId = x.HairProfileId,
                                    LoginUserId = x.UserId.ToString(),
                                    CreatedBy = x.WebLogin.UserEmail,
                                    CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(x.CreatedOn, easternZone),
                                }).ToList();
                                profile.IsPublicSalonNote = true;
                            }
                            else
                            {
                                profile.SalonNotesModel = salonNotes.OrderByDescending(x => x.CreatedOn).Where(x => x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId)).Select(x => new SalonNotesModel
                                {
                                    SalonNotes = x.Notes,
                                    HairProfileId = x.HairProfileId,
                                    LoginUserId = x.UserId.ToString(),
                                    CreatedBy = x.WebLogin.UserEmail,
                                    CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(x.CreatedOn, easternZone),
                                }).ToList();
                            }
                        }
                    }
                    else
                    {
                        profile.SalonNotesModel = salonNotes.OrderByDescending(x => x.CreatedOn).Select(x => new SalonNotesModel
                        {
                            SalonNotes = x.Notes,
                            HairProfileId = x.HairProfileId,
                            LoginUserId = x.UserId.ToString(),
                            CreatedBy = x.WebLogin.UserEmail,
                            CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(x.CreatedOn, easternZone),
                        }).ToList();
                        profile.IsPublicSalonNote = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetHairProfileCustomerTab2, UserId:" + hairProfileModel.UserId + ", Error: " + Ex.Message, Ex);
            }
            return profile;
        }

        public async Task<HairProfileCustomerModel> GetHairProfileCustomerExceptTab2(HairProfileCustomerModel hairProfileModel)
        {
            HairProfileCustomerModel profile = new HairProfileCustomerModel();
            var userName = await _userManager.FindByIdAsync(hairProfileModel.UserId);
            var latesthhcpId = context.HairProfiles.LastOrDefault(x => x.UserId == userName.UserName && x.IsActive == true && x.IsDrafted != true)?.Id;
            if (hairProfileModel.IsRequestedFromCustomer == true)
            {
                latesthhcpId = context.HairProfiles.LastOrDefault(x => x.UserId == userName.UserName && x.IsActive == true && x.IsDrafted != true && x.IsViewEnabled == true)?.Id;
            }
            if (hairProfileModel.HairProfileId != 0)
            {
                latesthhcpId = hairProfileModel.HairProfileId;
            }
            if (latesthhcpId == null)
            {
                return null;
            }
            try
            {
                profile = (from hr in context.HairProfiles
                           join sts in context.HairStrands
                           on hr.Id equals sts.HairProfileId into hs
                           from st in hs.DefaultIfEmpty()
                           where hr.Id == latesthhcpId
                           && hr.IsActive == true && hr.IsDrafted == false
                           select new HairProfileCustomerModel()
                           {
                               HairProfileId = hr.Id,
                               UserId = hr.UserId,
                               HairId = hr.HairId,
                               IsBasicHHCP = hr.IsBasicHHCP,
                               UserName = userName.FirstName + " " + userName.LastName,
                               AIResult = userName.AIResult,
                               CustomerTypeId = userName.CustomerTypeId,
                               IsAIV2Mobile = userName.IsAIV2Mobile,
                               //AIResultNew = context.CustomerAIResults.Where(x => x.HairProfileId == hr.Id).Select(x => x.AIResult).FirstOrDefault(),
                               AIResultNew = context.CustomerAIResults.Where(x => x.HairProfileId == hr.Id).FirstOrDefault() != null ? context.CustomerAIResults.Where(x => x.HairProfileId == hr.Id).Select(x => x.AIResult).FirstOrDefault() : context.CustomerAIResults.Where(x => x.UserId.ToString() == hairProfileModel.UserId).OrderBy(x => x.CreatedOn).Select(x => x.AIResult).FirstOrDefault(),
                               CountAIResults = (context.CustomerAIResults.Where(x => x.UserId.ToString() == hairProfileModel.UserId)).Count(),
                               IsVersion2 = context.CustomerAIResults.FirstOrDefault(x => x.HairProfileId == hr.Id && x.IsVersion2 == true) != null ? true : false,
                               HealthSummary = hr.HealthSummary,
                               ConsultantNotes = hr.ConsultantNotes,
                               RecommendationNotes = hr.RecommendationNotes,
                               HairAnalysisNotes = hr.HairAnalysisNotes,
                               SalonId = userName.SalonId,
                               IsViewEnabled = hr.IsViewEnabled,
                               HairAnalyst = context.HairAnalyst.Where(a=>a.HairAnalystId == hr.CreatedBy).FirstOrDefault(),
                               //HairAnalyst = context.HairAnalyst.Where(a => a.HairAnalystId == hr.CreatedBy).ToList(),
                               IsAlreadyShareHHCP = (context.SharedHHCP.Where(a => a.SharedBy == new Guid(hairProfileModel.UserId)).Any()==true?true:false),
                               HasHHCPSharedWithMe = (context.SharedHHCP.Where(a => a.SharedWith == new Guid(hairProfileModel.UserId)).Any() == true ? true : false),
                               CustomerTypeDesc = context.CustomerTypes.FirstOrDefault(x => x.CustomerTypeId == userName.CustomerTypeId).Description,
                               RecommendedVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedVideosCustomer
                               {
                                   Id = s.Id,
                                   MediaLinkEntityId = s.MediaLinkEntityId,
                                   HairProfileId = s.HairProfileId,
                                   Name = s.Name,
                                   // Videos = context.MediaLinkEntities.Where(x => x.MediaLinkEntityId == s.MediaLinkEntityId).Select(x => x.VideoId).ToList().ToString().Replace("watch","embed")
                                   Videos = (from media in context.MediaLinkEntities
                                             where media.MediaLinkEntityId == s.MediaLinkEntityId
                                             select media.VideoId.ToString().Replace("watch?v=", "embed/")).ToList()
                               }).ToList(),
                               RecommendedStyleRecipeVideos = context.RecommendedStyleRecipeVideos.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedVideosCustomer
                               {
                                   Id = s.Id,
                                   MediaLinkEntityId = s.MediaLinkEntityId,
                                   HairProfileId = s.HairProfileId,
                                   Name = s.Name,
                                   // Videos = context.MediaLinkEntities.Where(x => x.MediaLinkEntityId == s.MediaLinkEntityId).Select(x => x.VideoId).ToList().ToString().Replace("watch","embed")
                                   Videos = (from media in context.MediaLinkEntities
                                             where media.MediaLinkEntityId == s.MediaLinkEntityId
                                             select media.VideoId.ToString().Replace("watch?v=", "embed/")).ToList()
                               }).ToList(),
                               RecommendedIngredients = context.RecommendedIngredients.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedIngredientsCustomer
                               {
                                   Id = s.Id,
                                   IngredientId = s.IngredientId,
                                   HairProfileId = s.HairProfileId,
                                   Ingredients = context.IngedientsEntities.Where(x => x.IngedientsEntityId == s.IngredientId).Select(x => new IngredientsModels
                                   {
                                       Name = x.Name,
                                       ImageName = _adminUrl + "Ingredients/" + x.Image,
                                       Description = x.Description,
                                   }).ToList()
                               }).ToList(),
                               //--------------------------------------------
                               RecommendedTools = context.RecommendedTools.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedToolsCustomer
                               {
                                   Id = s.Id,
                                   ToolId = s.ToolId,
                                   HairProfileId = s.HairProfileId,
                                   ToolList = context.Tools.Where(x => x.Id == s.ToolId).Select(x => new ToolsModels
                                   {
                                       Name = x.ToolName,
                                       ImageName = x.Image,
                                       ToolDetail = x.ToolDetails
                                   }).ToList()
                               }).ToList(),


                               //  ImageName = "https://localhost:44322/tools/" + x.Image,  
                               //---------------------------------------------
                               recommendedStylistCustomers = context.RecommendedStylists.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedStylistCustomer
                               {
                                   Id = s.Id,
                                   StylistId = s.StylistId,
                                   HairProfileId = s.HairProfileId,
                                   Stylist = context.Stylists.Where(x => x.StylistId == s.StylistId).Select(x => new StylistCustomerModel
                                   {
                                       StylistName = x.StylistName,
                                       Salon = x.SalonName,
                                       Email = x.Email,
                                       Phone = x.PhoneNumber,
                                       Website = x.Website,
                                       Instagram = x.Instagram
                                   }).ToList()
                               }).ToList()

                           }).FirstOrDefault();
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetHairProfileCustomer, UserId:" + hairProfileModel.UserId + ", Error: " + Ex.Message, Ex);
            }

            int hairId = context.HairProfiles.Where(x => x.UserId == userName.UserName && x.IsActive == true && x.IsDrafted == false).Select(x => x.Id).LastOrDefault();
            if (hairProfileModel.HairProfileId != 0)
            {
                hairId = hairProfileModel.HairProfileId;
            }
            if (hairId != 0)
            {
                try
                {
                    profile.RecommendedRegimens = RecommendedRegimensCustomer(hairId);

                    List<int> productIds = context.RecommendedProducts.Where(x => x.HairProfileId == hairId).OrderByDescending(x => x.CreatedOn).Select(x => x.ProductId).ToList();
                    List<int?> types = context.ProductEntities.Where(x => productIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                    List<int?> parentIds = context.ProductTypes.Where(x => types.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var parents = context.ProductTypeCategories.Where(x => parentIds.Contains(x.Id)).ToList();
                    List<RecommendedProductsCustomer> productsTypesList = new List<RecommendedProductsCustomer>();
                    List<RecommendedProductsCustomer> styleproductsTypesList = new List<RecommendedProductsCustomer>();
                    var brandsList = context.Brands.Where(x => x.HideInSearch == true && x.IsActive == true).Select(x => x.BrandName).ToList();

                    foreach (var parentProduct in parents)
                    {
                        RecommendedProductsCustomer productsTypes = new RecommendedProductsCustomer();
                        productsTypes.ProductParentName = parentProduct.CategoryName;
                        productsTypes.ProductId = parentProduct.Id;
                        List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                        List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id))
                            .Select(x => x.ProductTypesId).Distinct().ToList();

                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesModels productsTypesModel = new ProductsTypesModels();
                                var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type
                                && x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id) && x.HideInSearch != true && !brandsList.Contains(x.BrandName)).ToList();
                                if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                                {
                                    productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                    productsTypesModel.ProductId = type;
                                }

                                productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                {
                                    Id = x.Id,
                                    BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                    ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                    HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                    ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault(),
                                    HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                    {
                                        Description = p.HairChallenges.Description,
                                        HairChallengeId = p.HairChallenges.HairChallengeId
                                    }).ToList(),
                                    HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                    {
                                        Description = p.HairGoal.Description,
                                        HairGoalId = p.HairGoal.HairGoalId
                                    }).ToList()
                                }).ToList();
                                productsTypesModels.Add(productsTypesModel);
                            }
                        }
                        productsTypes.ProductsTypes = productsTypesModels;
                        productsTypesList.Add(productsTypes);
                    }

                    //profile.RecommendedProducts = productsTypesList;

                    //Start Essential Products Code after multiple Product Type functionality merge recommended products
                    var newEssProds = (from s in context.ProductCommons
                                       join srecomm in context.RecommendedProducts
                                       on s.ProductEntityId equals srecomm.ProductId
                                       where srecomm.HairProfileId == hairId && s.ProductTypeId != null && s.IsActive == true
                                       select s).Distinct().ToList();
                    List<int?> typesNew = newEssProds.Select(x => x.ProductTypeId).Distinct().ToList();
                    List<int?> productIdsNew = newEssProds.Where(x => x.ProductTypeId != null).Select(x => x.ProductEntityId).Distinct().ToList();

                    List<int?> parentIdsNew = context.ProductTypes.Where(x => typesNew.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var parentsNew = context.ProductTypeCategories.Where(x => parentIdsNew.Contains(x.Id)).ToList();

                    foreach (var parentProduct in parentsNew)
                    {

                        List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                        List<int?> productByTypes = (from s in newEssProds
                                                     join pType in context.ProductTypes
                                                     on s.ProductTypeId equals pType.Id
                                                     where pType.ParentId == parentProduct.Id
                                                     select s.ProductTypeId).Distinct().ToList();

                        var existProdType = productsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id);
                        if (existProdType != null)
                        {
                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    var existType = existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type);
                                    if (existType != null)
                                    {
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && pCom.IsActive == true
                                                        && productIdsNew.Contains(prod.Id) && pType.Id == type
                                                        && pType.ParentId == parentProduct.Id
                                                        && prod.HideInSearch != true
                                                        && !brandsList.Contains(prod.BrandName)
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();


                                        existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type).Products.AddRange(products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                            HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                            ProductType = productType.ProductName,
                                            HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                            {
                                                Description = p.HairChallenges.Description,
                                                HairChallengeId = p.HairChallenges.HairChallengeId
                                            }).ToList(),
                                            HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                            {
                                                Description = p.HairGoal.Description,
                                                HairGoalId = p.HairGoal.HairGoalId
                                            }).ToList()
                                        }).ToList());
                                    }
                                    else
                                    {
                                        ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && pCom.IsActive == true
                                                        && productIdsNew.Contains(prod.Id)
                                                        && pType.Id == type && pType.ParentId == parentProduct.Id
                                                        && prod.HideInSearch != true
                                                        && !brandsList.Contains(prod.BrandName)
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                        if (productType != null)
                                        {
                                            productsTypesNew.ProductTypeName = productType.ProductName;
                                            productsTypesNew.ProductId = type;
                                        }

                                        productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                            HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                            ProductType = productType.ProductName,
                                            HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                            {
                                                Description = p.HairChallenges.Description,
                                                HairChallengeId = p.HairChallenges.HairChallengeId
                                            }).ToList(),
                                            HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                            {
                                                Description = p.HairGoal.Description,
                                                HairGoalId = p.HairGoal.HairGoalId
                                            }).ToList()
                                        }).ToList();
                                        productsTypesModels.Add(productsTypesNew);
                                    }

                                }
                            }
                            productsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id).ProductsTypes.AddRange(productsTypesModels);
                        }
                        else
                        {
                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                                    var products = (from prod in context.ProductEntities
                                                    join pCom in context.ProductCommons
                                                    on prod.Id equals pCom.ProductEntityId
                                                    join pType in context.ProductTypes
                                                    on pCom.ProductTypeId equals pType.Id
                                                    where pCom.ProductTypeId != null && pCom.IsActive == true
                                                    && productIdsNew.Contains(prod.Id)
                                                    && pType.Id == type && pType.ParentId == parentProduct.Id
                                                    && prod.HideInSearch != true
                                                    && !brandsList.Contains(prod.BrandName)
                                                    select prod).Distinct().ToList();
                                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                    if (productType != null)
                                    {
                                        productsTypesNew.ProductTypeName = productType.ProductName;
                                        productsTypesNew.ProductId = type;
                                    }

                                    productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => p.ProductClassificationId).ToList(),
                                        HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => p.HairChallengeId).ToList(),
                                        ProductType = productType.ProductName,
                                        HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                        {
                                            Description = p.HairChallenges.Description,
                                            HairChallengeId = p.HairChallenges.HairChallengeId
                                        }).ToList(),
                                        HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                        {
                                            Description = p.HairGoal.Description,
                                            HairGoalId = p.HairGoal.HairGoalId
                                        }).ToList()
                                    }).ToList();
                                    productsTypesModels.Add(productsTypesNew);
                                }
                            }
                            RecommendedProductsCustomer productsTypes = new RecommendedProductsCustomer();
                            productsTypes.ProductParentName = parentProduct.CategoryName;
                            productsTypes.ProductId = parentProduct.Id;
                            productsTypes.ProductsTypes = productsTypesModels;
                            productsTypesList.Add(productsTypes);
                        }

                    }
                    profile.RecommendedProducts = productsTypesList;
                    //End Essential Products Code after multiple Product Type functionality merge recommended products


                    //--style recipe products
                    var styleProds = (from s in context.ProductCommons
                                      join srecomm in context.RecommendedProductsStyleRecipe
                                      on s.ProductEntityId equals srecomm.ProductId
                                      where srecomm.HairProfileId == hairId && s.ProductTypeId != null && s.IsActive == true
                                      select s).Distinct().ToList();
                    List<int?> styletypesNew = styleProds.Select(x => x.ProductTypeId).Distinct().ToList();
                    List<int?> styleproductIdsNew = styleProds.Where(x => x.ProductTypeId != null).Select(x => x.ProductEntityId).Distinct().ToList();

                    List<int?> styleparentIdsNew = context.ProductTypes.Where(x => styletypesNew.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var styleparentsNew = context.ProductTypeCategories.Where(x => styleparentIdsNew.Contains(x.Id)).ToList();

                    foreach (var parentProduct in styleparentsNew)
                    {

                        List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                        List<int?> productByTypes = (from s in styleProds
                                                     join pType in context.ProductTypes
                                                     on s.ProductTypeId equals pType.Id
                                                     where pType.ParentId == parentProduct.Id
                                                     select s.ProductTypeId).Distinct().ToList();

                        var existProdType = styleproductsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id);
                        if (existProdType != null)
                        {
                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    var existType = existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type);
                                    if (existType != null)
                                    {
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && styleproductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();


                                        existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type).Products.AddRange(products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductType = productType.ProductName,
                                            HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                            {
                                                Description = p.HairChallenges.Description,
                                                HairChallengeId = p.HairChallenges.HairChallengeId
                                            }).ToList(),
                                            HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                            {
                                                Description = p.HairGoal.Description,
                                                HairGoalId = p.HairGoal.HairGoalId
                                            }).ToList()
                                        }).ToList());
                                    }
                                    else
                                    {
                                        ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && styleproductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                        if (productType != null)
                                        {
                                            productsTypesNew.ProductTypeName = productType.ProductName;
                                            productsTypesNew.ProductId = type;
                                        }

                                        productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductType = productType.ProductName,
                                            HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                            {
                                                Description = p.HairChallenges.Description,
                                                HairChallengeId = p.HairChallenges.HairChallengeId
                                            }).ToList(),
                                            HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                            {
                                                Description = p.HairGoal.Description,
                                                HairGoalId = p.HairGoal.HairGoalId
                                            }).ToList()
                                        }).ToList();
                                        productsTypesModels.Add(productsTypesNew);
                                    }

                                }
                            }
                            styleproductsTypesList.FirstOrDefault(x => x.ProductId == parentProduct.Id).ProductsTypes.AddRange(productsTypesModels);
                        }
                        else
                        {
                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    ProductsTypesModels productsTypesNew = new ProductsTypesModels();
                                    var products = (from prod in context.ProductEntities
                                                    join pCom in context.ProductCommons
                                                    on prod.Id equals pCom.ProductEntityId
                                                    join pType in context.ProductTypes
                                                    on pCom.ProductTypeId equals pType.Id
                                                    where pCom.ProductTypeId != null && styleproductIdsNew.Contains(prod.Id) && pType.Id == type && pType.ParentId == parentProduct.Id && pCom.IsActive == true
                                                    select prod).Distinct().ToList();
                                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                    if (productType != null)
                                    {
                                        productsTypesNew.ProductTypeName = productType.ProductName;
                                        productsTypesNew.ProductId = type;
                                    }

                                    productsTypesNew.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductType = productType.ProductName,
                                        HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                        {
                                            Description = p.HairChallenges.Description,
                                            HairChallengeId = p.HairChallenges.HairChallengeId
                                        }).ToList(),
                                        HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                        {
                                            Description = p.HairGoal.Description,
                                            HairGoalId = p.HairGoal.HairGoalId
                                        }).ToList()
                                    }).ToList();
                                    productsTypesModels.Add(productsTypesNew);
                                }
                            }
                            RecommendedProductsCustomer productsTypes = new RecommendedProductsCustomer();
                            productsTypes.ProductParentName = parentProduct.CategoryName;
                            productsTypes.ProductId = parentProduct.Id;
                            productsTypes.ProductsTypes = productsTypesModels;
                            styleproductsTypesList.Add(productsTypes);
                        }

                    }
                    profile.RecommendedProductsStyleRecipe = styleproductsTypesList;
                    //---



                    //Styling Regimens Code before multiple Product Type functionality
                    List<int> rProductIds = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hairId).OrderByDescending(x => x.CreatedOn).Select(x => x.ProductId).ToList();
                    List<int?> pTypes = context.ProductEntities.Where(x => rProductIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();

                    List<int?> pParentIds = context.ProductTypes.Where(x => pTypes.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var pParents = context.ProductTypeCategories.Where(x => pParentIds.Contains(x.Id)).ToList();
                    List<RecommendedProductsStylingModel> productsTypesStylingList = new List<RecommendedProductsStylingModel>();
                    foreach (var parentProduct in pParents)
                    {
                        RecommendedProductsStylingModel productsTypes = new RecommendedProductsStylingModel();
                        productsTypes.ProductParentName = parentProduct.CategoryName;
                        productsTypes.ProductId = parentProduct.Id;
                        List<ProductsTypesStylingModels> productsTypesModels = new List<ProductsTypesStylingModels>();
                        List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id))
                            .Select(x => x.ProductTypesId).Distinct().ToList();


                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type
                                && x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id) && x.HideInSearch != true && !brandsList.Contains(x.BrandName)).ToList();
                                if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                                {
                                    productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                    productsTypesModel.ProductId = type;
                                }

                                productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                {
                                    Id = x.Id,
                                    BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                    ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault(),
                                    HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                    {
                                        Description = p.HairChallenges.Description,
                                        HairChallengeId = p.HairChallenges.HairChallengeId
                                    }).ToList(),
                                    HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                    {
                                        Description = p.HairGoal.Description,
                                        HairGoalId = p.HairGoal.HairGoalId
                                    }).ToList()
                                }).ToList();
                                productsTypesModels.Add(productsTypesModel);
                            }
                        }
                        productsTypes.ProductsTypes = productsTypesModels;
                        productsTypesStylingList.Add(productsTypes);
                    }
                    //profile.RecommendedProductsStyling = productsTypesStylingList;

                    //Start Styling Regimens Code after multiple Product Type functionality merge recommended products
                    var newProds = (from s in context.ProductCommons
                                    join srecomm in context.RecommendedProductsStyleRegimens
                                    on s.ProductEntityId equals srecomm.ProductId
                                    where srecomm.HairProfileId == hairId && s.ProductTypeId != null && s.IsActive == true
                                    select s).Distinct().ToList();
                    List<int?> pTypesNew = newProds.Select(x => x.ProductTypeId).Distinct().ToList();
                    List<int?> rProductIdsNew = newProds.Where(x => x.ProductTypeId != null).Select(x => x.ProductEntityId).Distinct().ToList();
                    List<int?> pParentIdsNew = context.ProductTypes.Where(x => pTypesNew.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var pParentsNew = context.ProductTypeCategories.Where(x => pParentIdsNew.Contains(x.Id)).ToList();

                    foreach (var parentProduct in pParentsNew)
                    {

                        List<ProductsTypesStylingModels> productsTypesModels = new List<ProductsTypesStylingModels>();
                        List<int?> productByTypes = (from s in newProds
                                                     join pType in context.ProductTypes
                                                     on s.ProductTypeId equals pType.Id
                                                     where pType.ParentId == parentProduct.Id
                                                     select s.ProductTypeId).Distinct().ToList();

                        var existProdType = productsTypesStylingList.FirstOrDefault(x => x.ProductId == parentProduct.Id);
                        if (existProdType != null)
                        {
                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    var existType = existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type);
                                    if (existType != null)
                                    {
                                        //ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && pCom.IsActive == true
                                                        && rProductIdsNew.Contains(prod.Id)
                                                        && pType.Id == type && pType.ParentId == parentProduct.Id
                                                        && prod.HideInSearch != true
                                                        && !brandsList.Contains(prod.BrandName)
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();


                                        existProdType.ProductsTypes.FirstOrDefault(x => x.ProductId == type).Products.AddRange(products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductType = productType.ProductName,
                                            HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                            {
                                                Description = p.HairChallenges.Description,
                                                HairChallengeId = p.HairChallenges.HairChallengeId
                                            }).ToList(),
                                            HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                            {
                                                Description = p.HairGoal.Description,
                                                HairGoalId = p.HairGoal.HairGoalId
                                            }).ToList()
                                        }).ToList());
                                    }
                                    else
                                    {
                                        ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                        var products = (from prod in context.ProductEntities
                                                        join pCom in context.ProductCommons
                                                        on prod.Id equals pCom.ProductEntityId
                                                        join pType in context.ProductTypes
                                                        on pCom.ProductTypeId equals pType.Id
                                                        where pCom.ProductTypeId != null && pCom.IsActive == true
                                                        && rProductIdsNew.Contains(prod.Id)
                                                        && pType.Id == type && pType.ParentId == parentProduct.Id
                                                        && prod.HideInSearch != true
                                                        && !brandsList.Contains(prod.BrandName)
                                                        select prod).Distinct().ToList();
                                        var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                        if (productType != null)
                                        {
                                            productsTypesModel.ProductTypeName = productType.ProductName;
                                            productsTypesModel.ProductId = type;
                                        }

                                        productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                        {
                                            Id = x.Id,
                                            BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                            ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                            ProductLink = x.ProductLink,
                                            ProductDetails = x.ProductDetails,
                                            ProductName = x.ProductName,
                                            ProductType = productType.ProductName,
                                            HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                            {
                                                Description = p.HairChallenges.Description,
                                                HairChallengeId = p.HairChallenges.HairChallengeId
                                            }).ToList(),
                                            HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                            {
                                                Description = p.HairGoal.Description,
                                                HairGoalId = p.HairGoal.HairGoalId
                                            }).ToList()
                                        }).ToList();
                                        productsTypesModels.Add(productsTypesModel);
                                    }

                                }
                            }
                            productsTypesStylingList.FirstOrDefault(x => x.ProductId == parentProduct.Id).ProductsTypes.AddRange(productsTypesModels);
                        }
                        else
                        {
                            foreach (var type in productByTypes)
                            {
                                if (type != null)
                                {
                                    ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                    var products = (from prod in context.ProductEntities
                                                    join pCom in context.ProductCommons
                                                    on prod.Id equals pCom.ProductEntityId
                                                    join pType in context.ProductTypes
                                                    on pCom.ProductTypeId equals pType.Id
                                                    where pCom.ProductTypeId != null && pCom.IsActive == true
                                                    && rProductIdsNew.Contains(prod.Id)
                                                    && pType.Id == type && pType.ParentId == parentProduct.Id
                                                    && prod.HideInSearch != true
                                                    && !brandsList.Contains(prod.BrandName)
                                                    select prod).Distinct().ToList();
                                    var productType = context.ProductTypes.Where(y => y.Id == type).FirstOrDefault();
                                    if (productType != null)
                                    {
                                        productsTypesModel.ProductTypeName = productType.ProductName;
                                        productsTypesModel.ProductId = type;
                                    }

                                    productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                    {
                                        Id = x.Id,
                                        BrandName = context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                        ImageName = x.ImageName != null ? x.ImageName : configuration.GetSection("AWSBucket").Value + context.ProductImages.FirstOrDefault(y => y.IsActive == true && y.ProductEntityId == x.Id).ImageName,
                                        ProductLink = x.ProductLink,
                                        ProductDetails = x.ProductDetails,
                                        ProductName = x.ProductName,
                                        ProductType = productType.ProductName,
                                        HairChallenge = context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                        {
                                            Description = p.HairChallenges.Description,
                                            HairChallengeId = p.HairChallenges.HairChallengeId
                                        }).ToList(),
                                        HairGoals = context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == x.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                        {
                                            Description = p.HairGoal.Description,
                                            HairGoalId = p.HairGoal.HairGoalId
                                        }).ToList()
                                    }).ToList();
                                    productsTypesModels.Add(productsTypesModel);
                                }
                            }
                            RecommendedProductsStylingModel productsTypes = new RecommendedProductsStylingModel();
                            productsTypes.ProductParentName = parentProduct.CategoryName;
                            productsTypes.ProductId = parentProduct.Id;
                            productsTypes.ProductsTypes = productsTypesModels;
                            productsTypesStylingList.Add(productsTypes);
                        }

                    }

                    profile.RecommendedProductsStyling = productsTypesStylingList;
                    // End  Styling Regimens Code after multiple Product Type functionality merge recommended products
                    profile.HairStyle = (from sr in context.StyleRecipeHairStyle
                                         join hr in context.HairStyles
                                         on sr.HairStyleId equals hr.Id
                                         where sr.HairProfileId == hairId
                                         select hr.Style
                                       ).FirstOrDefault();
                    QuestionaireSelectedAnswer additionalHairInfo = new QuestionaireSelectedAnswer();
                    AdditionalHairInfo hairInfo = context.AdditionalHairInfo.Where(x => x.HairId == hairId).FirstOrDefault();
                    if (hairInfo != null)
                    {
                        additionalHairInfo.TypeId = hairInfo.TypeId;
                        additionalHairInfo.TypeDescription = hairInfo.TypeDescription;
                        additionalHairInfo.TextureId = hairInfo.TextureId;
                        additionalHairInfo.TextureDescription = hairInfo.TextureDescription;
                        additionalHairInfo.PorosityId = hairInfo.PorosityId;
                        additionalHairInfo.PorosityDescription = hairInfo.PorosityDescription;
                        additionalHairInfo.HealthId = hairInfo.HealthId;
                        additionalHairInfo.HealthDescription = hairInfo.HealthDescription;
                        additionalHairInfo.DensityId = hairInfo.DensityId;
                        additionalHairInfo.DensityDescription = hairInfo.DensityDescription;
                        additionalHairInfo.ElasticityId = hairInfo.ElasticityId;
                        additionalHairInfo.ElasticityDescription = hairInfo.ElasticityDescription;
                        additionalHairInfo.Goals = context.CustomerHairGoals.Where(x => x.HairInfoId == hairInfo.Id).Select(y => y.Description).ToList();
                        additionalHairInfo.Challenges = context.CustomerHairChallenge.Where(x => x.HairInfoId == hairInfo.Id).Select(y => y.Description).ToList();
                        profile.SelectedAnswers = additionalHairInfo;
                    }
                    if (!string.IsNullOrEmpty(hairProfileModel.LoginUserId))
                    {
                        profile.MyNotes = context.StylistNotesHHCPs.FirstOrDefault(x => x.HairProfileId == hairId && x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId))?.Notes;
                    }
                    if (!string.IsNullOrEmpty(hairProfileModel.LoginUserId))
                    {
                        var salonNotes = context.SalonNotesHHCP.Include(x => x.WebLogin).Where(x => x.HairProfileId == hairId);
                        profile.SalonNotes = salonNotes.FirstOrDefault(x => x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId))?.Notes;

                        var userDetail = context.WebLogins.FirstOrDefault(x => x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId));

                        TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("US/Eastern");

                        if (userDetail?.UserTypeId == (int)UserTypeEnum.B2B && salonNotes != null)
                        {
                            var salonId = context.SalonsOwners.FirstOrDefault(x => x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId) && x.IsActive == true)?.SalonId;
                            if (salonId > 0)
                            {
                                var salonDet = context.Salons.FirstOrDefault(x => x.SalonId == salonId);
                                if (salonDet.IsPublicNotes == true)
                                {
                                    profile.SalonNotesModel = salonNotes.OrderByDescending(x => x.CreatedOn).Select(x => new SalonNotesModel
                                    {
                                        SalonNotes = x.Notes,
                                        HairProfileId = x.HairProfileId,
                                        LoginUserId = x.UserId.ToString(),
                                        CreatedBy = x.WebLogin.UserEmail,
                                        CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(x.CreatedOn, easternZone),
                                    }).ToList();
                                    profile.IsPublicSalonNote = true;
                                }
                                else
                                {
                                    profile.SalonNotesModel = salonNotes.OrderByDescending(x => x.CreatedOn).Where(x => x.UserId == Convert.ToInt32(hairProfileModel.LoginUserId)).Select(x => new SalonNotesModel
                                    {
                                        SalonNotes = x.Notes,
                                        HairProfileId = x.HairProfileId,
                                        LoginUserId = x.UserId.ToString(),
                                        CreatedBy = x.WebLogin.UserEmail,
                                        CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(x.CreatedOn, easternZone),
                                    }).ToList();
                                }
                            }
                        }
                        else
                        {
                            profile.SalonNotesModel = salonNotes.OrderByDescending(x => x.CreatedOn).Select(x => new SalonNotesModel
                            {
                                SalonNotes = x.Notes,
                                HairProfileId = x.HairProfileId,
                                LoginUserId = x.UserId.ToString(),
                                CreatedBy = x.WebLogin.UserEmail,
                                CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(x.CreatedOn, easternZone),
                            }).ToList();
                            profile.IsPublicSalonNote = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Method: GetHairProfileCustomer, HairProfileId:" + hairId + ", Error: " + ex.Message, ex);
                }
            }
            if (profile != null)
            {
                int? latestQA = context.Questionaires.Where(x => x.UserId == hairProfileModel.UserId && x.IsActive == true).OrderByDescending(x => x.QA).FirstOrDefault()?.QA;

                //int latestQA = context.Questionaires.Where(x => x.UserId == hairProfileModel.UserId && x.IsActive == true).Max(x => x.QA);
                string uploadedImage = context.Questionaires.Where(x => x.UserId == hairProfileModel.UserId && x.QuestionId == 22 && x.QA == latestQA).Select(x => x.DescriptiveAnswer).LastOrDefault();
                profile.UserUploadedImage = uploadedImage;
            }
            return profile;
        }

        public List<HairStrandUploadNotificationModel> GetHairStrandUploadNotificationList()
        {
            try
            {


                var tempList = (from notification in context.HairStrandUploadNotification.Where(x => x.IsRead == false)
                                .Include(x => x.Salon)
                                .Include(x => x.HairProfile)
                                select new HairStrandUploadNotificationModel
                                {
                                    Id = notification.Id,
                                    SalonName = notification.Salon.SalonName,
                                    IsRead = notification.IsRead,
                                    UserName = notification.HairProfile.UserId,
                                    CreatedOn = notification.CreatedOn,
                                    HairProfileId = notification.HairProfileId,
                                    UserId = context.UserEntity.FirstOrDefault(x => x.Email == notification.HairProfile.UserId).Id.ToString()
                                }).OrderByDescending(x => x.CreatedOn).ToList();
                return tempList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairStrandUploadNotificationList, Error: " + ex.Message, ex);
                return null;
            }
        }
        public bool UpdateNotificationAsRead(HairStrandUploadNotificationModel notification)
        {
            var strandNotification = context.HairStrandUploadNotification.Where(y => y.Id == notification.Id).FirstOrDefault();
            try
            {
                if (strandNotification != null)
                {
                    strandNotification.IsRead = true;
                    context.SaveChanges();
                }
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError("Method: UpdateNotificationAsRead, NotificationId:" + notification.Id + ", Error: " + ex.Message, ex);
                return false;
            }
        }

        public CustomerAIResult GetLatestCustomerAIResult(Guid UserID)
        {
            try
            {
                var lstAIResults = context.CustomerAIResults.Where(c => c.UserId == UserID).ToList().OrderByDescending(c => c.CreatedOn).FirstOrDefault();
                return lstAIResults;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetLatestCustomerAIResult, UserId:" + UserID.ToString() + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public CustomerHairAIModel GetLatestCustomerAIResultAdmin(Guid UserId)
        {
            try
            {
                var user = _userManager.FindByIdAsync(UserId.ToString()).GetAwaiter().GetResult();
                if (user == null)
                {
                    return null;
                }
                var result = context.CustomerAIResults
                    .Where(c => c.UserId == UserId)
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(x => new CustomerHairAIModel
                    {
                        UserId = x.UserId,
                        AIResult = x.AIResult,
                        HairProfileId = x.HairProfileId,
                        CustomerAIResultId = x.CustomerAIResultId,
                        IsActive = x.IsActive,
                        CreatedOn = x.CreatedOn,
                        IsVersion2 = x.IsVersion2,
                        UserName = user.FirstName + " " + user.LastName,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    })
                    .FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetLatestCustomerAIResultAdmin, UserId:" + UserId.ToString() + ", Error: " + ex.Message, ex);
                return null;
            }
        }



        public List<DailyRoutineTrackerNotificationModel> GetHairDiarySubmitNotificationList()
        {
            try
            {
                var hairDiaryNotification = (from notification in context.DailyRoutineTracker
                                             where (notification.IsRead ?? true) == false
                                             select new DailyRoutineTrackerNotificationModel
                                             {
                                                 Id = notification.Id,
                                                 UserId = notification.UserId,
                                                 Description = notification.Description,
                                                 HairStyle = notification.HairStyle,
                                                 TrackTime = notification.TrackTime,
                                                 IsActive = notification.IsActive,
                                                 CurrentMood = notification.CurrentMood,
                                                 GuidanceNeeded = notification.GuidanceNeeded,
                                                 IsRead = notification.IsRead,
                                             }).OrderByDescending(x => x.CreatedOn).ToList();
                return hairDiaryNotification;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairDiarySubmitNotificationList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool UpdateNotificationHairDiaryAsRead(DailyRoutineTrackerNotificationModel notification)
        {
            var strandNotification = context.DailyRoutineTracker.Where(y => y.Id == notification.Id).FirstOrDefault();
            try
            {
                if (strandNotification != null)
                {
                    strandNotification.IsRead = true;
                    context.SaveChanges();
                }
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError("Method: UpdateNotificationHairDiaryAsRead, NotificationId:" + notification.Id + ", Error: " + ex.Message, ex);
                return false;
            }
        }
        public Models.ViewModels.StylistNotesHHCPModel SaveHairStylistNotes(Models.ViewModels.StylistNotesHHCPModel StylistNotesHHCPModel)
        {
            try
            {
                if (StylistNotesHHCPModel.HairProfileId != 0)
                {
                    var stylistNotes = context.StylistNotesHHCPs.FirstOrDefault(x => x.UserId == Convert.ToInt16(StylistNotesHHCPModel.UserId) && x.HairProfileId == StylistNotesHHCPModel.HairProfileId);
                    if (stylistNotes != null)
                    {
                        stylistNotes.Notes = StylistNotesHHCPModel.Notes;
                        context.StylistNotesHHCPs.Update(stylistNotes);
                    }
                    else
                    {
                        var obj = new StylistNotesHHCP();
                        obj.Notes = StylistNotesHHCPModel.Notes;
                        obj.HairProfileId = StylistNotesHHCPModel.HairProfileId;
                        obj.UserId = StylistNotesHHCPModel.UserId;
                        obj.CreatedOn = DateTime.Now;
                        context.StylistNotesHHCPs.Add(obj);
                    }
                }
                else
                {
                    return SaveSurveyQuestionaire_HairStylistNotes(StylistNotesHHCPModel);
                }
                context.SaveChanges();
                return StylistNotesHHCPModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveHairStylistNotes, Error: " + ex.Message, ex);
                return null;
            }
        }

        private StylistNotesHHCPModel SaveSurveyQuestionaire_HairStylistNotes(Models.ViewModels.StylistNotesHHCPModel StylistNotesHHCPM)
        {
            MyAvana.Models.Entities.Questionaire questionaire = new MyAvana.Models.Entities.Questionaire();
            questionaire.UserId = StylistNotesHHCPM.CustomerID;
            //Questionaire questionaire;
            List<Questionaire> objQuestionaire = new List<Questionaire>();
            int? latestQA = context.Questionaires.Where(x => x.UserId == questionaire.UserId && x.IsActive == true).OrderByDescending(x => x.QA).FirstOrDefault()?.QA;
            //var imageQuestionaire = _context.Questionaires.Where(x => x.QuestionId == 22 && x.UserId == questionaire.UserId && x.IsActive == true && x.QA == latestQA).LastOrDefault();
            try
            {
                Questionaire questionaire1 = new Questionaire();
                //questionaire1.DescriptiveAnswer = questionaire.DescriptiveAnswer;
                questionaire1.CreatedOn = DateTime.Now;
                questionaire1.IsActive = true;
                questionaire1.QuestionId = 22;
                questionaire1.UserId = questionaire.UserId;
                questionaire1.QA = latestQA ?? 0;
                context.Add(questionaire1);
                int HairProfileID = StylistNotesHHCPM.HairProfileId;
                var entity = _userManager.FindByIdAsync(questionaire.UserId).GetAwaiter().GetResult();
                if (entity != null)
                {
                    var hhcp = context.HairProfiles.FirstOrDefault(x => x.UserId == entity.Email && x.IsActive == true);
                    var obj = new StylistNotesHHCP();
                    if (hhcp == null)
                    {
                        UserEntity us = context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();
                        var hair = new Models.Entities.HairProfile();
                        hair.UserId = us.Email;
                        if (us.CustomerTypeId == (int)CustomerTypeEnum.DigitalAnalysis)
                        {
                            hair.HealthSummary = "Hi " + us.FirstName + " ! " + "\r\nYour Digital Hair Profile includes your introductory product recommendations based on a quick analysis of your hair. We incorporated your Hair Goals, Hair Challenges, Product Recommendations, and % breakdown of your Unique Hair Type Combination. To get a comprehensive healthy hair care plan, make sure to get your hair analysis kit in the menu to your left. This should get you started in the meantime! If you have any questions, please email us at support@myavana.com\r\nLove, \r\nMYAVANA ";
                        }
                        else
                        {
                            hair.HealthSummary = "Hi " + us.FirstName + " ! " + "\r\nYour Healthy Hair Care Plan includes your Hair ID, Hair Goals and Challenges, Hair Strand Analysis, Product Recommendations, Ingredients, Regimens, and Education. We have also included some personal notes from your MYAVANA lab analyst and hair consultant.\r\nLove, \r\nTanisha \r\nHair Analyst";
                        }
                        hair.AttachedQA = latestQA;
                        hair.IsActive = true;
                        hair.CreatedOn = DateTime.Now;
                        hair.IsDrafted = false;
                        hair.IsViewEnabled = true;
                        context.Add(hair);
                        context.SaveChanges();
                        HairProfileID = hair.Id;
                    }
                    else
                    {
                        HairProfileID = hhcp.Id;
                    }
                    obj.Notes = StylistNotesHHCPM.Notes;
                    obj.HairProfileId = HairProfileID;
                    obj.UserId = StylistNotesHHCPM.UserId;
                    obj.CreatedOn = DateTime.Now;
                    context.StylistNotesHHCPs.Add(obj);
                    context.SaveChanges();
                    return StylistNotesHHCPM;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveSurveyQuestionaire, UserId:" + questionaire.UserId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public DigitalAssessmentModelParameters CreateHHCPUsingScalpAnalysis(DigitalAssessmentModelParameters digitalAssessmentModelParam)
        {
            try
            {
                //DigitalAssessmentModel ret = new DigitalAssessmentModel();
                var entity = _userManager.FindByIdAsync(digitalAssessmentModelParam.Userid).GetAwaiter().GetResult();
                if (entity != null)
                {
                    var latestQAForHHCP = context.Questionaires.Where(x => x.UserId == digitalAssessmentModelParam.Userid && x.IsActive == true).Max(x => x.QA);
                    UserEntity us = context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();
                    Models.Entities.HairProfile hairProfile = new Models.Entities.HairProfile();
                    hairProfile.UserId = us.Email;
                    hairProfile.AttachedQA = latestQAForHHCP;
                    //hairProfile.HairType = digitalAssessmentModel.HairType;
                    hairProfile.HealthSummary = "Hi " + us.FirstName + " ! " + "\r\nYour Digital Hair Profile includes your introductory product recommendations based on a quick analysis of your hair. We incorporated your Hair Goals, Hair Challenges, Product Recommendations, and % breakdown of your Unique Hair Type Combination. To get a comprehensive healthy hair care plan, make sure to get your hair analysis kit in the menu to your left. This should get you started in the meantime! If you have any questions, please email us at support@myavana.com\r\nLove, \r\nMYAVANA ";
                    int res = CreateHairProfile(hairProfile);
                    if (res > 0)
                    {
                        digitalAssessmentModelParam.HairProfileId = res;
                        var Customer_AI_Result = context.CustomerAIResults.Where(x => x.UserId == us.Id).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                        Customer_AI_Result.HairProfileId = res;
                        context.SaveChanges();


                        if (digitalAssessmentModelParam.IsIPad == true)
                        {   //use scalp analysis
                            var HairScope = context.HairScope.Where(x => x.UserId == us.Id).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                            HairScope.HairProfileId = res;
                            int? latestQA = context.Questionaires.Where(x => x.UserId == us.Id.ToString() && x.IsActive == true).OrderByDescending(x => x.QA).FirstOrDefault()?.QA;
                            HairScope.QAVersion = latestQA;
                            context.SaveChanges();
                            var recPRods = UpdateRecommendedProductsSPForMobile(entity.HairType, res, digitalAssessmentModelParam.Userid);
                        }
                        else
                        {
                            var recPRods = UpdateRecommendedProductsSP(entity.HairType, res, digitalAssessmentModelParam.Userid,null);
                        }
                        AdditionalHairInfo additionalHairInfo = new AdditionalHairInfo();
                        additionalHairInfo.HairId = res;
                        context.Add(additionalHairInfo);
                        context.SaveChanges();
                        var selectedGoals = context.Questionaires.Where(x => x.UserId == entity.Id.ToString() && x.QuestionId == 25).Select(x => x.AnswerId).ToList();
                        List<CustomerHairGoals> customerHairGoals = new List<CustomerHairGoals>();
                        foreach (var hairGoal in selectedGoals)
                        {

                            string description = context.Answers.Where(x => x.AnswerId == hairGoal).Select(x => x.Description).FirstOrDefault();
                            CustomerHairGoals customerHairGoal = new CustomerHairGoals();
                            customerHairGoal.HairInfoId = additionalHairInfo.Id;
                            customerHairGoal.Description = description;
                            customerHairGoal.CreatedOn = DateTime.Now;
                            customerHairGoal.IsActive = true;
                            customerHairGoals.Add(customerHairGoal);

                        }
                        context.AddRange(customerHairGoals);
                        var selectedChallenges = context.Questionaires.Where(x => x.UserId == entity.Id.ToString() && x.QuestionId == 16).Select(x => x.AnswerId).ToList();
                        List<CustomerHairChallenge> customerHairChallenges = new List<CustomerHairChallenge>();
                        foreach (var challenge in selectedChallenges)
                        {
                            string description = context.Answers.Where(x => x.AnswerId == challenge).Select(x => x.Description).FirstOrDefault();
                            CustomerHairChallenge customerHairChallenge = new CustomerHairChallenge();
                            customerHairChallenge.HairInfoId = additionalHairInfo.Id;
                            customerHairChallenge.Description = description;
                            customerHairChallenge.CreatedOn = DateTime.Now;
                            customerHairChallenge.IsActive = true;
                            customerHairChallenges.Add(customerHairChallenge);

                        }
                        context.AddRange(customerHairChallenges);
                        context.SaveChanges();
                    }
                    //}


                }

                return digitalAssessmentModelParam;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateHHCPUsingScalpAnalysis, UserId:" + digitalAssessmentModelParam.Userid + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }
        public QuestionaireModelParameters IsQuestionaireExist(QuestionaireModelParameters questionaire)
        {
            try
            {
                var entity = _userManager.FindByIdAsync(questionaire.Userid).GetAwaiter().GetResult();
                var objCode = context.Questionaires.FirstOrDefault(x => x.UserId == questionaire.Userid && x.IsActive == true);
                var maxQA = context.Questionaires.Where(x => x.UserId == questionaire.Userid && x.IsActive == true).Max(x => (int?)x.QA) ?? 0;
                if (objCode != null)
                {
                    questionaire.IsExist = true;
                    questionaire.QuestionAnswerCount = context.Questionaires.Where(x => x.UserId == questionaire.Userid && x.IsActive == true && x.QA == maxQA).Select(x => x.QuestionId).Distinct().Count();
                }
                else
                {
                    questionaire.IsExist = false;
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: IsQuestionaireExist, UserId:" + questionaire.Userid + ", Error: " + Ex.Message, Ex);
            }
            return questionaire;
        }
        private string GetBase64FromImageUrl(string imageUrl)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(imageUrl).Result;
                response.EnsureSuccessStatusCode();

                var imageBytes = response.Content.ReadAsByteArrayAsync().Result;

                // Convert the byte array to a base64 string
                return Convert.ToBase64String(imageBytes);
            }
        }
        public Models.Entities.HairProfile AutoGenerateHHCP(Models.ViewModels.HairProfile hairProfile)
        {
            try
            {
                hairProfile.QA = hairProfile.QA - 1;
                using (var client = new HttpClient())
                {
                    var imageQuestionaire = context.Questionaires
                        .Where(x => x.QuestionId == 22 && x.UserId == hairProfile.UserId && x.IsActive == true && x.QA == hairProfile.QA)
                        .LastOrDefault();

                    if (imageQuestionaire != null)
                    {
                        string imageUrl = imageQuestionaire.DescriptiveAnswer;
                        //if (imageUrl.Contains("admin.myavana.com"))
                        //{

                        //    imageUrl = imageUrl.Replace("admin.myavana.com", "adminstaging.myavana.com");
                        //}
                        //else if (imageUrl.Contains("customer.myavana.com"))
                        //{
                        //    // Replace the domain for customer
                        //    imageUrl = imageUrl.Replace("customer.myavana.com", "customerstaging.myavana.com");
                        //}

                        // Convert the updated URL to base64 image data
                        string base64ImageData = GetBase64FromImageUrl(imageUrl);


                        byte[] imageBytes = Convert.FromBase64String(base64ImageData);
                        using (var imageStream = new System.IO.MemoryStream(imageBytes))
                        {
                            var formData = new MultipartFormDataContent();
                            var imageContent = new StreamContent(imageStream);
                            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                            formData.Add(imageContent, "imageFile", "image.png");

                            // Set the API URL and key
                            var apiUrl = "https://analysisai01.myavana.com/api/v2/Image/classifyimage";
                            client.DefaultRequestHeaders.Add("x-api-key", "1ebf78f8-e63a-4753-879d-f0d9bd7701a7");

                            var response = client.PostAsync(apiUrl, formData).Result;
                            response.EnsureSuccessStatusCode();
                            var responseString = response.Content.ReadAsStringAsync().Result;
                            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);
                            string hairTypeLabel = responseObject.hairTypeLabel;

                            var digitalAssessmentModel = new DigitalAssessmentModel
                            {
                                Userid = hairProfile.UserId,
                                HairType = hairTypeLabel,
                                AIResult = JsonConvert.SerializeObject(responseString).ToString(),
                                QA = hairProfile.QA
                            };

                            // Create Hair Profile
                            Models.Entities.HairProfile hairProfile1 = CreateHHCPFromAdminPortal(digitalAssessmentModel);
                            return hairProfile1;
                        }
                    }
                    else
                    {
                        _logger.LogError("Method: AutoGenerateHHCP, UserId:" + hairProfile.UserId + ", Error:No image found in questionaire ");
                        return null;
                    }
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: AutoGenerateHHCP, UserId:" + hairProfile.UserId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }
        public Models.Entities.HairProfile CreateHHCPFromAdminPortal(DigitalAssessmentModel digitalAssessmentModel)
        {
            try
            {
                DigitalAssessmentModel ret = new DigitalAssessmentModel();
                Models.Entities.HairProfile hairProfile = new Models.Entities.HairProfile();
                var entity = _userManager.FindByIdAsync(digitalAssessmentModel.Userid).GetAwaiter().GetResult();
                if (entity != null)
                {
                    UserEntity us = context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();


                    hairProfile.UserId = us.Email;
                    hairProfile.HairType = digitalAssessmentModel.HairType;
                    hairProfile.AttachedQA = digitalAssessmentModel.QA;
                    hairProfile.HealthSummary = "Hi " + us.FirstName + " ! " + "\r\nYour Healthy Hair Care Plan includes your Hair ID, Hair Goals and Challenges, Hair Strand Analysis, Product Recommendations, Ingredients, Regimens, and Education. We have also included some personal notes from your MYAVANA lab analyst and hair consultant.\r\nLove, \r\nTanisha \r\nHair Analyst";
                    hairProfile.AttachedQA = digitalAssessmentModel.QA;
                    //hairProfile.HealthSummary = "Hi " + us.FirstName + " ! " + "\r\nYour Digital Hair Profile includes your introductory product recommendations based on a quick analysis of your hair. We incorporated your Hair Goals, Hair Challenges, Product Recommendations, and % breakdown of your Unique Hair Type Combination. To get a comprehensive healthy hair care plan, make sure to get your hair analysis kit in the menu to your left. This should get you started in the meantime! If you have any questions, please email us at support@myavana.com\r\nLove, \r\nMYAVANA ";
                    int res = CreateHairProfileHairKit(hairProfile);
                    if (res > 0)
                    {
                        CustomerAIResult objCustomerAIResult = new CustomerAIResult();
                        objCustomerAIResult.UserId = entity.Id;
                        objCustomerAIResult.AIResult = digitalAssessmentModel.AIResult;
                        objCustomerAIResult.IsActive = true;
                        objCustomerAIResult.CreatedOn = DateTime.Now;
                        objCustomerAIResult.HairProfileId = res;
                        objCustomerAIResult.IsVersion2 = true;
                        context.Add(objCustomerAIResult);

                        context.SaveChanges();

                        var recPRods = UpdateRecommendedProductsSP(digitalAssessmentModel.HairType, res, digitalAssessmentModel.Userid,digitalAssessmentModel.QA);
                        AdditionalHairInfo additionalHairInfo = new AdditionalHairInfo();
                        additionalHairInfo.HairId = res;
                        context.Add(additionalHairInfo);
                        context.SaveChanges();
                        var selectedGoals = context.Questionaires.Where(x => x.UserId == entity.Id.ToString() && x.QuestionId == 25 && x.QA == digitalAssessmentModel.QA).Select(x => x.AnswerId).ToList();
                        List<CustomerHairGoals> customerHairGoals = new List<CustomerHairGoals>();
                        foreach (var hairGoal in selectedGoals)
                        {

                            string description = context.Answers.Where(x => x.AnswerId == hairGoal).Select(x => x.Description).FirstOrDefault();
                            CustomerHairGoals customerHairGoal = new CustomerHairGoals();
                            customerHairGoal.HairInfoId = additionalHairInfo.Id;
                            customerHairGoal.Description = description;
                            customerHairGoal.CreatedOn = DateTime.Now;
                            customerHairGoal.IsActive = true;
                            customerHairGoals.Add(customerHairGoal);

                        }
                        context.AddRange(customerHairGoals);

                        var selectedChallenges = context.Questionaires.Where(x => x.UserId == entity.Id.ToString() && x.QuestionId == 16 && x.QA == digitalAssessmentModel.QA).Select(x => x.AnswerId).ToList();
                        List<CustomerHairChallenge> customerHairChallenges = new List<CustomerHairChallenge>();
                        foreach (var challenge in selectedChallenges)
                        {
                            string description = context.Answers.Where(x => x.AnswerId == challenge).Select(x => x.Description).FirstOrDefault();
                            CustomerHairChallenge customerHairChallenge = new CustomerHairChallenge();
                            customerHairChallenge.HairInfoId = additionalHairInfo.Id;
                            customerHairChallenge.Description = description;
                            customerHairChallenge.CreatedOn = DateTime.Now;
                            customerHairChallenge.IsActive = true;
                            customerHairChallenges.Add(customerHairChallenge);

                        }
                        context.AddRange(customerHairChallenges);
                        context.SaveChanges();
                    }

                }
                hairProfile.CreatedBy = 1;
                return hairProfile;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateHHCPFromAdminPortal, UserId:" + digitalAssessmentModel.Userid + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }
        public string ShareEmailExist(string email, int hairProfileId,Guid sharedBy)
        {
            try
            {
                HairProfileCustomerModel profile = new HairProfileCustomerModel();
                var SharedWithUser = context.Users.Where(a => a.Email == email).FirstOrDefault();
                
                if (SharedWithUser != null)
                {
                    var SharedByUser = context.Users.Where(a => a.Id == sharedBy).FirstOrDefault();
                    EmailInformation emailInformation = new EmailInformation
                    {
                        
                        Email = SharedWithUser.Email,
                        Name = SharedByUser.FirstName + " " + SharedByUser.LastName
                    };
                    SharedHHCP sharedHHCP = new SharedHHCP();
                    sharedHHCP.HairProfileId = hairProfileId;
                    sharedHHCP.IsRevoked = false;
                    sharedHHCP.SharedOn = DateTime.Now;
                    sharedHHCP.SharedWith = SharedWithUser.Id;
                    sharedHHCP.SharedBy = sharedBy;
                    context.SharedHHCP.Add(sharedHHCP);
                    context.SaveChanges();
                    _emailService.SendEmail("SHAREHHCP", emailInformation);
                    return "1";
                }
                else
                {
                    return "This entered user does not exist";
                }
            }
            catch (Exception Ex)
            {
               _logger.LogError("Method: ShareEmailExist, email:" + email + ", hairProfileId: " + hairProfileId + ", sharedBy: " + sharedBy + ", Error: " + Ex.Message, Ex);
                return "0";
            }
            return "0";
        }

        public List<SharedHHCPModel> GetSharedHHCPList(Guid userId)
        {
            try
            {
                
                    var hhcpList = (from sh in context.SharedHHCP
                                    join hr in context.HairProfiles on 
                                    sh.HairProfileId equals hr.Id
                                    join usr in context.Users
                                    on sh.SharedWith equals usr.Id
                                    where sh.SharedBy.ToString().ToLower() == userId.ToString().ToLower()
                                   // && sh.IsRevoked==false
                                    select new SharedHHCPModel
                                    {
                                        Id=sh.Id,
                                        HairProfileId = hr.Id,
                                        SharedWith=sh.SharedWith,
                                        SharedWithUser= usr.FirstName + " " + usr.LastName,
                                        HHCPName = hr.IsBasicHHCP == true ? "Hair AI Results - " + hr.CreatedOn.ToString("MMM dd,yyyy | HH:mm") : "Hair SI Results - " + hr.CreatedOn.ToString("MMM dd,yyyy HH:mm"),
                                        SharedOn= sh.SharedOn.ToString("MMM dd,yyyy | HH:mm"),
                                        RevokedOn = sh.RevokedOn.ToString(),
                                        IsRevoked =sh.IsRevoked,
                                        UserEmail = context.Users.Where(a => a.Id == sh.SharedBy).Select(a => a.Email).FirstOrDefault()
                                    }).OrderByDescending(x => x.SharedOn).ToList();
                    return hhcpList;
                

            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetSharedHHCPList, UserId:" + userId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }
        public bool RevokeAccess(SharedHHCPModel sharedHHCP)
        {
            try
            {
                var objSharedHHCP = context.SharedHHCP.FirstOrDefault(x => x.Id == sharedHHCP.Id && x.SharedWith==sharedHHCP.SharedWith);
                {
                    if (objSharedHHCP != null)
                    {
                        objSharedHHCP.IsRevoked = true;
                        objSharedHHCP.RevokedOn = DateTime.Now;
                    }
                }
                context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: RevokeAccess, SharedHHCPId:" + sharedHHCP.Id + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public List<SharedHHCPModel> GetSharedWithMeHHCPList(Guid userId)
        {
            try
            {

                var hhcpList = (from sh in context.SharedHHCP
                                join hr in context.HairProfiles on
                                sh.HairProfileId equals hr.Id
                                join usr in context.Users
                                on sh.SharedBy equals usr.Id
                                where sh.SharedWith.ToString().ToLower() == userId.ToString().ToLower()
                                && sh.IsRevoked == false
                                select new SharedHHCPModel
                                {
                                    Id = sh.Id,
                                    HairProfileId = hr.Id,
                                    SharedBy = sh.SharedBy,
                                    SharedByUser = usr.FirstName + " " + usr.LastName,
                                    HHCPName = hr.IsBasicHHCP == true ? "Hair AI Results - " + hr.CreatedOn.ToString("MMM dd,yyyy | HH:mm") : "Hair SI Results - " + hr.CreatedOn.ToString("MMM dd,yyyy HH:mm"),
                                    SharedOn = sh.SharedOn.ToString("MMM dd,yyyy | HH:mm"),
                                    IsRevoked = sh.IsRevoked
                                }).OrderByDescending(x => x.SharedOn).ToList();
                return hhcpList;


            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetSharedWithMeHHCPList, UserId:" + userId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public bool AutoRecommendVideos(int? QA, int hairProfileId, string userId)
        {
            try
            {
                var dp_params = new DynamicParameters();
                dp_params.Add("UserID", userId, DbType.String);
                dp_params.Add("HairProfileId", hairProfileId, DbType.Int32);
                dp_params.Add("QA", QA, DbType.Int32);
                using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    connection.ExecuteAsync("AutoRecommendVideos", dp_params, commandType: CommandType.StoredProcedure).GetAwaiter().GetResult();
                }


                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: AutoRecommendVideos, UserId:" + userId + ", HairProfileId:" + hairProfileId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

    }
}
