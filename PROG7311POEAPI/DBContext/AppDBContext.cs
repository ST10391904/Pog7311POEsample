using Microsoft.EntityFrameworkCore;
using PROG7311POEAPI.Models;


namespace PROG7311POEAPI.DB;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ClientDetails> clientDetails { get; set; }
        public DbSet<Contracts> Contracts { get; set; }
        public DbSet<LogisticsManager> LogisticsManagers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Contracts>()
        .HasOne(c => c.ClientDetails)
        .WithMany(c => c.Contracts)
        .HasForeignKey(c => c.ClientID);
}
    }
