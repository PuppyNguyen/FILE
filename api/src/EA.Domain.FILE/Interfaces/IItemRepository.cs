using EA.Domain.FILE.Models;
using EA.NetDevPack.Data;

namespace EA.Domain.FILE.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<Item>> GetListCbx(Dictionary<string, object> filter);
    }
}
