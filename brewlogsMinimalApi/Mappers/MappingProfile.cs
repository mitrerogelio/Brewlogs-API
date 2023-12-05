using brewlogsMinimalApi.Dtos;
using brewlogsMinimalApi.Model;

namespace brewlogsMinimalApi.Mappers;

using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // CreateMap<Source, Destination>();
        CreateMap<BrewlogDto, Brewlog>();
    }
}