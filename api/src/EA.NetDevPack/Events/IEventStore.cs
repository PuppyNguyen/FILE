using EA.NetDevPack.Messaging;

namespace EA.NetDevPack.Events
{
    public interface IEventStore
    {
        void Save<T>(T theEvent) where T : Event;
    }
}