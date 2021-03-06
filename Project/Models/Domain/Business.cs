using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Business : BaseEntity
    {
        [Required]
        public string Name { set; get; }

        [Required]
        public int CountryId { set; get; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { set; get; }

        public ICollection<Family> Families { set; get; }
    }
}