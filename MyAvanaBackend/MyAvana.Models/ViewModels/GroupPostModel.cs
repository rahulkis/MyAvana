using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{    
    public class GroupPostModelParaMeters
    {
        public int Id { get; set; }
        public string HairType { get; set; }
        public String UserEmail { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string AudioUrl { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public ICollection<MyAvana.Models.Entities.Comments> Comments { get; set; }
        public List<TaggedUsersList> TaggedUsersList { get; set; }
    }
    public class TaggedUsersList
    {
        public int? PostId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }

    }
}
