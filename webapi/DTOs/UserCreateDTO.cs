using System.ComponentModel.DataAnnotations;

namespace webapi.DTOs
{
    public class UserCreateDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Full_Name { get; set; }
        [Required]
        public string Password { get; set; }

    }
}


        // public Guid UserId { get; set; }
        // [Required]
        // public string Email { get; set; } = string.Empty;
        // [Required]
        // public string Password { get; set; } = string.Empty;
        // public int Salt { get; set; }
        // [Required]
        // public string Full_Name { get; set; } = string.Empty;
        // public byte[]? Profile_Picture { get; set; }
        // public DateTime? DateOfBirth { get; set; } = DateTime.UtcNow;
        // public string? Description { get; set; }
        // public ICollection<Post>? Posts { get; set; }
        // public ICollection<Comment>? Comments { get; set; }
