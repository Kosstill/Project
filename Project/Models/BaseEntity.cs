namespace Project.Models
{
    using System.ComponentModel.DataAnnotations;
    public class BaseEntity
    {
        [Required]
        public int Id { set; get; }
    }
}