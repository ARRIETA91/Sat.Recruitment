using Sat.Recruitment.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task Add(User user);
    }
}
