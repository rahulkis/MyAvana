using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.ViewModels
{
    public class Authentication
    {
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class AlexaLinkingModel: Authentication
    {
        public string client_id { get; set; }
        public string response_type { get; set; }
        public string scope { get; set; }
        public string state { get; set; }
        public string redirect_uri { get; set; }
    }
}
