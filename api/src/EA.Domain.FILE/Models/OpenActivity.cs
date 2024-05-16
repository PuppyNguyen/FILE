using EA.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace EA.Domain.FILE.Models;

public partial class OpenActivity : Entity, IAggregateRoot
{

    public Guid ItemId { get; set; }

    public Guid AccountId { get; set; }

    public DateTime OpenDate { get; set; }

    public virtual Item Item { get; set; } = null!;
}
