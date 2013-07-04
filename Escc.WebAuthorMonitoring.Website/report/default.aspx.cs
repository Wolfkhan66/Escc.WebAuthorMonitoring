using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Escc.WebAuthorMonitoring.Fakes;
using EsccWebTeam.EastSussexGovUK;

namespace Escc.WebAuthorMonitoring.Website.report
{
    public partial class DefaultPage : System.Web.UI.Page
    {
        private readonly IWebAuthorMonitoringRepository _repo = new FakeRepository();
        private readonly IContentManagementProvider _cms = new FakeContentManagementSystem();
        private readonly ProblemReport _problem = new ProblemReport();

        protected void Page_Load(object sender, EventArgs e)
        {
            var pageUrl = ValidatePageUrlFromQueryString();
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
                AddSubmittedDataToReport();

                this._repo.SaveReport(this._problem);

                this.reportForm.Visible = false;
                this.confirm.Visible = true;
            }
        }

        private void AddSubmittedDataToReport()
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
            this._problem.MessageHtml = this.message.Text;
        }
    }
}