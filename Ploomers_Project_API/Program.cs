using Microsoft.EntityFrameworkCore;
using Ploomers_Project_API.Business.Implementations;
using Ploomers_Project_API.Mappers;
using Ploomers_Project_API.Models.Context;
using Ploomers_Project_API.Repository.Implementations;
using Ploomers_Project_API.Repository;
using Ploomers_Project_API.Business;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Ploomers_Project_API.Services.TokenService.Configuration;
using Ploomers_Project_API.Services.TokenService;
using Ploomers_Project_API.Services.DbService;

var builder = WebApplication.CreateBuilder(args);

// Token Config
var tokenConfigurations = new TokenConfiguration();

new ConfigureFromConfigurationOptions<TokenConfiguration>(
    builder.Configuration.GetSection("JwtSettings")
).Configure(tokenConfigurations);

// Token Implementation
builder.Services.AddSingleton(tokenConfigurations);

// Authentication Config
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = tokenConfigurations.Issuer,
        ValidAudience = tokenConfigurations.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
    };
});

// Authorization Config
builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});

// Database Config
var connectionStr = builder.Configuration.GetConnectionString("SqlServerLocal");
builder.Services.AddDbContext<SqlServerContext>(o => o.UseSqlServer(connectionStr));

// Mappers Config
builder.Services.AddAutoMapper(typeof(ClientProfile).Assembly);
builder.Services.AddAutoMapper(typeof(EmployeeProfile).Assembly);
builder.Services.AddAutoMapper(typeof(LoginProfile).Assembly);
builder.Services.AddAutoMapper(typeof(SaleProfile).Assembly);

builder.Services.AddControllers();
builder.Services.AddApiVersioning();

// Swagger Config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PloomeRs-API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme." + 
        "Add to the field below the following: Bearer {Token generated at signin endpoint}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Dependency Injections
builder.Services.AddScoped<IClientBusiness, ClientBusinessImplementation>();
builder.Services.AddScoped<ISaleBusiness, SaleBusinessImplementation>();
builder.Services.AddScoped<ILoginBusiness, LoginBusinessImplementation>();
builder.Services.AddScoped<IEmployeeBusiness, EmployeeBusinessImplementation>();

builder.Services.AddTransient<ITokenService, TokenServiceImplementation>();

builder.Services.AddScoped(typeof(IClientRepository), typeof(ClientRepositoryImplementation));
builder.Services.AddScoped(typeof(ISaleRepository), typeof(SaleRepositoryImplementation));
builder.Services.AddScoped(typeof(ILoginRepository), typeof(LoginRepositoryImplementation));
builder.Services.AddScoped(typeof(IEmployeeRepository), typeof(EmployeeRepositoryImplementation));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Database migration
try
{
    DbService.MigrationInitialisation(app);
}
catch (Exception)
{
    Console.WriteLine("No SQL Server DB available");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
