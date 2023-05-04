using Simple_Social_Media_App.Controllers.DTOs;
using Simple_Social_Media_App.DataAccess.Model;

namespace Simple_Social_Media_App.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>?> GetAll();
        Task<Post?> GetById(Guid id);
        Task<Post> CreatePost(CreatePostDTO postDto, UserDTO user);
        Task<Post?> UpdatePost(Guid id, PostUpdateDTO postUpdateDto);
        Task DeletePost(Guid id);
        Task<IEnumerable<Post>?> GetAllPostByUserId(Guid id);

    }
}
