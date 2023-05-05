using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        [Fact]
        public async Task UpdatePostOK()
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

            var postsPrev = await controller.GetPosts(); 


            // Act
            var result = await controller.CreatePost(postDto);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Post>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Post>(actual);
            Assert.Equal(postDto.Content,actual.Content);

            // Check state: users + 1
            var posts = await controller.GetPosts(); 

            var postsResponse = Assert.IsType<ObjectResult>(posts.Result);          // casting result to ObjectResult
            var postsValue = Assert.IsAssignableFrom<IEnumerable<Post>>(postsResponse.Value);

            var postsPrevResponse = Assert.IsType<ObjectResult>(postsPrev.Result);          // casting result to ObjectResult
            var postsPrevValue = Assert.IsAssignableFrom<IEnumerable<Post>>(postsPrevResponse.Value);

            Assert.Equal(postsPrevValue.Count() + 1, postsValue.Count());

        }


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

            var postsPrev = await controller.GetPosts(); 


            // Act
            var result = await controller.DeletePost(postId);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Guid>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Guid>(actual);


            // Check state: users - 1
            var posts = await controller.GetPosts(); 

            var postsResponse = Assert.IsType<ObjectResult>(posts.Result);          // casting result to ObjectResult
            var postsValue = Assert.IsAssignableFrom<IEnumerable<Post>>(postsResponse.Value);

            var postsPrevResponse = Assert.IsType<ObjectResult>(postsPrev.Result);          // casting result to ObjectResult
            var postsPrevValue = Assert.IsAssignableFrom<IEnumerable<Post>>(postsPrevResponse.Value);

            Assert.Equal(postsPrevValue.Count() - 1, postsValue.Count());
           
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