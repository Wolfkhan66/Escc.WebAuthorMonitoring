using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using eastsussexgovuk.webservices.TextXhtml.HouseStyle;
using Escc.WebAuthorMonitoring.SqlServer;
using EsccWebTeam.EastSussexGovUK;

namespace Escc.WebAuthorMonitoring.Website
{
    public partial class view : System.Web.UI.Page
    {
        private readonly IWebAuthorMonitoringRepository _repo = new SqlServerRepository();
        private ProblemReport _problem;

        protected void Page_Load(object sender, EventArgs e)
        {
            var reportId = GetReportIdFromQueryString();
            if (reportId == -1) return;

            _problem = _repo.ReadProblemReport(reportId);

            this.subject.InnerText = _problem.SubjectLine();
            this.reportDate.InnerText = DateTimeFormatter.FullBritishDateWithDay(_problem.ReportDate);
            this.messageHtml.Text = _problem.MessageHtml;

            DisplayWebAuthors();

        }

        private void DisplayWebAuthors()
        {
            foreach (WebAuthor webAuthor in _problem.WebAuthors)
            {
                this.webAuthors.Controls.Add(new LiteralControl("<li>" + HttpUtility.HtmlEncode(webAuthor.Name + ", " + webAuthor.EmailAddress) + "</li>"));
            }
        }

        private int GetReportIdFromQueryString()
        {
            if (!IsPostBack && String.IsNullOrEmpty(Request.QueryString["report"]))
            {
                EastSussexGovUKContext.HttpStatus400BadRequest(this.container);
            }

            try
            {
                return Int32.Parse(Request.QueryString["report"], CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                EastSussexGovUKContext.HttpStatus400BadRequest(this.container);
                return -1;
            }
            catch (OverflowException)
            {
                EastSussexGovUKContext.HttpStatus400BadRequest(this.container);
                return -1;
            }
        }
    }
}