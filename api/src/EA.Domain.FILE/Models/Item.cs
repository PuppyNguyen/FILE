using EA.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace EA.Domain.FILE.Models;


public partial class Item : Entity, IAggregateRoot
{

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

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Product { get; set; }

    public int? Status { get; set; }

    public string? Workspace { get; set; }

    public string? Content { get; set; }

    public string? Tenant { get; set; }

    public virtual ICollection<AccessPermission> AccessPermissions { get; } = new List<AccessPermission>();

    public virtual ICollection<Activity> Activities { get; } = new List<Activity>();

    public virtual ICollection<OpenActivity> OpenActivities { get; } = new List<OpenActivity>();

    public virtual Product? ProductNavigation { get; set; }
}
