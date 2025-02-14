using Microsoft.EntityFrameworkCore;

namespace TaskManagementApp.Services.TasksApi.Repositories
{
    public class TaskRepository : Repository<TaskItem, TaskContext>, ITaskRepository
    {
        

        public TaskRepository(IDbContextFactory<TaskContext> contextFactory) : base(contextFactory)
        {
            
        }
    }
}
