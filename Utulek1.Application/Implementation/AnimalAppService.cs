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
        IFileUploadService _fileUploadService;
        private readonly IAnimalRepository _animalRepository;

        public AnimalAppService( IFileUploadService fileUploadService, IAnimalRepository animalRepository)
        {
            _fileUploadService = fileUploadService ?? throw new ArgumentNullException(nameof(fileUploadService));
            _animalRepository = animalRepository;
        }

        public IList<Animal> Select(string? searchName = null, int? speciesId = null, string? status = null)
        {
            return _animalRepository.Select(searchName, speciesId, status);
        }

        public Animal? Select(int id)
        {
            return _animalRepository.Select(id);
        }

        public IList<Species> SelectSpecies()
        {
            return _animalRepository.SelectSpecies();
        }

        public IList<Breed> SelectBreeds()
        {
            return _animalRepository.SelectBreeds();
        }

        public void Create(Animal animal, IEnumerable<IFormFile> uploadedFiles)
        {
            if (uploadedFiles != null && uploadedFiles.Any())
            {
                animal.Photos ??= new List<Photo>();
                foreach (var file in uploadedFiles)
                {
                    string imageSrc = _fileUploadService.FileUpload(file, Path.Combine("img", "animals"));
                    animal.Photos.Add(new Photo { PhotoURL = imageSrc });
                }
            }

            _animalRepository.Create(animal);
        }

        public void Update(Animal animal, IEnumerable<IFormFile> uploadedFiles)
        {
            var existingAnimal = _animalRepository.Select(animal.AnimalID);

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
                    existingAnimal.Photos ??= new List<Photo>();
                    foreach (var file in uploadedFiles)
                    {
                        string imageSrc = _fileUploadService.FileUpload(file, Path.Combine("img", "animals"));
                        existingAnimal.Photos.Add(new Photo { PhotoURL = imageSrc });
                    }
                }

                

                _animalRepository.Update(existingAnimal);
            }
        }

        public bool DeletePhoto(int photoId)
        {
            return _animalRepository.DeletePhoto(photoId);
        }

        public bool Delete(int id)
        {
            return _animalRepository.Delete(id);
        }
    }
}
