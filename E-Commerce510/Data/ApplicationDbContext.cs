using E_Commerce510.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce510.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=E-Commerce510;Integrated Security=True;TrustServerCertificate=True");
        }

    }
}
