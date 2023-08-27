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
using Ploomers_Project_API.TokenService.Configuration;
using Ploomers_Project_API.TokenService;

var builder = WebApplication.CreateBuilder(args);

var tokenConfigurations = new TokenConfiguration();

new ConfigureFromConfigurationOptions<TokenConfiguration>(
    builder.Configuration.GetSection("JwtSettings")
).Configure(tokenConfigurations);

builder.Services.AddSingleton(tokenConfigurations);

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

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});

// Add services to the container.
var connectionStr = builder.Configuration.GetConnectionString("SqlServerDocker");
builder.Services.AddDbContext<SqlServerContext>(o => o.UseSqlServer(connectionStr));

builder.Services.AddAutoMapper(typeof(ClientProfile).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApiVersioning();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
