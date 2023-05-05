using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.DTOs;
using webapi.Model;
using webapi_test.controllers;
using webapi_test.utils;

namespace webapi_test
{
    public class CommentsControllerTests
    {

        [Fact]
        public async Task GetCommentsOK()
        {
            // Arrange
            var controller = await new CommentsControllerMoq().Instance();

            // Act
            var result = await controller.GetComments();

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<List<Comment>>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<List<Comment>>(actual);
        }


        [Fact]
        public async Task GetCommentOK()
        {
            // Arrange
            var controller = await new CommentsControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var comment = db.Comments.First();

            // Act
            var result = await controller.GetComment(comment.CommentId);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Comment>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Comment>(actual);
        }


        [Fact]
        public async Task GetCommentsByPostIdOK()
        {
            // Arrange
            var controller = await new CommentsControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var post = db.Posts.First();

            // Act
            var result = await controller.GetCommentsByPostId(post.PostId);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }


        [Fact]
        public async Task UpdateCommentOK()
        {
            // Arrange
            var controller = await new CommentsControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var sessionUser = db.Users.FirstOrDefault(x => x.Email.Equals("user1@mail.com"));

            var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ""+sessionUser?.UserId)
            }));
            controller.HttpContext.User = user;

            var comment = db.Comments.First();

            var commentDto = new CommentUpdateDTO("New comment");


            // Act
            var result = await controller.UpdateComment(comment.CommentId,commentDto);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Comment>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Comment>(actual);
            Assert.NotEqual(actual.Content,comment.Content);
        }


        [Fact]
        public async Task CreateCommentOK()
        {
            // Arrange
            var controller = await new CommentsControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var sessionUser = db.Users.FirstOrDefault(x => x.Email.Equals("user1@mail.com"));

            var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ""+sessionUser?.UserId)
            }));
            controller.HttpContext.User = user;

            var post = db.Posts.First();

            var commentDto = new CommentCreateDTO()
            {
                Content = "New comment",
                PostId = post.PostId
            };

            var commentsPrev = await controller.GetComments(); 


            // Act
            var result = await controller.CreateComment(commentDto);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Comment>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Comment>(actual);

            // Check state: comments + 1
            var comments = await controller.GetComments(); 

            var commentsResponse = Assert.IsType<ObjectResult>(comments.Result);          // casting result to ObjectResult
            var commentsValue = Assert.IsAssignableFrom<IEnumerable<Comment>>(commentsResponse.Value);

            var commentsPrevResponse = Assert.IsType<ObjectResult>(commentsPrev.Result);          // casting result to ObjectResult
            var commentsPrevValue = Assert.IsAssignableFrom<IEnumerable<Comment>>(commentsPrevResponse.Value);

            Assert.Equal(commentsPrevValue.Count() + 1, commentsValue.Count());

        }


        [Fact]
        public async Task DeleteCommentOK()
        {
            // Arrange
            var controller = await new CommentsControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var sessionUser = db.Users.FirstOrDefault(x => x.Email.Equals("user1@mail.com"));

            var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ""+sessionUser?.UserId)
            }));
            controller.HttpContext.User = user;

            var comment = db.Comments.First();

            var commentsPrev = await controller.GetComments(); 


            // Act
            var result = await controller.DeleteComment(comment.CommentId);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Guid>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Guid>(actual);

            // Check state: comments - 1
            var comments = await controller.GetComments(); 

            var commentsResponse = Assert.IsType<ObjectResult>(comments.Result);          // casting result to ObjectResult
            var commentsValue = Assert.IsAssignableFrom<IEnumerable<Comment>>(commentsResponse.Value);

            var commentsPrevResponse = Assert.IsType<ObjectResult>(commentsPrev.Result);          // casting result to ObjectResult
            var commentsPrevValue = Assert.IsAssignableFrom<IEnumerable<Comment>>(commentsPrevResponse.Value);

            Assert.Equal(commentsPrevValue.Count() - 1, commentsValue.Count());

        }


    }
}