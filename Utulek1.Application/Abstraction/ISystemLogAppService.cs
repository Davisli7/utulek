using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;

namespace Utulek1.Application.Abstraction
{
    public interface ISystemLogAppService
    {
        IList<SystemLog> Select(string? searchTerm = null, string? level = null);
        bool Delete(int id);
    }
}
