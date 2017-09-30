using System.Collections.Generic;
using MimeKit;
using MailKit.Net.Smtp;
using System.Linq;

namespace AlertCenter.Emails
{
    public class EmailClient
    {
        public void EmailAllByBcc(IEnumerable<string> emailAddresses, string topic, string message)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(Config.EmailName, Config.EmailAddress));
            mail.Bcc.AddRange(emailAddresses.Select(contact => new MailboxAddress(contact)));
            mail.Subject = topic;
            mail.Body = new TextPart("plain") { Text = message };
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(Config.EmailService);
                Config.EmailAuthenticate(client);
                client.Send(mail);
                client.Disconnect(true);
            }
        }
    }
}