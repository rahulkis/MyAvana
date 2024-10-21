using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class HairTypeUserEntity
    {
        public string HairType { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; }
    }

    public class GroupModel
    {
        public string UserEmail { get; set; }
    }
}
