using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAvanaQuestionaire.Services;

namespace VideoInterviewingWeb.Services
{
    public class AuthorizationPolicyHandlers :
     AuthorizationHandler<AuthorizationPolicyRequirement>
    {
        protected override Task HandleRequirementAsync(
               AuthorizationHandlerContext context,
               AuthorizationPolicyRequirement requirement)
        {
            var redirectContext = context.Resource as AuthorizationFilterContext;
            var user = context.User;
            var claim = context.User.FindFirst("IsPaid");
            if (claim != null)
            {
                var isPaid = bool.Parse(claim?.Value);
                if (isPaid)
                    context.Succeed(requirement);
                else
                {
                    redirectContext.Result = new RedirectToActionResult("packagepayment", "auth", null);
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            return Task.CompletedTask;
        }
    }
}
