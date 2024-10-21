using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
	public class HairScope
    {
		[Key]
		public int HairScopeId { get; set; }
		public Guid UserId { get; set; }

		[ForeignKey("HairProfileId")]
		public int? HairProfileId { get; set; }
		public virtual HairProfile HairProfile { get; set; }
		public int? QAVersion { get; set; }
		public string HairScopeResult { get; set; }
		public DateTime CreatedOn { get; set; }		 
	}
}
