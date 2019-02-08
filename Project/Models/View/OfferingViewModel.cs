using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class OfferingViewModel : BaseEntity
    {
        [Required]
        public string Name { set; get; }
    }
}