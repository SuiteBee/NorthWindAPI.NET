
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NorthWindAPI.Controllers.Mappers;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.Repositories;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Services;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.Mappers;
using NorthWindAPI.Views;
using NorthWindAPI.Views.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

/////////////////////////////////////////////////////////////
// Token Auth Configuration
/////////////////////////////////////////////////////////////
var jwtIssuer = configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

/////////////////////////////////////////////////////////////
// Register Servies
/////////////////////////////////////////////////////////////
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("SqliteConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Northwind API", 
        Description = "REST API backend for NorthWindWeb", 
        Version = "v1" 
    });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "NorthWindAPIAnnotation.xml"));
});

//Repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

//Services
builder.Services.AddCors();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();

//Views
builder.Services.AddScoped<IDashboardView, DashboardView>();

// Auto Mapper Config
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new OrderDtoMap());
    cfg.AddProfile(new OrderRequestMap());
    cfg.AddProfile(new CustomerDtoMap());
    cfg.AddProfile(new CustomerRequestMap());
    cfg.AddProfile(new CustomerResponseMap());
    cfg.AddProfile(new EmployeeResponseMap());
    cfg.AddProfile(new ProductDtoMap());
    cfg.AddProfile(new ProductResponseMap());
    cfg.AddProfile(new OrderResponseMap());
    cfg.AddProfile(new UserDtoMap());

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
    app.UseSwaggerUI(options =>
    {
        options.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();

//Enable CORS to allow anything from dev host
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000", "http://localhost:8000"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
