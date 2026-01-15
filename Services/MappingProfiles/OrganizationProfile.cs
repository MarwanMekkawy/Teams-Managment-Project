using AutoMapper;
using Domain.Entities;
using Shared.OrganizationDTOs;


namespace Services.MappingProfiles
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        { 
            CreateMap<Organization, OrganizationDto>();
            CreateMap<CreateOrganizationDto, Organization>();
            CreateMap<UpdateOrganizationDto, Organization>();
        }
    }
}
