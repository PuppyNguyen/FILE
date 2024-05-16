using EA.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace EA.Domain.FILE.Models;

public partial class AccessPermission : Entity, IAggregateRoot
{
    public Guid ItemId { get; set; }

    public Guid? AccountId { get; set; }

    public bool? Copy { get; set; }

    public bool? Download { get; set; }

    public bool? Write { get; set; }

    public bool? WriteContents { get; set; }

    public bool? Read { get; set; }

    public bool? Upload { get; set; }

    public bool? Manager { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? Owner { get; set; }

    public virtual Item Item { get; set; } = null!;
}
