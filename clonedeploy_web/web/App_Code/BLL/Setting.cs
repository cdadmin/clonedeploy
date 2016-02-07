using System;
using System.Collections.Generic;
using Helpers;
using MySql.Data.MySqlClient;

namespace BLL
{
    public class Setting
    {
        
       

        public static Models.Setting GetSetting(string settingName)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var setting = uow.SettingRepository.GetFirstOrDefault(s => s.Name == settingName);              
                setting.Value = Helpers.ParameterReplace.Between(setting.Value);             
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