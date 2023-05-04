
using System.ComponentModel.DataAnnotations;

namespace Simple_Social_Media_App.Controllers.DTOs
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
