using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace WebApiCodeGenLib
{
    public static class MailHelper
    {
        private static int Timeout = 180000;
        private static string _host;
        private static int _port;
        private static string _user;
        private static string _pass;
        private static bool _ssl;

        public static string Sender { get; set; }
        public static string RecipientCC { get; set; }
        public static string AttachmentFile { get; set; }



        public static void Send(string Recipient, string Subject, string Body)
        {
            var _host = ConfigurationManager.AppSettings["MailServer"];
            //Port- Represents the port number
            _port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            //MailAuthUser and MailAuthPass - Used for Authentication for sending email
            _user = ConfigurationManager.AppSettings["MailAuthUser"];
            _pass = ConfigurationManager.AppSettings["MailAuthPass"];
            _ssl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
            try
            {
                // We do not catch the error here... let it pass direct to the caller
                Attachment att = null;
                var message = new MailMessage(_user, Recipient, Subject, Body) { IsBodyHtml = true };
                if (RecipientCC != null)
                {
                    message.Bcc.Add(RecipientCC);
                }
                var smtp = new SmtpClient(_host, _port);

                if (!String.IsNullOrEmpty(AttachmentFile))
                {
                    if (File.Exists(AttachmentFile))
                    {
                        att = new Attachment(AttachmentFile);
                        message.Attachments.Add(att);
                    }
                }

                if (_user.Length > 0 && _pass.Length > 0)
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_user, _pass);
                    smtp.EnableSsl = _ssl;

                }

                smtp.Send(message);

                if (att != null)
                    att.Dispose();
                message.Dispose();
                smtp.Dispose();
            }

            catch (Exception ex)
            {

            }
        }
    }
}
