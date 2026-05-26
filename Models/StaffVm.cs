namespace PrimaryVets.Models
{
    public class StaffVm
    {
        public string? Name { get; set; }
        public StaffRoles Roles { get; set; }
        public Departments Department { get; set; }
        public string? Email { get; set; }
        public StaffStatus Status { get; set; }
    }

    public enum Departments
    {
        Surgery,
        Dermatology,
        Dental,
        ENT,
        FrontDesk
    }

    public enum StaffRoles
    {
        Veterinarian,
        Assistant,
        Nurse,
        Technician,
        Receptionist
    }
    public enum StaffStatus
    {
        Active,
        Inactive,
        Suspended
    }
}
