using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
        public class UserRoutineHairCareModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public IFormFile FormImage { get; set; }
            public string Image { get; set; }
        public string UserName { get; set; }
        public DateTime TrackDate { get; set; }
            public int RoutineTrackerId { get; set; }
            public bool IsProduct { get; set; }
            public bool IsIngredient { get; set; }
            public bool IsRegimen { get; set; }
        }
}
