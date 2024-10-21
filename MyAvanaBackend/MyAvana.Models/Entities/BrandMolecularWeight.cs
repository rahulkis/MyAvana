using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyAvana.Models.Entities
{
    public class BrandMolecularWeight
    {
        [Key]
        public int BrandMolecularWeightId { get; set; }

        [ForeignKey("MolecularWeightId")]
        public int MolecularWeightId { get; set; }
        public virtual MolecularWeight MolecularWeight { get; set; }

        [ForeignKey("BrandId")]
        public int BrandId { get; set; }
        public virtual Brands Brands { get; set; }

        public bool IsActive { get; set; }
    }
}
