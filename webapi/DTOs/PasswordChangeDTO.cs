using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webapi.DTOs
{
    public class PasswordChangeDTO
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

        public PasswordChangeDTO(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}
