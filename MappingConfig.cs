using AutoMapper;
using Villa_Services.Models;
using Villa_Services.Models.Dto;

namespace Villa_Services
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreate>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdate>().ReverseMap();

        }
    }
}
