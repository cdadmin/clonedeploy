using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace CloneDeploy_Common
{
    public class ApiUrl
    {
        public string DisplayName { get; set; }
        public string BaseUrl { get; set; }
    }

    public static class ApplicationServers
    {
        public static string _baseApiUrl { get; set; }
        public static List<ApiUrl> ServerList { get; set; }

        public static void Configure()
        {
            ServerList = GetServerList();
        }

        private static List<ApiUrl> GetServerList()
        {
            if(ServerList != null)
                ServerList.Clear();
            var list = new List<ApiUrl>();


            var filePath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "serverlist.csv";
            var Lines = File.ReadLines(filePath).Select(a => a.Split(','));
            foreach (var line in Lines)
            {
                var apiUrl = new ApiUrl();
                apiUrl.DisplayName = line[0];
                apiUrl.BaseUrl = line[1];
                list.Add(apiUrl);
            }
           return list;
            
        }
    }
}