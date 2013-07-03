
using System.Collections.Generic;

namespace Escc.WebAuthorMonitoring.SqlServer
{
    /// <summary>
    /// Use SQL Server to store quality monitoring messages sent to web authors
    /// </summary>
    public class SqlServerRepository : IWebAuthorMonitoringRepository
    {
        public IEnumerable<ProblemType> ReadProblemTypes()
        {
            throw new System.NotImplementedException();
        }
    }
}
