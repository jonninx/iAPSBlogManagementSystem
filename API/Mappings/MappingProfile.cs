using API.Models.RequestModels;
using API.Models.ResponseModels;
using AutoMapper;
using Core.Entities;

namespace API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BlogPost, BlogReadDto>();
            CreateMap<BlogCreateDto, BlogPost>();
            CreateMap<BlogUpdateDto, BlogPost>();

            CreateMap<Comment, CommentReadDto>();
            CreateMap<CommentCreateDto, Comment>();
        }
    }
}
