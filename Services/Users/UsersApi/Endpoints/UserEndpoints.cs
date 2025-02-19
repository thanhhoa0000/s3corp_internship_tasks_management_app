namespace TaskManagementApp.Services.UsersApi.Endpoints
{
    public class UserEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

            var group = app.MapGroup("/api/v{version:apiVersion}/users")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization("AdminOnly");

            group.MapGet("/", GetUsers);
            group.MapGet("/{userId:guid}", GetUser);
            group.MapPost("/", CreateUser);
            group.MapPut("/", UpdateUser);
            group.MapDelete("/{userId:guid}", DeleteUser);
        }

        public async Task<Results<Ok<IEnumerable<AppUserDto>>, BadRequest<string>>>
            GetUsers(
                [FromServices] IUserRepository repository,
                IMapper mapper,
                [FromServices] ILogger<UserEndpoints> logger,
                HttpContext httpContext,
                [FromQuery] int pageSize = 0,
                [FromQuery] int pageNumber = 1)
        {
            try
            {
                logger.LogInformation("Getting the users...");

                IEnumerable<AppUser> usersList
                    = await repository.GetAllAsync(tracked: false, pageSize: pageSize, pageNumber: pageNumber);

                Pagination pagination = new Pagination()
                {
                    PageSize = pageSize,
                    PageNumber = pageNumber
                };

                httpContext.Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagination);

                return TypedResults.Ok(mapper.Map<IEnumerable<AppUserDto>>(usersList));
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when getting the users!");
            }
        }

        public async Task<Results<Ok<AppUserDto>, NotFound<string>, BadRequest<string>>>
            GetUser(
                [FromServices] IUserRepository repository,
                Guid userId,
                IMapper mapper,
                ILogger<UserEndpoints> logger)
        {
            try
            {
                logger.LogInformation("Getting the user...");

                var user = await repository.GetAsync(u => u.Id == userId, tracked: false);

                if (user is null)
                {
                    logger.LogError($"\n---\nUser {userId} not found!\n---\n");

                    return TypedResults.NotFound($"Cannot find the user with ID \"{userId}\"");
                }

                return TypedResults.Ok(mapper.Map<AppUserDto>(user));
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when getting the user!");
            }
        }

        // Create user and assign the role for that user
        public async Task<Results<Ok<string>, BadRequest<string>>>
            CreateUser(
                [FromBody] CreateUserRequest request,
                [FromServices] IUserRepository repository,
                IMapper mapper,
                HttpContext httpContext,
                ILogger<UserEndpoints> logger)
        {
            try
            {
                if (request.User is null)
                {
                    logger.LogError("\n---\nInput user is null!\n---\n");

                    return TypedResults.BadRequest("No input user was found");
                }

                if (request.Role is null)
                {
                    logger.LogError("\n---\nInput role is null!\n---\n");

                    return TypedResults.BadRequest("No input role was found");
                }

                logger.LogInformation($"Creating user {request.User.Id}");

                if (await repository.GetAsync(u => u.Id == request.User.Id, tracked: false) is not null)
                {
                    logger.LogError($"\n---\nUser {request.User.Id} already existed!\n---\n");

                    return TypedResults.BadRequest("User already existed!");
                }

                AppUser user = mapper.Map<AppUser>(request.User);

                var passwordHasher = new PasswordHasher<AppUser>();
                user.PasswordHash = passwordHasher.HashPassword(user, request.DefaultPassword);
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.ConcurrencyStamp = Guid.NewGuid().ToString();
                
                await repository.CreateAsync(user);

                logger.LogInformation($"Assign role {request.Role.Name} to user {request.User.Id}");
                await repository.AssignRoleAsync(user.Id, request.Role.Id);
                

                var version = httpContext.GetRequestedApiVersion()?.ToString() ?? "1";

                return TypedResults.Ok("Create the user successfully!");
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when creating the user!");
            }
        }

        public async Task<Results<NoContent, NotFound<string>, BadRequest<string>>>
            UpdateUser(
                [FromBody] AppUserDto userDto,
                [FromServices] IUserRepository repository,
                IMapper mapper,
                ILogger<UserEndpoints> logger)
        {
            try
            {
                if (userDto is null)
                {
                    logger.LogError("\n---\nInput user is null!\n---\n");

                    return TypedResults.BadRequest("No input user was found");
                }

                logger.LogInformation($"Updating user {userDto.Id}");

                if (await repository.GetAsync(t => t.Id == userDto.Id, tracked: false) is null)
                {
                    logger.LogError($"\n---\nUser {userDto.Id} does not exist!\n---\n");

                    return TypedResults.NotFound("User does not exist!");
                }

                AppUser user = mapper.Map<AppUser>(userDto);
                await repository.UpdateAsync(user);

                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when updating the user!");
            }
        }

        public async Task<Results<NoContent, NotFound<string>, BadRequest<string>>>
            DeleteUser(
                [FromServices] IUserRepository repository,
                Guid userId,
                ILogger<UserEndpoints> logger)
        {
            try
            {
                if (userId.ToString().IsNullOrEmpty())
                {
                    logger.LogError("\n---\nInput user ID is null!\n---\n");

                    return TypedResults.BadRequest("No input user ID was found");
                }

                logger.LogInformation($"Deleting user {userId}");

                AppUser user = await repository.GetAsync(u => u.Id == userId, tracked: false);

                if (user is null)
                {
                    logger.LogError($"\n---\nUser {userId} does not exist!\n---\n");

                    return TypedResults.NotFound("User does not exist!");
                }

                await repository.RemoveAsync(user);

                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when deleting the user!");
            }
        }
    }
}
