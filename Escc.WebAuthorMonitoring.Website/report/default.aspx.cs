using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Escc.WebAuthorMonitoring.Fakes;
using Escc.WebAuthorMonitoring.MicrosoftCms;
using Escc.WebAuthorMonitoring.SqlServer;
using EsccWebTeam.EastSussexGovUK;

namespace Escc.WebAuthorMonitoring.Website.report
{
    public partial class DefaultPage : System.Web.UI.Page
    {
        private readonly IWebAuthorMonitoringRepository _repo = new SqlServerRepository();
        private readonly IContentManagementProvider _cms = new MicrosoftCmsProvider();
        private readonly ProblemReport _problem = new ProblemReport();
        private readonly IEnumerable<IReportListener> _listeners = new[] { new TestEmailListener() };

        protected void Page_Load(object sender, EventArgs e)
        {
            var pageUrl = ValidatePageUrlFromQueryString();
            if (pageUrl == null) return;

            this._problem.Page = _cms.ReadMetadataForPage(pageUrl);
            ReadWebAuthorsForPage(pageUrl);

            DisplayWebAuthors();
            DisplayLinkToPage();
            DisplayProblemTypes();
        }

        private void ReadWebAuthorsForPage(Uri pageUrl)
        {
            _problem.WebAuthorPermissionsGroupName = _cms.ReadPermissionsGroupNameForPage(pageUrl);
            if (!String.IsNullOrEmpty(_problem.WebAuthorPermissionsGroupName))
            {
                this._problem.WebAuthors.AddRange(_cms.ReadWebAuthorsInGroup(_problem.WebAuthorPermissionsGroupName));
            }
        }

        private void DisplayWebAuthors()
        {
            foreach (WebAuthor webAuthor in _problem.WebAuthors)
            {
                this.toList.Controls.Add(new LiteralControl("<li>" + HttpUtility.HtmlEncode(webAuthor.Name + ", " + webAuthor.EmailAddress) + "</li>"));
            }
        }

        private void DisplayLinkToPage()
        {
            if (this._problem.Page != null)
            {
                this.regardingPage.HRef = this._problem.Page.PageUrl.ToString();
                this.regardingPage.InnerText = this._problem.Page.PageTitle;
            }
        }

        private Uri ValidatePageUrlFromQueryString()
        {
            if (!IsPostBack && String.IsNullOrEmpty(Request.QueryString["page"]))
            {
                EastSussexGovUKContext.HttpStatus400BadRequest(this.container);
            }

            try
            {
                return new Uri(Request.QueryString["page"]);
            }
            catch (UriFormatException)
            {
                EastSussexGovUKContext.HttpStatus400BadRequest(this.container);
                return null;
            }
        }

        private void DisplayProblemTypes()
        {
            // Viewstate doesn't preserve the data-* attribute on postback, so viewstate is turned off and we do it ourselves
            this.problemTypes.Items.Clear();
            var index = 0;
            foreach (ProblemType problemType in _repo.ReadProblemTypes())
            {
                var item = new ListItem(problemType.Name, problemType.ProblemTypeId.ToString(CultureInfo.InvariantCulture));
                item.Attributes["data-default-text"] = HttpUtility.HtmlAttributeEncode(problemType.DefaultText);
                item.Selected = !String.IsNullOrEmpty(Request.Form[problemTypes.UniqueID + "$" + index.ToString(CultureInfo.InvariantCulture)]);
                this.problemTypes.Items.Add(item);
                index++;
            }
        }

        protected void RequireSubject_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (e == null) throw new ArgumentNullException("e");

            e.IsValid = (this.problemTypes.SelectedItem != null);
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                AddDataToReport();

                this._repo.SaveProblemReport(this._problem);

                this.PublishReport();

                this.reportForm.Visible = false;
                this.confirm.Visible = true;
            }
        }

        private void PublishReport()
        {
            foreach (var listener in _listeners) listener.ReportPublished(_problem);
        }

        private void AddDataToReport()
        {
            this._problem.ReportDate = DateTime.Now;
            foreach (ListItem item in this.problemTypes.Items)
            {
                if (item.Selected)
                {
                    try
                    {
                        this._problem.ProblemTypes.Add(new ProblemType() { ProblemTypeId = Int32.Parse(item.Value, CultureInfo.InvariantCulture), Name = item.Text });
                    }
                    catch (FormatException)
                    {
                        EastSussexGovUKContext.HttpStatus400BadRequest(this.container);
                    }
                    catch (OverflowException)
                    {
                        EastSussexGovUKContext.HttpStatus400BadRequest(this.container);
                    }
                }
            }

            var html = new StringBuilder();
            html.Append("<p>This message is about <a href=\"").Append(HttpUtility.HtmlAttributeEncode(_problem.Page.PageUrl.ToString())).Append("\">").Append(HttpUtility.HtmlEncode(_problem.Page.PageTitle)).Append("</a>.</p>");
            AddProblemTypes(html);
            html.Append(this.message.Text);
            AddRelatedReports(html);

            this._problem.MessageHtml = html.ToString();
        }


        private void AddProblemTypes(StringBuilder html)
        {
            if (_problem.ProblemTypes.Count > 1)
            {
                html.Append("<ul>");
                foreach (ProblemType problemType in _problem.ProblemTypes)
                {
                    html.Append("<li>").Append(HttpUtility.HtmlEncode(problemType.Name)).Append("</li>");
                }
                html.Append("</ul>");
            }
        }

        private void AddRelatedReports(StringBuilder html)
        {
            var relatedReports = _repo.ReadProblemReports(null, null, null, _problem.WebAuthorPermissionsGroupName, null);
            if (relatedReports.Count > 0)
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["Escc.WebAuthorMonitoring.ViewReportUrl"]))
                {
                    throw new ConfigurationErrorsException("The appSettings entry in web.config for 'Escc.WebAuthorMonitoring.ViewReportUrl' was not found.");
                }

                html.Append("<h2>Related messages</h2><ol>");
                foreach (ProblemReport related in relatedReports)
                {
                    var url = String.Format(CultureInfo.InvariantCulture, ConfigurationManager.AppSettings["Escc.WebAuthorMonitoring.ViewReportUrl"], HttpUtility.UrlEncode(related.ProblemReportId.ToString(CultureInfo.InvariantCulture)));
                    html.Append("<li><a href=\"").Append(url).Append("\">").Append(related.SubjectLine()).Append(" - Sent ").Append(related.ReportDate.ToString("d MMM yyyy", CultureInfo.CurrentCulture)).Append("</a></li>");
                }
                html.Append("</ol>");
            }
        }
    }
}