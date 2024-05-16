
namespace EA.Application.FILE.DTOs
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Size { get; set; }
        public bool IsFile { get; set; }
        public Guid? ParentId { get; set; }
        public string? MimeType { get; set; }
        public bool? HasChild { get; set; }
        public string? LocalPath { get; set; }
        public string? Cdn { get; set; }
        public string? Product { get; set; }
        public int? Status { get; set; }
        public string? Workspace { get; set; }
        public string? Content { get; set; }
        public string? Tenant { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class ItemQueryParams
    {
        public string? ParentId { get; set; }
        public string? Tenant { get; set; }
        public string? Workspace { get; set; }
        public int? Status { get; set; }
        public string? Product { get; set; }
    }
}
