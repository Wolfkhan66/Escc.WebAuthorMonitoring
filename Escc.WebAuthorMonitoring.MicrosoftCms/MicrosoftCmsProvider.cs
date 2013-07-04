
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using EsccWebTeam.Cms;
using Microsoft.ContentManagement.Publishing;

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

        /// <summary>
        /// Parses a page URL into one recognised by MCMS 2002.
        /// </summary>
        /// <param name="urlToParse">The URL to parse.</param>
        /// <returns></returns>
        public Uri ParsePageUrl(string urlToParse)
        {
            if (String.IsNullOrEmpty(urlToParse)) return null;

            var channel = CmsUtilities.ParseChannelUrl(urlToParse, CmsHttpContext.Current);
            if (channel != null)
            {
                var filename = urlToParse.Contains(".htm") ? Path.GetFileName(urlToParse) : String.Empty;
                if (filename.IndexOf("?", StringComparison.Ordinal) > -1)
                {
                    filename = filename.Substring(0, filename.IndexOf("?", StringComparison.Ordinal));
                }

                var sanitisedUrl = CmsUtilities.CorrectPublishedUrl(channel.UrlModePublished) + filename;
                return new Uri(Regex.Replace(sanitisedUrl, "[^a-z0-9-/.]", String.Empty)), UriKind.Relative);

            }
            return null;
        }
    }
}
