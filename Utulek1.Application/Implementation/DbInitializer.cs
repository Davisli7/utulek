using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Entities;

namespace Utulek1.Application.Implementation
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public DbInitializer(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
        }
    }
}