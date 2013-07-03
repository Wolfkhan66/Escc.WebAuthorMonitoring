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
        protected void Page_Load(object sender, EventArgs e)
        {
            var cms = new FakeContentManagementSystem();
            var repo = new FakeRepository();

            var pageUrl = ValidatePageUrlFromQueryString();
            var pageDetails = cms.ReadMetadataForPage(pageUrl);
            SetupRecipientList(cms, pageUrl);

            SetupLinkToPage(pageDetails);
            SetupProblemTypeList(repo);
        }

        private void SetupRecipientList(IContentManagementProvider cms, Uri pageUrl)
        {
            var groupName = cms.ReadPermissionsGroupNameForPage(pageUrl);
            if (!String.IsNullOrEmpty(groupName))
            {
                var webAuthors = cms.ReadWebAuthorsInGroup(groupName);
                foreach (WebAuthor webAuthor in webAuthors)
                {
                    this.toList.Controls.Add(new LiteralControl("<li>" + HttpUtility.HtmlEncode(webAuthor.Name + ", " + webAuthor.EmailAddress) + "</li>"));
                }
            }
        }

        private void SetupLinkToPage(Page pageDetails)
        {
            if (pageDetails != null)
            {
                this.regardingPage.HRef = pageDetails.PageUrl.ToString();
                this.regardingPage.InnerText = pageDetails.PageTitle;
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

        private void SetupProblemTypeList(IWebAuthorMonitoringRepository repo)
        {
            if (repo == null) throw new ArgumentNullException("repo");

            // Viewstate doesn't preserve the data-* attribute on postback, so viewstate is turned off and we do it ourselves
            this.problemTypes.Items.Clear();
            var index = 0;
            foreach (ProblemType problemType in repo.ReadProblemTypes())
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
    }
}