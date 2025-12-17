using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;

namespace Utulek1.Infrastructure.Repositories
{
    public interface ISystemLogRepository
    {
        IList<SystemLog> Select();
        bool Delete(int id);
    }
}