using DocsAndHospitals.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DocsAndHospitals.Persistence
{
    public class UserEfRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserEfRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
