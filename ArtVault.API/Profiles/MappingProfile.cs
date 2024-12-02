using AutoMapper;
using ArtVault.API.Models;
using ArtVault.API.DTOs;

namespace ArtVault.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Comment, CommentDto>();
            CreateMap<CreateCommentDto, Comment>();
            CreateMap<UpdateCommentDto, Comment>();
            CreateMap<Post, PostDto>();
            CreateMap<CreatePostDto, Post>();
            CreateMap<User, UserDto>();
            CreateMap<UserRequestDto, User>();
        }
    }
}
