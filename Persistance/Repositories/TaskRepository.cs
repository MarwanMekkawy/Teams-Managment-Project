using Domain.Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class TaskRepository : GenericRepository<TaskEntity,int> , ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context) { }
    }
}
