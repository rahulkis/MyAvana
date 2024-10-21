using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using NLog.Web.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Mail;
using ZendeskApi_v2.Models.Requests;
using System.Web;
using MyAvanaApi.Contract;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace MyAvana.CRM.Api.Services
{
    public class HairScopeService : IHairScopeService
    {
        private  AvanaContext context;
        private readonly Logger.Contract.ILogger _logger;
        private readonly IConfiguration configuration;
        public HairScopeService(AvanaContext _context,   IConfiguration configuration, Logger.Contract.ILogger logger)
        {
            context = _context;
            _logger = logger;
            this.configuration = configuration;
        }
        public HairScopeModel AddNewHairScope(HairScopeModel HairScope) 
        {
            try
            {
                
                var Hairscope = context.HairScope.Where(x => x.UserId.ToString().ToLower() == HairScope.AccessCode.ToString().ToLower() && x.HairProfileId ==null).OrderByDescending(x => x.CreatedOn).FirstOrDefault(); 
                if (Hairscope != null)
                { 
                    Hairscope.HairScopeResult = JsonConvert.SerializeObject(HairScope);  
                    context.SaveChanges();                 
                }
                else 
                {
                    HairScope hairscop = new HairScope();
                    hairscop.HairScopeResult = JsonConvert.SerializeObject(HairScope);
                    hairscop.HairProfileId = HairScope.HairProfileId;
                    hairscop.UserId = HairScope.AccessCode;
                    hairscop.QAVersion = HairScope.QAVersion;
                    hairscop.CreatedOn = DateTime.Now;
                    context.HairScope.Add(hairscop);
                    context.SaveChanges();
                }
                
                return HairScope;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: AddNewHairScope, UserId:" + HairScope.AccessCode+ ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public HairScopeModel GetHairScopeResultData(HairScopeModelParameters HairScopeParam)
        {
            try
            {

                var Hairscope = context.HairScope.Where(x => x.UserId.ToString().ToLower() == HairScopeParam.UserID.ToString().ToLower()).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                if (Hairscope != null)
                {                 
                    return JsonConvert.DeserializeObject<HairScopeModel>(Hairscope.HairScopeResult);
                }
                else
                {
                    return null;
                }       
       
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetHairScopeResultData, UserId:" + HairScopeParam.UserID+ ", Error: " + Ex.Message, Ex);
                return null;
            }
        }
        public HairScopeModel GetHairScopeResultDataWeb(HairScopeModel HairScopeParam)
        {
            try
            {

                var Hairscope = context.HairScope.Where(x => x.UserId.ToString().ToLower() == HairScopeParam.AccessCode.ToString().ToLower()).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                if (Hairscope != null)
                {
                        return JsonConvert.DeserializeObject<HairScopeModel>(Hairscope.HairScopeResult);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetHairScopeResultDataWeb, UserId:" + HairScopeParam.AccessCode.ToString() + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }
    }
}
