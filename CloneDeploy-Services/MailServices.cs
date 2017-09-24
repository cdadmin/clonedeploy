using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CloneDeploy_Common;
using log4net;

namespace CloneDeploy_Services.Helpers
{
    /// <summary>
    ///     Summary description for Mail
    /// </summary>
    public class MailServices
    {
        private readonly ILog log = LogManager.GetLogger(typeof(MailServices));
        public string Body { get; set; }
        public string MailTo { get; set; }
        public string Subject { get; set; }

        public void Send()
        {
            Task.Factory.StartNew(SendMailAsync);
        }

        private void SendMailAsync()
        {
            try
            {
                var message = new MailMessage(SettingServices.GetSettingValue(SettingStrings.SmtpMailFrom), MailTo)
                {
                    Subject = "CloneDeploy " + "(" + Subject + ")",
                    Body = Body
                };

                var client = new SmtpClient(SettingServices.GetSettingValue(SettingStrings.SmtpServer),
                    Convert.ToInt32(SettingServices.GetSettingValue(SettingStrings.SmtpPort)))
                {
                    Credentials =
                        new NetworkCredential(SettingServices.GetSettingValue(SettingStrings.SmtpUsername),
                            new EncryptionServices().DecryptText(
                                SettingServices.GetSettingValue(SettingStrings.SmtpPassword))),
                    EnableSsl = SettingServices.GetSettingValue(SettingStrings.SmtpSsl) == "Yes"
                };


                client.Send(message);
            }
            catch (Exception ex)
            {
                
                log.Error(ex.Message);
            }
        }
    }
}