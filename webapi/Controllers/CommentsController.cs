using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webapi.DTOs;
using webapi.Model;
using webapi.Repositories.Interfaces;

namespace webapi.Controllers
{
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;   
        private readonly IMapper _mapper;

        public CommentsController(ICommentRepository commentRepository, IUserRepository userRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }


        [HttpGet("/get_all_comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            try
            {
                var result = await _commentRepository.GetAll();
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

        [HttpGet("/get_comment/{id}")]
        public async Task<ActionResult<Comment>> GetComment(Guid id)
        {
            try
            {
                var result = await _commentRepository.GetById(id);
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

        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/update_comment/{id}")]
        public async Task<IActionResult> UpdateComment(Guid id, CommentUpdateDTO updateCommentDto)
        {
            try
            {
                var findComment = await _commentRepository.GetById(id);

                if (findComment == null) 
                { 
                    return NotFound("Couldn't find comment"); 
                }

                // Check that logged in user is only editting its own comments

                var userId = findComment.UserId;

                var loginId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (String.IsNullOrEmpty(loginId)) { return BadRequest("You need to login"); }

                if (!loginId.Equals(userId.ToString())) { return BadRequest("You can only edit your own comments"); }


                var result = await _commentRepository.UpdateComment(id, updateCommentDto);

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

        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/create_comment")]
        public async Task<ActionResult<Comment>> CreateComment(CommentCreateDTO commentDto)
        {
            try
            {
                var loginId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (loginId == null) { return BadRequest("You need to log in again"); }

                var currentUser = await _userRepository.GetById(Guid.Parse(loginId));

                if (currentUser == null) { return NotFound("Couldn't find logged in user"); }

                var result = await _commentRepository.CreateComment(commentDto, currentUser);
                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("/delete_comment/{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                var findComment = await _commentRepository.GetById(id);

                if (findComment == null) { return NotFound("Couldn't find comment"); }

                var userId = findComment.UserId;

                var loginId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (String.IsNullOrEmpty(loginId)) { return BadRequest("You need to log in again"); }

                if (!loginId.Equals(userId.ToString())) { return BadRequest("You can only delete your own comments"); }

                await _commentRepository.DeleteComment(id);
                return StatusCode(200, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/getCommentsByPostId/{id}")]
        public async Task<ActionResult<Comment>> GetCommentsByPostId(Guid id)
        {
            try
            {
                var posts = await _commentRepository.GetAllCommentsByPostId(id);
                return StatusCode(200, posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
