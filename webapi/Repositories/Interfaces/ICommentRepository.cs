using webapi.DTOs;
using webapi.Model;

namespace webapi.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>?> GetAll();
        Task<Comment?> GetById(Guid id);
        Task<Comment> CreateComment(CommentCreateDTO commentDto, UserDTO user);
        Task<Comment?> UpdateComment(Guid id, UpdateCommentDTO commentDto);
        Task DeleteComment(Guid id);
        Task<List<Comment>> GetAllCommentsByUser(UserDTO user);
        Task<IEnumerable<Comment>?> GetAllCommentsByPostId(Guid id);

    }
}
