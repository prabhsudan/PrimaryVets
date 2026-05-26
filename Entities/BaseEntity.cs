namespace PrimaryVets.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
