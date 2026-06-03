using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using VehicleAuction.Domain.Entities;

namespace VehicleAuction.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<User> UsersData => Set<User>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Auction> Auctions => Set<Auction>();
    public DbSet<Bid> Bids => Set<Bid>();
    public DbSet<TokenTransaction> TokenTransactions => Set<TokenTransaction>();
    public DbSet<TopUpRequest> TopUpRequests => Set<TopUpRequest>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var imageUrlsConverter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

        builder.Entity<Vehicle>()
            .Property(v => v.ImageUrls)
            .HasConversion(imageUrlsConverter);

        builder.Entity<Vehicle>()
            .HasOne(v => v.Auction)
            .WithOne(a => a.Vehicle)
            .HasForeignKey<Auction>(a => a.VehicleId);

        builder.Entity<Auction>()
            .HasMany(a => a.Bids)
            .WithOne(b => b.Auction)
            .HasForeignKey(b => b.AuctionId);

        builder.Entity<User>()
            .HasMany(u => u.Bids)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId);

        builder.Entity<User>()
            .HasMany(u => u.TokenTransactions)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);

        builder.Entity<User>()
            .HasMany(u => u.TopUpRequests)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);
    }
}
