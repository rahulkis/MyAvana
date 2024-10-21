using Microsoft.Extensions.Logging;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using MyAvana.Models.Entities;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly AvanaContext _context;
        private readonly Logger.Contract.ILogger _logger;
        public PromoCodeService(AvanaContext avanaContext, Logger.Contract.ILogger logger)
        {
            _context = avanaContext;
            _logger = logger;
        }
        public bool SavePromoCode(PromoCodeModel codeModel)
        {
            try
            {
                _context.PromoCodes.Add(new PromoCode()
                {
                    Code = codeModel.PromoCode,
                    CreatedDate = DateTime.UtcNow,
                    ExpireDate = codeModel.ExpireDate,
                    Active = true,
					StripePlanId = codeModel.StripePlanId,
					CreatedBy = "Admin",
                });
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: SavePromoCode, StripePlanId:" + codeModel.StripePlanId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public List<PromoCodeModel> GetPromoCodes()
        {
            try
            {
                List<PromoCodeModel> promoCodes = _context.PromoCodes.Where(x => x.Active == true).Select(x => new PromoCodeModel
                {
                    PromoCode = x.Code,
                    CreatedDate = x.CreatedDate,
                    ExpireDate = x.ExpireDate,
                    IsActive = x.Active
                }).OrderByDescending(x => x.CreatedDate).ToList();


                return promoCodes;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetPromoCodes, Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool DeletePromoCode(PromoCodeModel codeModel)
        {
            try
            {
                var objCode = _context.PromoCodes.FirstOrDefault(x => x.Code == codeModel.PromoCode);
                {
                    if (objCode != null)
                    {
                        objCode.Active = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeletePromoCode, Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public List<DiscountCodesModel> GetDiscountCodes()
        {
            try
            {
                List<DiscountCodesModel> promoCodes = _context.DiscountCodes.Where(x => x.IsActive == true).Select(x => new DiscountCodesModel
                {
                    DiscountCodeId = x.DiscountCodeId,
                    CreatedDate = x.CreatedDate,
                    ExpireDate = x.ExpireDate,
                    DiscountPercent = x.DiscountPercent,
                    DiscountCode = x.DiscountCode,
                    CreatedBy = x.CreatedBy,
                }).OrderByDescending(x => x.CreatedDate).ToList();


                return promoCodes;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetDiscountCodes, Error: " + ex.Message, ex);
                return null;
            }
        }
        public DiscountCodesModel SaveDiscountCode(DiscountCodesModel discountCode)
        {
            try
            {
                var duplicacyCheck = _context.DiscountCodes.FirstOrDefault(x => x.DiscountCode == discountCode.DiscountCode && x.DiscountPercent == discountCode.DiscountPercent && x.ExpireDate == discountCode.ExpireDate);
                if (duplicacyCheck != null)
                {
                    return null;
                }
                    var discountCodeModel = _context.DiscountCodes.FirstOrDefault(x => x.DiscountCodeId == discountCode.DiscountCodeId);

                if (discountCodeModel != null)
                {
                    discountCodeModel.DiscountCode = discountCode.DiscountCode;
                    discountCodeModel.DiscountPercent = discountCode.DiscountPercent;
                    discountCodeModel.ExpireDate = discountCode.ExpireDate;
                }
                else
                {
                    DiscountCodes objType = new DiscountCodes();
                    objType.DiscountCode = discountCode.DiscountCode;
                    objType.CreatedDate = DateTime.Now;
                    objType.ExpireDate = discountCode.ExpireDate;
                    objType.DiscountPercent = discountCode.DiscountPercent;
                    objType.CreatedBy = "Admin";
                    objType.IsActive = true;
                    _context.Add(objType);
                }
                _context.SaveChanges();
                return discountCode;
            }
            catch(Exception ex)
            {
                _logger.LogError("Method: SaveDiscountCode, DiscountCodeId:" + discountCode.DiscountCodeId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public DiscountCodesModel GetDiscountCodeById(DiscountCodesModel discountCode)
        {
            try
            {
                var objTag = _context.DiscountCodes.Where(x => x.DiscountCodeId == discountCode.DiscountCodeId).FirstOrDefault();

                DiscountCodesModel result = new DiscountCodesModel
                {
                    DiscountCode = objTag.DiscountCode,
                    DiscountPercent = objTag.DiscountPercent,
                    ExpireDate = objTag.ExpireDate,
                    DiscountCodeId = objTag.DiscountCodeId,
                    CreatedBy = objTag.CreatedBy,
                    CreatedDate = objTag.CreatedDate,
                    IsActive = objTag.IsActive
                };
                return result;
                //return objTag;
            }
            catch(Exception ex)
            {
                _logger.LogError("Method: GetDiscountCodeById, DiscountCodeId:" + discountCode.DiscountCodeId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool DeleteDiscountCode(DiscountCodesModel discountCode)
        {
            try
            {
                var objDiscount = _context.DiscountCodes.FirstOrDefault(x => x.DiscountCodeId == discountCode.DiscountCodeId);
                {
                    if (objDiscount != null)
                    {
                        objDiscount.IsActive = false;
                    }
                    _context.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Method: DeleteDiscountCode, DiscountCodeId:" + discountCode.DiscountCodeId + ", Error: " + ex.Message, ex);
                return false;
            }
        }
    }
}
