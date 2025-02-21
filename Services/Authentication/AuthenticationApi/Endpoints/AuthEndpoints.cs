namespace TaskManagementApp.Services.AuthenticationApi.Endpoints
{
    public class AuthEndpoints : ICarterModule
    {
        public sealed record LoginRefreshTokenRequest(string RefreshToken);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

            var group = app.MapGroup("/api/v{version:apiVersion}/auth")
                .WithApiVersionSet(apiVersionSet);

            group.MapPost("/login", Login);
            group.MapPost("/refresh_token_login", LoginWithRefreshToken);
            group.MapPost("/register", Register);
        }

        public async Task<Results<Ok<LoginResponse>, NotFound<string>, BadRequest<string>>>
            Login(
                [FromBody] LoginRequest request,
                [FromServices] IAuthRepository repository,
                [FromServices] ITokenProvider tokenProvider,
                UserManager<AppUser> userManager,
                IMapper mapper,
                [FromServices] ILogger<AuthEndpoints> logger,
                IConfiguration configuration)
        {
            try
            {
                logger.LogInformation("Signing the user in...");

                var user = await repository.GetUserAsync(u => u.UserName == request.UserName);

                bool isUserValid = await userManager.CheckPasswordAsync(user, request.Password);

                if (isUserValid == false)
                {
                    logger.LogError("Invalid credential!");
                    return TypedResults.BadRequest("Username or password is wrong!");
                }

                // If credential is valid, generate JWT token
                IEnumerable<Role> roles = (await userManager.GetRolesAsync(user))
                    .Select(r => (Role)Enum.Parse(typeof(Role), r));
                var token = tokenProvider.CreateToken(user, roles);
                
                logger.LogDebug($"configure: {configuration.GetValue<int>("RefreshTokenExpirationDays")}");

                var refreshToken = new RefreshToken()
                {
                    Id = Guid.NewGuid(),
                    AppUserId = user.Id,
                    Token = tokenProvider.GenerateRefreshToken(),
                    ExpireOnUtc = DateTime.UtcNow.AddDays(configuration.GetValue<int>("RefreshTokenLifeTimeInDays"))
                };
                
                await repository.CreateRefreshTokenAsync(refreshToken);

                LoginResponse response = new LoginResponse()
                {
                    AccessToken = token,
                    RefreshToken = refreshToken.Token
                };

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when signing the user in!");
            }
        }

        public async Task<Results<Ok<string>, BadRequest<string>>> 
            Register(
                [FromBody] RegistrationRequest request,
                [FromServices] ILogger<AuthEndpoints> logger,
                IMapper mapper,
                UserManager<AppUser> userManager)
        {
            try
            {
                logger.LogInformation("Registering the user...");

                AppUser user = mapper.Map<AppUser>(request);

                var result = await userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, request.Role.ToString());
                }

                return TypedResults.Ok("Register successfully!");
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when registering the user!");
            }
        }

        public async Task<Results<Ok<LoginResponse>, BadRequest<string>>> 
            LoginWithRefreshToken(
                [FromBody] LoginRefreshTokenRequest request,
                [FromServices] IAuthRepository repository,
                [FromServices] ITokenProvider tokenProvider,
                [FromServices] ILogger<AuthEndpoints> logger,
                IConfiguration configuration,
                UserManager<AppUser> userManager)
        {
            try
            {
                logger.LogInformation("Signing the user in with the refresh token...");

                var refreshToken = await repository.GetRefreshTokenAsync(
                    include: q => q.Include(r => r.AppUser),
                    filter: r => r.Token == request.RefreshToken,
                    tracked: false);

                if (refreshToken.ExpireOnUtc < DateTime.UtcNow)
                {
                    logger.LogError("Refresh token is expired!");
                    
                    return TypedResults.BadRequest("Refresh token is expired!");
                }
                
                IEnumerable<Role> roles = (await userManager.GetRolesAsync(refreshToken.AppUser!))
                    .Select(r => (Role)Enum.Parse(typeof(Role), r));
                
                string accessToken = tokenProvider.CreateToken(refreshToken.AppUser!, roles);
                
                refreshToken.Token = tokenProvider.GenerateRefreshToken();

                refreshToken.ExpireOnUtc =
                    DateTime.UtcNow.AddDays(configuration.GetValue<int>("RefreshTokenLifeTimeInDays"));
                
                await repository.UpdateRefreshTokenAsync(refreshToken);

                var response = new LoginResponse()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token
                };
                
                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError("\n---\nError(s) occured: \n---\n{error}", ex);

                return TypedResults.BadRequest("Error(s) occured when signing the user in!");
            }
        }
    }
}
