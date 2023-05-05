
using System.ComponentModel.DataAnnotations;

namespace webapi.DTOs
{
    public class CommentUpdateDTO
    {
        [Required]
        public string Content { get; set; }
        public CommentUpdateDTO(string content)
        {
            Content = content;
        }

    }
}
