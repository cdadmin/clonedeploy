using System.Collections.Generic;

namespace BLL
{
    public class Setting
    {
        
       
        //moved
        public static CloneDeploy_Web.Models.Setting GetSetting(string settingName)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var setting = uow.SettingRepository.GetFirstOrDefault(s => s.Name == settingName);              
                setting.Value = Helpers.ParameterReplace.Between(setting.Value);             
                return setting;
            }
        }

        //moved
        public static bool UpdateSetting(List<CloneDeploy_Web.Models.Setting> listSettings)
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