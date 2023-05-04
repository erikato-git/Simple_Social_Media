using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Simple_Social_Media_App.Controllers.DTOs;
using Simple_Social_Media_App.DataAccess;
using Simple_Social_Media_App.DataAccess.Model;
using Simple_Social_Media_App.Repositories.Interfaces;

namespace Simple_Social_Media_App.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CommentRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }


        public async Task<List<Comment>?> GetAll()
        {
            return await _dataContext.Comments.ToListAsync();
        }

        public async Task<Comment?> GetById(Guid id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id is empty");
            }

            var comment = await _dataContext.Comments.FindAsync(id);

            if(comment == null)
            {
                return null;
            }

            return comment;
        }

        public async Task<List<Comment>> GetAllCommentsByUser(UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("post is null");
            }

            var commentsByUser = await _dataContext.Comments.Where(x => x.UserId == user.UserId).ToListAsync();

            return commentsByUser;
        }


        public async Task<Comment> CreateComment(CommentCreateDTO commentDto, UserDTO userDto)
        {
            if ( commentDto == null || userDto == null )
            {
                throw new ArgumentNullException("comment, user or post is null");
            }

            var comment = _mapper.Map<Comment>(commentDto);
            comment.UserId= userDto.UserId;
            comment.PostId = new Guid(commentDto.PostId);

            var post = await _dataContext.Posts.FindAsync(comment.PostId);
            comment.Post = post;

            await _dataContext.Comments.AddAsync(comment);
            _dataContext.SaveChanges();

            return comment;
        }

        public async Task DeleteComment(Guid id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id is empty");
            }

            var comment = await _dataContext.Comments.FindAsync(id);

            if (comment == null)
            {
                throw new Exception("Comment not found");
            }

            _dataContext.Comments.Remove(comment);
            _dataContext.SaveChanges();

            return;
        }


        public async Task<Comment?> UpdateComment(Guid id, UpdateCommentDTO commentDto)
        {
            if(String.IsNullOrEmpty(id.ToString()) || commentDto == null)
            {
                throw new ArgumentNullException("id or post is null");
            }

            var found = await _dataContext.Comments.FindAsync(id);

            if(found == null)
            {
                return null;
            }

            _mapper.Map(commentDto, found);
            _dataContext.SaveChanges();

            return found;

        }

        public async Task<IEnumerable<Comment>?> GetAllCommentsByPostId(Guid id)
        {
            if(String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id is empty");
            }

            var commentsByPostId = await _dataContext.Comments.Where(x => x.PostId == id).ToListAsync();
            var sortedComments = commentsByPostId.OrderBy(x => x.CreatedAt);

            return sortedComments;            
        }

    }
}
