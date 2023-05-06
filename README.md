[![Continuous Integration (not deployment)](https://github.com/erikato-git/webapi/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/erikato-git/webapi/actions/workflows/ci-cd.yml)

# Simple Social Media app - ASP.NET Core, React(TypeScript) & Tailwind

### Website:
https://simplesocialmedia.herokuapp.com/
<br><br>
Create another account or use the test-account below:
- Email: new@mail.com
- Password: 123

<br>

### Run application in VS Code:
#### Run in development mode:
Activate database. Run postgres in a docker container. <br>
Make sure docker/docker-desktop is installed on your computer > open up terminal and type:
```
docker run -d -p 5432:5432 -e POSTGRES_USERNAME=postgres -e POSTGRES_PASSWORD=example postgres:latest
```
the environmental variables for POSTGRES_USERNAME and POSTGRES_PASSWORD are configured for the current connection-string in appsettings.json. Make sure the docker container is running by typing ```docker ps``` <br>

Activate server. Navigate terminal to 'webapi/webapi' (same folder as 'webapi.csproj') and type:
```
dotnet run
```
Activate client. Open up a new terminal and navigate to 'webapi/clientapp' in terminal and type:
```
npm install
```
to install all dependencies from package.json and then type:
```
npm start
```
Open up a browser and navigate to url: http://localhost:3000/

#### Run in production mode with docker-compose:
Navigate to same folder as docker-compose.yml in terminal (the root of the project) and type:
```
docker-compose up --build -d
```
Open up browser and navigate to url: http://localhost:5029/

<br>

### Technologies:
The core technologies used to build the application:
- ASP.NET Core
- React (TypeScript)
- Tailwind

<br>

### Solutions:
#### Server:
- AddDbContext configured in three different modes in postgres with dependency injection in Program.cs (can easily be changed to mssql): 
  - 1. Development mode: Configured to one connection-string used for developement.
  - 2. Production mode: Configured to another connection-string used for production.
  - 3. Deployment mode: Configured to antoher connection-string used for deployment for eg. Heroku and flyio.
  
  [Program.cs]

  ```cs
      builder.Services.AddDbContext<DataContext>(options =>
      {
          var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
          string connStr;

          // for 'dotnet run'
          if (env == "Development")
          {
              connStr = builder.Configuration["ConnectionStrings:DevelopmentConnection"];
              options.UseNpgsql(connStr);
          }
          // for 'docker-compose' and deploy to Heroku
          else
          {
              var docker = Environment.GetEnvironmentVariable("Docker_Env");

              if( docker == "Docker" )
              {
                  options.UseNpgsql(builder.Configuration["ConnectionStrings:ProductionConnection"]);
              }
              // Heroku
              else
              {
                  // Use connection string provided at runtime by Heroku.
                  var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                  if(!string.IsNullOrEmpty(connUrl))
                  {
                      // Parse connection URL to connection string for Npgsql
                      connUrl = connUrl.Replace("postgres://", string.Empty);
                      var pgUserPass = connUrl.Split("@")[0];
                      var pgHostPortDb = connUrl.Split("@")[1];
                      var pgHostPort = pgHostPortDb.Split("/")[0];
                      var pgDb = pgHostPortDb.Split("/")[1];
                      var pgUser = pgUserPass.Split(":")[0];
                      var pgPass = pgUserPass.Split(":")[1];
                      var pgHost = pgHostPort.Split(":")[0];
                      var pgPort = pgHostPort.Split(":")[1];

                      connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";

                      options.UseNpgsql(connStr);
                  }
              }
          }
      });
  ```
  
  <br>
  
- Authentication with cookies. When a user logs in the cookie is generated and based on the user's id. The id is used to validate if the user has authorization to different endpoints:

  [UsersController.cs]

  ```cs
      [HttpPost("/login")]
      [AllowAnonymous]
      public async Task<ActionResult> LogIn(LoginDTO loginDto)
      {
          try
          {
              if (!ModelState.IsValid)
              {
                  return Redirect("/login");
              }
              var found = await _userRepository.FindUserForLogin(loginDto);

              if (found == null)
              {
                  return NotFound();
              }

              var claims = new List<Claim>
              {
                  new Claim(ClaimTypes.Name, found.Full_Name),
                  new Claim(ClaimTypes.NameIdentifier, found.UserId.ToString()),
                  new Claim(ClaimTypes.Role, "User")
              };

              var ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
              var cp = new ClaimsPrincipal(ci);

              var properties = new AuthenticationProperties()
              {
                  AllowRefresh = true,
                  IsPersistent = true
              };

              await HttpContext.SignInAsync(cp, properties);

              return StatusCode(200,found);

          }
          catch (Exception ex)
          {
              return BadRequest(ex.Message);
          }
      }

  ```

  <br>
  
- DTOs used everywhere so vulnurable data won't be exposed. Model classes and DTOs change to each by Automapper during the application.  

  [UserRepository.cs]

  ```cs
        public async Task<UserDTO> PostUser(UserCreateDTO userDto)
        {
            if(userDto == null)
            {
                throw new ArgumentNullException("userDto is null");
            }

            Random rnd = new();
            int salt = rnd.Next();

            userDto.Password = HashPassword(userDto.Password,salt.ToString());
            var user = _mapper.Map<User>(userDto);

            user.Salt = salt;
            user.Description = "";

            await _context.Users.AddAsync(user);
            _context.SaveChanges();

            var dto = _mapper.Map<UserDTO>(user);

            return dto;
        }

        private string HashPassword(string password, string salt)
        {
            var passwordWithSalt = password + salt;

            using var sha = SHA512.Create();

            var bytes = Encoding.Default.GetBytes(passwordWithSalt);

            var hashed = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hashed);
        }

  ```

<br>

#### Client:
- All requests on client are handled with axios. The interceptors are configured to return response and use credentials. Generic requests based on the response are used for most of the client's requests:

  [UserAgent.ts]

  ```ts
        axios.defaults.baseURL = process.env.REACT_APP_BASE_URL;

        axios.interceptors.response.use(
            (response) => {
              return response;
            },
            (error) => {
              return Promise.reject(error);
            }
          );

        axios.interceptors.request.use((config) => {
          config.withCredentials = true;
          return config;
        });


        const responseBody = <T> (response: AxiosResponse<T>) => response.data;

        // Generic requests from our base-url, we can later attach the particular API-part
        const requests = {
            get: <T> (url: string) => axios.get<T>(url).then(responseBody),
            post: <T> (url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
            put: <T> (url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
            delete: <T> (url: string) => axios.delete<T>(url).then(responseBody)
        }
  ```

<br>

#### Tests:
- Controller classes in the test-project are configured with a fake database which uses UseInMemoryDatabase and are configured so ClaimsPrincipal can be attached in 'User' in HttpContext:

  [UsersControllerMoq.cs]

  ```cs
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
  ```
  
  [UsersControllerTests.cs]

  ```cs
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

        // Check state: users - 1
        var users = await controller.GetUsers(); 

        var usersResponse = Assert.IsType<ObjectResult>(users.Result);          // casting result to ObjectResult
        var usersValue = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(usersResponse.Value);

        var usersPrevResponse = Assert.IsType<ObjectResult>(usersPrev.Result);          // casting result to ObjectResult
        var usersPrevValue = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(usersPrevResponse.Value);

        Assert.Equal(usersPrevValue.Count() - 1, usersValue.Count());

    }
  ```


<br>

### Version 2.0: (Pending)
- CQRS instead of Repositories
- Save images in Cloudinary not postgres
- Chage React front-end with Angular



