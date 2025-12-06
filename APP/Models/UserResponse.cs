using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class UserResponse : Response
    {
        [DisplayName("User Name")]
        public string UserName { get; set; }

        public string Password { get; set; }
        
        public bool IsActive { get; set; }

        [DisplayName("Status")]
        public string IsActiveF { get; set; }

        public List<int> RoleIds { get; set; }

        public string Roles { get; set; }
    }
}
