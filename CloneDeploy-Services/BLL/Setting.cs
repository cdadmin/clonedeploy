using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public class Setting
    {



        public static SettingEntity GetSetting(string settingName)
        {
            using (var uow = new UnitOfWork())
            {
                var setting = uow.SettingRepository.GetFirstOrDefault(s => s.Name == settingName);              
                setting.Value = Helpers.ParameterReplace.Between(setting.Value);             
                return setting;
            }
        }

        public static bool UpdateSetting(List<SettingEntity> listSettings)
        {
            using (var uow = new UnitOfWork())
            {
                foreach (var setting in listSettings)
                    uow.SettingRepository.Update(setting, setting.Id);
                                   
                uow.Save();
            }
            return true;
        }
    }
}