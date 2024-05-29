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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using JewelryEC_Backend.Models.Roles.Entities;
using Asp.Versioning;
using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Core.Repository.EntityFramework;
using Newtonsoft.Json.Serialization;
using JewelryEC_Backend.Enum;
using System.Configuration;
using Newtonsoft.Json.Converters;
using JewelryEC_Backend.Utility;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConstr"));
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddEntityFrameworkStores<AppDbContext>()  
        .AddDefaultTokenProviders();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ValidateModelAttribute());
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new CamelCaseNamingStrategy()
    };
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});


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
builder.Services.AddScoped<IVnPayService, VnPayService>();


builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPhotoCloudService, PhotoCloudService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IProductCouponService, ProductCouponService>();
builder.Services.AddTransient<IUserCouponService, UserCouponService>();



builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ValidateModelAttribute());
});




builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });


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

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<CloudinarySettingsKey>(
        builder.Configuration.GetSection("CloudinarySettings")
    ); 


builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
    });

var app = builder.Build();

app.UseCors("CorsPolicy");
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
