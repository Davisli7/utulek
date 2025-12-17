using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Application.Abstraction;
using Utulek1.Infrastructure.Repositories;
using Utulek1.Domain.Entities;

namespace Utulek1.Application.Implementation
{
    public class SystemLogAppService : ISystemLogAppService
    {
        private readonly ISystemLogRepository _systemLogRepository;

        public SystemLogAppService(ISystemLogRepository systemLogRepository)
        {
            _systemLogRepository = systemLogRepository;
        }

        public IList<SystemLog> Select()
        {
            return _systemLogRepository.Select();
        }

        public bool Delete(int id)
        {
            return _systemLogRepository.Delete(id);
        }
    }
}
