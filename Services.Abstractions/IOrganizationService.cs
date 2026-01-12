using Shared.OrganizationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IOrganizationService
    {
        Task<IEnumerable<string>> GetAllAsync();
        Task<OrganizationDto> GetByIdAsync(int id);
        Task<OrganizationDto> CreateAsync(CreateOrganizationDto dto);
        Task<OrganizationDto> UpdateAsync(int id, UpdateOrganizationDto dto);
        Task DeleteAsync(int id);
    }
}

