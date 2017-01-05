
using System;
using System.Collections.Generic;

namespace Escc.WebAuthorMonitoring.Fakes
{
    /// <summary>
    /// Fake repository for web author monitoring
    /// </summary>
    public class FakeRepository //: IWebAuthorMonitoringRepository
    {
        /// <summary>
        /// Reads the possible types of problem which could be reported.
        /// </summary>
        /// <returns></returns>
        public IList<ProblemType> ReadProblemTypes()
        {
            return new List<ProblemType>()
                {
                    new ProblemType(){ ProblemTypeId=1, Name="Urgent problem", DefaultText = "We've noticed an urgent problem."},
                    new ProblemType() {ProblemTypeId=2, Name = "Common problem", DefaultText = "We've noticed a common problem."},
                    new ProblemType(){ProblemTypeId=3,Name = "Minor problem", DefaultText = "We've noticed a minor problem."}
                };
        }

        /// <summary>
        /// Saves the problemReport and adds a unique reference number in the <see cref="ProblemReport.ProblemReportId"/> property.
        /// </summary>
        /// <param name="problemReport">The problemReport.</param>
        public void SaveProblemReport(ProblemReport problemReport)
        {
            if (problemReport != null) problemReport.ProblemReportId = 12345;
        }

        /// <summary>
        /// Reads a problem problemReport.
        /// </summary>
        /// <param name="problemReportId">The problem problemReport id.</param>
        public ProblemReport ReadProblemReport(int problemReportId)
        {
            var report = new ProblemReport()
                {
                    ProblemReportId = 12345,
                    MessageHtml = "We've noticed an urgent problem.",
                    Page = new Page() { PageTitle = "Page title", PageUrl = new Uri("http://ww.example.org") },
                    ReportDate = DateTime.Now.AddDays(-10),
                    WebAuthorPermissionsGroupName = "Permissions group"
                };
            report.WebAuthors.Add(new WebAuthor() { Name = "John Smith", EmailAddress = "john.smith@example.org", UserName = "johnsmith", WebAuthorId = 1 });
            report.WebAuthors.Add(new WebAuthor() { Name = "Jane Smith", EmailAddress = "jane.smith@example.org", UserName = "janesmith", WebAuthorId = 2 });
            report.ProblemTypes.Add(new ProblemType() { Name = "Urgent problem" });
            return report;
        }


        /// <summary>
        /// Reads problem reports
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="webAuthorPermissionsGroupName">Name of the web author permissions group.</param>
        /// <param name="webAuthorName">Name or username of the web author.</param>
        /// <returns></returns>
        public IList<ProblemReport> ReadProblemReports(DateTime? startDate, DateTime? endDate, Uri pageUrl, string webAuthorPermissionsGroupName, string webAuthorName)
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

            list[0].WebAuthors.Add(new WebAuthor() { Name = "John Smith", EmailAddress = "john.smith@example.org", UserName = "johnsmith", WebAuthorId = 1 });
            list[0].WebAuthors.Add(new WebAuthor() { Name = "Jane Smith", EmailAddress = "jane.smith@example.org", UserName = "janesmith", WebAuthorId = 2 });
            list[0].ProblemTypes.Add(new ProblemType() { Name = "Urgent problem" });

            list[1].WebAuthors.Add(new WebAuthor() { Name = "John Smith", EmailAddress = "john.smith@example.org", UserName = "johnsmith", WebAuthorId = 1 });
            list[1].WebAuthors.Add(new WebAuthor() { Name = "Jane Smith", EmailAddress = "jane.smith@example.org", UserName = "janesmith", WebAuthorId = 2 });
            list[1].ProblemTypes.Add(new ProblemType() { Name = "Broken link" });

            return list;
        }
    }
}
