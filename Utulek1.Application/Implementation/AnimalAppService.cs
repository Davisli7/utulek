using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Entities;
using Utulek1.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Utulek1.Application.Implementation
{
    public class AnimalAppService : IAnimalAppService
    {
        UtulekDbContext _utulekDbContext;
        IFileUploadService _fileUploadService;

        public AnimalAppService(UtulekDbContext eshopDbContext, IFileUploadService fileUploadService)
        {
            _utulekDbContext = eshopDbContext ?? throw new ArgumentNullException(nameof(eshopDbContext));
            _fileUploadService = fileUploadService ?? throw new ArgumentNullException(nameof(fileUploadService));
        }

        public IList<Animal> Select()
        {
            return _utulekDbContext.Animals
                .Include(a => a.Breed)   // Načti plemeno
                .Include(a => a.Species) // Načti druh
                .Include(a => a.Photos)  // Načti fotky (pokud je chceš zobrazovat v seznamu)
                .ToList();
        }

        public Animal? Select(int id)
        {
            // Načteme zvíře i s fotkami, abychom je mohli v editaci zobrazit
            return _utulekDbContext.Animals
                                   .Include(a => a.Photos)
                                   .Include(a => a.Breed)    // <--- PŘIDAT TOTO (načte plemeno)
                                   .Include(a => a.Species)
                                   .FirstOrDefault(a => a.AnimalID == id);
        }

        // Přidejte tyto dvě metody do třídy AnimalAppService

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
                // Inicializujeme kolekci, pokud je null
                animal.Photos ??= new List<Photo>();

                foreach (var file in uploadedFiles)
                {
                    string imageSrc = _fileUploadService.FileUpload(file, Path.Combine("img", "animals"));

                    // Přidáme nový objekt Photo do kolekce
                    animal.Photos.Add(new Photo { PhotoURL = imageSrc });
                }
            }

            _utulekDbContext.Animals.Add(animal);
            _utulekDbContext.SaveChanges();
        }

        public void Update(Animal animal, IEnumerable<IFormFile> uploadedFiles)
        {
            // Najdeme původní záznam v databázi (včetně fotek)
            Animal? existingAnimal = _utulekDbContext.Animals
                                                     .Include(a => a.Photos)
                                                     .FirstOrDefault(a => a.AnimalID == animal.AnimalID);

            if (existingAnimal != null)
            {
                // Aktualizujeme základní údaje
                existingAnimal.Name = animal.Name;
                existingAnimal.Age = animal.Age;
                existingAnimal.Description = animal.Description;
                existingAnimal.BreedID = animal.BreedID;
                existingAnimal.SpeciesID = animal.SpeciesID;
                existingAnimal.Status = animal.Status;
                existingAnimal.ArrivalDate = animal.ArrivalDate;

                // Pokud byly nahrány NOVÉ fotky, přidáme je k těm stávajícím
                if (uploadedFiles != null && uploadedFiles.Any())
                {
                    // Inicializace listu, kdyby byl náhodou null (což by díky Include neměl být)
                    if (existingAnimal.Photos == null)
                        existingAnimal.Photos = new List<Photo>();

                    foreach (var file in uploadedFiles)
                    {
                        // Použijeme tvou existující službu pro nahrání souboru
                        string imageSrc = _fileUploadService.FileUpload(file, Path.Combine("img", "animals"));

                        existingAnimal.Photos.Add(new Photo { PhotoURL = imageSrc });
                    }
                }

                // Uložíme změny do databáze
                _utulekDbContext.SaveChanges();
            }
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
