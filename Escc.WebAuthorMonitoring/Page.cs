
using System;

namespace Escc.WebAuthorMonitoring
{
    /// <summary>
    /// A page of content about which a problem can be reported
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Gets or sets the content management system's internal id for the page.
        /// </summary>
        /// <value>The page id.</value>
        public string PageId { get; set; }

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>The page title.</value>
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>The page URL.</value>
        public Uri PageUrl { get; set; }
    }
}
