using System.Collections.Generic;
using System.Linq;
using CloneDeploy_ApiCalls;
using CloneDeploy_Common;
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

        public static bool ImageServerRole
        {
            get { return GetSettingValue(SettingStrings.ImageServerRole) == "1"; }
        }

        public static bool MulticastServerRole
        {
            get { return GetSettingValue(SettingStrings.MulticastServerRole) == "1"; }
        }

        public static bool ServerIsClusterPrimary
        {
            get { return GetSettingValue(SettingStrings.OperationMode) == "Cluster Primary"; }
        }

        public static bool ServerIsClusterSecondary
        {
            get { return GetSettingValue(SettingStrings.OperationMode) == "Cluster Secondary"; }
        }

        public static bool ServerIsNotClustered
        {
            get { return GetSettingValue(SettingStrings.OperationMode) == "Single"; }
        }

        public static bool TftpServerRole
        {
            get { return GetSettingValue(SettingStrings.TftpServerRole) == "1"; }
        }

        public ServerRoleDTO GetServerRoles()
        {
            var srDto = new ServerRoleDTO();
            srDto.Identifier = GetSettingValue(SettingStrings.ServerIdentifier);
            srDto.OperationMode = GetSettingValue(SettingStrings.OperationMode);
            srDto.IsTftpServer = TftpServerRole;
            srDto.IsMulticastServer = MulticastServerRole;
            return srDto;
        }

        public SettingEntity GetSetting(string settingName)
        {
            var setting = _uow.SettingRepository.GetFirstOrDefault(s => s.Name == settingName);
            setting.Value = StringManipulationServices.PlaceHolderReplace(setting.Value);
            return setting;
        }

        public static string GetSettingValue(string settingName)
        {
            var setting = new SettingServices().GetSetting(settingName);
            return setting != null ? setting.Value : string.Empty;
        }

        public bool UpdatePxeSettings(List<SettingEntity> settings)
        {
            if (!UpdateSetting(settings))
                return false;

            if (!ServerIsNotClustered)
            {
                var secondaryServers =
                    new SecondaryServerServices().SearchSecondaryServers()
                        .Where(x => x.TftpRole == 1);

                foreach (var server in secondaryServers)
                {
                    var result = new APICall(new SecondaryServerServices().GetToken(server.Name))
                        .ServiceAccountApi.UpdateSettings(settings);
                    if (!result)
                        return false;
                }

                return true;
            }

            return true;
        }

        public bool UpdateSetting(List<SettingEntity> listSettings)
        {
            foreach (var setting in listSettings)
            {
                if (setting.Name == "Munki SMB Password Encrypted" || setting.Name == "Smtp Password Encrypted")

                    setting.Value = new EncryptionServices().EncryptText(setting.Value);
                _uow.SettingRepository.Update(setting, setting.Id);
            }
            _uow.Save();

            return true;
        }
    }
}