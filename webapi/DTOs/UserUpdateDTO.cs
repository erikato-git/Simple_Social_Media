using System.ComponentModel.DataAnnotations;

namespace webapi.DTOs
{
    public class UserUpdateDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Full_Name { get; set; }
        public string Description  { get; set; }
        public string Profile_Picture { get; set; }
    }
}
