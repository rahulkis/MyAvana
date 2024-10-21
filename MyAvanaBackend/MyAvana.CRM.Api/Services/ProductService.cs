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
    public class ProductService : IProductsService
    {
        private readonly AvanaContext _context;
        private readonly IConfiguration configuration;
        private readonly Logger.Contract.ILogger _logger;

        public ProductService(AvanaContext avanaContext, IConfiguration configuration, Logger.Contract.ILogger logger)
        {
            _context = avanaContext;
            _logger = logger;
            this.configuration = configuration;
        }

        public bool DeleteProduct(ProductEntity productEntity)
        {
            try
            {
                var objProduct = _context.ProductEntities.FirstOrDefault(x => x.guid == productEntity.guid);
                {
                    if (objProduct != null)
                    {
                        objProduct.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteProduct, ProductId:" + productEntity.guid + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public ProductEntity ShowHideProduct(ProductEntity productEntity)
        {

            var objProduct = _context.ProductEntities.Where(x => x.guid == productEntity.guid).FirstOrDefault();

            try
            {
                if (objProduct != null)
                {
                    if (objProduct.HideInSearch == true)
                    {
                        objProduct.HideInSearch = false;
                    }
                    else
                    {
                        objProduct.HideInSearch = true;
                    }
                }
                _context.Update(objProduct);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: ShowHideProduct, ProductId:" + productEntity.guid + ", Error: " + ex.Message, ex);
            }

            return objProduct;
        }
        public ProductEntityEditModel GetProductById(ProductEntityEditModel productEntity)
        {
            try
            {
                ProductEntityEditModel product = (from prd in _context.ProductEntities
                                                  where prd.guid == productEntity.guid
                                                  && prd.IsActive == true
                                                  select new ProductEntityEditModel()
                                                  {
                                                      guid = prd.guid,
                                                      ProductTypesId = prd.ProductTypesId,
                                                      ProductName = prd.ProductName,
                                                      ActualName = prd.ActualName,
                                                      BrandName = _context.Brands.Where(y => y.BrandId == prd.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                                      ImageName = prd.ImageName,
                                                      Ingredients = prd.Ingredients,
                                                      ProductDetails = prd.ProductDetails,
                                                      ProductLink = prd.ProductLink,
                                                      UPCCode = prd.UPCCode,
                                                      BrandId=prd.BrandId,
                                                      ActualPrice = prd.Price.ToString().Substring(0, prd.Price.ToString().IndexOf('.')),
                                                      DecimalPrice = prd.Price.ToString().Substring(prd.Price.ToString().IndexOf('.') + 1),
                                                      IsActive = prd.IsActive,
                                                      CreatedOn = prd.CreatedOn,
                                                      productCommons = _context.ProductCommons.Where(x => x.ProductEntityId == prd.Id && x.IsActive == true).ToList(),
                                                      productImages = _context.ProductImages.Where(x=>x.ProductEntityId==prd.Id && x.IsActive ==true).ToList()

                                                  }).FirstOrDefault();

                return product;
                //ProductEntity productEntityModel = _context.ProductEntities.Where(x => x.guid == productEntity.guid).FirstOrDefault();
                //return productEntityModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductById, ProductId:" + productEntity.guid + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>


        //public List<ProductsModelList> GetProducts()
        //{
        //    try
        //    {
        //        List<ProductsModelList> lstProductModel = _context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
        //            Select(x => new ProductsModelList
        //            {
        //                Id = x.Id,
        //                guid = x.guid,
        //                ProductName = x.ProductName,
        //                ActualName = x.ActualName,
        //                BrandName = x.BrandName,
        //                HairType = _context.ProductCommons.Include(h => h.HairType).Where(p => p.ProductEntityId == x.Id && p.HairTypeId != null).Select(d => new HairType
        //                {
        //                    Description = d.HairType.Description
        //                }).ToList(),
        //                ImageName = x.ImageName,
        //                Ingredients = x.Ingredients,
        //                ProductDetails = x.ProductDetails,
        //                ProductLink = x.ProductLink,
        //                IsActive = x.IsActive,
        //                CreatedOn = x.CreatedOn,
        //                ProductTypeId = x.ProductTypesId,
        //                ProductType = x.ProductTypes.ProductName,
        //                ParentId = x.ProductTypes.ParentId,
        //                ParentName = _context.ProductTypes.Where(y => y.Id == x.ProductTypes.ParentId).Select(y => y.ProductName).FirstOrDefault(),
        //                Price = x.Price,
        //                HairChallenge = _context.ProductCommons.Include(h => h.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null).Select(d => new HairChallenges
        //                {
        //                    Description = d.HairChallenges.Description
        //                }).ToList(),
        //                ProductIndicate = _context.ProductCommons.Include(h => h.ProductIndicator).Where(p => p.ProductEntityId == x.Id && p.ProductIndicatorId != null).Select(d => new ProductIndicator
        //                {
        //                    Description = d.ProductIndicator.Description
        //                }).ToList(),
        //                ProductTag = _context.ProductCommons.Include(h => h.ProductTags).Where(p => p.ProductEntityId == x.Id && p.ProductTagsId != null).Select(d => new ProductTags
        //                {
        //                    Description = d.ProductTags.Description
        //                }).ToList(),
        //                ProductClassificatio = _context.ProductCommons.Include(h => h.ProductClassification).Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null).Select(d => new ProductClassification
        //                {
        //                    Description = d.ProductClassification.Description
        //                }).ToList()
        //            }).ToList();

        //        return lstProductModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        //public List<ProductsModelList> GetProducts()
        //{
        //    try
        //    {
        //        var productCommons = _context.ProductCommons.Include(x => x.HairChallenges).Include(y => y.ProductIndicator).Include(z => z.HairType).
        //            Include(h => h.ProductTags).Include(h => h.ProductClassification).ToList();
        //        List<ProductsModelList> lstProductModel = _context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
        //            Select(x => new ProductsModelList
        //            {
        //                Id = x.Id,
        //                guid = x.guid,
        //                ProductName = x.ProductName,
        //                ActualName = x.ActualName,
        //                BrandName = x.BrandName,
        //                HairType = productCommons.Where(p => p.ProductEntityId == x.Id && p.HairTypeId != null).Select(d => new HairType
        //                {
        //                    Description = d.HairType.Description
        //                }).ToList(),
        //                ImageName = x.ImageName,
        //                Ingredients = x.Ingredients,
        //                ProductDetails = x.ProductDetails,
        //                ProductLink = x.ProductLink,
        //                IsActive = x.IsActive,
        //                CreatedOn = x.CreatedOn,
        //                ProductTypeId = x.ProductTypesId,
        //                ProductType = x.ProductTypes.ProductName,
        //                ParentId = x.ProductTypes.ParentId,
        //                ParentName = _context.ProductTypes.Where(y => y.Id == x.ProductTypes.ParentId).Select(y => y.ProductName).FirstOrDefault(),
        //                Price = x.Price,
        //                HairChallenge = productCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null).Select(d => new HairChallenges
        //                {
        //                    Description = d.HairChallenges.Description
        //                }).ToList(),
        //                ProductIndicate = productCommons.Where(p => p.ProductEntityId == x.Id && p.ProductIndicatorId != null).Select(d => new ProductIndicator
        //                {
        //                    Description = d.ProductIndicator.Description
        //                }).ToList(),
        //                ProductTag = productCommons.Where(p => p.ProductEntityId == x.Id && p.ProductTagsId != null).Select(d => new ProductTags
        //                {
        //                    Description = d.ProductTags.Description
        //                }).ToList(),
        //                ProductClassificatio = productCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null).Select(d => new ProductClassification
        //                {
        //                    Description = d.ProductClassification.Description
        //                }).ToList()
        //            }).ToList();

        //        return lstProductModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        public List<ProductsModelList> GetProducts()
        {
            try
            {
                this._context.Database.SetCommandTimeout(280);
                var productCommons = _context.ProductCommons.Where(x => x.IsActive == true).Include(x => x.HairChallenges).Include(y => y.ProductIndicator).Include(z => z.HairType).
                    Include(h => h.ProductTags).Include(h => h.ProductClassification).ToList();
                var result = _context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();

                List<ProductsModelList> lstProductModel = result.Select(x => new ProductsModelList
                {
                    Id = x.Id,
                    guid = x.guid,
                    ProductName = x.ProductName,
                    ActualName = x.ActualName,
                    BrandName = _context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                    HairType = productCommons.Where(p => p.ProductEntityId == x.Id && p.HairTypeId != null).Select(d => new HairType
                    {
                        Description = d.HairType.Description
                    }).ToList(),
                    ImageName = x.ImageName,
                    Ingredients = x.Ingredients,
                    ProductDetails = x.ProductDetails,
                    ProductLink = x.ProductLink,
                    IsActive = x.IsActive,
                    HideInSearch = x.HideInSearch != null ? x.HideInSearch : false,
                    CreatedOn = x.CreatedOn,
                    ProductTypeId = x.ProductTypesId,
                    ProductType = x.ProductTypes.ProductName,
                    ParentId = x.ProductTypes.ParentId,
                    UPCCode = x.UPCCode,
                    ParentName = _context.ProductTypes.Where(y => y.Id == x.ProductTypes.ParentId).Select(y => y.ProductName).FirstOrDefault(),
                    Price = x.Price,
                    HairChallenge = productCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null).Select(d => new HairChallenges
                    {
                        Description = d.HairChallenges.Description
                    }).ToList(),
                    ProductIndicate = productCommons.Where(p => p.ProductEntityId == x.Id && p.ProductIndicatorId != null).Select(d => new ProductIndicator
                    {
                        Description = d.ProductIndicator.Description
                    }).ToList(),
                    ProductTag = productCommons.Where(p => p.ProductEntityId == x.Id && p.ProductTagsId != null).Select(d => new ProductTags
                    {
                        Description = d.ProductTags.Description
                    }).ToList(),
                    ProductClassificatio = productCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null).Select(d => new ProductClassification
                    {
                        Description = d.ProductClassification.Description
                    }).ToList()
                }).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProducts, Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<ProductsModelList> GetBrands()
        {
            try
            {
                List<ProductsModelList> lstProductModel = _context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
                    Select(x => new ProductsModelList
                    {
                        Id = x.Id,
                        guid = x.guid,
                        ProductName = x.ProductName,
                        ActualName = x.ActualName,
                        BrandName = _context.Brands.Where(y => y.BrandId == x.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                        ImageName = x.ImageName,
                        Ingredients = x.Ingredients,
                        ProductDetails = x.ProductDetails,
                        ProductLink = x.ProductLink,
                        IsActive = x.IsActive,
                        CreatedOn = x.CreatedOn,
                        ProductTypeId = x.ProductTypesId,
                        ProductType = x.ProductTypes.ProductName,
                        ParentId = x.ProductTypes.ParentId,
                        ParentName = _context.ProductTypes.Where(y => y.Id == x.ProductTypes.ParentId).Select(y => y.ProductName).FirstOrDefault(),
                        Price = x.Price,
                    }).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetBrands, Error: " + ex.Message, ex);
                return null;
            }
        }

        public ProductEntityModel SaveProducts(ProductEntityModel productEntity)
        {
            try
            {
                List<HairType> listHairType = JsonConvert.DeserializeObject<List<HairType>>(productEntity.TypeFor);
                List<HairChallenges> listHairChallenges = JsonConvert.DeserializeObject<List<HairChallenges>>(productEntity.HairChallenges);
                List <HairGoal> listHairGoals = JsonConvert.DeserializeObject<List<HairGoal>>(productEntity.HairGoals);
                List<ProductType> listProductTypes = JsonConvert.DeserializeObject<List<ProductType>>(productEntity.ProductTypes);
                List<ProductIndicator> listProductIndicators = JsonConvert.DeserializeObject<List<ProductIndicator>>(productEntity.ProductIndicator);
                List<ProductTags> listProductTags = JsonConvert.DeserializeObject<List<ProductTags>>(productEntity.ProductTags);
                List<CustomerPreference> listCustomerPreferences = JsonConvert.DeserializeObject<List<CustomerPreference>>(productEntity.CustomerPreferences);
                List<ProductClassification> listProductClassification = JsonConvert.DeserializeObject<List<ProductClassification>>(productEntity.ProductClassification);
                List<BrandClassification> listBrandClassification = JsonConvert.DeserializeObject<List<BrandClassification>>(productEntity.BrandClassification);
                List<HairStyles> listHairStyles = JsonConvert.DeserializeObject<List<HairStyles>>(productEntity.HairStyles);
                List<ProductRecommendationStatus> listProductRecommendationStatuses = JsonConvert.DeserializeObject<List<ProductRecommendationStatus>>(productEntity.ProductRecommendationStatuses);
                List<MolecularWeight> listMolecularWeights = JsonConvert.DeserializeObject<List<MolecularWeight>>(productEntity.MolecularWeights);


                ProductEntity objProductList = new ProductEntity();
                objProductList = _context.ProductEntities.Where(x => x.guid == productEntity.guid && x.IsActive == true).FirstOrDefault();
                if (objProductList != null)
                {
                    objProductList.ProductName = productEntity.ProductName;
                    objProductList.ActualName = productEntity.ActualName;
                    objProductList.BrandName = _context.Brands.Where(x => x.BrandId == productEntity.BrandId).Select(x => x.BrandName).FirstOrDefault();
                    objProductList.ImageName = productEntity.ImageName;
                    objProductList.Ingredients = productEntity.Ingredients;
                    objProductList.ProductDetails = productEntity.ProductDetails;
                    objProductList.ProductLink = productEntity.ProductLink;
                    objProductList.HairChallenges = productEntity.HairChallenges;
                    objProductList.ProductIndicator = productEntity.ProductIndicator;
                    objProductList.Product = productEntity.ProductClassification;
                    objProductList.Price = productEntity.Price;
                    objProductList.UPCCode = productEntity.UPCCode;
                    objProductList.BrandId = productEntity.BrandId;
                    objProductList.CreatedOn = DateTime.UtcNow;
                    _context.ProductEntities.Update(objProductList);

                    var allProductCommonList = _context.ProductCommons.Where(x => x.ProductEntityId == objProductList.Id && x.IsActive == true);
                    foreach (var item in allProductCommonList)
                    {
                        item.IsActive = false;
                    }

                    _context.SaveChanges();
                }
                else {
                    objProductList = new ProductEntity();
                    objProductList.guid = Guid.NewGuid();
                    objProductList.ProductName = productEntity.ProductName;
                    objProductList.ActualName = productEntity.ActualName;
                    objProductList.BrandName = _context.Brands.Where(x => x.BrandId == productEntity.BrandId).Select(x => x.BrandName).FirstOrDefault();
                    objProductList.ImageName = productEntity.ImageName;
                    objProductList.Ingredients = productEntity.Ingredients;
                    objProductList.ProductDetails = productEntity.ProductDetails;
                    objProductList.ProductLink = productEntity.ProductLink;
                    objProductList.HairChallenges = productEntity.HairChallenges;
                    objProductList.ProductIndicator = productEntity.ProductIndicator;
                    objProductList.Product = productEntity.ProductClassification;
                    objProductList.IsActive = true;
                    objProductList.CreatedOn = DateTime.UtcNow;
                    objProductList.Price = productEntity.Price;
                    objProductList.UPCCode = productEntity.UPCCode;
                    objProductList.BrandId = productEntity.BrandId;
                    _context.Add(objProductList);
                    _context.SaveChanges();
                }

                foreach (var spec in listHairType)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.HairTypeId = spec.HairTypeId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listHairChallenges)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.HairChallengeId = spec.HairChallengeId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listHairGoals)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.HairGoalId = spec.HairGoalId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listProductTypes)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.ProductTypeId = spec.Id;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listProductIndicators)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.ProductIndicatorId = spec.ProductIndicatorId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listProductTags)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.ProductTagsId = spec.ProductTagsId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }


                foreach (var spec in listProductClassification)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.ProductClassificationId = spec.ProductClassificationId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listBrandClassification)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.BrandClassificationId = spec.BrandClassificationId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listCustomerPreferences)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.CustomerPreferenceId = spec.CustomerPreferenceId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }
                foreach (var spec in listHairStyles)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.HairStylesId= spec.Id;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }
                foreach (var spec in listProductRecommendationStatuses)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.ProductRecommendationStatusId = spec.ProductRecommendationStatusId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }
                foreach (var spec in listMolecularWeights)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.MolecularWeightId = spec.MolecularWeightId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }
                //var _aws3Services = new Aws3Services(configuration, _context);
                //if (productEntity.Files != null)
                //{
                //    int num = 1;
                //    foreach (var file in productEntity.Files)
                //    {
                //        var fileName = DateTime.Now.ToString("yyyyMMddHHmmssmm")+'_'+num;
                //        var result = _aws3Services.UploadFile(file, fileName).GetAwaiter().GetResult();
                //        if (result == true)
                //        {
                //            var objProductImage = new ProductImage();
                //            objProductImage.ImageName = fileName;
                //            objProductImage.ProductEntityId = objProductList.Id;
                //            objProductImage.IsActive = true;
                //            objProductImage.CreatedOn = DateTime.Now;
                //            _context.Add(objProductImage);
                //            _context.SaveChanges();
                //        }
                //        num++;
                //    }
                //}



                return productEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveProducts, Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool DeleteProductCategory(ProductType productType)
        {
            try
            {
                var objProduct = _context.ProductTypeCategories.FirstOrDefault(x => x.Id == productType.Id);
                {
                    if (objProduct != null)
                    {
                        objProduct.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteProductCategory, ProductTypeId:" + productType.Id + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public bool DeleteProductType(ProductType productType)
        {
            try
            {
                var objProduct = _context.ProductTypes.FirstOrDefault(x => x.Id == productType.Id);
                {
                    if (objProduct != null)
                    {
                        objProduct.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteProductType, ProductTypeId:" + productType.Id + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public bool DeleteProductTag(ProductTags productType)
        {
            try
            {
                var objProduct = _context.ProductTags.FirstOrDefault(x => x.ProductTagsId == productType.ProductTagsId);
                {
                    if (objProduct != null)
                    {
                        objProduct.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteProductTag, ProductTagsId:" + productType.ProductTagsId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public bool DeleteHairGoal(HairGoal hairGoal)
        {
            try
            {
                var objHairGoal = _context.HairGoals.FirstOrDefault(x => x.HairGoalId == hairGoal.HairGoalId);
                {
                    if (objHairGoal != null)
                    {
                        objHairGoal.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteHairGoal, HairGoalId:" + hairGoal.HairGoalId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public bool DeleteHairStyle(HairStyles hairStyle)
        {
            try
            {
                var objHairStyle = _context.HairStyles.FirstOrDefault(x => x.Id == hairStyle.Id);
                {
                    if (objHairStyle != null)
                    {
                        objHairStyle.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteHairStyle, HairStyleId:" + hairStyle.Id + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public bool DeleteHairChallenge(HairChallenges hairChallenge)
        {
            try
            {
                var objHairChallenge = _context.HairChallenges.FirstOrDefault(x => x.HairChallengeId == hairChallenge.HairChallengeId);
                {
                    if (objHairChallenge != null)
                    {
                        objHairChallenge.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteHairChallenge, HairChallengeId:" + hairChallenge.HairChallengeId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public ProductType GetProductTypeById(ProductType productType)
        {
            try
            {
                ProductType objType = _context.ProductTypes.Where(x => x.Id == productType.Id).FirstOrDefault();
                return objType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductTypeById, ProductTypeId:" + productType.Id + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public ProductTags GetProductTagById(ProductTags productTag)
        {
            try
            {
                ProductTags objTag = _context.ProductTags.Where(x => x.ProductTagsId == productTag.ProductTagsId).FirstOrDefault();
                return objTag;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductTagById, ProductTagsId:" + productTag.ProductTagsId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public HairGoal GetHairGoalById(HairGoal hairGoal)
        {
            try
            {
                HairGoal objGoal = _context.HairGoals.Where(x => x.HairGoalId == hairGoal.HairGoalId).FirstOrDefault();
                return objGoal;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairGoalById, HairGoalId:" + hairGoal.HairGoalId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public HairStyles GetHairStyleById(HairStyles hairstyle)
        {
            try
            {
                HairStyles objHairStyle = _context.HairStyles.Where(x => x.Id == hairstyle.Id).FirstOrDefault();
                return objHairStyle;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairStyleById, HairstyleId:" + hairstyle.Id + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public HairChallenges GetHairChallengeById(HairChallenges hairChallenge)
        {
            try
            {
                HairChallenges objChallenge = _context.HairChallenges.Where(x => x.HairChallengeId == hairChallenge.HairChallengeId).FirstOrDefault();
                return objChallenge;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairChallengeById, HairChallengeId:" + hairChallenge.HairChallengeId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public ProductTypeCategoriesList GetProductCategoryById(ProductTypeCategoriesList productType)
        {
            try
            {
                ProductTypeCategoriesList objType = _context.ProductTypeCategories.Where(x => x.Id == productType.ProductTypeId).
                    Select(x => new ProductTypeCategoriesList
                    {
                        ProductTypeId = x.Id,
                        CategoryName = x.CategoryName,
                        IsHair = x.IsHair,
                        IsRegimen = x.IsRegimens
                    }).FirstOrDefault();
                return objType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductCategoryById, ProductTypeId:" + productType.ProductTypeId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<ProductTypeCategoryModelList> GetProductsType()
        {
            try
            {
                List<ProductTypeCategoryModelList> productTypes = _context.ProductTypes.Include(x => x.ProductTypeCategory).Where(x => x.IsActive == true && x.ParentId != null).OrderByDescending(x => x.CreatedOn)
                   .Select(x => new ProductTypeCategoryModelList
                   {
                       Id = x.Id,
                       ProductName = x.ProductName,
                       CreatedOn = x.CreatedOn,
                       IsActive = x.IsActive,
                       CategoryName = _context.ProductTypeCategories.FirstOrDefault(c => c.Id == x.ParentId).CategoryName,
                       Rank = x.Rank
                   }).ToList();

                //ProductTypeModel productModel = new ProductTypeModel();
                //productModel.productType = productTypes;
                return productTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductsType, Error: " + ex.Message, ex);
                return null;
            }
        }
       
        public List<HairStyles> GetHairStyles()
        {
            try
            {
                List<HairStyles> hairStyles = _context.HairStyles.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();

                return hairStyles;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairStyles, Error: " + ex.Message, ex);
                return null;
            }
        }

        public ProductTypeCategoryModel SaveProductType(ProductTypeCategoryModel productType)
        {
            try
            {
                var productTypeModel = _context.ProductTypes.FirstOrDefault(x => x.Id == productType.Id);
                if (productTypeModel != null)
                {
                    productTypeModel.ProductName = productType.ProductName;
                    productTypeModel.ParentId = productType.CategoryId;
                    productTypeModel.Rank = productType.Rank;
                }
                else
                {
                    ProductType objType = new ProductType();
                    objType.ProductName = productType.ProductName;
                    objType.CreatedOn = DateTime.Now;
                    objType.IsActive = true;
                    objType.ParentId = productType.CategoryId;
                    objType.Rank = productType.Rank;

                    _context.Add(objType);
                }
                _context.SaveChanges();
                return productType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveProductType, ProductTypeId:" + productType.Id + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public ProductTags SaveProductTag(ProductTags productType)
        {
            try
            {
                var productTypeModel = _context.ProductTags.FirstOrDefault(x => x.ProductTagsId == productType.ProductTagsId);
              
                if (productTypeModel != null)
                {
                   
                    productTypeModel.Description = productType.Description;
                }
                else
                {
                    ProductTags objType = new ProductTags();
                    objType.Description = productType.Description;
                    objType.CreatedOn = DateTime.Now;
                    objType.IsActive = true;
                    _context.Add(objType);
                }
                _context.SaveChanges();
                return productType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveProductTag, ProductTagsId:" + productType.ProductTagsId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public HairGoal SaveHairGoal(HairGoal hairGoal)
        {
            try
            {
                var hairGoalModel = _context.HairGoals.FirstOrDefault(x => x.HairGoalId == hairGoal.HairGoalId);
                if (hairGoalModel != null)
                {
                    hairGoalModel.Description = hairGoal.Description;
                }
                else
                {
                    HairGoal objType = new HairGoal();
                    objType.Description = hairGoal.Description;
                    objType.CreatedOn = DateTime.Now;
                    objType.IsActive = true;

                    _context.Add(objType);
                }
                _context.SaveChanges();
                return hairGoal;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveHairGoal, HairGoalId:" + hairGoal.HairGoalId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public HairStyles SaveHairStyle(HairStyles hairStyle)
        {
            try
            {
                var hairStyleModel = _context.HairStyles.FirstOrDefault(x => x.Id == hairStyle.Id);
                if (hairStyleModel != null)
                {
                    hairStyleModel.Style = hairStyle.Style;
                }
                else
                {
                    HairStyles objType = new HairStyles();
                    objType.Style = hairStyle.Style;
                    objType.CreatedOn = DateTime.Now;
                    objType.IsActive = true;

                    _context.Add(objType);
                }
                _context.SaveChanges();
                return hairStyle;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveHairStyle, HairStyleId:" + hairStyle.Id + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public HairChallenges SaveHairChallenge(HairChallenges hairChallenges)
        {
            try
            {
                var hairGoalModel = _context.HairChallenges.FirstOrDefault(x => x.HairChallengeId == hairChallenges.HairChallengeId);
                if (hairGoalModel != null)
                {
                    hairGoalModel.Description = hairChallenges.Description;
                }
                else
                {
                    HairChallenges objType = new HairChallenges();
                    objType.Description = hairChallenges.Description;
                    objType.CreatedOn = DateTime.Now;
                    objType.IsActive = true;

                    _context.Add(objType);
                }
                _context.SaveChanges();
                return hairChallenges;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveHairChallenge, HairChallengeId:" + hairChallenges.HairChallengeId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public ProductTypeCategoriesList SaveProductCategory(ProductTypeCategoriesList productType)
        {
            try
            {
                var productTypeModel = _context.ProductTypeCategories.FirstOrDefault(x => x.Id == productType.ProductTypeId);
                if (productTypeModel != null)
                {
                    productTypeModel.CategoryName = productType.CategoryName;
                    productTypeModel.IsHair = productType.IsHair;
                    productTypeModel.IsRegimens = productType.IsRegimen;
                }
                else

                {
                    ProductTypeCategory objType = new ProductTypeCategory();
                    objType.CategoryName = productType.CategoryName;
                    objType.CreatedOn = DateTime.Now;
                    objType.IsHair = productType.IsHair;
                    objType.IsRegimens = productType.IsRegimen;
                    objType.IsActive = true;
                    _context.Add(objType);
                }
                _context.SaveChanges();
                return productType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveProductCategory, ProductTypeId:" + productType.ProductTypeId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<ProductTypesList> GetProductTypes()
        {
            try
            {
                List<ProductTypesList> lstProductModel = _context.ProductTypes.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
                    Select(x => new ProductTypesList
                    {
                        ProductTypeId = x.Id,
                        ProductTypeName = x.ProductName,
                        ParentId = x.ParentId
                    }).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductTypes, Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<ProductEntity> GetProductsList()
        {
            try
            {
                List<ProductEntity> lstProductModel = _context.ProductEntities.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
                    Select(x => new ProductEntity
                    {
                        Id= x.Id,
                        ActualName = x.ActualName
                    }).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductsList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<ProductTypeCategoriesList> GetProductsCategoryList()
        {
            try
            {
                List<ProductTypeCategoriesList> lstProductModel = _context.ProductTypeCategories.Where(y => y.IsActive == true).OrderByDescending(x => x.CreatedOn).
                    Select(x => new ProductTypeCategoriesList
                    {
                        ProductTypeId = x.Id,
                        CategoryName = x.CategoryName,
                        IsHair = x.IsHair,
                        IsRegimen = x.IsRegimens
                    }).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductsCategoryList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<HairType> GetHairTypesList()
        {
            try
            {
                List<HairType> lstHairType = _context.HairTypes.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairTypesList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<HairChallenges> GetHairChallengesList()
        {
            try
            {
                List<HairChallenges> lstHairType = _context.HairChallenges.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairChallengesList, Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<HairGoal> GetHairGoalsList()
        {
            try
            {
                List<HairGoal> lstHairGoal = _context.HairGoals.Where(x => x.IsActive == true).ToList();
                return lstHairGoal;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairGoalsList, Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<ProductIndicator> GetProductIndicatorsList()
        {
            try
            {
                List<ProductIndicator> lstHairType = _context.ProductIndicator.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductIndicatorsList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<IngedientsEntity> GetIngredientsList()
        {
            try
            {
                List<IngedientsEntity> lstHairType = _context.IngedientsEntities.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetIngredientsList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<ProductTags> GetProductTagList()
        {
            try
            {
                List<ProductTags> lstHairType = _context.ProductTags.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductTagList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<ProductClassification> GetProductClassificationList()
        {
            try
            {
                List<ProductClassification> lstHairType = _context.ProductClassification.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductClassificationList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<CustomerPreference> GetCustomerPreferenceList()
        {
            try
            {
                List<CustomerPreference> lstHairType = _context.CustomerPreference.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetCustomerPreferenceList, Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<BrandClassification> GetBrandClassificationList()
        {
            try
            {
                List<BrandClassification> lstHairType = _context.BrandClassifications.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetBrandClassificationList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public IEnumerable<ProductEntityModel> AddProductList(IEnumerable<ProductEntityModel> productData)
        {
            try
            {
                if (productData != null)
                {
                    foreach (var product in productData)
                    {
                        var productType = _context.ProductTypes.Where(x => x.ProductName == product.ProductType && x.ParentId != null).Select(x => x.Id).FirstOrDefault();
                        if (productType == 0)
                        {
                            ProductType objType = new ProductType();
                            objType.ProductName = product.ProductType;
                            objType.ParentId = null;
                            objType.IsActive = true;
                            objType.CreatedOn = DateTime.Now;
                            _context.Add(objType);
                            _context.SaveChanges();

                            ProductType objType2 = new ProductType();
                            objType2.ProductName = product.ProductType;
                            objType2.ParentId = objType.Id;
                            objType2.IsActive = true;
                            objType2.CreatedOn = DateTime.Now;
                            _context.Add(objType2);
                            _context.SaveChanges();
                        }

                        ProductEntity objProduct = new ProductEntity();
                        objProduct.guid = Guid.NewGuid();
                        objProduct.ProductName = product.ProductName;
                        objProduct.ActualName = product.ActualName;
                        objProduct.BrandName = product.BrandName;
                        objProduct.TypeFor = product.TypeFor;
                        objProduct.ImageName = product.ImageName;
                        objProduct.Ingredients = product.Ingredients;
                        objProduct.ProductDetails = product.ProductDetails;
                        objProduct.ProductLink = product.ProductLink;
                        objProduct.Price = product.Price;
                        objProduct.Product = product.ProductClassification;
                        objProduct.ProductTypesId = _context.ProductTypes.Where(x => x.ProductName == product.ProductType && x.ParentId != null).Select(x => x.Id).FirstOrDefault(); ;
                        objProduct.HairChallenges = product.HairChallenges;
                        objProduct.ProductIndicator = product.ProductIndicator;
                        objProduct.ProductTags = product.ProductTags;
                        objProduct.IsActive = true;
                        objProduct.CreatedOn = DateTime.Now;
                        objProduct.UPCCode = product.UPCCode;

                        _context.Add(objProduct);
                        _context.SaveChanges();
                    }
                }
                return productData;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: AddProductList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public (JsonResult result, bool success, string error) UploadFile(IFormFile file, UserEntity entity)
        {
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var extension = Path.GetExtension(fileName);
                    if (extension != ".xlsx")
                    {
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var imageURL = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        if (entity != null)
                        {
                            UserEntity us = _context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();
                            us.ImageURL = "http://localhost:5004/" + "Resources/" + "Images/" + fileName;
                            _context.SaveChanges();
                        }
                        return (new JsonResult(""), true, "http://localhost:5004/" + "Resources/" + "Images/" + fileName);
                    }
                    else
                    {
                        _logger.LogError("Method: UploadFile, UserId:" + entity.Id + ", Error:Wrong Image File");
                        return (new JsonResult(""), false, "Wrong Image File");
                    }
                }
                else
                {
                    
                    return (new JsonResult(""), false, "");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: UploadFile, UserId:" + entity.Id + ", Error: " + ex.Message, ex);
                return (new JsonResult(""), false, "");
            }
        }


        public List<ProductTypeCategory> GetProductCategory()
        {
            try
            {
                List<ProductTypeCategory> lstProductModel = _context.ProductTypeCategories.Where(x => x.IsActive == true).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductCategory, Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<ProductsModelList> GetStylingProducts()
        {
            try
            {
                _context.Database.SetCommandTimeout(280);

                var styleCat = _context.ProductTypeCategories.AsNoTracking().Where(x => x.IsActive == true && x.IsRegimens == true).Select(y => y.Id).ToArray();
                

                var productTypes = _context.ProductTypes.AsNoTracking().Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
                        Select(x => new ProductTypesList
                        {
                            ProductTypeId = x.Id,
                            ProductTypeName = x.ProductName,
                            ParentId = x.ParentId
                        }).ToList();

                var styleProductType = productTypes.Where(x => styleCat.Contains(x.ParentId ?? 0)).ToList().Select(y => y.ProductTypeId).ToArray();

                var brandsList = _context.Brands
                                   .Where(x => x.HideInSearch == true && x.IsActive == true)
                                   .Select(x => x.BrandName)
                                   .ToList();

                var productsListings = (from products in _context.ProductEntities
                                                 .Where(x=>x.HideInSearch != true && !brandsList.Contains(x.BrandName))
                                                 .Include(x => x.ProductTypes)
                                                 join common in _context.ProductCommons.Include(x => x.ProductType)
                                                 .Where(x => x.ProductTypeId != null && x.IsActive==true)
                                                 on products.Id equals common.ProductEntityId into gj
                                                 from common1 in gj.DefaultIfEmpty()
                                                 where products.IsActive 
                                                 select new ProductsModelList
                                                 {
                                                     Id = products.Id,
                                                     ProductName = products.ProductName,
                                                     BrandName = _context.Brands
                                                                   .Where(y => y.BrandId == products.BrandId)
                                                                   .Select(y => y.BrandName)
                                                                   .FirstOrDefault(),
                                                     ProductTypeId = products.ProductTypes != null ? products.ProductTypesId : common1.ProductTypeId,
                                                     ProductType = products.ProductTypes != null ? products.ProductTypes.ProductName : common1.ProductType.ProductName,
                                                     ParentId = products.ProductTypes != null ? products.ProductTypes.ParentId : common1.ProductType.ParentId,
                                                     ParentName = products.ProductTypes != null
                                                                  ? _context.ProductTypes.AsNoTracking()
                                                                    .Where(y => y.Id == products.ProductTypes.ParentId)
                                                                    .Select(y => y.ProductName)
                                                                    .FirstOrDefault()
                                                                  : _context.ProductTypes.AsNoTracking()
                                                                    .Where(y => y.Id == common1.ProductType.ParentId)
                                                                    .Select(y => y.ProductName)
                                                                    .FirstOrDefault()
                                                 }).Distinct().ToList();
                   List<ProductsModelList> productsModelListsStyling = productsListings.Where(x => styleProductType.Contains(x.ProductTypeId ?? 0)).OrderBy(p => p.ProductName).ToList();
                return productsModelListsStyling;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetStylingProducts, Error: " + ex.Message, ex);
                return new List<ProductsModelList>(); // Return an empty list in case of error
            }
        }
        public ProductsListings GetAllProducts()
        {
            try
            {

                this._context.Database.SetCommandTimeout(280);
                ProductsListings productsListings = new ProductsListings();
                productsListings.TypeCategories = _context.ProductTypeCategories.AsNoTracking().Where(x => x.IsActive == true).ToList();
                var essCat = productsListings.TypeCategories.Where(x => x.IsHair == true).Select(y => y.Id).ToArray();
                var styleCat = productsListings.TypeCategories.Where(x => x.IsRegimens == true).Select(y => y.Id).ToArray();

                productsListings.ProductClassifications = _context.ProductClassification.Where(x => x.IsActive == true).ToList();
                productsListings.HairChallenges = _context.HairChallenges.Where(x => x.IsActive == true).ToList();
                productsListings.HairGoals = _context.HairGoals.Where(x => x.IsActive == true).ToList();
                productsListings.ProductTypes = _context.ProductTypes.AsNoTracking().Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
                        Select(x => new ProductTypesList
                        {
                            ProductTypeId = x.Id,
                            ProductTypeName = x.ProductName,
                            ParentId = x.ParentId
                        }).ToList();

                var essProductType = productsListings.ProductTypes.Where(x => essCat.Contains(x.ParentId ?? 0)).ToList().Select(y => y.ProductTypeId).ToArray();
                var styleProductType = productsListings.ProductTypes.Where(x => styleCat.Contains(x.ParentId ?? 0)).ToList().Select(y => y.ProductTypeId).ToArray();

                using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {

                    var brandsList = _context.Brands.Where(x => x.HideInSearch == true && x.IsActive == true).Select(x => x.BrandName).ToList();
                    //connection.Open();
                    //var result =  connection.QueryAsync<int>("SelectAllProductsFromB2B", null, commandTimeout: 300, commandType: CommandType.StoredProcedure).GetAwaiter().GetResult();
                    //var productIds = result.ToList();

                    productsListings.ProductsModelLists = (from products in _context.ProductEntities
                                                           //.Where(x => productIds.Contains(x.Id)).Take(100)
                                                           .Where(x => x.HideInSearch != true && !brandsList.Contains(x.BrandName))
                                                           .Include(x => x.ProductTypes)
                                                           join common in _context.ProductCommons.Include(x => x.ProductType).Where(x => x.ProductTypeId != null && x.IsActive == true)
                                                           on products.Id equals common.ProductEntityId
                                                           into gj
                                                           from common1 in gj.DefaultIfEmpty()
                                                           where products.IsActive == true
                                                           select (new ProductsModelList
                                                           {
                                                               Id = products.Id,
                                                               ProductName = products.ProductName,
                                                               BrandName = _context.Brands.Where(y => y.BrandId == products.BrandId).Select(y => y.BrandName).FirstOrDefault(),
                                                               ProductTypeId = products.ProductTypes != null ? products.ProductTypesId : common1.ProductTypeId,
                                                               ProductType = products.ProductTypes != null ? products.ProductTypes.ProductName : common1.ProductType.ProductName,
                                                               ParentId = products.ProductTypes != null ? products.ProductTypes.ParentId : common1.ProductType.ParentId,
                                                               ParentName = products.ProductTypes != null ? _context.ProductTypes.AsNoTracking().
                                                               Where(y => y.Id == products.ProductTypes.ParentId)
                                                               .Select(y => y.ProductName).FirstOrDefault() :
                                                               _context.ProductTypes.AsNoTracking().
                                                               Where(y => y.Id == (products.ProductTypes != null ? products.ProductTypes.ParentId : common1.ProductType.ParentId))
                                                               .Select(y => y.ProductName).FirstOrDefault(),
                                                               ProductClassificatio = _context.ProductCommons.Include(p => p.ProductClassification).Where(p => p.ProductEntityId == products.Id && p.ProductClassificationId != null && p.IsActive == true).Select(p => new ProductClassification
                                                               {
                                                                   Description = p.ProductClassification.Description,
                                                                   ProductClassificationId = p.ProductClassification.ProductClassificationId
                                                               }).ToList(),
                                                               HairChallenge = _context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == products.Id && p.HairChallengeId != null && p.IsActive == true).Select(p => new HairChallenges
                                                               {
                                                                   Description = p.HairChallenges.Description,
                                                                   HairChallengeId = p.HairChallenges.HairChallengeId
                                                               }).ToList(),
                                                               HairGoals = _context.ProductCommons.Include(p => p.HairGoal).Where(p => p.ProductEntityId == products.Id && p.HairGoalId != null && p.IsActive == true).Select(p => new HairGoal
                                                               {
                                                                   Description = p.HairGoal.Description,
                                                                   HairGoalId = p.HairGoal.HairGoalId
                                                               }).ToList()
                                                           })).Distinct().ToList();
                }
              

               

                productsListings.ProductsModelListsEssential = productsListings.ProductsModelLists.Where(x => essProductType.Contains(x.ProductTypeId ?? 0)).ToList();
                productsListings.ProductsModelListsStyling = productsListings.ProductsModelLists.Where(x => styleProductType.Contains(x.ProductTypeId ?? 0)).ToList();

                return productsListings;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetAllProducts, Error: " + ex.Message, ex);
                ProductsListings productsListings = new ProductsListings();
                productsListings.Message = ex.Message;
                return productsListings;
            }
        }

        public async Task<SearchProductResponse> GetAllAsync(SearchProductResponse searchProductResponse)
        {
            try
            {
                List<HairChallenges> listHairChallenges = JsonConvert.DeserializeObject<List<HairChallenges>>(searchProductResponse.advanceSearchProduct.HairChallenges);
                List<ProductType> listProductTypes = JsonConvert.DeserializeObject<List<ProductType>>(searchProductResponse.advanceSearchProduct.ProductTypes);
                List<ProductIndicator> listProductIndicators = JsonConvert.DeserializeObject<List<ProductIndicator>>(searchProductResponse.advanceSearchProduct.ProductIndicator);
                List<ProductTags> listProductTags = JsonConvert.DeserializeObject<List<ProductTags>>(searchProductResponse.advanceSearchProduct.ProductTags);
                List<ProductClassification> listProductClassification = JsonConvert.DeserializeObject<List<ProductClassification>>(searchProductResponse.advanceSearchProduct.ProductClassification);
                List<BrandClassification> listBrandClassification = JsonConvert.DeserializeObject<List<BrandClassification>>(searchProductResponse.advanceSearchProduct.BrandClassification);
                List<BrandsModelList> listProductBrand = JsonConvert.DeserializeObject<List<BrandsModelList>>(searchProductResponse.advanceSearchProduct.ProductBrands);
                List<HairGoal> listHairGoals = JsonConvert.DeserializeObject<List<HairGoal>>(searchProductResponse.advanceSearchProduct.HairGoals);
                List<HairType> listHairType = JsonConvert.DeserializeObject<List<HairType>>(searchProductResponse.advanceSearchProduct.HairTypes);
                List<CustomerPreference> listCustomerPreference = JsonConvert.DeserializeObject<List<CustomerPreference>>(searchProductResponse.advanceSearchProduct.CustomerPreferences);
                List<HairStyles> listHairStyles = JsonConvert.DeserializeObject<List<HairStyles>>(searchProductResponse.advanceSearchProduct.HairStyles);
                List<ProductRecommendationStatus> listProductRecommendationStatuses = JsonConvert.DeserializeObject<List<ProductRecommendationStatus>>(searchProductResponse.advanceSearchProduct.ProductRecommendationStatuses);
                List<MolecularWeight> listMolecularWeights = JsonConvert.DeserializeObject<List<MolecularWeight>>(searchProductResponse.advanceSearchProduct.MolecularWeights);

                var dp_params = new DynamicParameters();
                dp_params.Add("PageSize", searchProductResponse.pageSize, DbType.Int64);
                dp_params.Add("Skip", searchProductResponse.skip, DbType.Int64);
                dp_params.Add("TotalRows", 0, DbType.Int32, ParameterDirection.Output);
                dp_params.Add("Search", searchProductResponse.searchValue, DbType.String);
                dp_params.Add("SortColumn", searchProductResponse.sortColumn, DbType.String);
                dp_params.Add("SortDirection", searchProductResponse.sortDirection, DbType.String);

                if (listHairChallenges.Count > 0)
                    dp_params.Add("HairChallenges", string.Join(",", listHairChallenges.Select(item => item.HairChallengeId).ToArray()), DbType.String);
                else
                    dp_params.Add("HairChallenges", "", DbType.String);

                if (listProductTypes.Count > 0)
                    dp_params.Add("ProductTypes", string.Join(",", listProductTypes.Select(item => item.Id).ToArray()), DbType.String);
                else
                    dp_params.Add("ProductTypes", "", DbType.String);

                if (listProductIndicators.Count > 0)
                    dp_params.Add("ProductIndicator", string.Join(",", listProductIndicators.Select(item => item.ProductIndicatorId).ToArray()), DbType.String);
                else
                    dp_params.Add("ProductIndicator", "", DbType.String);

                if (listProductTags.Count > 0)
                    dp_params.Add("ProductTags", string.Join(",", listProductTags.Select(item => item.ProductTagsId).ToArray()), DbType.String);
                else
                    dp_params.Add("ProductTags", "", DbType.String);

                if (listProductClassification.Count > 0)
                    dp_params.Add("ProductClassification", string.Join(",", listProductClassification.Select(item => item.ProductClassificationId).ToArray()), DbType.String);
                else
                    dp_params.Add("ProductClassification", "", DbType.String);

                if (listProductBrand.Count > 0)
                    dp_params.Add("BrandName", string.Join(",", listProductBrand.Select(item => item.BrandName).ToArray()), DbType.String);
                else
                    dp_params.Add("BrandName", "", DbType.String);

                if (listHairGoals.Count > 0)
                    dp_params.Add("HairGoal", string.Join(",", listHairGoals.Select(item => item.HairGoalId).ToArray()), DbType.String);
                else
                    dp_params.Add("HairGoal", "", DbType.String);

                if (listHairType.Count > 0)
                    dp_params.Add("HairType", string.Join(",", listHairType.Select(item => item.HairTypeId).ToArray()), DbType.String);
                else
                    dp_params.Add("HairType", "", DbType.String);

                if (listCustomerPreference.Count > 0)
                    dp_params.Add("CustomerPreference", string.Join(",", listCustomerPreference.Select(item => item.CustomerPreferenceId).ToArray()), DbType.String);
                else
                    dp_params.Add("CustomerPreference", "", DbType.String);

                if (listHairStyles.Count > 0)
                    dp_params.Add("HairStyles", string.Join(",", listHairStyles.Select(item => item.Id).ToArray()), DbType.String);
                else
                    dp_params.Add("HairStyles", "", DbType.String);

                if (listProductRecommendationStatuses.Count > 0)
                    dp_params.Add("ProductRecommendationStatus", string.Join(",", listProductRecommendationStatuses.Select(item => item.ProductRecommendationStatusId).ToArray()), DbType.String);
                else
                    dp_params.Add("ProductRecommendationStatus", "", DbType.String);

                if (listMolecularWeights.Count > 0)
                    dp_params.Add("MolecularWeight", string.Join(",", listMolecularWeights.Select(item => item.MolecularWeightId).ToArray()), DbType.String);
                else
                    dp_params.Add("MolecularWeight", "", DbType.String);

                using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<ProductsModelList>("SelectAllProducts_1", dp_params,commandTimeout:300, commandType: CommandType.StoredProcedure);
                    searchProductResponse.Data = result.ToList();
                    searchProductResponse.RecordsTotal = dp_params.Get<int>("TotalRows");
                    return searchProductResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetAllAsync, Error: " + ex.Message, ex);
                throw ex;
            }

        }

        public ProductClassification GetProductClassificationById(ProductClassification productClassification)
        {
            try
            {
                ProductClassification obj = _context.ProductClassification.Where(x => x.ProductClassificationId == productClassification.ProductClassificationId).FirstOrDefault();
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductClassificationById, ProductClassificationId:" + productClassification.ProductClassificationId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public CustomerPreference GetCustomerPreferenceById(CustomerPreference customerPreference)
        {
            try
            {
                CustomerPreference obj = _context.CustomerPreference.Where(x => x.CustomerPreferenceId == customerPreference.CustomerPreferenceId).FirstOrDefault();
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetCustomerPreferenceById, CustomerPreferenceId:" + customerPreference.CustomerPreferenceId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public BrandClassification GetBrandClassificationById(BrandClassification brandClassification)
        {
            try
            {
                BrandClassification obj = _context.BrandClassifications.Where(x => x.BrandClassificationId == brandClassification.BrandClassificationId).FirstOrDefault();
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetBrandClassificationById, BrandClassificationId:" + brandClassification.BrandClassificationId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public BrandClassification SaveBrandClassification(BrandClassification brandClassification)
        {
            try
            {
                var productClassificationModel = _context.BrandClassifications.FirstOrDefault(x => x.BrandClassificationId == brandClassification.BrandClassificationId);
                if (productClassificationModel != null)
                {
                    productClassificationModel.Description = brandClassification.Description;
                }
                else
                {
                    BrandClassification obj = new BrandClassification();
                    obj.Description = brandClassification.Description;
                    obj.CreatedOn = DateTime.Now;
                    obj.IsActive = true;

                    _context.Add(obj);
                }
                _context.SaveChanges();
                return brandClassification;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveBrandClassification, BrandClassificationId:" + brandClassification.BrandClassificationId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool DeleteBrandClassification(BrandClassification productClassification)
        {
            try
            {
                var objProduct = _context.BrandClassifications.FirstOrDefault(x => x.BrandClassificationId == productClassification.BrandClassificationId);
                {
                    if (objProduct != null)
                    {
                        objProduct.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteBrandClassification, BrandClassificationId:" + productClassification.BrandClassificationId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public ProductClassification SaveProductClassification(ProductClassification productClassification)
        {
            try
            {
                var productClassificationModel = _context.ProductClassification.FirstOrDefault(x => x.ProductClassificationId == productClassification.ProductClassificationId);
                if (productClassificationModel != null)
                {
                    productClassificationModel.Description = productClassification.Description;
                }
                else
                {
                    ProductClassification obj = new ProductClassification();
                    obj.Description = productClassification.Description;
                    obj.CreatedOn = DateTime.Now;
                    obj.IsActive = true;

                    _context.Add(obj);
                }
                _context.SaveChanges();
                return productClassification;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SaveProductClassification, ProductClassificationId:" + productClassification.ProductClassificationId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public bool DeleteProductClassification(ProductClassification productClassification)
        {
            try
            {
                var objProduct = _context.ProductClassification.FirstOrDefault(x => x.ProductClassificationId == productClassification.ProductClassificationId);
                {
                    if (objProduct != null)
                    {
                        objProduct.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteProductClassification, ProductClassificationId:" + productClassification.ProductClassificationId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public List<BrandsModelList> GetAllBrands()
        {
            try
            {
                List<BrandsModelList> lstBrandModel = _context.ProductEntities.Where(x => x.IsActive == true).GroupBy(x => x.BrandName).Select(g => new BrandsModelList
                {
                    BrandName = g.FirstOrDefault().BrandName
                }).ToList();


                return lstBrandModel;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetAllBrands, Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public CustomerPreference SaveCustomerPreference(CustomerPreference customerPreference)
        {
            try
            {
                var customerPreferenceModel = _context.CustomerPreference.FirstOrDefault(x => x.CustomerPreferenceId == customerPreference.CustomerPreferenceId);
                if (customerPreferenceModel != null)
                {
                    customerPreferenceModel.Description = customerPreference.Description;
                }
                else
                {
                    CustomerPreference obj = new CustomerPreference();
                    obj.Description = customerPreference.Description;
                    obj.CreatedOn = DateTime.Now;
                    obj.IsActive = true;

                    _context.Add(obj);
                }
                _context.SaveChanges();
                return customerPreference;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SaveCustomerPreference, CustomerPreferenceId:" + customerPreference.CustomerPreferenceId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public bool DeleteCustomerPreference(CustomerPreference customerPreference)
        {
            try
            {
                var objProduct = _context.CustomerPreference.FirstOrDefault(x => x.CustomerPreferenceId == customerPreference.CustomerPreferenceId);
                {
                    if (objProduct != null)
                    {
                        objProduct.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteCustomerPreference, CustomerPreferenceId:" + customerPreference.CustomerPreferenceId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public List<ProductRecommendationStatus> GetProductRecommendationStatusList()
        {
            try
            {
                List<ProductRecommendationStatus> productRecommendationStatus = _context.ProductRecommendationStatus.Where(x => x.IsActive == true).ToList();
                return productRecommendationStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductRecommendationStatusList, Error: " + ex.Message, ex);
                return null;
            }
        }
        public ProductRecommendationStatus SaveProductRecommendationStatus(ProductRecommendationStatus recommendationStatus)
        {
            try
            {
                var productRecommendationStatusModel = _context.ProductRecommendationStatus.FirstOrDefault(x => x.ProductRecommendationStatusId == recommendationStatus.ProductRecommendationStatusId);
                if (productRecommendationStatusModel != null)
                {
                    productRecommendationStatusModel.Description = recommendationStatus.Description;
                }
                else
                {
                    ProductRecommendationStatus objType = new ProductRecommendationStatus();
                    objType.Description = recommendationStatus.Description;
                    objType.CreatedOn = DateTime.Now;
                    objType.IsActive = true;

                    _context.Add(objType);
                }
                _context.SaveChanges();
                return recommendationStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveProductRecommendationStatus, ProductRecommendationStatusId:" + recommendationStatus.ProductRecommendationStatusId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public ProductRecommendationStatus GetProductRecommendationStatusById(ProductRecommendationStatus recommendationStatus)
        {
            try
            {
                ProductRecommendationStatus objRecommendationStatus = _context.ProductRecommendationStatus.Where(x => x.ProductRecommendationStatusId == recommendationStatus.ProductRecommendationStatusId).FirstOrDefault();
                return objRecommendationStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProductRecommendationStatusById, ProductRecommendationStatusId:" + recommendationStatus.ProductRecommendationStatusId + ", Error: " + ex.Message, ex);
                return null;
            }
        }
        public bool DeleteProductRecommendationStatus(ProductRecommendationStatus recommendationStatus)
        {
            try
            {
                var objRecommendationStatus = _context.ProductRecommendationStatus.FirstOrDefault(x => x.ProductRecommendationStatusId == recommendationStatus.ProductRecommendationStatusId);
                {
                    if (objRecommendationStatus != null)
                    {
                        objRecommendationStatus.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteProductRecommendationStatus, ProductRecommendationStatusId:" + recommendationStatus.ProductRecommendationStatusId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
    }
}
