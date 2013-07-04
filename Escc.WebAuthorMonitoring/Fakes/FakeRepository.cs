
using System;
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
        public IEnumerable<ProblemReport> ReadProblemReports(DateTime? startDate, DateTime? endDate, ProblemType problemType, Uri pageUrl, string webAuthorPermissionsGroup, string webAuthorName)
        {
            var list = new List<ProblemReport>()
                {
                    new ProblemReport()
                        {
                            ProblemReportId = 12345,
                            MessageHtml = "We've noticed an urgent problem.",
                            Page = new Page(){PageTitle = "Page title", PageUrl = new Uri("http://ww.example.org")},
                            ReportDate = DateTime.Now.AddDays(-10),
                            WebAuthorPermissionsGroupName = "Permissions group"
                        },
                                            new ProblemReport()
                        {
                            ProblemReportId = 54321,
                            MessageHtml = "We've noticed a minor problem.",
                            Page = new Page(){PageTitle = "Page title", PageUrl = new Uri("http://ww.example.org")},
                            ReportDate = DateTime.Now.AddDays(-30),
                            WebAuthorPermissionsGroupName = "Permissions group"
                        }

                };

            list[0].WebAuthors.Add(new WebAuthor() { Name = "John Smith", EmailAddress = "john.smith@example.org", UserName = "johnsmith", UserId = "1" });
            list[0].WebAuthors.Add(new WebAuthor() { Name = "Jane Smith", EmailAddress = "jane.smith@example.org", UserName = "janesmith", UserId = "2" });
            list[0].ProblemTypes.Add(new ProblemType() { Name = "Urgent problem" });

            list[1].WebAuthors.Add(new WebAuthor() { Name = "John Smith", EmailAddress = "john.smith@example.org", UserName = "johnsmith", UserId = "1" });
            list[1].WebAuthors.Add(new WebAuthor() { Name = "Jane Smith", EmailAddress = "jane.smith@example.org", UserName = "janesmith", UserId = "2" });
            list[1].ProblemTypes.Add(new ProblemType() { Name = "Broken link" });

            return list;
        }
    }
}
