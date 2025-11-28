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
            return _utulekDbContext.Animals.ToList();
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
