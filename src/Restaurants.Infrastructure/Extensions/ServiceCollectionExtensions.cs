using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
/*using Microsoft.AspNetCore.Http;*/
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements.MinimumAge;
using Restaurants.Infrastructure.Authorization.Requirements.MinimumRestaurants;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Configuration;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Infrastructure.Storage;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
  public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("Dev");
    services.AddDbContext<RestaurantsDbContext>(options =>
        options
            .UseSqlServer(connectionString)
            .EnableSensitiveDataLogging());

    services
        .AddIdentity<User, IdentityRole>()
        // .AddIdentityApiEndpoints<User>()
        // .AddRoles<IdentityRole>()
        .AddClaimsPrincipalFactory<RestaurantUserClaimsPrincipalFactory>()
        .AddEntityFrameworkStores<RestaurantsDbContext>();

    services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
    services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
    services.AddScoped<IDishesRepository, DishesRepository>();
    services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
    services.AddScoped<IAuthorizationHandler, MinimumRestaurantsRequirementHandler>();
    services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();

    services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
    services.AddScoped<IBlobStorageService, BlobStorageService>();

    services.AddSingleton<TokenProvider>();

    services.AddAuthorizationBuilder()
        .AddPolicy(PolicyNames.HasNationality,
            builder => builder.RequireClaim(AppClaimTypes.Nationality, "USA", "RSA"))
        .AddPolicy(PolicyNames.AtLeast20, builder => builder.AddRequirements(new MinimumAgeRequirement(20)))
        .AddPolicy(PolicyNames.AtLeast2RestaurantsCreated,
            builder => builder.AddRequirements(new MinimumRestaurantsRequirement(2)));

    var secretKey = configuration.GetValue<string>("Jwt:Secret")!;

    services.AddAuthentication(options =>
        {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
          options.RequireHttpsMetadata = false;
          options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuer = true,
            ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
            ValidateAudience = true,
            ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
          };
        });
    /*.AddCookie(*/
    /*    options =>*/
    /*    {*/
    /*      options.Events.OnRedirectToLogin = context =>*/
    /*      {*/
    /*        context.Response.StatusCode = StatusCodes.Status401Unauthorized;*/
    /*        return Task.CompletedTask;*/
    /*      };*/
    /**/
    /*      options.Events.OnRedirectToAccessDenied = context =>*/
    /*      {*/
    /*        context.Response.StatusCode = StatusCodes.Status403Forbidden;*/
    /*        return Task.CompletedTask;*/
    /*      };*/
    /**/
    /**/
    /*      options.LoginPath = string.Empty;*/
    /*      options.AccessDeniedPath = string.Empty;*/
    /*    });*/
  }
}
