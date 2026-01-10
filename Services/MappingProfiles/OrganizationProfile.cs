using AutoMapper;
using Domain.Entities;
using Shared.OrganizationDTOs;


namespace Services.MappingProfiles
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            // Entity -> DTO
            CreateMap<Organization, OrganizationDto>()
                .ForMember(dest => dest.TeamNames, opt => opt.MapFrom(src => src.Teams.Select(t => t.Name)))
                .ForMember(dest => dest.UserEmails, opt => opt.MapFrom(src => src.Users.Select(u => u.Email)));

            // DTO -> Entity
            CreateMap<CreateOrganizationDto, Organization>();
            CreateMap<UpdateOrganizationDto, Organization>();
        }
    }
}
