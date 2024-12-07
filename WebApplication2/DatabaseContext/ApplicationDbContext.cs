using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.DatabaseContext
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public ApplicationDbContext()
        {

        }
        public virtual DbSet<City> Cities { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().HasData(new City()
            {
                CityId =Guid.Parse("8f30bedc-47dd-4286-8950-73d8a68e5d41"),
                CityName = "Hyderabad",
            }) ;
            modelBuilder.Entity<City>().HasData(new City()
            {
                CityId = Guid.Parse("8f30bedc-47dd-4286-8950-73d8a68e5d42"),
                CityName = "Chennai",
            });
        }

    }
}
