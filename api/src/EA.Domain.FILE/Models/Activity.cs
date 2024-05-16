using EA.NetDevPack.Domain;


namespace EA.Domain.FILE.Models;

public partial class Activity : Entity, IAggregateRoot
{

    public Guid ItemId { get; set; }

    public Guid AccountId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Body { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
