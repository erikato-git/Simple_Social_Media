using AutoMapper;
using Microsoft.Extensions.Hosting;
using Simple_Social_Media_App.Controllers.DTOs;
using Simple_Social_Media_App.DataAccess.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace Simple_Social_Media_App.Utils
{
    public class MapperService : Profile
    {
        public MapperService()
        {
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserUpdateDTO, User>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<PostUpdateDTO, Post>();
            CreateMap<CreatePostDTO, Post>();
            CreateMap<UpdateCommentDTO, Comment>();
            CreateMap<CommentCreateDTO, Comment>();

        }
    }
}
