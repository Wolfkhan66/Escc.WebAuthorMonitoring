using System;
using System.Configuration;
using System.Net;
using System.Net.Http;

namespace Escc.WebAuthorMonitoring
{
    public class UmbracoService
    {
        private readonly HttpClient _client;
        public UmbracoService()
        {
            var siteUri = ConfigurationManager.AppSettings["SiteUri"];

            siteUri = string.Format("{0}Api/UmbracoWebApi/", siteUri);
            var handler = new HttpClientHandler
            {
                Credentials =
                    new NetworkCredential(ConfigurationManager.AppSettings["apiuser"],
                        ConfigurationManager.AppSettings["apikey"])
            };

            // Set a long timeout because some queries have to check all pages and can take a long time
            _client = new HttpClient(handler) { BaseAddress = new Uri(siteUri), Timeout = TimeSpan.FromMinutes(5) };
        }

        public HttpResponseMessage GetMessage(string uriPath)
        {
            var response =  _client.GetAsync(uriPath).Result;
            if (!response.IsSuccessStatusCode) return null;
            return response;
        }
    }
}
