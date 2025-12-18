using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;

namespace Utulek1.Infrastructure.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly UtulekDbContext _context;
        public AnimalRepository(UtulekDbContext context)
        {
            _context = context;
        }

        public async Task<List<Animal>> GetAllAsync()
        {
            return await _context.Animals.Include(a => a.Breed).Include(a => a.Species).ToListAsync();
        }

        public bool DeletePhoto(int photoId)
        {
            var photo = _context.Photos.FirstOrDefault(p => p.PhotoID == photoId);

            if (photo == null)
            {
                return false;
            }

            _context.Photos.Remove(photo);

            _context.SaveChanges();

            return true;
        }

        public async Task<Animal> GetByIdAsync(int id)
        {
            return await _context.Animals
                .Include(a => a.Breed)
                .Include(a => a.Species)
                .Include(a => a.Photos)
                .FirstOrDefaultAsync(a => a.AnimalID == id);
        }

        public async Task AddAsync(Animal animal)
        {
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Animal animal)
        {
            _context.Animals.Update(animal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal != null)
            {
                _context.Animals.Remove(animal);
                await _context.SaveChangesAsync();
            }
        }

        public IList<Animal> Select(string? searchName = null, int? speciesId = null, string? status = null)
        {
            IQueryable<Animal> query = _context.Animals
                                       .Include(a => a.Species)
                                       .Include(a => a.Breed)
                                       .Include(a => a.Photos);

            if (!string.IsNullOrEmpty(searchName))
                query = query.Where(a => a.Name.Contains(searchName));

            if (speciesId.HasValue)
                query = query.Where(a => a.SpeciesID == speciesId.Value);

            if (!string.IsNullOrEmpty(status) && status != "All")
                query = query.Where(a => a.Status == status);

            return query.OrderByDescending(a => a.AnimalID).ToList();
        }

        public Animal? Select(int id)
        {
            return _context.Animals
                             .Include(a => a.Photos)
                             .Include(a => a.Breed)
                             .Include(a => a.Species)
                             .FirstOrDefault(a => a.AnimalID == id);
        }

        public void Create(Animal animal)
        {
            _context.Animals.Add(animal);
            _context.SaveChanges();
        }

        public void Update(Animal animal)
        {

            var existing = _context.Animals.Find(animal.AnimalID);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(animal);
                _context.SaveChanges();
            }
        }


        public bool Delete(int id)
        {
            var animal = _context.Animals.Find(id);
            if (animal != null)
            {
                _context.Animals.Remove(animal);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IList<Species> SelectSpecies() => _context.Species.ToList();
        public IList<Breed> SelectBreeds() => _context.Breeds.ToList();
    }
}

