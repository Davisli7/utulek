using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;

namespace Utulek1.Infrastructure.Repositories
{
    public class SystemLogRepository : ISystemLogRepository
    {
        private readonly UtulekDbContext _dbContext;

        public SystemLogRepository(UtulekDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<SystemLog> Select()
        {
            return _dbContext.SystemLogs.OrderByDescending(x => x.TimeStamp).ToList();
        }

        public bool Delete(int id)
        {
            var log = _dbContext.SystemLogs.FirstOrDefault(x => x.Id == id);
            if (log != null)
            {
                _dbContext.SystemLogs.Remove(log);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}