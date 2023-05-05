using AutoMapper;
using Microsoft.EntityFrameworkCore;
using webapi.DTOs;
using webapi.Model;
using webapi.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace webapi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Queries

        public async Task<List<UserDTO>?> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            List<UserDTO> usersDto = _mapper.Map<List<User>, List<UserDTO>>(users);
            return usersDto;
        }

        public async Task<UserDTO?> GetById(Guid id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id null");
            }

            var user = await _context.Users.FindAsync(id);
            var userDto = _mapper.Map<UserDTO>(user);

            return userDto;
        }

        // Commands

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

        public async Task<UserDTO?> UpdateUser(Guid id, UserUpdateDTO userDto)
        {
            if (String.IsNullOrEmpty(id.ToString()) || userDto == null)
            {
                throw new ArgumentNullException("id or userDto is null");
            }

            var found = await _context.Users.FindAsync(id);

            if(found == null)
            {
                return null;
            }

            _mapper.Map(userDto, found);

            _context.SaveChanges();

            var dto = _mapper.Map<UserDTO>(found);

            return dto;
        }

        public async Task DeleteUser(Guid id)
        {
            if(String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id is null");
            }

            var user = await _context.Users.FindAsync(id);

            if(user == null)
            {
                throw new Exception("User not found");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return;
        }

        public async Task<UserDTO?> FindUserForLogin(LoginDTO loginDto)
        {
            if(loginDto == null)
            {
                throw new ArgumentNullException("loginDto is null");
            }

            var emailFound = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(loginDto.Email));

            if(emailFound == null)
            {
                return null;
            }

            var hashedPassword = HashPassword(loginDto.Password, emailFound.Salt.ToString());
            
            var found = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(loginDto.Email) && x.Password.Equals(hashedPassword));;
            var userDto = _mapper.Map<UserDTO>(found);

            return userDto;
        }

        public async Task<UserDTO> FindUserByEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email is null");
            }

            var found = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));

            if(found == null) 
            {
                throw new Exception("User not found");
            }

            var userDto = _mapper.Map<UserDTO>(found);

            return userDto;
        }

        public async Task<bool> CheckEmailIsFree(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email is null");
            }

            var found = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));

            if (found == null)
            {
                return true;
            }

            return false;
        }


        public async Task<bool> ChangePassword(UserDTO userDto, string oldPassword, string newPassword)
        {
            if(userDto == null){
                throw new ArgumentNullException("no user");
            }

            if(String.IsNullOrEmpty(oldPassword) || String.IsNullOrEmpty(newPassword)){
                throw new ArgumentException("Old password or new password is missing");
            }

            var found = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userDto.UserId);

            if(found == null){
                throw new Exception("Logged in user wasn't found");
            }

            var oldPasswordHashed = HashPassword(oldPassword, found.Salt.ToString());

            if(oldPasswordHashed != found.Password){
                throw new Exception("old hashed password doesn't match with hashed password stored in database");
            }

            var newPasswordHashed = HashPassword(newPassword, found.Salt.ToString());

            found.Password = newPasswordHashed;

            var updateStatus = await _context.SaveChangesAsync() > 0;

            if(!updateStatus){
                throw new Exception("Something went wrong when saving new password to database");
            }

            return updateStatus;
        }

    }
}
