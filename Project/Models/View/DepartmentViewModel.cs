using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class DepartmentViewModel : BaseEntity
    {
        [Required]
        public string Name { set; get; }
    }
}