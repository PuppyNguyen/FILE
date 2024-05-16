using EA.NetDevPack.Domain;

namespace EA.NetDevPack.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        Task<IEnumerable<T>> GetAll();
        void Add(T t);
        void Update(T t);
        void Remove(T t);
        Task<T> GetById(Guid id);
        Task<Boolean> CheckExistById(Guid id);
    }
}