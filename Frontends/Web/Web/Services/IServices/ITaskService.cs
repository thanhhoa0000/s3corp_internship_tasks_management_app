namespace TaskManagementApp.Frontends.Web.Services.IServices
{
    public interface ITaskService
    {
        Task<Response?> GetTasksAsync();
        Task<Response?> GetTaskAsync(Guid taskId);
        Task<Response?> CreateTaskAsync(TaskItemDto taskDto);
        Task<Response?> UpdateTaskAsync(TaskItemDto taskDto);        
        Task<Response?> DeleteTaskAsync(Guid taskId);
    }
}
