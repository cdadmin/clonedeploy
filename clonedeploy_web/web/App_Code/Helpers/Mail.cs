using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Helpers
{
    /// <summary>
    /// Summary description for Mail
    /// </summary>
    public class Mail
    {
        public static string SuccessfulLogin
        {
            get { return "Successful Login"; }
        }

        public static string FailedLogin
        {
            get { return "Failed Login"; }
        }

        public static string TaskStarted
        {
            get { return "3.19.3-WDS"; }
        }

        public static string TaskCompleted
        {
            get { return "3.19.3-WDS"; }
        }

        public static string ImageApproved
        {
            get { return "3.19.3-WDS"; }
        }

        public static string ResizeFailed
        {
            get { return "3.19.3-WDS"; }
        }

        public string Subject { get; set; }
        public string Body { get; set; }

     
        public void Send(string notificationType)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;

            /*switch (notificationType)
            {
                case "Successful Login":
                    if (Settings.NotifySuccessfulLogin != "1")
                        return;
                    break;
                case "Failed Login":
                    if (Settings.NotifyFailedLogin != "1")
                        return;
                    break;
                case "Task Started":
                    if (Settings.NotifyTaskStarted != "1")
                        return;
                    break;
                case "Task Completed":
                    if (Settings.NotifyTaskCompleted != "1")
                        return;
                    break;
                case "Image Approved":
                    if (Settings.NotifyImageApproved != "1")
                        return;
                    break;
                case "Resize Failed":
                    if (Settings.NotifyResizeFailed != "1")
                        return;
                    break;
            }*/

            Task.Factory.StartNew(SendMailAsync);
        }

        private void SendMailAsync()
        {
            var message = new MailMessage(Settings.SmtpMailFrom, Settings.SmtpMailTo)
            {
                Subject = "Clone Deploy " + "("+ Subject +")",
            };

            var client = new SmtpClient(Settings.SmtpServer, Convert.ToInt16(Settings.SmtpPort))
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
                Logger.Log(ex.Message);
            }
        }
    }
}