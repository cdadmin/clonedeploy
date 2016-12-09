using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using log4net;

namespace CloneDeploy_Services.Helpers
{
    /// <summary>
    /// Summary description for Mail
    /// </summary>
    public class Mail
    {
        private readonly ILog log = LogManager.GetLogger("ApplicationLog");

        public string Subject { get; set; }
        public string Body { get; set; }
        public string MailTo { get; set; }

        public void Send()
        {
             Task.Factory.StartNew(SendMailAsync);
        }

        private void SendMailAsync()
        {
            var message = new MailMessage(Settings.SmtpMailFrom, MailTo)
            {
                Subject = "Clone Deploy " + "("+ Subject +")",
                Body = Body
            };

            var client = new SmtpClient(Settings.SmtpServer, Convert.ToInt32(Settings.SmtpPort))
            {
                Credentials = new NetworkCredential(Settings.SmtpUsername, new Helpers.Encryption().DecryptText(Settings.SmtpPassword)),
                EnableSsl = Settings.SmtpSsl == "Yes"
            };

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
        }
    }
}