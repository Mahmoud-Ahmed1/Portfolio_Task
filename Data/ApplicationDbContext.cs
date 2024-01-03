using Microsoft.EntityFrameworkCore;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>()
                .HasOne(s => s.PortfolioItem)
                .WithMany(p => p.Skills)
                .HasForeignKey(s => s.PortfolioItemId);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.PortfolioItem)
                .WithMany(pi => pi.Projects)
                .HasForeignKey(p => p.PortfolioItemId);
        }
    }
}
