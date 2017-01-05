using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.WebAuthorMonitoring.WebApi.Model
{
    class Page
    {
        public int PageId { get; set; }
        public string PageTitle { get; set; }

        public Page(int pageId, string pageTitle)
        {
            PageId = pageId;
            PageTitle = pageTitle;
        }
    }
}
