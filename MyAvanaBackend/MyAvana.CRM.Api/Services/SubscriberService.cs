﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly AvanaContext _context;
        private readonly Logger.Contract.ILogger _logger;
        public SubscriberService(AvanaContext context, Logger.Contract.ILogger logger)
        {
            _context = context;
            _logger = logger;
        }


        public List<SubscriberModel> GetSubscriberList()
        {
            try
            {

              
                List<SubscriberModel> Subscriberlist = (from paymt in _context.PaymentEntities
                                                        join usr in _context.Users
                                 on paymt.EmailAddress equals usr.Email
                                                        select new SubscriberModel()
                                                        {
                                                            IsActive = paymt.IsActive,
                                                            ProviderName=paymt.ProviderName,
                                                            UserName = usr.FirstName + " " + usr.LastName,
                                                            UserEmail = usr.Email,
                                                            UserId = usr.Id.ToString()
                                                        }).Where(x => x.IsActive == true).ToList();

                return Subscriberlist;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetSubscriberList, Error: " + ex.Message, ex);
                return null;
            }
        }


        public bool CancelSubscription(SubscriberModel subscriberModel)
        {
            try
            {
                var subscriber = _context.PaymentEntities.FirstOrDefault(x => x.EmailAddress == subscriberModel.UserEmail);
                {
                    if (subscriber != null)
                    {
                        subscriber.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: CancelSubscription, Email:" + subscriberModel.UserEmail + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }


    }
}
