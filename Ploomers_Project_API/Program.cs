using Microsoft.EntityFrameworkCore;
using Ploomers_Project_API.Business.Implementations;
using Ploomers_Project_API.Mappers;
using Ploomers_Project_API.Models.Context;
using Ploomers_Project_API.Repository.Implementations;
using Ploomers_Project_API.Repository;
using Ploomers_Project_API.Business;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionStr = builder.Configuration.GetConnectionString("SqlServerDb");
builder.Services.AddDbContext<SqlServerContext>(o => o.UseSqlServer(connectionStr));

builder.Services.AddAutoMapper(typeof(ClientProfile).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IClientBusiness, ClientBusinessImplementation>();
builder.Services.AddScoped<ISaleBusiness, SaleBusinessImplementation>();

builder.Services.AddScoped(typeof(IClientRepository), typeof(ClientRepositoryImplementation));
builder.Services.AddScoped(typeof(ISaleRepository), typeof(SaleRepositoryImplementation));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
