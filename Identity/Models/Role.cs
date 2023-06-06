using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class Role
    {
        public string RoleId { get; set; }

        [Required(ErrorMessage = "The role name is required.")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
