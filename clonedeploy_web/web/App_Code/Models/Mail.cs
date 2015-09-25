using System;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Helpers;

namespace Models
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
        public string Ip { get; set; }
        public string Time { get; set; }

        public Mail()
        {
            Time = DateTime.Now.ToString("MM-dd-yy h:mm:ss tt");
        }
        public void Send(string notificationType)
        {
            //Mail not enabled
            if (string.IsNullOrEmpty(Settings.SmtpServer)) return;

            switch (notificationType)
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
            }

            object objIp = Ip;
            if (objIp == null)
                Ip = (string)HttpContext.Current.Session["ip_address"];

            Task.Factory.StartNew(SendMailAsync);
        }

        private void SendMailAsync()
        {
            var message = new MailMessage(Settings.SmtpMailFrom, Settings.SmtpMailTo)
            {
                Subject = "CrucibleWDS " + "("+ Subject +")",
                Body = Body + Environment.NewLine + Ip + Environment.NewLine + Time
            };

            var client = new SmtpClient(Settings.SmtpServer, Convert.ToInt16(Settings.SmtpPort))
            {
                Credentials = new System.Net.NetworkCredential(Settings.SmtpUsername, Settings.SmtpPassword),
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