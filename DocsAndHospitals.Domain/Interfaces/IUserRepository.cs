using DocsAndHospitals.Models;
using System.Threading.Tasks;

namespace DocsAndHospitals.Persistence
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetByEmailAsync(string email);
    }
}
