using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class CountryViewModel : BaseEntity
    {
        [Required]
        public string Name { set; get; }
    }

    public class CountryDetailsViewModel : CountryViewModel
    {
        [Required]
        public int Code { set; get; }
    }
}