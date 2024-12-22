using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Infrastructure.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using E_Commerce.Core.Dtos.AuthenticationDto;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Services;
using E_Commerce.Core.ServicesContract;
using E_Commerce.Infrastructure.UnitOfWorkConfig;
using Microsoft.AspNetCore.Identity.UI.Services;
using E_Commerce.Core.MappingProfile;
using E_Commerce.Core.Queries.BrandQueries;
using E_Commerce.Core.Caching;
using E_Commerce.API.FileServices;
using StackExchange.Redis;

namespace E_Commerce.API.StartupExtensions
{
    /// <summary>
    /// Provides extension methods for configuring services in the application.
    /// </summary>
    public static class ConfigureServiceExtension
    {
        /// <summary>
        /// Configures the services required for the application.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="configuration">The configuration to use for setting up the services.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection ServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 5;
            })
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders()
              .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
              .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(o =>
               {
                   o.RequireHttpsMetadata = false;
                   o.SaveToken = false;
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidIssuer = configuration["JWT:Issuer"],
                       ValidAudience = configuration["JWT:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                       ClockSkew = TimeSpan.Zero
                   };
               });
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(1);
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost3000", builder =>
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .SetIsOriginAllowed(origin => true)
                           .WithExposedHeaders("Set-Cookie"));
            });
            services.AddLogging();
            services.AddDistributedMemoryCache();
            services.AddMemoryCache();
            services.AddSingleton<IConnectionMultiplexer>(x =>
            {
                var options = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(options);
            });
            services.AddAutoMapper(typeof(BrandConfig).Assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetAllBrandQuery).Assembly));
            services.AddHealthChecks();

            services.Configure<JwtDTO>(configuration.GetSection("JWT"));
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IFileServices, FileService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductImagesService, ProductImagesService>();
            services.AddScoped<ITechnicalSpecificationService, TechnicalSpecificationService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IOrderServices , OrderServices>();
            services.AddScoped<IDeliveryMethodServices, DeliveryMethodServices>();
            services.AddScoped<IAddressServices, AddressServices>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce APP", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
            });
            return services;
        }
    }
}
