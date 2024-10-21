using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaQuestionaire.Services
{
    public class AuthorizationPolicyRequirement : IAuthorizationRequirement
    {
        public bool IsPaid { get; set; }
        public AuthorizationPolicyRequirement(bool paid)
        {
            IsPaid = paid;
        }
    }
}
