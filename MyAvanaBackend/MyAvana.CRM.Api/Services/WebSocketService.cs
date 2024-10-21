using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvanaApi.Contract;
using MyAvanaApi.Models.Entities;

namespace MyAvana.CRM.Api.Services
{
    public class WebSocketService:IWebSocket
    {
        private readonly ITokenService _tokenService;
        private readonly AvanaContext _context;
        private readonly Logger.Contract.ILogger _logger;      
        public WebSocketService(AvanaContext context, Logger.Contract.ILogger logger)
        {            
            _context = context;
            _logger = logger;           
        }     
        public bool UpdateUserLastPing(string userid)
        {
            UserEntity user = _context.UserEntity.Where(x => x.Id.ToString().ToUpper() == userid.ToUpper()).LastOrDefault();
            if (user != null)
            {
                user.LastPingTime = DateTime.UtcNow;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
