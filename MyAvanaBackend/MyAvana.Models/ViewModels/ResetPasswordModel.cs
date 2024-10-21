using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvanaApi.Models.ViewModels
{
    public class ResetPasswordModel
    {
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}
