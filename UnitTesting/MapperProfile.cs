using API.Models.RequestModels;
using API.Models.ResponseModels;
using AutoMapper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<BlogPost, BlogReadDto>();

            CreateMap<BlogCreateDto, BlogPost>()
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.PublicationDate, opt => opt.MapFrom(src => DateTime.UtcNow))  
            .ForMember(dest => dest.Tags, opt => opt.Ignore())  
            .ForMember(dest => dest.ImagePath, opt => opt.Ignore())  
            .ForMember(dest => dest.Comments, opt => opt.Ignore()) 
            .ForMember(dest => dest.Likes, opt => opt.Ignore())  
            .ForMember(dest => dest.Images, opt => opt.Ignore());
            
            CreateMap<BlogUpdateDto, BlogPost>();
        }
    }
}
