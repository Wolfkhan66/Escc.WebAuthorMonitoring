
using System.Collections.Generic;

namespace Escc.WebAuthorMonitoring
{
    /// <summary>
    /// A repository to store quality monitoring messages sent to web authors
    /// </summary>
    public interface IWebAuthorMonitoringRepository
    {
        /// <summary>
        /// Reads the possible types of problem which could be reported.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProblemType> ReadProblemTypes();

        /// <summary>
        /// Saves the report and adds a unique reference number in the <see cref="ProblemReport.ProblemReportId"/> property.
        /// </summary>
        /// <param name="report">The report.</param>
        void SaveReport(ProblemReport report);
    }
}
