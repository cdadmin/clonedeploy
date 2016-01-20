using System;
using System.Collections.Generic;
using Helpers;
using MySql.Data.MySqlClient;

namespace BLL
{
    public class Setting
    {
        
        public static bool ExportDatabase()
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

        public static Models.Setting GetSetting(string settingName)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var setting = uow.SettingRepository.GetFirstOrDefault(s => s.Name == settingName);
                //Handle replacement of [server-ip] placeholder as well as ip addresses on different ports
                if (setting.Name != "Web Path") return setting;
                if (setting.Value.Contains("[server-ip]"))
                {
                    setting.Value = setting.Value.Replace("[server-ip]", GetSetting("Server IP").Value);
                }
                return setting;
            }
        }

        public static bool UpdateSetting(List<Models.Setting> listSettings)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                foreach (var setting in listSettings)
                    uow.SettingRepository.Update(setting, setting.Id);
                                   
                uow.Save();
            }
            return true;
        }
    }
}