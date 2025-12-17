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
            // 1. Najdeme fotku podle ID
            var photo = _context.Photos.FirstOrDefault(p => p.PhotoID == photoId);

            // Pokud neexistuje, vrátíme false
            if (photo == null)
            {
                return false;
            }

            // 2. Smažeme ji z DbSetu (příprava SQL DELETE)
            _context.Photos.Remove(photo);

            // 3. Potvrdíme změny do databáze
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
    }
}

