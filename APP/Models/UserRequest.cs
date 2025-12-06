using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class UserRequest : Request
    {
        [Required, StringLength(10)]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required, StringLength(15)]
        public string Password { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        [Required]
        [DisplayName("Roles")]
        public List<int> RoleIds { get; set; }
    }
}
