using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;

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
                setting.Value = Utility.Between(setting.Value);             
                return setting;
            
        }

        public  bool UpdateSetting(List<SettingEntity> listSettings)
        {

            foreach (var setting in listSettings)
            {
                if (setting.Name == "Munki SMB Password Encrypted" || setting.Name == "Smtp Password Encrypted" ||
                    setting.Name == "Image ReadWrite Password Encrypted" ||
                    setting.Name == "Image ReadOnly Password Encrypted")
                    setting.Value = new Encryption().EncryptText(setting.Value);
                _uow.SettingRepository.Update(setting, setting.Id);
            }
            _uow.Save();
            
            return true;
        }

        public ServerRoleDTO GetServerRoles()
        {
            var srDto = new ServerRoleDTO();
            srDto.Identifier = Settings.ServerIdentifier;
            srDto.OperationMode = Settings.OperationMode;
            srDto.IsImageServer = Settings.ImageServerRole;
            srDto.IsTftpServer = Settings.TftpServerRole;
            srDto.IsMulticastServer = Settings.MulticastServerRole;
            return srDto;
            
        }

        public ImageShareDTO GetImageShare()
        {
            var imageShare = new ImageShareDTO();
            imageShare.Type = _uow.SettingRepository.GetFirstOrDefault(s => s.Name == "Image Share Type").Value;
            imageShare.Server = _uow.SettingRepository.GetFirstOrDefault(s => s.Name == "Image Server Ip").Value;
            imageShare.Name = _uow.SettingRepository.GetFirstOrDefault(s => s.Name == "Image Share Name").Value;
            imageShare.Domain = _uow.SettingRepository.GetFirstOrDefault(s => s.Name == "Image Domain").Value;
            imageShare.ReadWriteUser = _uow.SettingRepository.GetFirstOrDefault(s => s.Name == "Image ReadWrite Username").Value;
            imageShare.ReadWritePassword =
                new Encryption().DecryptText(
                    _uow.SettingRepository.GetFirstOrDefault(s => s.Name == "Image ReadWrite Password Encrypted").Value);
            imageShare.ReadOnlyUser = _uow.SettingRepository.GetFirstOrDefault(s => s.Name == "Image ReadOnly Username").Value;
            imageShare.ReadOnlyPassword =
                new Encryption().DecryptText(
                    _uow.SettingRepository.GetFirstOrDefault(s => s.Name == "Image ReadOnly Password Encrypted").Value);
            imageShare.PhysicalPath = _uow.SettingRepository.GetFirstOrDefault(s => s.Name == "Image Physical Path").Value;
            imageShare.QueueSize = _uow.SettingRepository.GetFirstOrDefault(s => s.Name == "Image Queue Size").Value;

            return imageShare;
        }
    }
}