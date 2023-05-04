using AutoMapper;
using webapi.DTOs;
using webapi.Model;

namespace webapi.Utils
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
            CreateMap<CommentUpdateDTO, Comment>();
            CreateMap<CommentCreateDTO, Comment>();

        }
    }
}
