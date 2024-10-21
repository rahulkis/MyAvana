using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
namespace MyAvana.Models.ViewModels
{  
    public class UserList
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string imageurl { get; set; }        

    }
    public class VideoCallModel
    {
        public string RoomId { get; set; }
        public string RoomClass { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }

    }
}
