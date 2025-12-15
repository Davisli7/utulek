using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Application.Abstraction;
using Utulek1.Application.ViewModels;
using Utulek1.Domain.Entities;

namespace Utulek1.Application.Implementation
{
    public class UserAppService : IUserAppService
    {
        private readonly UserManager<User> _userManager;

        public UserAppService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IList<UserListItemViewModel>> SelectAll()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserListItemViewModel>();

            foreach (var user in users)
            {
                // Získáme role uživatele (async operace, proto to nejde jednoduše v LINQ Selectu)
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserListItemViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = roles.FirstOrDefault() ?? "Zákazník" // Pokud nemá roli, je to zákazník
                });
            }

            return userList;
        }

        public async Task<bool> Delete(int id, int currentUserId)
        {
            // OCHRANA: Nemůžu smazat sám sebe
            if (id == currentUserId) return false;

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            // Smazání uživatele
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangeRole(int id, string newRole, int currentUserId)
        {
            // OCHRANA: Nemůžu měnit roli sobě (abych se neomylem sesadil z Admina)
            if (id == currentUserId) return false;

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            // 1. Získáme aktuální role
            var currentRoles = await _userManager.GetRolesAsync(user);

            // 2. Odebereme všechny stávající role
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // 3. Přidáme novou roli
            var result = await _userManager.AddToRoleAsync(user, newRole);

            return result.Succeeded;
        }
    }
}