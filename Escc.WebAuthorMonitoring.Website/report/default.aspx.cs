using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using Escc.WebAuthorMonitoring.Fakes;

namespace Escc.WebAuthorMonitoring.Website.report
{
    public partial class DefaultPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var repo = new FakeRepository();
            SetupProblemTypeList(repo);
        }

        private void SetupProblemTypeList(IWebAuthorMonitoringRepository repo)
        {
            if (repo == null) throw new ArgumentNullException("repo");

            foreach (ProblemType problemType in repo.ReadProblemTypes())
            {
                var item = new ListItem(problemType.Name, problemType.ProblemTypeId.ToString(CultureInfo.InvariantCulture));
                item.Attributes["data-default-text"] = HttpUtility.HtmlAttributeEncode(problemType.DefaultText);
                this.problemTypes.Items.Add(item);
            }
        }
    }
}