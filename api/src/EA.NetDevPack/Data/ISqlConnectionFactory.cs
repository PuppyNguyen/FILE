using System.Data;

namespace EA.NetDevPack.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}