using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.DTOs;
using webapi.Model;
using webapi.Repositories.Interfaces;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostsController(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }


        [HttpGet("/get_all_posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            try
            {
                var result = await _postRepository.GetAll();
                if (result == null)
                {
                    return NotFound();
                }
                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/get_post/{id}")]
        public async Task<ActionResult<Post>> GetPost(Guid id)
        {
            try
            {
                var result = await _postRepository.GetById(id);
                if (result == null)
                {
                    return NotFound();
                }
                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/update_post/{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, PostUpdateDTO postUpdateDto)
        {
            try
            {
                var findPost = await _postRepository.GetById(id);

                if(findPost == null) { return NotFound("Couldn't find post"); }

                var userId = findPost.UserId;

                var loginId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (String.IsNullOrEmpty(loginId)) { return BadRequest("You need to login"); }

                // Check that logged in user can't edit another user's post
                if (!loginId.Equals(userId.ToString())) { return BadRequest("You can only edit your own posts"); }

                var result = await _postRepository.UpdatePost(id,postUpdateDto);
                if (result == null)
                {
                    return NotFound("User not found");
                }
                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/create_post")]
        public async Task<ActionResult<Post>> CreatePost(CreatePostDTO postDto)
        {
            try
            {
                var loginId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                if(loginId == null) 
                { 
                    return BadRequest("You need to log in again"); 
                }

                var currentUser = await _userRepository.GetById(Guid.Parse(loginId));

                if(currentUser== null) 
                { 
                    return NotFound("Couldn't find logged in user"); 
                }

                var result = await _postRepository.CreatePost(postDto,currentUser);
                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("/delete_post/{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            try
            {
                var findPost = await _postRepository.GetById(id);

                if (findPost == null) { return NotFound("Couldn't find post"); }

                var userId = findPost.UserId;

                var loginId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (String.IsNullOrEmpty(loginId)) { return BadRequest("You need to log in"); }

                // Check that logged in user can't delete another user's post
                if (!loginId.Equals(userId.ToString())) { return BadRequest("You can only delete your own posts"); }

                await _postRepository.DeletePost(id);
                return StatusCode(200, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/getPostsByUserId/{id}")]
        public async Task<ActionResult<Post>> GetPostsByUserId(Guid id)
        {
            try
            {
                var loginId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (String.IsNullOrEmpty(loginId))
                {
                    return BadRequest("You need to log in again");
                }

                if(!loginId.Equals(id.ToString()))
                {
                    return BadRequest("You were trying to access another user's posts");
                }

                var posts = await _postRepository.GetAllPostByUserId(id);
                return StatusCode(200, posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }

}

