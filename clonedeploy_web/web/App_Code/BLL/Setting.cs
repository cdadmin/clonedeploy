using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Global;

namespace BLL
{
    public class Setting
    {
        private static readonly DAL.Setting _da = new DAL.Setting();

        public bool ExportDatabase()
        {
            throw new Exception("Not Implemented");
        }
        public static string GetServerIpWithPort()
        {
            var ipAddress = Settings.ServerIp;
            var port = Settings.WebServerPort;
            if (port != "80" && port != "443" && !string.IsNullOrEmpty(port))
            {
                ipAddress += ":" + port;
            }

            return ipAddress;
        }

        public static string GetValueForAdminView(string settingValue)
        {
            if (settingValue.Contains(Settings.ServerIp))
                return settingValue.Replace(Settings.ServerIp, "[server-ip]");

            return settingValue;
        }

        public static string GetSetting(string settingName)
        {
            var setting = _da.Read(settingName);
            //Handle replacement of [server-ip] placeholder as well as ip addresses on different ports
            if (setting.Name != "Web Path" && setting.Name != "Nfs Upload Path" && setting.Name != "Nfs Deploy Path" &&
                setting.Name != "SMB Path") return setting.Value;
            if (setting.Value.Contains("[server-ip]"))
            {
                setting.Value = setting.Value.Replace("[server-ip]", GetSetting("Server IP"));
            }
            return setting.Value;
        }

        public bool UpdateSetting(List<Models.Setting> listSettings)
        {
            foreach(var setting in listSettings)
                _da.Update(setting);
            return true;
        }
    }
}