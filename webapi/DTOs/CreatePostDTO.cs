using webapi.Model;

namespace webapi.DTOs
{
    public class CreatePostDTO
    {
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
