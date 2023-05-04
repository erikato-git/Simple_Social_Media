
using System.ComponentModel.DataAnnotations;

namespace webapi.DTOs
{
    public class CommentUpdateDTO
    {
        [Required]
        public string Content { get; set; }
    }
}
