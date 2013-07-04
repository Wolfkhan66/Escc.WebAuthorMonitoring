
using System;
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

        public void SaveReport(ProblemReport report)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ProblemReport> ReadProblemReports(DateTime? startDate, DateTime? endDate, ProblemType problemType, Uri pageUrl, string webAuthorPermissionsGroup, string webAuthorName)
        {
            throw new NotImplementedException();
        }
    }
}
