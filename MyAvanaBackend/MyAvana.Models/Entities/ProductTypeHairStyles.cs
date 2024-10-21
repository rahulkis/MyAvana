using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ProductTypeHairStyles
    {
        public int Id { get; set; }
        [ForeignKey("Id")]
        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }
        [ForeignKey("HairStyleId")]
        public int HairStyleId { get; set; }
        public virtual HairStyles HairStyles { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
