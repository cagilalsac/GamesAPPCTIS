using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class RoleResponse : Response
    {
        public string Name { get; set; }

        [DisplayName("User Count")]
        public int UserCount { get; set; }

        public List<UserResponse> Users { get; set; }
    }
}
