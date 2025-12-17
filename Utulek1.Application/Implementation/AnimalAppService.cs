using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Entities;
using Utulek1.Infrastructure;
using Utulek1.Infrastructure.Repositories;

namespace Utulek1.Application.Implementation
{
    public class AnimalAppService : IAnimalAppService
    {
        UtulekDbContext _utulekDbContext;
        IFileUploadService _fileUploadService;
        private readonly IAnimalRepository _animalRepository;

        public AnimalAppService(UtulekDbContext eshopDbContext, IFileUploadService fileUploadService, IAnimalRepository animalRepository)
        {
            _utulekDbContext = eshopDbContext ?? throw new ArgumentNullException(nameof(eshopDbContext));
            _fileUploadService = fileUploadService ?? throw new ArgumentNullException(nameof(fileUploadService));
            _animalRepository = animalRepository;
        }

        public IList<Animal> Select(string? searchName = null, int? speciesId = null, string? status = null)
        {
            IQueryable<Animal> query = _utulekDbContext.Animals
                                       .Include(a => a.Species)
                                       .Include(a => a.Breed)
                                       .Include(a => a.Photos);
            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(a => a.Name.Contains(searchName));
            }

            if (speciesId.HasValue)
            {
                query = query.Where(a => a.SpeciesID == speciesId.Value);
            }

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(a => a.Status == status);
            }

            return query.OrderByDescending(a => a.AnimalID).ToList();
        }

        public Animal? Select(int id)
        {
            return _utulekDbContext.Animals
                                   .Include(a => a.Photos)
                                   .Include(a => a.Breed)    
                                   .Include(a => a.Species)
                                   .FirstOrDefault(a => a.AnimalID == id);
        }


        public IList<Species> SelectSpecies()
        {
            return _utulekDbContext.Species.ToList();
        }

        public IList<Breed> SelectBreeds()
        {
            return _utulekDbContext.Breeds.ToList();
        }

        public void Create(Animal animal, IEnumerable<IFormFile> uploadedFiles)
        {
            if (animal == null) throw new ArgumentNullException(nameof(animal));

            if (uploadedFiles != null && uploadedFiles.Any())
            {
                animal.Photos ??= new List<Photo>();

                foreach (var file in uploadedFiles)
                {
                    string imageSrc = _fileUploadService.FileUpload(file, Path.Combine("img", "animals"));

                    animal.Photos.Add(new Photo { PhotoURL = imageSrc });
                }
            }

            _utulekDbContext.Animals.Add(animal);
            _utulekDbContext.SaveChanges();
        }

        public void Update(Animal animal, IEnumerable<IFormFile> uploadedFiles)
        {
            Animal? existingAnimal = _utulekDbContext.Animals
                                                     .Include(a => a.Photos)
                                                     .FirstOrDefault(a => a.AnimalID == animal.AnimalID);

            if (existingAnimal != null)
            {
                existingAnimal.Name = animal.Name;
                existingAnimal.Age = animal.Age;
                existingAnimal.Description = animal.Description;
                existingAnimal.BreedID = animal.BreedID;
                existingAnimal.SpeciesID = animal.SpeciesID;
                existingAnimal.Status = animal.Status;
                existingAnimal.ArrivalDate = animal.ArrivalDate;

                if (uploadedFiles != null && uploadedFiles.Any())
                {
                    if (existingAnimal.Photos == null)
                        existingAnimal.Photos = new List<Photo>();

                    foreach (var file in uploadedFiles)
                    {
                        string imageSrc = _fileUploadService.FileUpload(file, Path.Combine("img", "animals"));

                        existingAnimal.Photos.Add(new Photo { PhotoURL = imageSrc });
                    }
                }

                _utulekDbContext.SaveChanges();
            }
        }

        public bool DeletePhoto(int photoId)
        {
            return _animalRepository.DeletePhoto(photoId);
        }

        public bool Delete(int id)
        {
            bool deleted = false;

            Animal? product
                = _utulekDbContext.Animals.FirstOrDefault(Animal => Animal.AnimalID == id);

            if (product != null)
            {
                _utulekDbContext.Animals.Remove(product);
                _utulekDbContext.SaveChanges();
                deleted = true;
            }

            return deleted;
        }
    }
}
