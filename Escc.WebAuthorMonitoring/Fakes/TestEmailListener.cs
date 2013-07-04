
using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Escc.WebAuthorMonitoring.Fakes
{
    /// <summary>
    /// Builds an email but does not send it to the real web authors
    /// </summary>
    public class TestEmailListener : EmailListener
    {
        protected override void CustomiseEmail(MailMessage email)
        {
            ChangeEmailRecipient(email);
        }

        private static void ChangeEmailRecipient(MailMessage email)
        {
            if (email == null) throw new ArgumentNullException("email");

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Escc.WebAuthorMonitoring.TestEmailListenerAddress"]))
            {
                var toHtml = new StringBuilder();
                toHtml.Append("<p>To:</p><ul>");
                foreach (MailAddress address in email.To)
                {
                    toHtml.Append("<li>").Append(HttpUtility.HtmlEncode(address.DisplayName)).Append(", ").Append(HttpUtility.HtmlEncode(address.Address)).Append("</li>");
                }
                toHtml.Append("</ul>");
                email.Body = toHtml.ToString() + email.Body;

                email.To.Clear();
                email.To.Add(ConfigurationManager.AppSettings["Escc.WebAuthorMonitoring.TestEmailListenerAddress"]);
            }
            else
            {
                throw new ConfigurationErrorsException("The appSettings entry in web.config for 'Escc.WebAuthorMonitoring.TestEmailListenerAddress' was not found.");
            }
        }
    }
}
