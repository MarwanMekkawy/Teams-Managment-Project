using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstractions;
using Shared.OrganizationDTOs;


namespace Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public OrganizationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        // Get status //
        public async Task<(int totalUsers, int totalTeams, int activeProjects, int archivedProjects, int totalTasks, int completedTasks, int overdueTasks)> GetStatsAsync(int id)
        {
            return await unitOfWork.organizations.GetOrganizationStatsAsync(id) ?? (0, 0, 0, 0, 0, 0, 0);              //empty tuple
        }

        // Crud methods //
        public async Task<List<string>> GetAllAsync(int pageNumber, int pageSize)
        {        
            return (await unitOfWork.organizations.GetAllSelectedAsync(o => o.Name, pageNumber, pageSize)).ToList();
        }

        public async Task<OrganizationDto> GetByIdAsync(int id)
        {
            var organization = await unitOfWork.organizations.GetAsync(id);
            if (organization == null) throw new NotFoundException($"Organization with ID {id} not found"); 
            return mapper.Map<OrganizationDto>(organization);

        }

        public async Task<OrganizationDto> CreateAsync(CreateOrganizationDto dto)
        {
            var organization = mapper.Map<Organization>(dto);
            unitOfWork.organizations.Add(organization);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<OrganizationDto>(organization);
        }

        public async Task<OrganizationDto> UpdateAsync(int id, UpdateOrganizationDto dto)
        {
            var existingOrganization = await unitOfWork.organizations.GetAsync(id);
            if (existingOrganization == null) throw new NotFoundException($"Organization with ID {id} not found");
            existingOrganization.Name = dto.Name ?? existingOrganization.Name;
            unitOfWork.organizations.Update(existingOrganization);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<OrganizationDto>(existingOrganization);
        }

        public async Task DeleteAsync(int id)
        {
            var existingOrganization = await unitOfWork.organizations.GetAsync(id);
            if (existingOrganization == null) throw new NotFoundException($"Organization with ID {id} not found");
            unitOfWork.organizations.Delete(existingOrganization);
            await unitOfWork.SaveChangesAsync();
        }

        // Soft Delete methods //
        public async Task SoftDeleteAsync(int id)
        {
            var existingOrganization = await unitOfWork.organizations.GetAsync(id);
            if (existingOrganization == null) throw new NotFoundException($"Organization with ID {id} not found");
            existingOrganization.IsDeleted = true;
            unitOfWork.organizations.Update(existingOrganization);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RestoreAsync(int id)
        {
            var organization = await unitOfWork.organizations.GetIncludingDeletedAsync(id);
            if (organization == null)  throw new NotFoundException($"Organization with ID {id} not found");
            if (!organization.IsDeleted) throw new BadRequestException("The Entity is Not a deleted Entity");

            organization.IsDeleted = false;
            await unitOfWork.SaveChangesAsync();
        }
        public async Task<List<OrganizationDto>> GetAllDeletedOrganizationsAsync()
        {
            var deletedOrgs = await unitOfWork.organizations.GetAllSoftDeletedAsync();
            return mapper.Map<List<OrganizationDto>>(deletedOrgs);
        }
    }
}
