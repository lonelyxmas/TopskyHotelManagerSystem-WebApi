using SqlSugar;

namespace EOM.TSHotelManagement.Shared
{
    public interface ISqlSugarClientFactory
    {
        ISqlSugarClient CreateClient(string dbName = null);
    }
}
