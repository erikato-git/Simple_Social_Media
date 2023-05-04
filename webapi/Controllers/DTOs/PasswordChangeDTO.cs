using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Simple_Social_Media_App.Controllers.DTOs
{
    public class PasswordChangeDTO
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
