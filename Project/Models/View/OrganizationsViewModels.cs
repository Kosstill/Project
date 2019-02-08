using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class OrganizationViewModel : BaseEntity
    {
        [Required]
        public string Name { set; get; }
    }

    public class OrganizationDetailsViewModel : OrganizationViewModel
    {
        [Required]
        public int Code { set; get; }
        [Required]
        public string OrganizationType { set; get; }
        [Required]
        public string Owner { set; get; }
    }
}