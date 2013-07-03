
using System;

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
    }
}
