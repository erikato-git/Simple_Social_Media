using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi;
using webapi.DTOs;
using webapi.Model;
using webapi_test.controllers;
using webapi_test.utils;

namespace webapi_test
{
    public class PostsControllerTests
    {

        [Fact]
        public async Task GetPostsOK()
        {
            // Arrange
            var controller = await new PostsControllerMoq().Instance();

            // Act
            var result = await controller.GetPosts();

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }


        [Fact]
        public async Task GetPostOK()
        {
            // Arrange
            var controller = await new PostsControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var post = db.Posts.First();

            // Act
            var result = await controller.GetPost(post.PostId);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Post>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Post>(actual);
        }


        // Needs HttpContex setup

        [Fact]
        public async Task UpdatePostOK()
        {
            // Arrange
            var controller = await new PostsControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var sessionUser = db.Users.FirstOrDefault(x => x.Email.Equals("user1@mail.com"));

            var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ""+sessionUser.UserId)
            }));
            controller.HttpContext.User = user;

            var postDto = new PostUpdateDTO()
            {
                Content = "New content"
            };
            var postId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA211");
            var oldContent = db.Posts.Find(postId)?.Content;

            // Act
            var result = await controller.UpdatePost(postId,postDto);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Post>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Post>(actual);
            Assert.NotEqual(oldContent,actual.Content);
        }


        [Fact]
        public async Task CreatePostOK()
        {
            // Arrange
            var controller = await new PostsControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var sessionUser = db.Users.FirstOrDefault(x => x.Email.Equals("user1@mail.com"));

            var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ""+sessionUser?.UserId)
            }));
            controller.HttpContext.User = user;

            var postDto = new CreatePostDTO()
            {
                Content = "New post"
            };

            // Act
            var result = await controller.CreatePost(postDto);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Post>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Post>(actual);
            Assert.Equal(postDto.Content,actual.Content);
        }


        // TODO: Tjek at Posts er 1 mindre efter Delete
        [Fact]
        public async Task DeletePostOK()
        {
            // Arrange
            var controller = await new PostsControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var sessionUser = db.Users.FirstOrDefault(x => x.Email.Equals("user1@mail.com"));

            var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ""+sessionUser?.UserId)
            }));
            controller.HttpContext.User = user;

            var postDto = new CreatePostDTO()
            {
                Content = "New post"
            };

            var postId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA211");

            // Act
            var result = await controller.DeletePost(postId);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Guid>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Guid>(actual);
        }


        [Fact]
        public async Task GetPostsByUserIdOK()
        {
            // Arrange
            var controller = await new PostsControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var sessionUser = db.Users.FirstOrDefault(x => x.Email.Equals("user1@mail.com"));

            var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ""+sessionUser?.UserId)
            }));
            controller.HttpContext.User = user;


            // Act
            var result = await controller.GetPostsByUserId(sessionUser.UserId);

            // Assert
            Assert.NotNull(result);
            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }
        
    }

}