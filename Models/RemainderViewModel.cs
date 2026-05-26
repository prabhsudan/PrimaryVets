namespace PrimaryVets.Models
{
    public class RemainderViewModel
    {
        public string? Title { get; set; }        
        public string? PatientName { get; set; }    
        public string? Subtitle { get; set; }      

        public DateTime Date { get; set; }      
        public TimeSpan Time { get; set; }           

        public string? Treatment { get; set; }      

        public AppointmentStatus Status { get; set; }

        public string? Description { get; set; }
        public string? Notes { get; set; }
    }

    public enum AppointmentStatus
    {
        Upcoming,
        Completed,
        Cancelled,
        Missed
    }
}
