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
}
