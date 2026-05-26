using PrimaryVets.Entities;

namespace PrimaryVets.Interface
{
    public interface IUserRepository
    {
        Task<VetAdminUser?> GetUserByUsernameAsync(string username);
        Task UpdatePasswordAsync(VetAdminUser user);
    }
}
