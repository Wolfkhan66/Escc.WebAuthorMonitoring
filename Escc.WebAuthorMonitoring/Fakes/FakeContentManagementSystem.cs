
using System;

namespace Escc.WebAuthorMonitoring.Fakes
{
    /// <summary>
    /// A fake content management system for testing
    /// </summary>
    public class FakeContentManagementSystem : IContentManagementProvider
    {
        /// <summary>
        /// Reads metadata for a content managed page
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public Page ReadMetadataForPage(Uri pageUrl)
        {
            return new Page() { PageId = "1", PageTitle = "Example page", PageUrl = pageUrl };
        }
    }
}
