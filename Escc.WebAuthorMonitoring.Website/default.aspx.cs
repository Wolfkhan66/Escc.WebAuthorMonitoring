using System;
using eastsussexgovuk.webservices.TextXhtml.HouseStyle;
using Escc.WebAuthorMonitoring.Fakes;
using EsccWebTeam.Data.Web;

namespace Escc.WebAuthorMonitoring.Website
{
    public partial class DefaultPage : System.Web.UI.Page
    {
        private readonly IWebAuthorMonitoringRepository _repo = new FakeRepository();
        private readonly IContentManagementProvider _cms = new FakeContentManagementSystem();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.future1.MaximumValue = DateTimeFormatter.ISODate(DateTime.Today);

            var reports = _repo.ReadProblemReports(null, null, null, null, null);

            if (reports.Count > 0)
            {
                this.table.DataSource = reports;
                this.table.DataBind();
            }
            else
            {
                this.noneFound.Visible = true;
            }
        }


        protected void Search_Click(object sender, EventArgs e)
        {
            ConvertPostToGetRequest();
        }


        private void ConvertPostToGetRequest()
        {
            if (!IsPostBack) return;
            if (!IsValid) return;

            var urlFilter = _cms.ParsePageUrl(this.url.Text);
            var appliedUrlFilter = _cms.ParsePageUrl(Request.QueryString["url"]);

            var fromFilter = ParseDateFilter(this.from.Text);
            var appliedFromFilter = ParseDateFilter(Request.QueryString["from"]);

            var toFilter = ParseDateFilter(this.to.Text);
            var appliedToFilter = ParseDateFilter(Request.QueryString["to"]);

            if (urlFilter != appliedUrlFilter || fromFilter != appliedFromFilter || toFilter != appliedToFilter)
            {
                Http.Status303SeeOther(
                    new Uri(
                        "default.aspx?url=" + urlFilter +
                        "&from=" + (fromFilter.HasValue ? DateTimeFormatter.ISODate(fromFilter.Value) : String.Empty)
                        + "&to=" + (toFilter.HasValue ? DateTimeFormatter.ISODate(toFilter.Value) : String.Empty),
                        UriKind.Relative));
            }
        }

        private DateTime? ParseDateFilter(string dateToParse)
        {
            return String.IsNullOrEmpty(dateToParse) ? null : (DateTime?)DateTime.Parse(dateToParse);
        }
    }
}