
using System.ComponentModel.DataAnnotations;

namespace webapi.DTOs
{
    public class LoginDTO
    {
        public LoginDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required]
        public string Email { get; }
        [Required]
        public string Password { get; }
    }
}
