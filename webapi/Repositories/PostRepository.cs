using AutoMapper;
using Microsoft.EntityFrameworkCore;
using webapi.DTOs;
using webapi.Model;
using webapi.Repositories.Interfaces;

namespace webapi.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public PostRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        // Queries

        public async Task<IEnumerable<Post>?> GetAll()
        {
            var posts = await _dataContext.Posts.ToListAsync();
            var sorted = posts.OrderByDescending(x => x.CreatedAt);
            return sorted;
        }

        public async Task<Post?> GetById(Guid id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id is empty");
            }

            var post = await _dataContext.Posts.FindAsync(id);

            return post;
        }

        // Commands

        public async Task<Post?> UpdatePost(Guid id, PostUpdateDTO postUpdateDto)
        {
            if (String.IsNullOrEmpty(id.ToString()) || postUpdateDto == null)
            {
                throw new ArgumentNullException("id is empty or dto is null");
            }

            var found = await _dataContext.Posts.FindAsync(id);

            if(found == null)
            {
                throw new Exception("Couldn't find logged in user");
            }

            _mapper.Map(postUpdateDto, found);
            _dataContext.SaveChanges();

            return found;
        }

        public async Task<Post> CreatePost(CreatePostDTO postDto, UserDTO userDto)
        {
            if(postDto == null || userDto == null)
            {
                throw new ArgumentNullException("post or user is null");
            }

            var user = _mapper.Map<User>(userDto);

            var post = _mapper.Map<Post>(postDto);
            post.UserId = user.UserId;

            await _dataContext.Posts.AddAsync(post);
            _dataContext.SaveChanges();

            return post;
        }

        public async Task DeletePost(Guid id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id is empty");
            }

            var post = await _dataContext.Posts.FindAsync(id);

            if (post == null)
            {
                throw new Exception("User not found");
            }

            _dataContext.Posts.Remove(post);
            _dataContext.SaveChanges();

            return;
        }

        public async Task<IEnumerable<Post>?> GetAllPostByUserId(Guid id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id is empty");
            }

            var postsByUserId = await _dataContext.Posts.Where(x => x.UserId == id).ToListAsync();
            var sortedPosts = postsByUserId.OrderByDescending(x => x.CreatedAt);

            // TODO: Divide it in chunks later on by 'skip' and 'take' (eg. when using infinit-scrollbar in frontend) and better practice not sending a lot of data pr request
            return sortedPosts;
        }
    }
}
