using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;

namespace CloneDeploy_Services
{
    public class SettingServices
    {
         private readonly UnitOfWork _uow;

        public SettingServices()
        {
            _uow = new UnitOfWork();
        }


        public  SettingEntity GetSetting(string settingName)
        {
          
                var setting = _uow.SettingRepository.GetFirstOrDefault(s => s.Name == settingName);              
                setting.Value = CloneDeploy_App.Helpers.ParameterReplace.Between(setting.Value);             
                return setting;
            
        }

        public  bool UpdateSetting(List<SettingEntity> listSettings)
        {
          
                foreach (var setting in listSettings)
                    _uow.SettingRepository.Update(setting, setting.Id);
                                   
                _uow.Save();
            
            return true;
        }
    }
}