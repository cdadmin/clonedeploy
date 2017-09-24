using System;
using System.Collections.Generic;
using CloneDeploy_ApiCalls;
using CloneDeploy_Common;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;

namespace CloneDeploy_Services
{
    public class SecondaryServerServices
    {
        private readonly UnitOfWork _uow;

        public SecondaryServerServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddSecondaryServer(SecondaryServerEntity secondaryServer)
        {
            var actionResult = new ActionResultDTO();

            //Verify connection to secondary server
            //Get token
            var customApiCall = new CustomApiCallDTO();
            customApiCall.BaseUrl = new Uri(secondaryServer.ApiURL);
            var token = new APICall(customApiCall).TokenApi.Get(secondaryServer.ServiceAccountName,
                secondaryServer.ServiceAccountPassword);
            var serverRoles = new ServerRoleDTO();
            if (token != null)
            {
                if (!string.IsNullOrEmpty(token.error_description))
                {
                    actionResult.ErrorMessage = token.error_description;
                    return actionResult;
                }
                customApiCall.Token = token.access_token;
                serverRoles = new APICall(customApiCall).ServiceAccountApi.GetServerRoles();
                if (serverRoles.OperationMode != "Cluster Secondary")
                {
                    actionResult.ErrorMessage =
                        "Could Not Add Secondary Server.  It's Operation Mode Must First Be Changed To Cluster Secondary.";
                    return actionResult;
                }
                if (!serverRoles.IsImageServer && !serverRoles.IsTftpServer && !serverRoles.IsMulticastServer)
                {
                    actionResult.ErrorMessage =
                        "Could Not Add Secondary Server.  You Must First Assign Roles To The Server";
                    return actionResult;
                }
                if (serverRoles.Identifier == SettingServices.GetSettingValue(SettingStrings.ServerIdentifier))
                {
                    actionResult.ErrorMessage =
                        "Could Not Add Secondary Server.  Server Identifiers Must Be Different";
                    return actionResult;
                }
            }
            else
            {
                actionResult.ErrorMessage = "Unknown Error While Attempting To Contact Secondary Server";
                return actionResult;
            }

            secondaryServer.Name = serverRoles.Identifier;

            secondaryServer.TftpRole = Convert.ToInt16(serverRoles.IsTftpServer);
            secondaryServer.MulticastRole = Convert.ToInt16(serverRoles.IsMulticastServer);
            secondaryServer.ServiceAccountPassword =
                new EncryptionServices().EncryptText(secondaryServer.ServiceAccountPassword);

            var validationResult = ValidateSecondaryServer(secondaryServer, true);
            if (validationResult.Success)
            {
                _uow.SecondaryServerRepository.Insert(secondaryServer);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = secondaryServer.Id;
            }

            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        public ActionResultDTO DeleteSecondaryServer(int secondaryServerId)
        {
            var secondaryServer = GetSecondaryServer(secondaryServerId);
            if (secondaryServer == null)
                return new ActionResultDTO {ErrorMessage = "Secondary Server Not Found", Id = 0};
            _uow.SecondaryServerRepository.Delete(secondaryServerId);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = secondaryServer.Id;
            return actionResult;
        }

        public List<SecondaryServerEntity> GetAll()
        {
            return _uow.SecondaryServerRepository.Get();
        }

        public List<SecondaryServerEntity> GetAllWithActiveRoles()
        {
            return _uow.SecondaryServerRepository.Get(x => (x.MulticastRole == 1 || x.TftpRole == 1) && x.IsActive == 1);
        }

        public List<SecondaryServerEntity> GetAllWithMulticastRole()
        {
            return _uow.SecondaryServerRepository.Get(x => x.MulticastRole == 1 && x.IsActive == 1);
        }

        public List<SecondaryServerEntity> GetAllWithTftpRole()
        {
            return _uow.SecondaryServerRepository.Get(x => x.TftpRole == 1 && x.IsActive == 1);
        }

        public SecondaryServerEntity GetSecondaryServer(int secondaryServerId)
        {
            if (secondaryServerId == -1) return new SecondaryServerEntity() {IsActive = 1, Id = -1};
            return _uow.SecondaryServerRepository.GetById(secondaryServerId);
        }

        public SecondaryServerEntity GetSecondaryServerByName(string serverName)
        {
            return _uow.SecondaryServerRepository.GetFirstOrDefault(x => x.Name == serverName);
        }

        public CustomApiCallDTO GetToken(string serverName)
        {
            var secondaryServer = GetSecondaryServerByName(serverName);
            var customApiCall = new CustomApiCallDTO();
            customApiCall.BaseUrl = new Uri(secondaryServer.ApiURL);
            customApiCall.Token = secondaryServer.LastToken;
            if (new APICall(customApiCall).ServiceAccountApi.Test())
                return customApiCall;
            var token = new APICall(customApiCall).TokenApi.Get(secondaryServer.ServiceAccountName,
                new EncryptionServices().DecryptText(secondaryServer.ServiceAccountPassword));

            if (token != null)
            {
                if (!string.IsNullOrEmpty(token.error_description))
                {
                    return null;
                }
                customApiCall.Token = token.access_token;
                secondaryServer.LastToken = token.access_token;
                _uow.SecondaryServerRepository.Update(secondaryServer, secondaryServer.Id);
                _uow.Save();
            }
            return customApiCall;
        }

        public List<SecondaryServerEntity> SearchSecondaryServers(string searchString = "")
        {
            return _uow.SecondaryServerRepository.Get(s => s.Name.Contains(searchString));
        }

        public string TotalCount()
        {
            return _uow.SecondaryServerRepository.Count();
        }

        public ActionResultDTO UpdateSecondaryServer(SecondaryServerEntity secondaryServer)
        {
            var actionResult = new ActionResultDTO();

            var s = GetSecondaryServer(secondaryServer.Id);
            if (s == null) return new ActionResultDTO {ErrorMessage = "Secondary Server Not Found", Id = 0};
            secondaryServer.Name = s.Name;
            secondaryServer.MulticastRole = s.MulticastRole;
            secondaryServer.TftpRole = s.TftpRole;

            var password = !string.IsNullOrEmpty(secondaryServer.ServiceAccountPassword)
                ? secondaryServer.ServiceAccountPassword
                : new EncryptionServices().DecryptText(s.ServiceAccountPassword);


            if (secondaryServer.IsActive == 1)
            {
                //Verify connection to secondary server
                //Get token
                var customApiCall = new CustomApiCallDTO();
                customApiCall.BaseUrl = new Uri(secondaryServer.ApiURL);
                var token = new APICall(customApiCall).TokenApi.Get(secondaryServer.ServiceAccountName, password);
                var serverRoles = new ServerRoleDTO();
                if (token != null)
                {
                    if (!string.IsNullOrEmpty(token.error_description))
                    {
                        actionResult.ErrorMessage = token.error_description;
                        return actionResult;
                    }
                    customApiCall.Token = token.access_token;
                    serverRoles = new APICall(customApiCall).ServiceAccountApi.GetServerRoles();
                    if (serverRoles.OperationMode != "Cluster Secondary")
                    {
                        actionResult.ErrorMessage =
                            "Could Not Add Secondary Server.  It's Operation Mode Must First Be Changed To Cluster Secondary.";
                        return actionResult;
                    }
                    if (!serverRoles.IsImageServer && !serverRoles.IsTftpServer && !serverRoles.IsMulticastServer)
                    {
                        actionResult.ErrorMessage =
                            "Could Not Add Secondary Server.  You Must First Assign Roles To The Server";
                        return actionResult;
                    }
                    if (serverRoles.Identifier == SettingServices.GetSettingValue(SettingStrings.ServerIdentifier))
                    {
                        actionResult.ErrorMessage =
                            "Could Not Add Secondary Server.  Server Identifiers Must Be Different";
                        return actionResult;
                    }
                }
                else
                {
                    actionResult.ErrorMessage = "Unknown Error While Attempting To Contact Secondary Server";
                    return actionResult;
                }


                secondaryServer.Name = serverRoles.Identifier;

                secondaryServer.TftpRole = Convert.ToInt16(serverRoles.IsTftpServer);
                secondaryServer.MulticastRole = Convert.ToInt16(serverRoles.IsMulticastServer);
            }
            


            secondaryServer.ServiceAccountPassword = !string.IsNullOrEmpty(secondaryServer.ServiceAccountPassword)
                    ? new EncryptionServices().EncryptText(secondaryServer.ServiceAccountPassword)
                    : s.ServiceAccountPassword;
            
            var validationResult = ValidateSecondaryServer(secondaryServer, false);

            if (validationResult.Success)
            {
                _uow.SecondaryServerRepository.Update(secondaryServer, secondaryServer.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = secondaryServer.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateSecondaryServer(SecondaryServerEntity secondaryServer,
            bool isNewSecondaryServer)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(secondaryServer.Name))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Secondary Server Name Is Not Valid";
                return validationResult;
            }

            if (isNewSecondaryServer)
            {
                if (_uow.SecondaryServerRepository.Exists(h => h.Name == secondaryServer.Name))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Secondary Server Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalSecondaryServer = _uow.SecondaryServerRepository.GetById(secondaryServer.Id);
                if (originalSecondaryServer.Name != secondaryServer.Name)
                {
                    if (_uow.SecondaryServerRepository.Exists(h => h.Name == secondaryServer.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Secondary Server Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}