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
    public class UsersControllerTests
    {

        [Fact]
        public async Task GetUsersOK()
        {
            // Arrange
            var controller = await new UsersControllerMoq().Instance();

            // Act
            var result = await controller.GetUsers();

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<List<UserDTO>>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<List<UserDTO>>(actual);
        }


        [Fact]
        public async Task GetUserOK()
        {
            // Arrange
            var controller = await new UsersControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var user = db.Users.First();

            // Act
            var result = await controller.GetUser(user.UserId);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<UserDTO>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<UserDTO>(actual);
        }


        // State
        [Fact]
        public async Task PostUserOK()
        {
            // Arrange
            var controller = await new UsersControllerMoq().Instance();
            var newUser = new UserCreateDTO()
            {
                Email = "new@mail.com",
                Password = "123",
                Full_Name = "new user",
            };

            var usersPrev = await controller.GetUsers(); 


            // Act
            var result = await controller.PostUser(newUser);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result.Result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<UserDTO>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<UserDTO>(actual);

            // Check state: users + 1
            var users = await controller.GetUsers(); 

            var usersResponse = Assert.IsType<ObjectResult>(users.Result);          // casting result to ObjectResult
            var usersValue = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(usersResponse.Value);

            var usersPrevResponse = Assert.IsType<ObjectResult>(usersPrev.Result);          // casting result to ObjectResult
            var usersPrevValue = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(usersPrevResponse.Value);

            Assert.Equal(usersPrevValue.Count() + 1, usersValue.Count());

        }


        [Fact]
        public async Task FindUserByEmailOK()
        {
            // Arrange
            var controller = await new UsersControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var user = db.Users.First();

            // Act
            var result = await controller.FindUserByEmail(user.Email);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<UserDTO>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<UserDTO>(actual);
        }


        [Fact]
        public async Task CheckEmailIsFreeOK()
        {
            // Arrange
            var controller = await new UsersControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var user = db.Users.First();
            var falseMail = "hgcyusdnjdcoilksamdakdsa";

            // Act
            var resultTrue = await controller.CheckEmailIsFree(user.Email);
            var resultFalse = await controller.CheckEmailIsFree(falseMail);

            // Assert
            Assert.NotNull(resultTrue);
            Assert.NotNull(resultFalse);

            var response1 = Assert.IsType<ObjectResult>(resultTrue);          // casting result to ObjectResult
            var actual1 = Assert.IsAssignableFrom<bool>(response1.Value);

            var response2 = Assert.IsType<ObjectResult>(resultFalse);          // casting result to ObjectResult
            var actual2 = Assert.IsAssignableFrom<bool>(response2.Value);

            Assert.Equal(StatusCodes.Status200OK, response1.StatusCode);
            Assert.IsType<bool>(actual1);
            Assert.False(actual1);
            
            Assert.Equal(StatusCodes.Status200OK, response2.StatusCode);
            Assert.IsType<bool>(actual2);
            Assert.True(actual2);
        }


        [Fact]
        public async Task LogInOK()
        {
            // Arrange
            var controller = await new UsersControllerMoq().Instance();
            
            var db = await new DbContextMoq().Instance();

            var newUser = new UserCreateDTO()
            {
                Email = "new@mail.com",
                Password = "123",
                Full_Name = "new user",
            };

            await controller.PostUser(newUser);
            var loginDto = new LoginDTO("new@mail.com","123"); 

            // Act
            var result = await controller.LogIn(loginDto);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<UserDTO>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<UserDTO>(actual);
        }


        [Fact]
        public async Task DeleteUserOK()
        {
            // Arrange
            var controller = await new UsersControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var userToBeDeleted = db.Users.First();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ""+userToBeDeleted.UserId)
            }));

            controller.HttpContext.User = user;

            var usersPrev = await controller.GetUsers(); 


            // Act
            var result = await controller.DeleteUser(userToBeDeleted.UserId);

            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<Guid>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<Guid>(actual);

            // Check state: users + 1
            var users = await controller.GetUsers(); 

            var usersResponse = Assert.IsType<ObjectResult>(users.Result);          // casting result to ObjectResult
            var usersValue = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(usersResponse.Value);

            var usersPrevResponse = Assert.IsType<ObjectResult>(usersPrev.Result);          // casting result to ObjectResult
            var usersPrevValue = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(usersPrevResponse.Value);

            Assert.Equal(usersPrevValue.Count() - 1, usersValue.Count());

        }


        [Fact]
        public async Task ReturnLoggedInUserWhileSessionHasntExpiredOK()
        {
            // Arrange
            var controller = await new UsersControllerMoq().Instance();
            var db = await new DbContextMoq().Instance();
            var sessionUser = db.Users.First();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ""+sessionUser.UserId)
            }));

            controller.HttpContext.User = user;

            // Act
            var result = await controller.ReturnLoggedInUserWhileSessionHasntExpired();
            // Assert
            Assert.NotNull(result);

            var response = Assert.IsType<ObjectResult>(result);          // casting result to ObjectResult
            var actual = Assert.IsAssignableFrom<UserDTO>(response.Value);

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.IsType<UserDTO>(actual);
            Assert.Equal(sessionUser.UserId,actual.UserId);
        }


        // ChangePassword
        // TODO: Testen går galt, når der skal tjekkes for om 'oldpassword' matcher eksisterende hashed password for brugeren hvis Id matcher med HttpContext
        // [Fact]
        // public async Task ChangePasswordOK()
        // {
        //     // Arrange
        //     var controller = await new UsersControllerMoq().Instance();
        //     var db = await new DbContextMoq().Instance();
        //     var sessionUser = db.Users.First();

        //     var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        //     {
        //         new Claim(ClaimTypes.NameIdentifier, ""+sessionUser.UserId)
        //     }));

        //     controller.HttpContext.User = user;

        //     var passwordDTO = new PasswordChangeDTO()
        //     {
        //         OldPassword = sessionUser.Password,
        //         NewPassword = "234"
        //     };

        //     // Act
        //     var result = await controller.ChangePassword(passwordDTO);
        //     // Assert
        //     Assert.NotNull(result);

        //     var response = Assert.IsType<ObjectResult>(result);          // casting result to ObjectResult
        //     var actual = Assert.IsAssignableFrom<bool>(response.Value);

        //     Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        //     Assert.IsType<bool>(actual);
        // }

        // Logout
        // TODO
        // [HttpPost("/logout")]


    }
}