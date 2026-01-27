using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Services.Abstractions;
using Shared.Claims;
using Shared.TaskDTOs;


namespace Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // Crud methods //
        public async Task<TaskDto> GetByIdAsync(int id, UserClaims userCredentials)
        {
            var task = await unitOfWork.tasks.GetByIdWithProjectAndTeamAndMembersAsync(id);
            if (task == null)
                throw new NotFoundException($"Task with ID {id} not found");

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break;

                case UserRole.Manager:
                    if (task.Project.Team.OrganizationId != userCredentials.OrgId)
                        throw new ForbiddenException("Managers can only access tasks in their organization");
                    break;

                case UserRole.TeamLeader:
                    var leaderInTeam = task.Project.Team.Members.Any(m => m.UserId == userCredentials.UserId);
                    if (!leaderInTeam) throw new ForbiddenException("Team leaders can only access tasks in their teams");
                    break;

                case UserRole.Member:
                    var isAssigned = task.AssigneeId == userCredentials.UserId;
                    var memberInTeam = task.Project.Team.Members.Any(m => m.UserId == userCredentials.UserId);
                    if (!isAssigned && !memberInTeam)throw new ForbiddenException("Members can only access assigned tasks or tasks in their teams");
                    break;

                default:
                    throw new ForbiddenException("Unauthorized role");
            }

            return mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto, UserClaims userCredentials)
        {
            var project = await unitOfWork.projects.GetByIdWithTeamAndMembersAsync(dto.ProjectId);
            if (project == null) throw new NotFoundException($"Project with ID {dto.ProjectId} not found");


            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break;

                case UserRole.Manager:
                    if (project.Team.OrganizationId != userCredentials.OrgId)
                        throw new ForbiddenException("Managers can only create tasks in teams within their organization");
                    break;

                case UserRole.TeamLeader:
                    var leaderInTeam = project.Team.Members.Any(m => m.UserId == userCredentials.UserId);
                    if (!leaderInTeam) throw new ForbiddenException("Team leaders can only create tasks in their teams");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to create tasks");
            }

            var task = mapper.Map<TaskEntity>(dto);
            unitOfWork.tasks.Add(task);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> UpdateAsync(int id, UpdateTaskDto dto, UserClaims userCredentials)
        {
            var task = await unitOfWork.tasks.GetByIdWithProjectAndTeamAndMembersAsync(id);
            if (task == null) throw new NotFoundException($"Task with ID {id} not found");


            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break;

                case UserRole.Manager:
                    if (task.Project.Team.OrganizationId != userCredentials.OrgId)
                        throw new ForbiddenException("Managers can only update tasks in teams within their organization");
                    break;

                case UserRole.TeamLeader:
                    var leaderInTeam = task.Project.Team.Members.Any(m => m.UserId == userCredentials.UserId);
                    if (!leaderInTeam) throw new ForbiddenException("Team leaders can only update tasks in their teams");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to update this task");
            }

            task.Title = dto.Title ?? task.Title;
            task.Description = dto.Description ?? task.Description;
            task.DueDate = dto.DueDate ?? task.DueDate;
            task.Status = dto.Status ?? task.Status;
            if (dto.AssigneeId.HasValue && dto.AssigneeId != task.AssigneeId)
            {
                var newUser = await unitOfWork.users.GetAsync(dto.AssigneeId.Value);
                if (newUser == null) throw new NotFoundException("Assignee not found");

                switch (userCredentials.Role)
                {
                    case UserRole.Admin:
                        break;

                    case UserRole.Manager:
                        if (newUser.OrganizationId != userCredentials.OrgId) throw new ForbiddenException("Cannot assign outside your organization");
                        break;

                    case UserRole.TeamLeader:
                        var isInTeam = await unitOfWork.teamMembers.ExistsInTeamAsync(task.Project.TeamId, dto.AssigneeId.Value);
                        if (!isInTeam) throw new ForbiddenException("Cannot assign outside your team");
                        break;
                }
            }

            unitOfWork.tasks.Update(task);
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<TaskDto>(task);
        }

        public async Task DeleteAsync(int id, UserClaims userCredentials)
        {
            var task = await unitOfWork.tasks.GetByIdWithProjectAndTeamAndMembersAsync(id);
            if (task == null) throw new NotFoundException($"Task with ID {id} not found");

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break; 

                case UserRole.Manager:
                    if (task.Project.Team.OrganizationId != userCredentials.OrgId)
                        throw new ForbiddenException("Managers can only delete tasks in their organization");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to delete this task");
            }

            unitOfWork.tasks.Delete(task);
            await unitOfWork.SaveChangesAsync();
        }

        // Soft Delete methods //
        public async Task SoftDeleteAsync(int id, UserClaims userCredentials)
        {
            var task = await unitOfWork.tasks.GetByIdWithProjectAndTeamAndMembersAsync(id);
            if (task == null) throw new NotFoundException($"Task with ID {id} not found");

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break; 

                case UserRole.Manager:
                    if (task.Project.Team.OrganizationId != userCredentials.OrgId)
                        throw new ForbiddenException("Managers can only soft-delete tasks in their organization");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to soft-delete this task");
            }

            task.IsDeleted = true;
            unitOfWork.tasks.Update(task);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RestoreAsync(int id, UserClaims userCredentials)
        {
            var task = await unitOfWork.tasks.GetByIdWithProjectAndTeamAndMembersAsync(id);
            if (task == null) throw new NotFoundException($"Task with ID {id} not found");
            if (!task.IsDeleted)throw new BadRequestException("The task is not a deleted entity");

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break; 

                case UserRole.Manager:
                    if (task.Project.Team.OrganizationId != userCredentials.OrgId) throw new ForbiddenException("Managers can only restore tasks in their organization");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to restore this task");
            }

            task.IsDeleted = false;
            unitOfWork.tasks.Update(task);
            await unitOfWork.SaveChangesAsync();
        }
        

        public async Task<List<TaskDto>> GetAllDeletedTasksAsync(int pageNumber, int pageSize, UserClaims userCredentials)
        {
            IEnumerable<TaskEntity> deletedTasks;

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    deletedTasks = await unitOfWork.tasks.GetAllSoftDeletedAsync(pageNumber, pageSize);
                    break;

                case UserRole.Manager:                   
                    deletedTasks = await unitOfWork.tasks.GetAllSoftDeletedByOrganizationIncludingProjectsAndTeamsAsync(userCredentials.OrgId, pageNumber, pageSize);                                  
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to access deleted tasks");
            }

            return mapper.Map<List<TaskDto>>(deletedTasks);
        }

        // get methods related to another entity //
        public async Task<List<TaskDto>> GetTasksByProjectAsync(int projectId)
        {
            var tasks = await unitOfWork.tasks.GetByProjectAndStatusAsync(projectId);
            if (tasks == null) throw new NotFoundException($"Project with ID {projectId} not found");
            return mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<List<TaskDto>> GetTasksByUserAsync(int userId)
        {
            var tasks = await unitOfWork.tasks.GetByAssigneeAndStatusAsync(userId);
            if (tasks == null) throw new NotFoundException($"User with ID {userId} not found");
            return mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<List<TaskDto>> GetOverdueTasksAsync(int organizationId)
        {
            var overDueTasks = await unitOfWork.tasks.GetOverdueAsync(organizationId);
            if (overDueTasks == null) throw new NotFoundException($"Organization with ID {organizationId} not found");
            return mapper.Map<List<TaskDto>>(overDueTasks);
        }

        // Specific update methods //
        public async Task AssignToUserAsync(int taskId, int userId, UserClaims userCredentials)
        {
            var task = await unitOfWork.tasks.GetByIdWithProjectAndTeamAndMembersAsync(taskId);
            if (task == null) throw new NotFoundException($"Task with ID {taskId} not found");

            var user = await unitOfWork.users.GetAsync(userId);
            if (user == null) throw new NotFoundException($"User with ID {userId} not found");

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break;

                case UserRole.Manager:                    
                    if (task.Project.Team.OrganizationId != userCredentials.OrgId || user.OrganizationId != userCredentials.OrgId)
                        throw new ForbiddenException("Managers can only assign tasks to users in their organization");
                    break;

                case UserRole.TeamLeader:
                    var isLeaderInTeam = task.Project.Team.Members.Any(m => m.UserId == userCredentials.UserId);
                    var assigneeInTeam = task.Project.Team.Members.Any(m => m.UserId == userId);
                    if (!isLeaderInTeam || !assigneeInTeam) throw new ForbiddenException("Team leaders can only assign tasks to users in their team");
                    break;

                default:
                    throw new ForbiddenException("Unauthorized role");
            }

            task.AssigneeId = userId;
            unitOfWork.tasks.Update(task);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(int id, TaskEntityStatus? status, UserClaims userCredentials)
        {
            var task = await unitOfWork.tasks.GetByIdWithProjectAndTeamAndMembersAsync(id);
            if (task == null) throw new NotFoundException($"Task with ID {id} not found");

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break;

                case UserRole.Manager:
                    if (task.Project.Team.OrganizationId != userCredentials.OrgId)
                        throw new ForbiddenException("Managers can only modify tasks in their organization");
                    break;

                case UserRole.TeamLeader:
                    if (!task.Project.Team.Members.Any(m => m.UserId == userCredentials.UserId))
                        throw new ForbiddenException("Team leaders can only modify tasks in their teams");
                    break;

                case UserRole.Member:
                    if (task.AssigneeId != userCredentials.UserId)
                        throw new ForbiddenException("Members can only modify tasks assigned to them");
                    break;

                default:
                    throw new ForbiddenException("Unauthorized role");
            }

            if (!status.HasValue) throw new BadRequestException("Status is required");
            task.Status = status.Value;
            unitOfWork.tasks.Update(task);
            await unitOfWork.SaveChangesAsync();
        }      
    }
}
