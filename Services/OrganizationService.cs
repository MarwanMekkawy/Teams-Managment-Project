using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Services.Abstractions;
using Shared.OrganizationDTOs;


namespace Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public OrganizationService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<string>> GetAllAsync()
        {
         
            return await unitOfWork.organizations.GetAllSelectedAsync(o => o.Name);
        }

        public async Task<OrganizationDto> GetByIdAsync(int id)
        {
            var organization = await unitOfWork.organizations.GetAsync(id);
            if (organization == null) throw new KeyNotFoundException($"Organization with ID {id} not found"); 
            return mapper.Map<OrganizationDto>(organization);

        }

        public async Task<OrganizationDto> CreateAsync(CreateOrganizationDto dto)
        {
            var organization = mapper.Map<Organization>(dto);
            await unitOfWork.organizations.AddAsync(organization);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<OrganizationDto>(organization);
        }

        public async Task<OrganizationDto> UpdateAsync(int id, UpdateOrganizationDto dto)
        {
            var existingOrganization = await unitOfWork.organizations.GetAsync(id);
            if (existingOrganization == null) throw new KeyNotFoundException($"Organization with ID {id} not found");
            mapper.Map<OrganizationDto>(existingOrganization);
            unitOfWork.organizations.Update(existingOrganization);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<OrganizationDto>(existingOrganization);
        }

        public async Task DeleteAsync(int id)
        {
            var existingOrganization = await unitOfWork.organizations.GetAsync(id);
            if (existingOrganization == null) throw new KeyNotFoundException($"Organization with ID {id} not found");
            unitOfWork.organizations.Delete(existingOrganization);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
