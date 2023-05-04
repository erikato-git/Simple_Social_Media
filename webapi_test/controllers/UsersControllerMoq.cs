

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using webapi.Controllers;
using webapi.Model;
using webapi.Repositories;
using webapi.Utils;
using webapi_test.utils;

namespace webapi_test.controllers
{
    public class UsersControllerMoq
    {
        public async Task<UsersController> Instance()
        {
            var fakeDb = await new DbContextMoq().Instance();

            // --- Automapper ---
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperService());   
            });
            var mapper = config.CreateMapper();

            var userRepository = new UserRepository(fakeDb,mapper);

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var services = new ServiceCollection();
            services.AddSingleton<IAuthenticationService>(authServiceMock.Object);

            var controller = new UsersController(userRepository)
            {
                ControllerContext = new ControllerContext {
                    HttpContext = new DefaultHttpContext {
                        RequestServices = services.BuildServiceProvider()
                    }
                }
            };
            
            return controller;
        }
    }
}