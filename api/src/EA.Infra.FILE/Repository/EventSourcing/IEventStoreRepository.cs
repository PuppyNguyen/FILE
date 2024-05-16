using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EA.NetDevPack.Events;

namespace EA.Infra.FILE.Repository.EventSourcing
{
    public interface IEventStoreRepository : IDisposable
    {
        void Store(StoredEvent theEvent);
        Task<IList<StoredEvent>> All(Guid aggregateId);
    }
}