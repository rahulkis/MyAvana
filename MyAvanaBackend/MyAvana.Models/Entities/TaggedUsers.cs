using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace MyAvana.Models.Entities
{
    public class TaggedUsers
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
