using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Escc.WebAuthorMonitoring
{
    public class UmbracoContentManagementSystem : IContentManagementProvider
    {
        public Page ReadMetadataForPage(Uri pageUrl)
        {
            var umbracoService = new UmbracoService();
            var response = umbracoService.GetMessage("GetPage?url=" + pageUrl);
            var metaData = response.Content.ReadAsStringAsync().Result;
            Page page = JsonConvert.DeserializeObject<Page>(metaData);

            return new Page() { PageId = page.PageId, PageTitle = page.PageTitle, PageUrl = pageUrl };
        }

        public string ReadPermissionsGroupNameForPage(Uri pageUrl)
        {   
                var umbracoService = new UmbracoService();
                var response = umbracoService.GetMessage("GetPage?url=" + pageUrl);
                var metaData = response.Content.ReadAsStringAsync().Result;
                Page page = JsonConvert.DeserializeObject<Page>(metaData);

            // umbraco doesnt have groups like the old CMS so here the group is the page title
            return page.PageTitle + " Web Authors";
            }

        public IEnumerable<WebAuthor> ReadWebAuthorsInGroup(string pageUrl)
        {
            var umbracoService = new UmbracoService();
            var response = umbracoService.GetMessage("GetPageAuthors?url=" + pageUrl);
            var authors = response.Content.ReadAsStringAsync().Result;
            WebAuthor[] webauthors = JsonConvert.DeserializeObject<WebAuthor[]>(authors);

            return webauthors;
        }

        public Uri ParsePageUrl(string urlToParse)
        {
            if (String.IsNullOrEmpty(urlToParse)) return null;
            return new Uri(urlToParse, UriKind.RelativeOrAbsolute);
        }
    }
}
