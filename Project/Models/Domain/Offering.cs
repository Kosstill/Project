using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Offering : BaseEntity
    {
        [Required]
        public string Name { set; get; }

        [Required]
        public int FamilyId { set; get; }
        [ForeignKey(nameof(FamilyId))]
        public Family Family { set; get; }

        public virtual ICollection<Department> Departments { set; get; }
    }
}