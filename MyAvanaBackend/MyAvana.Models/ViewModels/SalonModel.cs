using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class SalonModel
    {

        public int SalonId { get; set; }


        public string SalonName { get; set; }

   
        public string Address { get; set; }

  
        public string EmailAddress { get; set; }

  
        public string PhoneNumber { get; set; }

        
        public bool IsActive { get; set; }

        public bool IsPublicNotes { get; set; }

       
        public int TotalRecords { get; set; }
     
        public string PublicNotes { get; set; }
        public string SalonLogo { get; set; }

     
        public IFormFile File { get; set; }
    }


    public class SalonLoginDetail
    {
        public string SalonName { get; set; }
        public string SalonLogo { get; set; }
    }
}
