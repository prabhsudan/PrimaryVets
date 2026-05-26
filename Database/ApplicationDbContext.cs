using Microsoft.EntityFrameworkCore;
using PrimaryVets.Entities;

namespace PrimaryVets.Database
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        
        }
        public DbSet<VetAdminUser> VetAdminUsers { get; set; }
        public DbSet<PetRegistration> PetRegistrations { get; set; }
    }
}

