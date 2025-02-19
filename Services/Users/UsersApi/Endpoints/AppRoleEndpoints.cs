using Microsoft.IdentityModel.Tokens;
using TaskManagementApp.Services.UsersApi.Models.Dtos;

namespace TaskManagementApp.Services.UsersApi.Endpoints
{
    public class AppRoleEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

            var group = app.MapGroup("/api/v{version:apiVersion}/roles")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization("AdminOnly");

            group.MapGet("/", GetRoles);
            group.MapGet("/{roleId:guid}", GetRole);
            group.MapPost("/", CreateRole);
            group.MapPut("/", UpdateRole);
            group.MapDelete("/{roleId:guid}", DeleteRole);
        }

        public async Task<Results<Ok<IEnumerable<AppRole>>, BadRequest<string>>>
            GetRoles(
                [FromServices] IAppRoleRepository repository,
                [FromServices] ILogger<AppRoleEndpoints> logger,
                HttpContext httpContext,
                [FromQuery] int pageSize = 0,
                [FromQuery] int pageNumber = 1)
        {
            try
            {
                logger.LogInformation("Getting the roles...");

                IEnumerable<AppRole> rolesList =
                    await repository.GetAllAsync(tracked: false, pageSize: pageSize, pageNumber: pageNumber);

                Pagination pagination = new Pagination()
                {
                    PageSize = pageSize,
                    PageNumber = pageNumber
                };

                httpContext.Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagination);

                return TypedResults.Ok(rolesList);
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when getting the roles!");
            }
        }

        public async Task<Results<Ok<AppRole>, NotFound<string>, BadRequest<string>>>
            GetRole(
                [FromServices] IAppRoleRepository repository,
                Guid roleId,
                IMapper mapper,
                ILogger<AppRoleEndpoints> logger)
        {
            try
            {
                logger.LogInformation("Getting the role...");

                var role = await repository.GetAsync(r => r.Id == roleId);

                if (role is null)
                {
                    logger.LogError($"\n---\nRole {roleId} not found!\n---\n");

                    return TypedResults.NotFound($"Cannot find the role \"{roleId}\"");
                }

                return TypedResults.Ok(role);
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when getting the role!");
            }
        }

        public async Task<Results<Ok<string>, BadRequest<string>>>
            CreateRole(
                [FromBody] string roleName,
                [FromServices] IAppRoleRepository repository,
                IMapper mapper,
                HttpContext httpContext,
                ILogger<AppRoleEndpoints> logger)
        {
            try
            {
                if (roleName.IsNullOrEmpty())
                {
                    logger.LogError("\n---\nInput role name is null!\n---\n");

                    return TypedResults.BadRequest("No input role name was found");
                }

                logger.LogInformation("Creating the role...");

                if (await repository.GetAsync(r => r.Name == roleName, tracked: false) is not null)
                {
                    logger.LogError($"\n---\nRole {roleName} already existed!\n---\n");

                    return TypedResults.BadRequest($"Role \"{roleName}\" already existed!");
                }                              

                var role = new AppRole()
                {
                    Id = Guid.NewGuid(),
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };

                await repository.CreateAsync(role);

                var version = httpContext.GetRequestedApiVersion()?.ToString() ?? "1";

                return TypedResults.Ok("Create the role successfully!");
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when creating the role!");
            }
        }

        public async Task<Results<NoContent, NotFound<string>, Conflict<string>, BadRequest<string>>>
            UpdateRole(
                [FromBody] AppRoleDto roleDto,
                [FromServices] IAppRoleRepository repository,
                IMapper mapper,
                ILogger<AppRoleEndpoints> logger)
        {
            try
            {
                if (roleDto is null)
                {
                    logger.LogError("\n---\nInput role is null!\n---\n");

                    return TypedResults.BadRequest("No input role was found");
                }

                logger.LogInformation($"Updating role {roleDto.Name}");

                var role = await repository.GetAsync(r => r.Name == roleDto.Name);

                if (role is null)
                {
                    logger.LogError($"\n---\nRole {roleDto.Name} not found!\n---\n");

                    return TypedResults.NotFound($"Cannot find the role \"{roleDto.Name}\"");
                }

                if (role.ConcurrencyStamp != roleDto.ConcurrencyStamp)
                {
                    logger.LogError("Concurrency conflict!");
                    return TypedResults.Conflict("Concurrency conflict! Role was modified by another user.");
                }

                AppRole roleForUpdating = mapper.Map<AppRole>(roleDto);
                await repository.UpdateAsync(roleForUpdating);

                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when updating the role!");
            }
        }

        public async Task<Results<NoContent, NotFound<string>, BadRequest<string>>>
            DeleteRole(
                [FromServices] IAppRoleRepository repository,
                Guid roleId,
                ILogger<AppRoleEndpoints> logger)
        {
            try
            {
                logger.LogInformation($"Deleting role {roleId}");

                var role = await repository.GetAsync(r => r.Id == roleId);

                if (role is null)
                {
                    logger.LogError($"\n---\nRole {roleId} not found!\n---\n");

                    return TypedResults.NotFound($"Cannot find the role \"{roleId}\"");
                }

                await repository.RemoveAsync(role);

                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when deleting the role!");
            }
        }
    }
}
