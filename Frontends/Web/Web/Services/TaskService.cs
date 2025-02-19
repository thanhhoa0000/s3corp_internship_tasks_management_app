namespace TaskManagementApp.Frontends.Web.Services
{
    public class TaskService : ITaskService
    {
        private readonly IBaseService _service;

        public TaskService(IBaseService service)
        {
            _service = service;
        }

        public async Task<Response?> CreateTaskAsync(TaskItemDto taskDto)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Post,
                Body = taskDto,
                Url = ApiUrlProperties.TasksUrl + "/api/v1/tasks"
            }, bearer: true);
        }

        public async Task<Response?> DeleteTaskAsync(Guid taskId)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Delete,
                Url = ApiUrlProperties.TasksUrl + $"/api/v1/tasks/{taskId}"
            }, bearer: true);
        }

        public async Task<Response?> GetTaskAsync(Guid taskId)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Get,
                Url = ApiUrlProperties.TasksUrl + $"/api/v1/tasks/{taskId}"
            }, bearer: true);
        }

        public async Task<Response?> GetTasksAsync()
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Get,
                Url = ApiUrlProperties.TasksUrl + "/api/v1/tasks"
            }, bearer: true);
        }

        public async Task<Response?> UpdateTaskAsync(TaskItemDto taskDto)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Put,
                Body = taskDto,
                Url = ApiUrlProperties.TasksUrl + "/api/v1/tasks"
            }, bearer: true);
        }
    }
}
