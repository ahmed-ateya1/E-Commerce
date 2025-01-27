using E_Commerce.Core.Caching;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos.AuthenticationDto;
using E_Commerce.Core.Services;
using E_Commerce.Core.ServicesContract;
using E_Commerce.Core.Validators.AddressValidator;
using FluentValidation;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Core.DependencyInjectionConfig
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<AddressAddRequestValidator>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductImagesService, ProductImagesService>();
            services.AddScoped<ITechnicalSpecificationService, TechnicalSpecificationService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IDeliveryMethodServices, DeliveryMethodServices>();
            services.AddScoped<IAddressServices, AddressServices>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IDealService, DealService>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddTransient<IEmailSender, EmailSender>();

            return services;
        }
    }
}
