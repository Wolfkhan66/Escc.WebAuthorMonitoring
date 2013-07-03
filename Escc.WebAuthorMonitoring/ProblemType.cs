
namespace Escc.WebAuthorMonitoring
{
    /// <summary>
    /// A type of problem which can be reported to responsible web authors
    /// </summary>
    public class ProblemType
    {
        /// <summary>
        /// Gets or sets the problem type id.
        /// </summary>
        /// <value>The problem type id.</value>
        public int ProblemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the default text.
        /// </summary>
        /// <value>The default text.</value>
        public string DefaultText { get; set; }
    }
}
