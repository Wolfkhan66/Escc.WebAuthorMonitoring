using System;
using System.Net.Mail;
using System.Text;

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
            var method = new SendEmailDelegate(SendEmail);
            method.BeginInvoke(report, null, null);
        }

        /// <summary>
        /// Execute <see cref="SendEmail"/> asynchronously
        /// </summary>
        private delegate void SendEmailDelegate(ProblemReport report);

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
                    try
                    {
                        smtp.Send(email);
                    }
                    catch (SmtpException e)
                    {
                        throw e;
                    }
                }
            }
        }

        private static void BuildEmailBody(ProblemReport report, MailMessage email)
        {
            var html = new StringBuilder("<div style=\"font-family:Arial\">");
            html.Append(report.MessageHtml.Replace("<h2>", "<h2 style=\"font-size:1.1em\">"));
            html.Append("</div>");
            email.IsBodyHtml = true;
            email.Body = html.ToString();
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
