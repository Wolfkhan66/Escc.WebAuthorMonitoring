using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Services;
using Escc.WebAuthorMonitoring.Fakes;
using Escc.WebAuthorMonitoring.SqlServer;

namespace Escc.WebAuthorMonitoring.WebService
{
    /// <summary>
    /// Web service layer for reporting problems to web authors
    /// </summary>
    [WebService(Namespace = "http://www.eastsussex.gov.uk/Escc.WebAuthorMonitoring.WebService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebAuthorMonitoring : System.Web.Services.WebService
    {
        /// <summary>
        /// Save a problem report to the configured repository, and publish it to registered listeners
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="problemTypes">The problem types.</param>
        /// <param name="message">The message.</param>
        /// <returns>
        /// 	<c>true</c> if report published, or <c>false</c> if there were no web authors to notify
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the arguments are missing</exception>
        /// <exception cref="UriFormatException">Thrown if <c>pageUrl</c> is not a valid URL</exception>
        [WebMethod(Description = "Save a problem report to the configured repository, and publish it to registered listeners")]
        public bool SaveAndPublish(string pageUrl, int[] problemTypes, string message)
        {
            if (String.IsNullOrEmpty(pageUrl)) throw new ArgumentNullException("pageUrl");
            if (problemTypes == null || problemTypes.Length == 0) throw new ArgumentNullException("problemTypes");
            if (String.IsNullOrEmpty(message)) throw new ArgumentNullException("message");

            var problem = new ProblemReport();

            IContentManagementProvider cms = new UmbracoContentManagementSystem();
            problem.Page = cms.ReadMetadataForPage(new Uri(pageUrl));
            problem.ReportDate = DateTime.Now;
            problem.MessageHtml = message;

            AddWebAuthorsToProblem(problem, cms);

            if (problem.WebAuthors.Count > 0)
            {
                IWebAuthorMonitoringRepository repo = new SqlServerRepository();
                var problemTypesFromRepo = repo.ReadProblemTypes();
                AddProblemTypesToProblem(problem, problemTypes, problemTypesFromRepo);

                // Check problem type was recognised
                if (problem.ProblemTypes.Count == 0)
                {
                    throw new ArgumentException("problemTypes");
                }

                CreateHtmlMessage(problem);

                repo.SaveProblemReport(problem);

                IEnumerable<IReportListener> listeners = new[] { String.IsNullOrEmpty(ConfigurationManager.AppSettings["Escc.WebAuthorMonitoring.TestEmailListenerAddress"]) ? new EmailListener() : new TestEmailListener() };
                foreach (var listener in listeners) listener.ReportPublished(problem);

                return true;
            }
            else
            {
                return false;
            }
        }

        private void CreateHtmlMessage(ProblemReport problem)
        {
            var html = new StringBuilder();
            html.Append("<p>This message is about <a href=\"").Append(HttpUtility.HtmlAttributeEncode(problem.Page.PageUrl.ToString())).Append("\">").Append(HttpUtility.HtmlEncode(problem.Page.PageTitle)).Append("</a>.</p>");
            if (problem.ProblemTypes.Count > 1)
            {
                html.Append("<ul>");
                foreach (ProblemType problemType in problem.ProblemTypes)
                {
                    html.Append("<li>").Append(HttpUtility.HtmlEncode(problemType.Name)).Append("</li>");
                }
                html.Append("</ul>");
            }
            else
            {
                html.Append(problem.ProblemTypes[0].DefaultText);
            }
            html.Append(problem.MessageHtml);

            problem.MessageHtml = html.ToString();
        }

        private static void AddProblemTypesToProblem(ProblemReport problem, IEnumerable<int> problemTypes, IList<ProblemType> problemTypesFromRepo)
        {
            foreach (int problemTypeId in problemTypes)
            {
                foreach (ProblemType problemType in problemTypesFromRepo)
                {
                    if (problemTypeId == problemType.ProblemTypeId)
                    {
                        problem.ProblemTypes.Add(problemType);
                        break;
                    }
                }
            }
        }

        private void AddWebAuthorsToProblem(ProblemReport problem, IContentManagementProvider cms)
        {
            problem.WebAuthorPermissionsGroupName = cms.ReadPermissionsGroupNameForPage(problem.Page.PageUrl);
            if (!String.IsNullOrEmpty(problem.WebAuthorPermissionsGroupName))
            {
                problem.WebAuthors.AddRange(cms.ReadWebAuthorsInGroup(problem.WebAuthorPermissionsGroupName));
            }
        }
    }
}
