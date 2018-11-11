using Microsoft.EntityFrameworkCore;

namespace FortuneTeller.Service.Models
{
    public class FortuneContext : DbContext
    {
        public FortuneContext(DbContextOptions<FortuneContext> options) :
            base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fortune>().HasData(new FortuneEntity() {Id = 1, Text = "Run with Endurance"});
            modelBuilder.Entity<Fortune>().HasData(new FortuneEntity() {Id = 2, Text = "He who began with complete"});
        }
        public DbSet<FortuneEntity> Fortunes { get; set; }
    }
}
