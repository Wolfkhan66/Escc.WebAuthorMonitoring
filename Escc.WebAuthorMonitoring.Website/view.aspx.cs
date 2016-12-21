using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using Escc.Dates;
using Escc.EastSussexGovUK.Skins;
using Escc.EastSussexGovUK.Views;
using Escc.EastSussexGovUK.WebForms;
using Escc.Web;
using Escc.WebAuthorMonitoring.SqlServer;

namespace Escc.WebAuthorMonitoring.Website
{
    public partial class view : System.Web.UI.Page
    {
        private readonly IWebAuthorMonitoringRepository _repo = new SqlServerRepository();
        private ProblemReport _problem;

        protected void Page_Load(object sender, EventArgs e)
        {
            var skinnable = Master as BaseMasterPage;
            if (skinnable != null)
            {
                skinnable.Skin = new CustomerFocusSkin(ViewSelector.CurrentViewIs(MasterPageFile));
            }

            var reportId = GetReportIdFromQueryString();
            if (reportId == -1) return;

            _problem = _repo.ReadProblemReport(reportId);

            this.subject.InnerText = _problem.SubjectLine();
            this.reportDate.InnerText = _problem.ReportDate.ToBritishDateWithDay();
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
                new HttpStatus().BadRequest();
            }

            try
            {
                return Int32.Parse(Request.QueryString["report"], CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                new HttpStatus().BadRequest();
                return -1;
            }
            catch (OverflowException)
            {
                new HttpStatus().BadRequest();
                return -1;
            }
        }
    }
}