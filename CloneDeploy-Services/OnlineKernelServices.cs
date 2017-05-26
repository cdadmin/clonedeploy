using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using CloneDeploy_ApiCalls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using log4net;
using Newtonsoft.Json;

namespace CloneDeploy_Services
{
    public class OnlineKernelServices
    {
        private readonly ILog log = LogManager.GetLogger("ApplicationLog");

        public bool DownloadKernel(OnlineKernel onlineKernel)
        {
            if (SettingServices.ServerIsClusterPrimary)
            {
                foreach (var tftpServer in new SecondaryServerServices().GetAllWithTftpRole())
                {
                    var result = new APICall(new SecondaryServerServices().GetToken(tftpServer.Name))
                        .ServiceAccountApi.DownloadOnlineKernel(onlineKernel);
                    if (!result)
                        return false;
                }
            }

            return WebDownload(onlineKernel);  
        }

        public List<OnlineKernel> GetAllOnlineKernels()
        {
            var wc = new WebClient();
            try
            {
                var data = wc.DownloadData("https://sourceforge.net/projects/clonedeploy/files/kernels.json");
                var text = Encoding.UTF8.GetString(data);
                return JsonConvert.DeserializeObject<List<OnlineKernel>>(text);
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                return null;
            }  
        }

        private bool WebDownload(OnlineKernel onlineKernel)
        {
            var baseUrl = "https://sourceforge.net/projects/clonedeploy/files/kernels/";
            using (var wc = new WebClient())
            {
                try
                {
                    wc.DownloadFile(new Uri(baseUrl + onlineKernel.BaseVersion + "/" + onlineKernel.FileName),
                        SettingServices.GetSettingValue(SettingStrings.TftpPath) + "kernels" + Path.DirectorySeparatorChar + onlineKernel.FileName);
                    return true;
                }
                catch (Exception ex)
                {
                    log.Debug(ex.Message);
                    return false;
                }
            }
        }
    }
}