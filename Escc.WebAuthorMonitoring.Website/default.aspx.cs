using System;
using System.Globalization;
using System.Text.RegularExpressions;
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
            ValidateFilters();

            ConvertPostToGetRequest();

            RepopulateSearchForm();

            DisplayProblemReports();
        }

        private void RepopulateSearchForm()
        {
            if (IsPostBack && !IsValid) return;

            var appliedUrlFilter = _cms.ParsePageUrl(Request.QueryString["url"]);
            if (appliedUrlFilter != null)
            {
                this.url.Text = appliedUrlFilter.ToString();
            }

            this.webAuthor.Text = ParseWebAuthorFilter(Request.QueryString["webauthor"]);

            var appliedFromFilter = ParseDateFilter(Request.QueryString["from"]);
            if (appliedFromFilter.HasValue)
            {
                this.from.Text = DateTimeFormatter.ISODate(appliedFromFilter.Value);
            }

            var appliedToFilter = ParseDateFilter(Request.QueryString["to"]);
            if (appliedToFilter.HasValue)
            {
                this.to.Text = DateTimeFormatter.ISODate(appliedToFilter.Value);
            }
        }

        private void DisplayProblemReports()
        {
            var appliedUrlFilter = _cms.ParsePageUrl(Request.QueryString["url"]);
            var appliedWebAuthorFilter = ParseWebAuthorFilter(Request.QueryString["webauthor"]);
            var appliedFromFilter = ParseDateFilter(Request.QueryString["from"]);
            var appliedToFilter = ParseDateFilter(Request.QueryString["to"]);

            var reports = _repo.ReadProblemReports(appliedFromFilter, appliedToFilter, appliedUrlFilter, null, appliedWebAuthorFilter);

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

        private void ValidateFilters()
        {
            this.future1.MaximumValue = DateTimeFormatter.ISODate(DateTime.Today);

            if (IsPostBack) Validate();
        }


        private void ConvertPostToGetRequest()
        {
            if (!IsPostBack) return;
            if (!IsValid) return;

            var urlFilter = _cms.ParsePageUrl(this.url.Text);
            var appliedUrlFilter = _cms.ParsePageUrl(Request.QueryString["url"]);

            var webAuthorFilter = ParseWebAuthorFilter(this.webAuthor.Text);
            var appliedWebAuthorFilter = ParseWebAuthorFilter(Request.QueryString["webauthor"]);

            var fromFilter = ParseDateFilter(this.from.Text);
            var appliedFromFilter = ParseDateFilter(Request.QueryString["from"]);

            var toFilter = ParseDateFilter(this.to.Text);
            var appliedToFilter = ParseDateFilter(Request.QueryString["to"]);

            if (urlFilter != appliedUrlFilter || webAuthorFilter != appliedWebAuthorFilter || fromFilter != appliedFromFilter || toFilter != appliedToFilter)
            {
                Http.Status303SeeOther(
                    new Uri(
                        "default.aspx?url=" + urlFilter +
                        "&webauthor=" + webAuthorFilter +
                        "&from=" + (fromFilter.HasValue ? DateTimeFormatter.ISODate(fromFilter.Value) : String.Empty)
                        + "&to=" + (toFilter.HasValue ? DateTimeFormatter.ISODate(toFilter.Value) : String.Empty),
                        UriKind.Relative));
            }
        }

        private static string ParseWebAuthorFilter(string text)
        {
            // Restrict to characters valid in a name. Note \p{L} means any letter including diacritics.
            return String.IsNullOrEmpty(text) ? String.Empty : Regex.Replace(text, @"[^ \p{L}'()-]", String.Empty);
        }

        private static DateTime? ParseDateFilter(string dateToParse)
        {
            return String.IsNullOrEmpty(dateToParse) ? null : (DateTime?)DateTime.Parse(dateToParse, CultureInfo.InvariantCulture);
        }
    }
}