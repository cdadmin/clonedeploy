using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class NetBootProfileServices
    {
        private readonly UnitOfWork _uow;

        public NetBootProfileServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddNetBootProfile(NetBootProfileEntity profile)
        {
            var validationResult = ValidateProfile(profile, true);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.NetBootProfileRepository.Insert(profile);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = profile.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        public ActionResultDTO DeleteProfile(int profileId)
        {
            var profile = GetProfile(profileId);
            if (profile == null) return new ActionResultDTO {ErrorMessage = "NetBoot Profile Not Found", Id = 0};
            _uow.NetBootProfileRepository.Delete(profileId);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = profile.Id;
            return actionResult;
        }

        public bool DeleteProfileNbiEntries(int profileId)
        {
            _uow.NbiEntryRepository.DeleteRange(x => x.ProfileId == profileId);
            _uow.Save();
            return true;
        }

        public NetBootProfileEntity GetProfile(int profileId)
        {
            return _uow.NetBootProfileRepository.GetById(profileId);
        }

        public NetBootProfileEntity GetProfileFromIp(string ip)
        {
            return _uow.NetBootProfileRepository.GetFirstOrDefault(x => x.Ip == ip);
        }

        public List<NbiEntryEntity> GetProfileNbiEntries(int profileId)
        {
            return _uow.NbiEntryRepository.Get(x => x.ProfileId == profileId);
        }

        public List<NetBootProfileEntity> SearchNetBootProfiles(string searchString = "")
        {
            return _uow.NetBootProfileRepository.Get(s => s.Name.Contains(searchString));
        }

        public string TotalCount()
        {
            return _uow.NetBootProfileRepository.Count();
        }

        public ActionResultDTO UpdateNetBootProfile(NetBootProfileEntity profile)
        {
            var s = GetProfile(profile.Id);
            if (s == null) return new ActionResultDTO {ErrorMessage = "NetBoot Profile Not Found", Id = 0};
            var validationResult = ValidateProfile(profile, false);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.NetBootProfileRepository.Update(profile, profile.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = profile.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateProfile(NetBootProfileEntity profile, bool isNewProfile)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(profile.Name))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "NetBoot Profile Name Is Not Valid";
                return validationResult;
            }

            if (isNewProfile)
            {
                if (_uow.NetBootProfileRepository.Exists(h => h.Name == profile.Name))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "A Profile With This Name Already Exists";
                    return validationResult;
                }

                if (_uow.NetBootProfileRepository.Exists(h => h.Ip == profile.Ip))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "A Profile With This Ip Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalProfile = _uow.NetBootProfileRepository.GetById(profile.Id);
                if (originalProfile.Name != profile.Name)
                {
                    if (_uow.NetBootProfileRepository.Exists(h => h.Name == profile.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "A Profile With This Name Already Exists";
                        return validationResult;
                    }
                }

                if (originalProfile.Ip != profile.Ip)
                {
                    if (_uow.NetBootProfileRepository.Exists(h => h.Ip == profile.Ip))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "A Profile With This Ip Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}