
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using EsccWebTeam.Cms;
using EsccWebTeam.Cms.Permissions;
using EsccWebTeam.Data.ActiveDirectory;
using Microsoft.ContentManagement.Publishing;

namespace Escc.WebAuthorMonitoring.MicrosoftCms
{
    /// <summary>
    /// Provides access to Microsoft CMS 2002 for web author quality monitoring
    /// </summary>
    public class MicrosoftCmsProvider : IContentManagementProvider
    {
        /// <summary>
        /// Reads metadata for a content managed page
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public Page ReadMetadataForPage(Uri pageUrl)
        {
            if (pageUrl == null) throw new ArgumentNullException("pageUrl");

            var page = new Page();
            page.PageUrl = pageUrl;
            page.PageTitle = pageUrl.ToString();

            var hierarchyItem = CmsHttpContext.Current.Searches.GetByUrl(pageUrl.AbsolutePath.ToString());
            if (hierarchyItem == null)
            {
                return page;
            }

            var posting = hierarchyItem as Posting;
            if (posting == null)
            {
                var channel = hierarchyItem as Channel;
                if (channel != null) posting = CmsUtilities.DefaultPostingInChannel(channel);
            }

            if (posting != null)
            {
                if (posting.Placeholders["phDefTitle"] != null)
                {
                    page.PageTitle = posting.Placeholders["phDefTitle"].Datasource.RawContent;
                }
                else
                {
                    page.PageTitle = posting.DisplayName;
                }
            }

            return page;
        }

        /// <summary>
        /// Reads the name of the permissions group for the supplied page.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <returns></returns>
        public string ReadPermissionsGroupNameForPage(Uri pageUrl)
        {
            if (pageUrl == null) throw new ArgumentNullException("pageUrl");

            var channel = CmsUtilities.ParseChannelUrl(pageUrl.ToString(), CmsHttpContext.Current);
            if (channel != null)
            {
                var groups = CmsPermissions.ReadCmsGroupsForChannel(channel);
                if (groups[CmsRole.Editor].Count > 0)
                {
                    // there should be only one
                    return groups[CmsRole.Editor][0];
                }
            }
            return null;
        }

        /// <summary>
        /// Reads the web authors in a permissions group.
        /// </summary>
        /// <param name="groupName">Name of the permissions group.</param>
        /// <returns></returns>
        public IEnumerable<WebAuthor> ReadWebAuthorsInGroup(string groupName)
        {
            if (String.IsNullOrEmpty(groupName)) throw new ArgumentNullException("groupName");

            var webAuthors = new List<WebAuthor>();
            var userNames = CmsPermissions.ReadActiveDirectoryUsersForCmsGroup(groupName);

            var searcher = new ADSearcher();
            searcher.PropertiesToLoad.Add("SamAccountName");
            searcher.PropertiesToLoad.Add("DisplayName");
            searcher.PropertiesToLoad.Add("Mail");

            foreach (var userName in userNames[CmsRole.Editor])
            {
                var searchFor = userName;
                if (userName.StartsWith("WinNT://ESCC/", StringComparison.InvariantCultureIgnoreCase))
                {
                    searchFor = userName.Substring(13);
                }

                var users = searcher.GetUserBySamAccountName(searchFor);
                foreach (ADUser user in users)
                {
                    webAuthors.Add(new WebAuthor()
                        {
                            UserName = user.SamAccountName,
                            Name = user.DisplayName,
                            EmailAddress = user.Mail
                        });
                }
            }
            return webAuthors;
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
                return new Uri(Regex.Replace(sanitisedUrl, "[^a-z0-9-/.]", String.Empty), UriKind.Relative);

            }
            return null;
        }
    }
}
