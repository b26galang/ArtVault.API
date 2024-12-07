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
            CreateMap<CommentDto, Comment>();
            CreateMap<CreateCommentDto, Comment>();
            CreateMap<UpdateCommentDto, Comment>();
            CreateMap<Post, PostDto>();
            CreateMap<CreatePostDto, Post>();
            CreateMap<UpdatePostDto, Post>();
            CreateMap<User, UserDto>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserCreationDto, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Automatically generated
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore()); // Set in the entity
        }
    }
}
