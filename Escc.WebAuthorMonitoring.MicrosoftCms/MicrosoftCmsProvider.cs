
using System;
using System.Collections.Generic;

namespace Escc.WebAuthorMonitoring.MicrosoftCms
{
    /// <summary>
    /// Provides access to Microsoft CMS 2002 for web author quality monitoring
    /// </summary>
    public class MicrosoftCmsProvider : IContentManagementProvider
    {
        public Page ReadMetadataForPage(Uri pageUrl)
        {
            throw new NotImplementedException();
        }

        public string ReadPermissionsGroupNameForPage(Uri pageUrl)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WebAuthor> ReadWebAuthorsInGroup(string groupName)
        {
            throw new NotImplementedException();
        }
    }
}
