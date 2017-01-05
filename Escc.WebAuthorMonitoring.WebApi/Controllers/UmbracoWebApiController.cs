using System;
using System.Net;
using System.Net.Http;
using Escc.BasicAuthentication.WebApi;
using System.Web.Http;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;
using Escc.WebAuthorMonitoring.WebApi.Model;
using System.Collections.Generic;

namespace Escc.WebAuthorMonitoring.WebApi.Controllers
{
    [Authorize]
    public class UmbracoWebApiController : UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage GetPage(string url)
        {
            // take the URL and create a URI
            Uri uri = new Uri(url);
            // Use the URI absolutePath to get the page node
            var node = UmbracoContext.ContentCache.GetByRoute(uri.AbsolutePath); 
            // Create a page object with the nodes ID and Name
            Page page = new Page(node.Id, node.Name);

            // Return the Page object as an HttpResponseMessage
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, page);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPageAuthors(string url)
        {
            // Create a list to store a pages users
            List<Users> userList = new List<Users>();

            // Create a uri from the url
            Uri uri = new Uri(url);
            // Use the uri absolute path to get the page node from the contentCache
            var node = UmbracoContext.ContentCache.GetByRoute(uri.AbsolutePath);
            // use the nodes id to get the node from the ContentService
            var content = Services.ContentService.GetById(node.Id);
            // Use the content to get a collection of permissions for the 'entity'
            var entityPermissions = Services.ContentService.GetPermissionsForEntity(content);

            // for each item in the list
            foreach (var entity in entityPermissions)
            {
                // get the user from the entity item
                var user = Services.UserService.GetUserById(entity.UserId);
                // add the users name, email, username and id to the list
                userList.Add(new Users( user.Name, user.Email,  user.Username, user.Id));
            }

            // return the user list as an HttpResponseMessage
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, userList);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
