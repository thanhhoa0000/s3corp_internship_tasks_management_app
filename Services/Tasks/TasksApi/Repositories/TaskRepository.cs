using Microsoft.EntityFrameworkCore;

namespace TaskManagementApp.Services.TasksApi.Repositories
{
    public class TaskRepository : Repository<TaskItem, TaskContext>, ITaskRepository
    {
        private readonly TaskContext _context;

        public TaskRepository(IDbContextFactory<TaskContext> contextFactory) : base(contextFactory)
        {
            _context = contextFactory.CreateDbContext();
        }
    }
}
