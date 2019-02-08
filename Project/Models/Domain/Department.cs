using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Department : BaseEntity
    {
        [Required]
        public string Name { set; get; }

        [Required]
        public int OfferingId { set; get; }
        [ForeignKey(nameof(OfferingId))]
        public virtual Offering Offering { set; get; }
    }
}