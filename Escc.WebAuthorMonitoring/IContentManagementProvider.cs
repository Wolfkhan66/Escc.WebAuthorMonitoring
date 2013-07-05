
using System;
using System.Collections.Generic;

namespace Escc.WebAuthorMonitoring
{
    /// <summary>
    /// A web content management system 
    /// </summary>
    public interface IContentManagementProvider
    {
        /// <summary>
        /// Reads metadata for a content managed page
        /// </summary>
        /// <returns></returns>
        Page ReadMetadataForPage(Uri pageUrl);

        /// <summary>
        /// Reads the name of the permissions group for the supplied page.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <returns></returns>
        string ReadPermissionsGroupNameForPage(Uri pageUrl);

        /// <summary>
        /// Reads the web authors in a permissions group.
        /// </summary>
        /// <param name="groupName">Name of the permissions group.</param>
        /// <returns></returns>
        IEnumerable<WebAuthor> ReadWebAuthorsInGroup(string groupName);

        /// <summary>
        /// Parses a page URL into one recognised by the Content Management System.
        /// </summary>
        /// <param name="urlToParse">The URL to parse.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        Uri ParsePageUrl(string urlToParse);
    }
}
