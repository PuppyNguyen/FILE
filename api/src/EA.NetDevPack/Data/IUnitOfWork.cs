using System.Threading.Tasks;

namespace EA.NetDevPack.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}