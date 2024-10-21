using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class CustomerTypeHistory
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("OldCustomerTypeId")]
        public int? OldCustomerTypeId { get; set; }
        public virtual CustomerType OldCustomerType { get; set; }
        [ForeignKey("NewCustomerTypeId")]
        public int NewCustomerTypeId { get; set; }
        public virtual CustomerType NewCustomerType { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("UpdatedByUserId")]
        public int? UpdatedByUserId { get; set; }
        public virtual WebLogin UpdatedBy { get; set; }
        [ForeignKey("CustomerId")]
        public Guid CustomerId { get; set; }
        public UserEntity Customer { get; set; }
        public string Comment { get; set; }

    }
}
