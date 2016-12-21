using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escc.WebAuthorMonitoring
{
    /// <summary>
    /// A minimal implementation of <see cref="IContentManagementProvider"/> for testing
    /// </summary>
    /// <seealso cref="Escc.WebAuthorMonitoring.IContentManagementProvider" />
    public class FakeContentManagementSystem : IContentManagementProvider
    {
        public Page ReadMetadataForPage(Uri pageUrl)
        {
            return null;
        }

        public string ReadPermissionsGroupNameForPage(Uri pageUrl)
        {
            return String.Empty;
        }

        public IEnumerable<WebAuthor> ReadWebAuthorsInGroup(string groupName)
        {
            return new WebAuthor[0];
        }

        public Uri ParsePageUrl(string urlToParse)
        {
            return null;
        }
    }
}
