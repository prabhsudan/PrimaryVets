namespace PrimaryVets.Entities
{
    public class Page : BaseEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
    }

    public class RolePage : BaseEntity
    {
        public int Id { get; set; }
        public string? RoleName { get; set; }
        public int PageId { get; set; }
        public bool IsActive { get; set; }
    }
}
