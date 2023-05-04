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


        // Needs HttpContex setup

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

            var commentDto = new CommentUpdateDTO()
            {
                Content = "New comment"
            };

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

            // Act
            var result = await controller.CreateComment(commentDto);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Comment>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Comment>(actual);
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

            // Act
            var result = await controller.DeleteComment(comment.CommentId);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Guid>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Guid>(actual);
        }


    }
}