using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Application.ViewModels;

namespace Utulek1.Application.Abstraction
{
    public interface IUserAppService
    {
        Task<IList<UserListItemViewModel>> SelectAll(string? searchEmail, string? roleFilter);
        Task<bool> Delete(int id, int currentUserId);

        Task<bool> ChangeRole(int id, string newRole, int currentUserId);
    }
}