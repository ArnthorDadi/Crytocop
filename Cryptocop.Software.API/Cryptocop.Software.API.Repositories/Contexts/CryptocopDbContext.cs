using Cryptocop.Software.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cryptocop.Software.API.Repositories.Contexts
{
    public class CryptocopDbContext : DbContext
    {
        public CryptocopDbContext(DbContextOptions<CryptocopDbContext> options) : base(options) {  }
        // Set Up Connections
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // --- USER CONNECTIONS ---
            // User -> Address
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses);

            // User -> ShoppingCart
            modelBuilder.Entity<User>()
                .HasOne(u => u.ShoppingCart)
                .WithOne(s => s.User);

            // User -> PaymentCard
            modelBuilder.Entity<PaymentCard>()
                .HasOne(p => p.User)
                .WithMany(u => u.PaymentCards);

            // User -> Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders);
            // ------------------------
            
            // ShoppingCart -> ShoppingCartItem
            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.ShoppingCart)
                .WithMany(sc => sc.ShoppingCartItems);

            // Order -> OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems);
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<PaymentCard> PaymentCards { get; set; }
        public DbSet<JwtToken> JwtTokens { get; set; }
        public DbSet<User> Users { get; set; }
    }
}