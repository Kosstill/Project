using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class FamilyViewModel : BaseEntity
    {
        [Required]
        public string Name { set; get; }
    }
}