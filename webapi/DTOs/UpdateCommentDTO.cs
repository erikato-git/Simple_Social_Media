
using System.ComponentModel.DataAnnotations;

namespace webapi.DTOs
{
    public class UpdateCommentDTO
    {
        [Required]
        public string Content { get; set; }
    }
}
