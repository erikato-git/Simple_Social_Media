using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webapi.Model
{
    public class Post
    {
        [Key]
        public Guid PostId { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid? UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
