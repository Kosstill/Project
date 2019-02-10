using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Organization : BaseEntity
    {
        [Required]
        public string Name { set; get; }
        [Required]
        public int Code { set; get; }
        [Required]
        public string OrganizationType { set; get; }
        [Required]
        public string Owner { set; get; }

        public ICollection<Country> Countries { set; get; }
    }
}