using webapi.Model;
using System.ComponentModel.DataAnnotations;

namespace webapi.DTOs
{
    public class CommentCreateDTO
    {
        public string Content { get; set; }

        [Required]
        public string PostId { get; set; }

    }
}
