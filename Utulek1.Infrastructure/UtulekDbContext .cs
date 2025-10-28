using Microsoft.EntityFrameworkCore;
using Utulek1.Domain.Entities;

namespace Utulek1.Infrastructure
{
    public class UtulekDbContext : DbContext
    {
        public UtulekDbContext(DbContextOptions<UtulekDbContext> options)
            : base(options) { }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AdoptionRequest> AdoptionRequests { get; set; }


    }
}