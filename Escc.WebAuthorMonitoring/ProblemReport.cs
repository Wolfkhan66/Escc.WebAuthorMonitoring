using System;
using System.Collections.Generic;
using System.Globalization;

namespace Escc.WebAuthorMonitoring
{
    /// <summary>
    /// A report of a problem on a page for which web authors are responsible
    /// </summary>
    public class ProblemReport
    {
        /// <summary>
        /// Gets or sets the problem report id.
        /// </summary>
        /// <value>The problem report id.</value>
        public int ProblemReportId { get; set; }

        /// <summary>
        /// Gets or sets the web authors.
        /// </summary>
        /// <value>The web authors.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<WebAuthor> WebAuthors { get; private set; }

        /// <summary>
        /// Gets or sets the name of the web author permissions group.
        /// </summary>
        /// <value>The name of the web author permissions group.</value>
        public string WebAuthorPermissionsGroupName { get; set; }

        /// <summary>
        /// Gets or sets the problem types.
        /// </summary>
        /// <value>The problem types.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ProblemType> ProblemTypes { get; private set; }

        /// <summary>
        /// Gets or sets the message HTML.
        /// </summary>
        /// <value>The message HTML.</value>
        public string MessageHtml { get; set; }

        /// <summary>
        /// Gets or sets the page the report refers to.
        /// </summary>
        /// <value>The page.</value>
        public Page Page { get; set; }

        /// <summary>
        /// Gets or sets the report date.
        /// </summary>
        /// <value>The report date.</value>
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// Gets or sets the related reports.
        /// </summary>
        /// <value>The related reports.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ProblemReport> RelatedReports { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProblemReport"/> class.
        /// </summary>
        public ProblemReport()
        {
            this.WebAuthors = new List<WebAuthor>();
            this.ProblemTypes = new List<ProblemType>();
            this.RelatedReports = new List<ProblemReport>();
        }

        /// <summary>
        /// Generates a subject line suitable for sending the report by email
        /// </summary>
        /// <returns></returns>
        public string SubjectLine()
        {
            var problemType = (ProblemTypes.Count == 1) ? ProblemTypes[0].Name : "Multiple issues";
            return "Website - " + problemType + " Ref: " + ProblemReportId.ToString(CultureInfo.InvariantCulture);
        }
    }
}
