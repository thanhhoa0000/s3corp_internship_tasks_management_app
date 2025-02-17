using Microsoft.IdentityModel.Tokens;

namespace TaskManagementApp.Services.TasksApi.Endpoints
{
    public class TaskItemEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

            var group = app.MapGroup("/api/v{version:apiVersion}/tasks")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization("NormalUserOnly");

            group.MapGet("/", GetTasks);
            group.MapGet("/{taskId:guid}", GetTask);
            group.MapPost("/", CreateTask);
            group.MapPut("/", UpdateTask);
            group.MapDelete("/{taskId:guid}", DeleteTask);
        }

        public async Task<Results<Ok<IEnumerable<TaskItemDto>>, BadRequest<string>>>
            GetTasks(
                [FromServices] ITaskRepository repository,
                IMapper mapper,
                [FromServices] ILoggerFactory loggerFactory,
                HttpContext httpContext,
                [FromQuery] int pageSize = 0,
                [FromQuery] int pageNumber = 1)
        {
            ILogger logger = loggerFactory.CreateLogger(nameof(TaskItemEndpoints));

            try
            {
                logger.LogInformation("Getting the tasks...");

                IEnumerable<TaskItem> tasksList 
                    = await repository.GetAllAsync(tracked: false, pageSize: pageSize, pageNumber: pageNumber);

                Pagination pagination = new Pagination()
                {
                    PageSize = pageSize,
                    PageNumber = pageNumber
                };

                httpContext.Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagination);

                return TypedResults.Ok(mapper.Map<IEnumerable<TaskItemDto>>(tasksList));
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when getting the tasks!");
            }
        }

        public async Task<Results<Ok<TaskItemDto>, NotFound<string>, BadRequest<string>>>
            GetTask(
                [FromServices] ITaskRepository repository,
                Guid taskId,
                IMapper mapper,
                ILoggerFactory loggerFactory)
        {
            ILogger logger = loggerFactory.CreateLogger(nameof(TaskItemEndpoints));

            try
            {
                logger.LogInformation("Getting the task...");

                var task = await repository.GetAsync(t => t.Id == taskId, tracked: false);

                if (task is null)
                {
                    logger.LogError($"\n---\nTask {taskId} not found!\n---\n");

                    return TypedResults.NotFound($"Cannot find the task with ID \"{taskId}\"");
                }

                return TypedResults.Ok(mapper.Map<TaskItemDto>(task));
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when getting the task!");
            }
        }

        public async Task<Results<Created, BadRequest<string>>> 
            CreateTask(
                [FromBody] TaskItemDto taskDto,
                [FromServices] ITaskRepository repository,
                IMapper mapper,
                HttpContext httpContext,
                ILoggerFactory loggerFactory)
        {
            ILogger logger = loggerFactory.CreateLogger(nameof(TaskItemEndpoints));

            try
            {
                if (taskDto is null)
                {
                    logger.LogError("\n---\nInput task is null!\n---\n");

                    return TypedResults.BadRequest("No input task was found");
                }

                logger.LogInformation($"Creating task {taskDto.Id}");

                if (await repository.GetAsync(t => t.Id == taskDto.Id, tracked:false) is not null)
                {
                    logger.LogError($"\n---\nTask {taskDto.Id} already existed!\n---\n");

                    return TypedResults.BadRequest("Task already existed!");
                }

                TaskItem task = mapper.Map<TaskItem>(taskDto);
                await repository.CreateAsync(task);

                var version = httpContext.GetRequestedApiVersion()?.ToString() ?? "1";

                return TypedResults.Created($"/api/v{version}/tasks/{taskDto.Id}");
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when creating the task!");
            }
        }

        public async Task<Results<NoContent, NotFound<string>, BadRequest<string>>>
            UpdateTask(
                [FromBody] TaskItemDto taskDto,
                [FromServices] ITaskRepository repository,
                IMapper mapper,
                ILoggerFactory loggerFactory)
        {
            ILogger logger = loggerFactory.CreateLogger(nameof(TaskItemEndpoints));

            try
            {
                if (taskDto is null)
                {
                    logger.LogError("\n---\nInput task is null!\n---\n");

                    return TypedResults.BadRequest("No input task was found");
                }

                logger.LogInformation($"Updating task {taskDto.Id}");

                if (await repository.GetAsync(t => t.Id == taskDto.Id, tracked: false) is null)
                {
                    logger.LogError($"\n---\nTask {taskDto.Id} does not exist!\n---\n");

                    return TypedResults.NotFound("Task does not exist!");
                }

                TaskItem task = mapper.Map<TaskItem>(taskDto);
                task.ModifiedAt = DateTime.UtcNow;

                await repository.UpdateAsync(task);

                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when updating the task!");
            }
        }

        public async Task<Results<NoContent, NotFound<string>, BadRequest<string>>>
            DeleteTask(
                Guid taskId,
                [FromServices] ITaskRepository repository,
                ILoggerFactory loggerFactory)
        {
            ILogger logger = loggerFactory.CreateLogger(nameof(TaskItemEndpoints));

            try
            {
                if (taskId.ToString().IsNullOrEmpty())
                {
                    logger.LogError("\n---\nInput task ID is null!\n---\n");

                    return TypedResults.BadRequest("No input task ID was found");
                }

                logger.LogInformation($"Deleting task {taskId}");

                TaskItem task = await repository.GetAsync(t => t.Id == taskId, tracked: false);

                if (task is null)
                {
                    logger.LogError($"\n---\nTask {taskId} does not exist!\n---\n");

                    return TypedResults.NotFound("Task does not exist!");
                }

                await repository.RemoveAsync(task);

                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when deleting the task!");
            }
        }
    }
}
