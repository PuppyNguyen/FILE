using EA.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace EA.Domain.FILE.Models;

public partial class Product : Entity, IAggregateRoot
{

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public virtual ICollection<Item> Items { get; } = new List<Item>();
}
