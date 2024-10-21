using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAvana.Models.Entities
{
    public class SalonsOwner
    {
        [Key]
        public int SalonOwnerId { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [ForeignKey("SalonId")]
        public int SalonId { get; set; }
        public bool IsActive { get; set; }

        public virtual WebLogin User { get; set; }
        public virtual Salons Salon { get; set; }
    }
}
