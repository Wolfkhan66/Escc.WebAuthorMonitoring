
using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Reads the name of the permissions group for the supplied page.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <returns></returns>
        public string ReadPermissionsGroupNameForPage(Uri pageUrl)
        {
            return "Example group";
        }

        /// <summary>
        /// Reads the web authors in a permissions group.
        /// </summary>
        /// <param name="groupName">Name of the permissions group.</param>
        /// <returns></returns>
        public IEnumerable<WebAuthor> ReadWebAuthorsInGroup(string groupName)
        {
            return new List<WebAuthor>()
                {
                    new WebAuthor(){Name="John Smith", EmailAddress = "john.smith@example.org", UserName = "johnsmith", WebAuthorId = 1},
                    new WebAuthor(){Name="Jane Smith", EmailAddress = "jane.smith@example.org", UserName = "janesmith", WebAuthorId = 2}
                };
        }

        /// <summary>
        /// Parses a page URL into one recognised by the Content Management System.
        /// </summary>
        /// <param name="urlToParse">The URL to parse.</param>
        /// <returns></returns>
        public Uri ParsePageUrl(string urlToParse)
        {
            if (String.IsNullOrEmpty(urlToParse)) return null;

            return new Uri(urlToParse, UriKind.RelativeOrAbsolute);
        }
    }
}
