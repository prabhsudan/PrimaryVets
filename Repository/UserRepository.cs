using Microsoft.EntityFrameworkCore;
using PrimaryVets.Database;
using PrimaryVets.Entities;
using PrimaryVets.Interface;

namespace PrimaryVets.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VetAdminUser?> GetUserByUsernameAsync(string username)
        {
            return await _context.VetAdminUsers.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task UpdatePasswordAsync(VetAdminUser user)
        {
            _context.VetAdminUsers.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
