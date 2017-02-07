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