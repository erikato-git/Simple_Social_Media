using Simple_Social_Media_App.DataAccess.Model;

namespace Simple_Social_Media_App.Controllers.DTOs
{
    public class CreatePostDTO
    {
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
