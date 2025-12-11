using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class UserLoginRequest
    {
        [Required, StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required, StringLength(15, MinimumLength = 4)]
        public string Password { get; set; }
    }
}
