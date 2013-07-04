using System;
using System.Configuration;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Escc.WebAuthorMonitoring
{
    /// <summary>
    /// Sends an email to web authors when a report is published
    /// </summary>
    public class EmailListener : IReportListener
    {
        /// <summary>
        /// Receive a new report and send it by email
        /// </summary>
        /// <param name="report">The report.</param>
        public void ReportPublished(ProblemReport report)
        {
            SendEmail(report);
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="report">The report.</param>
        private void SendEmail(ProblemReport report)
        {
            if (report == null) throw new ArgumentNullException("report");

            using (var email = new MailMessage())
            {
                foreach (WebAuthor author in report.WebAuthors)
                {
                    email.To.Add(new MailAddress(author.EmailAddress, author.Name));
                }

                email.Subject = report.SubjectLine();

                BuildEmailBody(report, email);

                CustomiseEmail(email);

                using (var smtp = new SmtpClient())
                {
                    smtp.Send(email);
                }
            }
        }

        private static void BuildEmailBody(ProblemReport report, MailMessage email)
        {
            var html = new StringBuilder("<div style=\"font-family:Arial\">");

            AddProblemTypes(report, html);

            html.Append(report.MessageHtml);

            AddRelatedReports(report, html);

            html.Append("</div>");
            email.IsBodyHtml = true;
            email.Body = html.ToString();
        }

        private static void AddProblemTypes(ProblemReport report, StringBuilder html)
        {
            if (report.ProblemTypes.Count > 1)
            {
                html.Append("<ul>");
                foreach (ProblemType problemType in report.ProblemTypes)
                {
                    html.Append("<li>").Append(HttpUtility.HtmlEncode(problemType.Name)).Append("</li>");
                }
                html.Append("</ul>");
            }
        }

        private static void AddRelatedReports(ProblemReport report, StringBuilder html)
        {
            if (report.RelatedReports.Count > 0)
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["Escc.WebAuthorMonitoring.ViewReportUrl"]))
                {
                    throw new ConfigurationErrorsException("The appSettings entry in web.config for 'Escc.WebAuthorMonitoring.ViewReportUrl' was not found.");
                }

                html.Append("<h2>Related messages</h2><ol>");
                foreach (ProblemReport related in report.RelatedReports)
                {
                    var url = String.Format(CultureInfo.InvariantCulture, ConfigurationManager.AppSettings["Escc.WebAuthorMonitoring.ViewReportUrl"], HttpUtility.UrlEncode(related.ProblemReportId.ToString(CultureInfo.InvariantCulture)));
                    html.Append("<li><a href=\"").Append(url).Append("\">").Append(related.SubjectLine()).Append(" - Sent ").Append(related.ReportDate.ToString("d MMM yyyy", CultureInfo.CurrentCulture)).Append("</a></li>");
                }
                html.Append("</ol>");
            }
        }

        /// <summary>
        /// Provides a chance for derived types to customises the email before it is sent.
        /// </summary>
        /// <param name="email">The email.</param>
        protected virtual void CustomiseEmail(MailMessage email)
        {
        }
    }
}
