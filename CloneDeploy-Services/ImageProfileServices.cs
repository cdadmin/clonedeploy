using System;
using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;

namespace CloneDeploy_Services
{
    public class ImageProfileServices
    {
        private readonly ImageServices _imageServices;
        private readonly UnitOfWork _uow;

        public ImageProfileServices()
        {
            _uow = new UnitOfWork();
            _imageServices = new ImageServices();
        }

        public ActionResultDTO AddProfile(ImageProfileEntity profile)
        {
            var validationResult = ValidateImageProfile(profile, true);
            var actionResult = new ActionResultDTO();
          
            if (validationResult.Success)
            {
                _uow.ImageProfileRepository.Insert(profile);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = profile.Id;
            }

            return actionResult;
        }

        public void CloneProfile(int imageProfileId)
        {
            var imageProfile = _uow.ImageProfileRepository.GetById(imageProfileId);
            var originalName = imageProfile.Name;
            using (var uow = new UnitOfWork())
            {
                for (var c = 1; c <= 100; c++)
                {
                    var newProfileName = imageProfile.Name + "_" + c;
                    if (uow.ImageProfileRepository.Exists(h => h.Name == newProfileName))
                        continue;

                    var clonedProfile = imageProfile;
                    clonedProfile.Name = newProfileName;
                    clonedProfile.Description = "Cloned From " + originalName;
                    AddProfile(clonedProfile);
                    break;
                }
            }
        }

        public ActionResultDTO DeleteImageProfileFileFolders(int profileId)
        {
            _uow.ImageProfileFileFolderRepository.DeleteRange(x => x.ProfileId == profileId);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = profileId;
            return actionResult;
        }

        public ActionResultDTO DeleteImageProfileScripts(int profileId)
        {
            _uow.ImageProfileScriptRepository.DeleteRange(x => x.ProfileId == profileId);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = profileId;
            return actionResult;
        }

        public bool DeleteImageProfileSysprepTags(int profileId)
        {
            _uow.ImageProfileSysprepRepository.DeleteRange(x => x.ProfileId == profileId);
            _uow.Save();
            return true;
        }

        public ActionResultDTO DeleteProfile(int profileId)
        {
            var profile = ReadProfile(profileId);
            if (profile == null) return new ActionResultDTO {ErrorMessage = "Image Profile Was Not Found", Id = 0};
            _uow.ImageProfileRepository.Delete(profileId);

            _uow.Save();
            var computers = _uow.ComputerRepository.Get(x => x.ImageProfileId == profileId);
            var computerService = new ComputerServices();
            foreach (var computer in computers)
            {
                computer.ImageProfileId = -1;
                computerService.UpdateComputer(computer);
            }

            var groups = _uow.GroupRepository.Get(x => x.ImageProfileId == profileId);
            var groupService = new GroupServices();
            foreach (var group in groups)
            {
                group.ImageProfileId = -1;
                groupService.UpdateGroup(group);
            }

            var groupProperties = _uow.GroupPropertyRepository.Get(x => x.ImageProfileId == profileId);
            var groupPropertyService = new GroupPropertyServices();
            foreach (var groupProperty in groupProperties)
            {
                groupProperty.ImageProfileId = -1;
                groupPropertyService.UpdateGroupProperty(groupProperty);
            }

            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = profile.Id;

            return actionResult;
        }

        public List<ImageProfileWithImage> GetAllProfiles()
        {
            return _uow.ImageProfileRepository.GetImageProfilesWithImages();
        }

        public string MinimumClientSizeForGridView(int profileId, int hdNumber)
        {
            try
            {
                var profile = ReadProfile(profileId);
                var fltClientSize = new ClientPartitionHelper(profile).HardDrive(hdNumber, 1)/1024f/1024f/1024f;
                return Math.Abs(fltClientSize) < 0.1f ? "< 100M" : fltClientSize.ToString("#.##") + " GB";
            }
            catch
            {
                return "N/A";
            }
        }

        public ImageProfileWithImage ReadProfile(int profileId)
        {
            return _uow.ImageProfileRepository.GetImageProfileWithImage(profileId);
        }



        public List<ImageProfileFileFolderEntity> SearchImageProfileFileFolders(int profileId)
        {
            return _uow.ImageProfileFileFolderRepository.Get(x => x.ProfileId == profileId,
                q => q.OrderBy(t => t.Priority));
        }

        public List<ImageProfileScriptEntity> SearchImageProfileScripts(int profileId)
        {
            return _uow.ImageProfileScriptRepository.Get(x => x.ProfileId == profileId, q => q.OrderBy(t => t.Priority));
        }

        public List<ImageProfileSysprepTagEntity> SearchImageProfileSysprepTags(int profileId)
        {
            return _uow.ImageProfileSysprepRepository.Get(x => x.ProfileId == profileId, q => q.OrderBy(t => t.Priority));
        }

        public ActionResultDTO UpdateProfile(ImageProfileEntity profile)
        {
            var existingProfile = ReadProfile(profile.Id);
            if (existingProfile == null)
                return new ActionResultDTO {ErrorMessage = "Image Profile Not Found", Id = 0};

            var actionResult = new ActionResultDTO();
            var validationResult = ValidateImageProfile(profile, false);
         
            if (validationResult.Success)
            {
                if (!string.IsNullOrEmpty(profile.ModelMatch))
                    profile.ModelMatch = profile.ModelMatch.ToLower();
                _uow.ImageProfileRepository.Update(profile, profile.Id);
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

        public ImageProfileEntity GetModelMatch(string model,string environment)
        {
            if (string.IsNullOrEmpty(model)) return null;
            var profiles =
                _uow.ImageProfileRepository.Get(
                    x => !string.IsNullOrEmpty(x.ModelMatch) && !x.ModelMatchType.Equals("Disabled"));
            if (!profiles.Any()) return null;
            model = model.ToLower();

            var environmentProfiles = new List<ImageProfileEntity>();
            foreach (var profile in profiles)
            {
                var image = new ImageServices().GetImage(profile.ImageId);
                if(image.Environment.Equals(environment))
                    environmentProfiles.Add(profile);
            }


            //done in seperate loops to match most specific first
            foreach (var profile in environmentProfiles)
            {
                if (profile.ModelMatchType.Equals("Equals") && model.Equals(profile.ModelMatch))
                    return profile;
            }
            foreach (var profile in environmentProfiles)
            {
                if (profile.ModelMatchType.Equals("Starts With") && model.StartsWith(profile.ModelMatch))
                    return profile;
            }
            foreach (var profile in environmentProfiles)
            {
                if (profile.ModelMatchType.Equals("Ends With") && model.EndsWith(profile.ModelMatch))
                    return profile;
            }
            foreach (var profile in environmentProfiles)
            {
                if (profile.ModelMatchType.Equals("Contains") && model.Contains(profile.ModelMatch))
                    return profile;
            }
            return null;
        }

        private ValidationResultDTO ValidateImageProfile(ImageProfileEntity imageProfile, bool isNewImageProfile)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(imageProfile.Name))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Image Profile Name Is Not Valid";
                return validationResult;
            }

            if (isNewImageProfile)
            {
                if (
                    _uow.ImageProfileRepository.Exists(
                        h => h.Name == imageProfile.Name && h.ImageId == imageProfile.ImageId))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Image Profile Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalImageProfile = _uow.ImageProfileRepository.GetById(imageProfile.Id);
                if (originalImageProfile.Name != imageProfile.Name)
                {
                    if (
                        _uow.ImageProfileRepository.Exists(
                            h => h.Name == imageProfile.Name && h.ImageId == imageProfile.ImageId))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Image Profile Already Exists";
                        return validationResult;
                    }
                }

                if (!string.IsNullOrEmpty(imageProfile.ModelMatch))
                {
                    var profilesWithModelMatch =
                        _uow.ImageProfileRepository.Get(x => x.ModelMatch.Equals(imageProfile.ModelMatch.ToLower()) && x.Id != imageProfile.Id);
                    if (profilesWithModelMatch.Any())
                    {
                        var image = _uow.ImageRepository.GetById(profilesWithModelMatch.First().ImageId);
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Model Match Already Exists On Image: " + image.Name;
                    }
                }
            }

            return validationResult;
        }
    }
}