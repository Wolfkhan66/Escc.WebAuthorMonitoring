
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
        /// Saves the report and adds a unique reference number in the <see cref="ProblemReport.ProblemReportId"/> property.
        /// </summary>
        /// <param name="report">The report.</param>
        void SaveProblemReport(ProblemReport report);

        /// <summary>
        /// Reads a problem report.
        /// </summary>
        /// <param name="problemReportId">The problem report id.</param>
        ProblemReport ReadProblemReport(int problemReportId);

        /// <summary>
        /// Reads problem reports
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="problemType">Type of the problem.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="webAuthorPermissionsGroup">Name of the web author permissions group.</param>
        /// <param name="webAuthorName">Name or username of the web author.</param>
        /// <returns></returns>
        IList<ProblemReport> ReadProblemReports(DateTime? startDate, DateTime? endDate, ProblemType problemType, Uri pageUrl, string webAuthorPermissionsGroup, string webAuthorName);
    }
}
