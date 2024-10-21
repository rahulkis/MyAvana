using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvanaQuestionaireModel
{
	public class DiscountCodeListModel
	{
		
		public string DiscountCode { get; set; }
		
		public DateTime? CreatedDate { get; set; }
		
		public DateTime? ExpireDate { get; set; }
		
		public int DiscountPercent { get; set; }
		
		public bool IsActive { get; set; }

	}
}
