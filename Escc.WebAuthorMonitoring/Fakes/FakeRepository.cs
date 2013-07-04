
using System.Collections.Generic;

namespace Escc.WebAuthorMonitoring.Fakes
{
    /// <summary>
    /// Fake repository for web author monitoring
    /// </summary>
    public class FakeRepository : IWebAuthorMonitoringRepository
    {
        /// <summary>
        /// Reads the possible types of problem which could be reported.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProblemType> ReadProblemTypes()
        {
            return new List<ProblemType>()
                {
                    new ProblemType(){ ProblemTypeId=1, Name="Urgent problem", DefaultText = "We've noticed an urgent problem."},
                    new ProblemType() {ProblemTypeId=2, Name = "Common problem", DefaultText = "We've noticed a common problem."},
                    new ProblemType(){ProblemTypeId=3,Name = "Minor problem", DefaultText = "We've noticed a minor problem."}
                };
        }

        /// <summary>
        /// Saves the report and adds a unique reference number in the <see cref="ProblemReport.ProblemReportId"/> property.
        /// </summary>
        /// <param name="report">The report.</param>
        public void SaveReport(ProblemReport report)
        {
            if (report != null) report.ProblemReportId = 12345;
        }
    }
}
