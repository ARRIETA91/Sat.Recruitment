using Sat.Recruitment.Core.Commons;
using Sat.Recruitment.Core.Entities;
using System.Threading.Tasks;

namespace Sat.Recruitment.Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> ExistsUser(User user);
        Task<Result> CreateUser(User user);
    }
}
