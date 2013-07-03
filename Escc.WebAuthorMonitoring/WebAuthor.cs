namespace Escc.WebAuthorMonitoring
{
    /// <summary>
    /// A web author responsible for editing web content
    /// </summary>
    public class WebAuthor
    {
        /// <summary>
        /// Gets or sets the web author's real name.
        /// </summary>
        /// <value>The web author's real name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the web author's username, which may change over time.
        /// </summary>
        /// <value>The username.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>The email address.</value>
        public string EmailAddress { get; set; }

        /// <summary>
        /// A system id for the web author which does not change
        /// </summary>
        public string UserId { get; set; }
    }
}