var builder = WebApplication.CreateBuilder(args);

string key = File.ReadAllText("/run/secrets/aes_key").Trim();
string iv = File.ReadAllText("/run/secrets/aes_iv").Trim();
var secretHandler = new SecretHandler(key, iv);

string jwtPath = Path.Combine(Directory.GetCurrentDirectory(), "jwt_properties.json");
var jwtProperties = File.ReadAllText(jwtPath);

File.WriteAllText(
    Path.Combine(Directory.GetCurrentDirectory(), "jwt_properties.enc"), 
    secretHandler.Encrypt(jwtProperties));

// Add services to the container.
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddDbContextFactory<AuthenticationContext>(options
    => options.UseSqlServer(
        builder.Configuration.GetConnectionString("UsersDB"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(15),
            errorNumbersToAdd: null)
        ));

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AuthenticationContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<ITokenProvider, TokenProvider>();
builder.Services.AddSingleton<SecretHandler>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddCarter();

builder.Services.AddAutoMapper(config =>
{
    config.CreateMap<AppUser, AppUserDto>().ReverseMap();
    config.CreateMap<NormalUser, NormalUserDto>().ReverseMap();
    config.CreateMap<AdminUser, AdminUserDto>().ReverseMap();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapCarter();

app.UseHttpsRedirection();

app.Run();
