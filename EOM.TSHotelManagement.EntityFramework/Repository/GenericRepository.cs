using SqlSugar;

namespace EOM.TSHotelManagement.EntityFramework
{
    public class GenericRepository<T> : SimpleClient<T> where T : class, new()
    {
        public GenericRepository(ISqlSugarClient client) : base(client)
        {
            base.Context.Aop.OnError = (ex) => { };
        }
    }
}
