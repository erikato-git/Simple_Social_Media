
using System.ComponentModel.DataAnnotations;

namespace webapi.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; }
        [Required]
        public string Password { get; }

        public LoginDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }

    }
}
