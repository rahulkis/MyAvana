using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAvana.Models.Entities
{
    public class RecommendedStyleRecipeVideos
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public int MediaLinkEntityId { get; set; }
		public string ThumbNail { get; set; }
		public DateTime? CreatedOn { get; set; }
		public bool? IsActive { get; set; }

		[ForeignKey("HairProfileId")]
		public int HairProfileId { get; set; }

		public virtual HairProfile HairProfile { get; set; }
	}
}
