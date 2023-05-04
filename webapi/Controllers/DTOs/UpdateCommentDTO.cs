
using System.ComponentModel.DataAnnotations;

namespace Simple_Social_Media_App.Controllers.DTOs
{
    public class UpdateCommentDTO
    {
        [Required]
        public string Content { get; set; }
    }
}
