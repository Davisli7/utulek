using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Utulek1.Domain.Entities;

namespace Utulek1.Infrastructure
{

    public class UtulekDbContext : IdentityDbContext<User, Role, int>
    {
        public UtulekDbContext(DbContextOptions<UtulekDbContext> options)
            : base(options) { }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<AdoptionRequest> AdoptionRequests { get; set; }
        public DbSet<Carousel> Carousels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 
        }
    }
}