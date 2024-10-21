using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using MyAvanaQuestionaireModel;

namespace VideoInterviewingWeb.Services
{
    public  class AuthenticationService : Controller
    {
        public  async Task<bool> setIdentityClaims(UserModel loginModel)
        {
            var claims = new List<Claim>
                                            {
                                              new Claim(ClaimTypes.Email, loginModel.Email),
                                              new Claim("IsPaid", loginModel.IsPaid.ToString())
                                            };

            var claimsIdentity = new ClaimsIdentity(
              claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity), authProperties);
            return true;
        }
    }
}
