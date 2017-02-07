using Escc.EastSussexGovUK.Skins;
using Escc.EastSussexGovUK.Views;
using Escc.EastSussexGovUK.WebForms;
using Escc.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Escc.WebAuthorMonitoring.Website
{
    public partial class getReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var skinnable = Master as BaseMasterPage;
            if (skinnable != null)
            {
                skinnable.Skin = new CustomerFocusSkin(ViewSelector.CurrentViewIs(MasterPageFile));
            }
            GetPageReport();
        }

        public void GetPageReport()
        {
            if (IsPostBack)
            {
                var Utext = url.Text;
                var QueryString = "report/?page=" + Utext;
                new HttpStatus().SeeOther(QueryString);
            }
        }
    }
}