using System.ComponentModel.DataAnnotations;

namespace PrimaryVets.Entities
{
    public class VetAdminUser:BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
    }
}
