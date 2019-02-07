namespace Project.Models
{
    using Microsoft.AspNetCore.Identity;
    public class User : IdentityUser
    {
        public string Name { set; get; }
        public string Surname { set; get; }
        public string Address { set; get; }
    }
}