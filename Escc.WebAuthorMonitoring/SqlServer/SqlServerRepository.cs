
using System;
using System.Collections.Generic;

namespace Escc.WebAuthorMonitoring.SqlServer
{
    /// <summary>
    /// Use SQL Server to store quality monitoring messages sent to web authors
    /// </summary>
    public class SqlServerRepository : IWebAuthorMonitoringRepository
    {
        public IList<ProblemType> ReadProblemTypes()
        {
            throw new System.NotImplementedException();
        }

        public void SaveProblemReport(ProblemReport report)
        {
            throw new System.NotImplementedException();
        }

        public ProblemReport ReadProblemReport(int problemReportId)
        {
            throw new NotImplementedException();
        }

        public IList<ProblemReport> ReadProblemReports(DateTime? startDate, DateTime? endDate, Uri pageUrl, string webAuthorPermissionsGroup, string webAuthorName)
        {
            throw new NotImplementedException();
        }
    }
}
