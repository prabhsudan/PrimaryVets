namespace PrimaryVets.Models
{
    public class AppointmentSchedule
    {
        public string TimeSlot { get; set; } = string.Empty;
        public string Doctor1 { get; set; } = string.Empty;
        public string Doctor2 { get; set; } = string.Empty;
        public string Staff1 { get; set; } = string.Empty;
        public string Staff2 { get; set; } = string.Empty;
    }
    public class AppointmentCreateViewModel
    {
        public Guid? OwnerId { get; set; }
        public string? Name { get; set; }
        public List<AppointmentSchedule>? Schedules { get; set; }
    }

    public class AppointmentDetailVM
    {
        public DateTime AppointmentDate { get; set; }
        public string? TimeSlot { get; set; } 
        public string? AppointmentType { get; set; }
        public string? DoctorName { get; set; }
        public string? OwnerName { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? ReasonForVisit { get; set; }
        public string? DoctorNotes { get; set; }
    }
}
