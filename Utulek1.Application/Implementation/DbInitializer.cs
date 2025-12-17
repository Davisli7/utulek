using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Entities;
using Utulek1.Infrastructure; 

namespace Utulek1.Application.Implementation
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly UtulekDbContext _dbContext; 

        public DbInitializer(UserManager<User> userManager, RoleManager<Role> roleManager, UtulekDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext; 
        }

        public void Initialize()
        {
            string[] roles = { "Admin", "Manager", "Customer" };
            foreach (var role in roles)
            {
                if (!_roleManager.RoleExistsAsync(role).Result)
                {
                    _roleManager.CreateAsync(new Role(role)).Wait();
                }
            }

            if (_userManager.FindByNameAsync("admin").Result == null)
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@utulek.cz",
                    FirstName = "Hlavni",
                    LastName = "Administrator",
                    EmailConfirmed = true
                };

                var result = _userManager.CreateAsync(admin, "Admin123!").Result;
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
            }

            if (_userManager.FindByNameAsync("manager").Result == null)
            {
                var manager = new User
                {
                    UserName = "manager",
                    Email = "manager@utulek.cz",
                    FirstName = "Pan",
                    LastName = "Vedouci",
                    EmailConfirmed = true
                };

                var result = _userManager.CreateAsync(manager, "Manager123!").Result;
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(manager, "Manager").Wait();
                }
            }
            if (!_dbContext.Species.Any())
            {
                var pes = new Species { Name = "Pes" };
                var kocka = new Species { Name = "Kočka" };

                _dbContext.Species.AddRange(pes, kocka);
                _dbContext.SaveChanges(); 

                _dbContext.Breeds.AddRange(
                    new Breed { Name = "Kříženec", SpeciesID = pes.SpeciesID },
                    new Breed { Name = "Německý ovčák", SpeciesID = pes.SpeciesID },
                    new Breed { Name = "Zlatý retrívr", SpeciesID = pes.SpeciesID },
                    new Breed { Name = "Jezevčík", SpeciesID = pes.SpeciesID },

                    new Breed { Name = "Kříženec", SpeciesID = kocka.SpeciesID },
                    new Breed { Name = "Evropská krátkosrstá", SpeciesID = kocka.SpeciesID },
                    new Breed { Name = "Britská modrá", SpeciesID = kocka.SpeciesID }
                );

                _dbContext.SaveChanges();
            }
        }
    }
}