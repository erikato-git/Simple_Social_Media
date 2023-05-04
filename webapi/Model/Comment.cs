using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webapi.Model
{
    public class Comment
    {
        [Key]
        public Guid CommentId { get; set; }
        [Required]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid? PostId { get; set; }
        [JsonIgnore]
        public Post? Post { get; set; }
        public Guid? UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
