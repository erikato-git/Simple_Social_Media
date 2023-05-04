using Microsoft.AspNetCore.Mvc;
using Simple_Social_Media_App.Controllers.DTOs;
using Simple_Social_Media_App.DataAccess.Model;

namespace Simple_Social_Media_App.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserDTO>?> GetAll();
        Task<UserDTO?> GetById(Guid id);
        Task<UserDTO> PostUser(UserCreateDTO user);
        Task<UserDTO?> UpdateUser(Guid id, UserUpdateDTO userDTO);
        Task DeleteUser(Guid id);
        Task<UserDTO?> FindUserForLogin(LoginDTO loginDto);
        Task<UserDTO> FindUserByEmail(string email);
        Task<bool> CheckEmailIsFree(string email);
        Task<bool> ChangePassword(UserDTO userDto, string oldPassword, string newPassword);

    }
}
