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

        public async Task<IList<UserListItemViewModel>> SelectAll(string? searchEmail, string? roleFilter)
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserListItemViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserListItemViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = roles.FirstOrDefault() ?? "Zákazník" 
                });
            }


            if (!string.IsNullOrEmpty(searchEmail))
            {
                userList = userList.Where(u => u.Email != null &&
                                               u.Email.Contains(searchEmail, StringComparison.CurrentCultureIgnoreCase))
                                   .ToList();
            }

            if (!string.IsNullOrEmpty(roleFilter) && roleFilter != "All")
            {
                userList = userList.Where(u => u.Role == roleFilter).ToList();
            }

            return userList;
        }

        public async Task<bool> Delete(int id, int currentUserId)
        {
            if (id == currentUserId) return false;

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangeRole(int id, string newRole, int currentUserId)
        {
            if (id == currentUserId) return false;

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            var currentRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await _userManager.AddToRoleAsync(user, newRole);

            return result.Succeeded;
        }
    }
}