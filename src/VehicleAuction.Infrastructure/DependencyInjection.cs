using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleAuction.Application.Interfaces;
using VehicleAuction.Domain.Interfaces;
using VehicleAuction.Infrastructure.Persistence;
using VehicleAuction.Infrastructure.Persistence.Repositories;
using VehicleAuction.Infrastructure.Services;

namespace VehicleAuction.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
                ?? @"Server=(localdb)\mssqllocaldb;Database=VehicleAuctionDb;Trusted_Connection=True;TrustServerCertificate=True"));

        Stripe.StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"] ?? "sk_test_placeholder";

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IBidRepository, BidRepository>();
        services.AddScoped<ITokenTransactionRepository, TokenTransactionRepository>();
        services.AddScoped<ITopUpRequestRepository, TopUpRequestRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuctionNotificationService, AuctionNotificationService>();
        services.AddScoped<IAuctionSchedulerService, AuctionSchedulerService>();

        return services;
    }
}
