using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CloneDeploy_Entities;
using CloneDeploy_Services.Helpers;
using log4net;
using Newtonsoft.Json;

namespace CloneDeploy_Services
{
    public class OnlineKernelServices
    {
        private readonly ILog log = LogManager.GetLogger("ApplicationLog");

        public List<OnlineKernel> GetAllOnlineKernels()
        {
            WebClient wc = new WebClient();
            byte[] data = wc.DownloadData("http://localhost/clonedeploy/api/public/kernels.json");
            var text = Encoding.UTF8.GetString(data);

            return JsonConvert.DeserializeObject<List<OnlineKernel>>(text);
        }

        public bool DownloadKernel(OnlineKernel onlineKernel)
        {
            var baseUrl = "https://sourceforge.net/projects/clonedeploy/files/kernels/";
            //todo run against all secondary servers
            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.DownloadFile(new Uri(baseUrl + onlineKernel.BaseVersion + "/" + onlineKernel.FileName),
                   Settings.TftpPath + "kernels" + Path.DirectorySeparatorChar + onlineKernel.FileName);
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
