
namespace Escc.WebAuthorMonitoring
{
    /// <summary>
    /// A component which is notified about a new problem report when it is published 
    /// </summary>
    public interface IReportListener
    {
        /// <summary>
        /// Receive a new report and act upon it
        /// </summary>
        /// <param name="report">The report.</param>
        void ReportPublished(ProblemReport report);
    }
}
