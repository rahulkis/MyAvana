﻿using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class IndicatorService : IIndicatorService
    {
        private readonly AvanaContext _context;
        private readonly Logger.Contract.ILogger _logger;
        public IndicatorService(AvanaContext context, Logger.Contract.ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<IndicatorModel> GetIndicators()
        {
            try
            {
                List<IndicatorModel> lstProductModel = _context.ProductIndicator.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
                    Select(x => new IndicatorModel
                    {
                        ProductIndicatorId = x.ProductIndicatorId,
                        Description = x.Description,
                        CreatedOn = x.CreatedOn,
                    }).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetIndicators, Error: " + ex.Message, ex);
                return null;
            }
        }

        public IndicatorModel SaveIndicator(IndicatorModel indicatorEntity)
        {
            try
            {
                ProductIndicator indicator = _context.ProductIndicator.Where(x => x.ProductIndicatorId == indicatorEntity.ProductIndicatorId).FirstOrDefault();
                if (indicator != null)
                {
                    indicator.Description = indicatorEntity.Description;
                }
                else
                {
                    ProductIndicator productIndicator = new ProductIndicator();
                    productIndicator.Description = indicatorEntity.Description;
                    productIndicator.IsActive = true;
                    productIndicator.CreatedOn = DateTime.Now;
                    _context.Add(productIndicator);

                }
                _context.SaveChanges();
                return indicatorEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveIndicator, ProductIndicatorId:" + indicatorEntity.ProductIndicatorId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public IndicatorModel GetIndicatorById(IndicatorModel IndicatorModel)
        {
            try
            {
                IndicatorModel toolsEntity = _context.ProductIndicator.Where(x => x.ProductIndicatorId == IndicatorModel.ProductIndicatorId).
                    Select(x => new IndicatorModel
                    {
                        ProductIndicatorId = x.ProductIndicatorId,
                        Description = x.Description,
                        IsActive = x.IsActive,
                        CreatedOn = x.CreatedOn
                    }).FirstOrDefault();
                _context.SaveChanges();
                return toolsEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetIndicatorById, ProductIndicatorId:" + IndicatorModel.ProductIndicatorId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool DeleteIndicator(IndicatorModel IndicatorModel)
        {
            try
            {
                var indicator = _context.ProductIndicator.FirstOrDefault(x => x.ProductIndicatorId == IndicatorModel.ProductIndicatorId);
                {
                    if (indicator != null)
                    {
                        indicator.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError("Method: DeleteIndicator, ProductIndicatorId:" + IndicatorModel.ProductIndicatorId + ", Error: " + ex.Message, ex);
                return false;
            }
        }

    }
}
