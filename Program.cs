using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NorthWindAPI.Services;
using NorthWindAPI.Data.Context;

using NorthWindAPI.Services.Mappers;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Repositories;
using AutoMapper.EquivalencyExpression;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

/////////////////////////////////////////////////////////////
// Register Servies
/////////////////////////////////////////////////////////////
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Northwind API", Version = "v1" });
});

//Repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

//Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Auto Mapper Config
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingOrder());
    cfg.AddCollectionMappers();
    cfg.UseEntityFrameworkCoreModel<AppDbContext>();
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
