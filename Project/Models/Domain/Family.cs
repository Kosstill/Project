using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Family : BaseEntity
    {
        [Required]
        public string Name { set; get; }

        [Required]
        public int BusinessId { set; get; }
        [ForeignKey(nameof(BusinessId))]
        public virtual Business Business { set; get; }

        public virtual ICollection<Offering> Offerings { set; get; }
    }
}