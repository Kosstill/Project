using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Country : BaseEntity
    {
        [Required]
        public string Name { set; get; }
        [Required]
        public long Code { set; get; }

        public int? OrganizationId { set; get; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { set; get; }

        public virtual ICollection<Business> Businesses { set; get; }
    }
}