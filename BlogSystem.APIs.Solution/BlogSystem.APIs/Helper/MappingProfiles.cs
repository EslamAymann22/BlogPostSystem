using AutoMapper;
using BlogSystem.APIs.DTOs;
using BlogSystem.Core.Entities;

namespace BlogSystem.APIs.Helper
{
    public class MappingProfiles : Profile
    {



        public MappingProfiles()
        {


            

            CreateMap<Post, PostDtoToReturn>()
                .ForMember(dest => dest.Status, D => D.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Author, D => D.MapFrom(src => src.Author.DisplayName))
                .ForMember(dest => dest.Tags, D => D.MapFrom(src => src.Tags.Select(tag => tag.Name).ToList()))
                .ForMember(dest => dest.Category, D => D.MapFrom(src => src.Category.Name));

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.AuthorName, D => D.MapFrom(src => src.Post.Author.DisplayName));

            //CreateMap<PostDtoToReturn, Post>()
            //.ForMember(dest => dest.Tags, D => D.MapFrom()

        }

        

    }
}
