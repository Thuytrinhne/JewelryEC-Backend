using FluentValidation.WebApi;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Extensions;
using JewelryEC_Backend.Filters;
using JewelryEC_Backend.Models.Auths;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Repository.IRepository;
using JewelryEC_Backend.Repository;
using JewelryEC_Backend.Service;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using JewelryEC_Backend.Service;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using JewelryEC_Backend.Models.Roles.Entities;
using Asp.Versioning;


using JewelryEC_Backend.UnitOfWork;
using JewelryEC_Backend.Repository;
using JewelryEC_Backend.Repository.IRepository;
using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Core.Repository.EntityFramework;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApiVersioning(x =>
{
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.DefaultApiVersion = new ApiVersion(1, 0);
    x.ReportApiVersions = true;
    //// x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
    //x.ApiVersionReader = ApiVersionReader.Combine(
    //    new QueryStringApiVersionReader("api-version"),
    //    new HeaderApiVersionReader("x-Version"),
    //    new MediaTypeApiVersionReader("ver"));

}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
}
);

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddEntityFrameworkStores<AppDbContext>()  
        .AddDefaultTokenProviders();



builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IRedisShoppingCartService, RedisShoppingCartService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IProductCouponService, ProductCouponService>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ValidateModelAttribute());
});




builder.Services.AddControllers();


#region Repositories
builder.Services.AddTransient(typeof(IGenericRepository<>),
    typeof(GenericRepository<>));
builder.Services.AddTransient<ICatalogRepository, CatalogRepository>(); // don't need if use UOW
#endregion
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});
#region authentication 
// when we are authenticating, we are validating 3 things
// (secret,issuer, audience )
// => we will have to validate our token using all 3 of them
#endregion
builder.AddAppAuthetication();
builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


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
app.UseAuthentication();


app.MapControllers();

app.Run();
