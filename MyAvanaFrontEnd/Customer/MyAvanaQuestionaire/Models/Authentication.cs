using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaQuestionaire.Models
{
    public class Authentication
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        public string token { get; set; }
        public bool RememberMe { get; set; }
    }
}
