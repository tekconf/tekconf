using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace TekConf.Api.Infrastructure.DataAccess
{
    public class TekConfConfiguration : DbConfiguration
    {
        public TekConfConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new DefaultExecutionStrategy());
        }
    }
}