using Microsoft.EntityFrameworkCore;

namespace StocksApp_Whole.Entities
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<BuyOrder> buyOrderDb { get; set; }
        public DbSet<SellOrder> sellOrderDb { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map entities to tables
            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");

            // Optionally, create seed data. Not necessary here.



        }

    }
}
