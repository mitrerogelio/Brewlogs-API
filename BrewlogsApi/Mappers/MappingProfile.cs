using BrewlogsApi.Dtos;
using BrewlogsApi.Model;

namespace BrewlogsApi.Mappers;

using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // CreateMap<Source, Destination>();
        CreateMap<BrewlogDto, Brewlog>();
        CreateMap<AccountForRegistrationDto, Account>();
    }
}