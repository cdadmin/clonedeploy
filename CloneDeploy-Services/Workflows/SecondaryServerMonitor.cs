using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Services.Helpers;

namespace CloneDeploy_Services.Workflows
{
    public class SecondaryServerMonitor
    {
        private readonly SecondaryServerServices _secondaryServerServices;

        public SecondaryServerMonitor()
        {
            _secondaryServerServices = new SecondaryServerServices();
        }

        public void Execute()
        {
            if (SettingServices.GetSettingValue(SettingStrings.MonitorSecondaryServer) == "0")
            {
                return;
            }

            var secServers = _secondaryServerServices.GetAll();
            foreach (var server in secServers)
            {
                bool serverIsUp = false;
                var counter = 1;
                while (counter <= 3)
                {
                    var result = _secondaryServerServices.GetToken(server.Name);
                    if (result != null)
                    {
                        if (!string.IsNullOrEmpty(result.Token))
                        {
                            //connection test passed
                            serverIsUp = true;
                            break;
                        }
                    }
                    counter++;
                }
                if (serverIsUp && server.IsActive != 1)
                {
                    //mark server active
                    server.IsActive = 1;
                    UpdateServer(server);
                }
                else if (!serverIsUp && server.IsActive != 0)
                {
                    //mark sever inactive
                    server.IsActive = 0;
                    UpdateServer(server);
                }
                
         
            }
        }

        private void UpdateServer(SecondaryServerEntity server)
        {
            //without this the password will try and be updated without being decrypted first causing an incorrect password.
            //this prevents the password from being updated.
            server.ServiceAccountPassword = new EncryptionServices().DecryptText(server.ServiceAccountPassword);
            _secondaryServerServices.UpdateSecondaryServer(server);

            SendNotificationEmail(server);
        }

        private void SendNotificationEmail(SecondaryServerEntity server)
        {
            //Mail not enabled
            if (SettingServices.GetSettingValue(SettingStrings.SmtpEnabled) == "0") return;
            var message = server.IsActive == 1 ? " Status Changed To Active" : " Status Changed To Inactive";

            foreach (
                var user in
                    new UserServices().SearchUsers("")
                        .Where(x => x.NotifyServerStatusChange == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                var mail = new MailServices
                {
                    MailTo = user.Email,
                    Body = server.Name + message,
                    Subject = server.Name
                };
                mail.Send();
            }
        }
    }
}
