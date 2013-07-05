
using System;
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
        IList<ProblemType> ReadProblemTypes();

        /// <summary>
        /// Saves the problemReport and adds a unique reference number in the <see cref="ProblemReport.ProblemReportId"/> property.
        /// </summary>
        /// <param name="problemReport">The problemReport.</param>
        void SaveProblemReport(ProblemReport problemReport);

        /// <summary>
        /// Reads a problem problemReport.
        /// </summary>
        /// <param name="problemReportId">The problem problemReport id.</param>
        ProblemReport ReadProblemReport(int problemReportId);

        /// <summary>
        /// Reads problem reports
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="webAuthorPermissionsGroupName">Name of the web author permissions group.</param>
        /// <param name="webAuthorName">Name or username of the web author.</param>
        /// <returns></returns>
        IList<ProblemReport> ReadProblemReports(DateTime? startDate, DateTime? endDate, Uri pageUrl, string webAuthorPermissionsGroupName, string webAuthorName);
    }
}
