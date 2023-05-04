using Simple_Social_Media_App.DataAccess.Model;
using System.ComponentModel.DataAnnotations;

namespace Simple_Social_Media_App.Controllers.DTOs
{
    public class CommentCreateDTO
    {
        public string Content { get; set; }

        [Required]
        public string PostId { get; set; }

    }
}
