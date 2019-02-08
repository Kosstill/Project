using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class BusinessViewModel : BaseEntity
    {
        [Required]
        public string Name { set; get; }
    }
}