using System.ComponentModel.DataAnnotations;
using System;

namespace Identity.Models
{
    public class OrganizationSetup
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Motto { get; set; }
        public string logo { get; set; }
        public string Abbreviation { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string MapLink { get; set; }
        public DateTime CreatedDate { get; set; }
        public OrganizationSetup()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
        }
    }
}
