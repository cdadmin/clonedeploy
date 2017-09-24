using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using CloneDeploy_Common;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaBE;
using CloneDeploy_Services.Helpers;
using CsvHelper;
using log4net;

namespace CloneDeploy_Services
{
    public class ImageServices
    {
        private readonly UnitOfWork _uow;

        private readonly UserServices _userServices = new UserServices();
        private readonly ILog log = LogManager.GetLogger(typeof(ImageServices));

        public ImageServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddImage(ImageEntity image)
        {
            var validationResult = ValidateImage(image, true);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.ImageRepository.Insert(image);
                _uow.Save();
                actionResult.Id = image.Id;
                var defaultProfile = SeedDefaultImageProfile(image.Id);
                defaultProfile.ImageId = image.Id;
                new ImageProfileServices().AddProfile(defaultProfile);

                var dirCreateResult = new FilesystemServices().CreateNewImageFolders(image.Name);
                actionResult.Success = dirCreateResult;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
                return actionResult;
            }

            return actionResult;
        }

        public static string Calculate_Hash(string fileName)
        {
            long read = 0;
            var r = -1;
            const long bytesToRead = 100*1024*1024;
            const int bufferSize = 4096;
            var buffer = new byte[bufferSize];
            var sha = new SHA256Managed();

            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                while (read <= bytesToRead && r != 0)
                {
                    read += r = stream.Read(buffer, 0, bufferSize);
                    sha.TransformBlock(buffer, 0, r, null, 0);
                }
            }
            sha.TransformFinalBlock(buffer, 0, 0);
            return string.Join("", sha.Hash.Select(x => x.ToString("x2")));
        }

        public ActionResultDTO CheckApprovalAndChecksum(ImageEntity image, int userId)
        {
            var actionResult = new ActionResultDTO();
            if (image == null)
            {
                actionResult.Success = false;
                actionResult.ErrorMessage = "Image Does Not Exist";
                return actionResult;
            }

            if (image.Enabled == 0)
            {
                actionResult.Success = false;
                actionResult.ErrorMessage = "Image Is Not Enabled";
                return actionResult;
            }

            if (SettingServices.GetSettingValue(SettingStrings.RequireImageApproval).ToLower() == "true")
            {
                var user = _userServices.GetUser(userId);
                if (user.Membership != "Administrator") //administrators don't need image approval
                {
                    if (!Convert.ToBoolean(image.Approved))
                    {
                        actionResult.Success = false;
                        actionResult.ErrorMessage = "Image Has Not Been Approved";
                        return actionResult;
                    }
                }
            }

            actionResult.Success = true;
            return actionResult;
        }

        public bool DeleteAllProfilesForImage(int imageId)
        {
            _uow.ImageProfileRepository.DeleteRange(x => x.ImageId == imageId);
            _uow.Save();
            return true;
        }

        public bool DeleteAllUserManagementsForImage(int imageId)
        {
            _uow.UserImageManagementRepository.DeleteRange(x => x.ImageId == imageId);
            _uow.Save();
            return true;
        }

        public ActionResultDTO DeleteImage(int imageId)
        {
            var image = GetImage(imageId);
            if (image == null)
                return new ActionResultDTO {ErrorMessage = "Image Not Found", Id = 0};
            var result = new ActionResultDTO();

            if (Convert.ToBoolean(image.Protected))
            {
                result.ErrorMessage = "This Image Is Protected And Cannot Be Deleted";
                return result;
            }

            _uow.ImageRepository.Delete(image.Id);
            _uow.Save();
            result.Id = imageId;
            //Check if image name is empty or null, return if so or something will be deleted that shouldn't be
            if (string.IsNullOrEmpty(image.Name)) return result;
            DeleteAllUserManagementsForImage(image.Id);
            DeleteAllProfilesForImage(image.Id);
            var delDirectoryResult = new FilesystemServices().DeleteImageFolders(image.Name);
            result.Success = delDirectoryResult;

            return result;
        }

        public void ExportCsv(string path)
        {
            using (var csv = new CsvWriter(new StreamWriter(path)))
            {
                csv.Configuration.RegisterClassMap<ImageCsvMap>();
                csv.WriteRecords(SearchImages());
            }
        }

        public ImageEntity GetImage(int imageId)
        {
            return _uow.ImageRepository.GetById(imageId);
        }

        public List<AuditLogEntity> GetImageAuditLogs(int imageId, int limit)
        {
            if (limit == 0) limit = int.MaxValue;
            return
                _uow.AuditLogRepository.Get(x => x.ObjectType == "Image" && x.ObjectId == imageId)
                    .OrderByDescending(x => x.Id)
                    .Take(limit)
                    .ToList();
        }

        public List<ImageEntity> GetOnDemandImageList(int userId = 0)
        {
            if (userId == 0)
                return _uow.ImageRepository.Get(i => i.IsVisible == 1 && i.Enabled == 1, q => q.OrderBy(p => p.Name));
            if (_userServices.GetUser(userId).Membership == "Administrator")
                return _uow.ImageRepository.Get(i => i.IsVisible == 1 && i.Enabled == 1, q => q.OrderBy(p => p.Name));

            var user = _userServices.GetUser(userId);
            if (user.ImageManagementEnabled == 0)
                return _uow.ImageRepository.Get(i => i.IsVisible == 1 && i.Enabled == 1, q => q.OrderBy(p => p.Name));

            var userManagedImages = _userServices.GetUserImageManagements(userId);
            var listOfImages = new List<ImageEntity>();
            listOfImages.AddRange(
                userManagedImages.Select(
                    managedImage =>
                        _uow.ImageRepository.GetFirstOrDefault(
                            i => i.IsVisible == 1 && i.Id == managedImage.ImageId && i.Enabled == 1)));
            return listOfImages;
        }

        public List<ImageFileInfo> GetPartitionImageFileInfoForGridView(int imageId, string selectedHd,
            string selectedPartition)
        {
            var image = GetImage(imageId);
            return new FilesystemServices().GetPartitionFileSize(image.Name, selectedHd, selectedPartition);
        }

        public string ImageCountUser(int userId)
        {
            if (_userServices.GetUser(userId).Membership == "Administrator")
                return TotalCount();

            var user = _userServices.GetUser(userId);
            if (user.ImageManagementEnabled == 0)
            {
                return TotalCount();
            }
            var userManagedImages = _userServices.GetUserImageManagements(userId);
            return userManagedImages.Count.ToString();
        }

        public string ImageSizeOnServerForGridView(string imageName, string hdNumber)
        {
            return new FilesystemServices().GetHdFileSize(imageName, hdNumber);
        }

        public int ImportCsv(string csvContents)
        {
            var importCounter = 0;
            using (var csv = new CsvReader(new StringReader(csvContents)))
            {
                csv.Configuration.RegisterClassMap<ImageCsvMap>();
                var records = csv.GetRecords<ImageEntity>();
                foreach (var image in records)
                {
                    if (AddImage(image).Success)
                        importCounter++;
                }
            }
            return importCounter;
        }

        public List<ImageWithDate> SearchImages(string searchString = "")
        {
            var images = _uow.ImageRepository.Get(i => i.Name.Contains(searchString));

            var listWithDate = new List<ImageWithDate>();
            foreach (var image in images)
            {
                var imageWithDate = new ImageWithDate();
                imageWithDate.Id = image.Id;
                imageWithDate.Name = image.Name;
                imageWithDate.Environment = image.Environment;
                imageWithDate.Approved = image.Approved;
                imageWithDate.LastUsed = new AuditLogServices().GetImageLastUsedDate(image.Id);
                imageWithDate.ClassificationId = image.ClassificationId;
                listWithDate.Add(imageWithDate);
            }

            return listWithDate;
        }

        public List<ImageWithDate> SearchImagesForUser(int userId, string searchString = "")
        {
            if (_userServices.GetUser(userId).Membership == "Administrator")
                return SearchImages(searchString);

            var user = _userServices.GetUser(userId);
            if (user.ImageManagementEnabled == 0)
                return SearchImages(searchString);

            var listOfImages = new List<ImageEntity>();
            var userManagedImages = _userServices.GetUserImageManagements(userId);
            listOfImages.AddRange(
                userManagedImages.Select(
                    managedImage =>
                        _uow.ImageRepository.GetFirstOrDefault(
                            i => i.Name.Contains(searchString) && i.Id == managedImage.ImageId)));

            var listWithDate = new List<ImageWithDate>();
            foreach (var image in listOfImages)
            {
                var imageWithDate = new ImageWithDate();
                imageWithDate.Id = image.Id;
                imageWithDate.Name = image.Name;
                imageWithDate.Environment = image.Environment;
                imageWithDate.Approved = image.Approved;
                imageWithDate.LastUsed = new AuditLogServices().GetImageLastUsedDate(image.Id);
                imageWithDate.ClassificationId = image.ClassificationId;
                listWithDate.Add(imageWithDate);
            }

            return listWithDate;
        }

        public List<ImageProfileEntity> SearchProfiles(int imageId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ImageProfileRepository.Get(p => p.ImageId == imageId, q => q.OrderBy(p => p.Name));
            }
        }

        public ImageProfileEntity SeedDefaultImageProfile(int imageId)
        {
            var image = GetImage(imageId);
            var imageProfile = new ImageProfileEntity();
            imageProfile.Kernel = SettingStrings.DefaultKernel64;
            imageProfile.BootImage = "initrd.xz";
            imageProfile.Name = "default";
            imageProfile.Description = "Auto Generated Via New Image.";
            imageProfile.SkipCore = 0;
            imageProfile.SkipClock = 0;
            imageProfile.RemoveGPT = 0;
            imageProfile.SkipShrinkVolumes = 0;
            imageProfile.SkipShrinkLvm = 0;
            imageProfile.SkipExpandVolumes = 0;
            imageProfile.FixBcd = 0;
            imageProfile.FixBootloader = 1;
            imageProfile.PartitionMethod = "Dynamic";
            imageProfile.Compression = "lz4";
            imageProfile.CompressionLevel = "1";
            imageProfile.TaskCompletedAction = "Reboot";
            imageProfile.ChangeName = 1;
            if (image.Environment == "macOS")
                imageProfile.OsxTargetVolume = "Macintosh HD";

            return imageProfile;
        }

        public void SendImageApprovedEmail(int imageId)
        {
            //Mail not enabled
            if (SettingServices.GetSettingValue(SettingStrings.SmtpEnabled) == "0") return;

            var image = GetImage(imageId);
            foreach (
                var user in
                    _userServices.SearchUsers("")
                        .Where(x => x.NotifyImageApproved == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                var mail = new MailServices
                {
                    MailTo = user.Email,
                    Body = image.Name + " Has Been Approved",
                    Subject = "Image Approved"
                };
                mail.Send();
            }
        }

        private string TotalCount()
        {
            return _uow.ImageRepository.Count();
        }

        public ActionResultDTO UpdateImage(ImageEntity image)
        {
            var originalImage = GetImage(image.Id);
            if (originalImage == null)
                return new ActionResultDTO {ErrorMessage = "Image Not Found", Id = 0};
            var result = new ActionResultDTO();

            var updateFolderName = originalImage.Name != image.Name;
            var oldName = originalImage.Name;
            var validationResult = ValidateImage(image, false);
            if (validationResult.Success)
            {
                _uow.ImageRepository.Update(image, image.Id);
                _uow.Save();
                result.Id = image.Id;
                if (updateFolderName)
                {
                    result.Success = new FilesystemServices().RenameImageFolder(oldName, image.Name);
                }
                else
                {
                    result.Success = true;
                }
            }
            return result;
        }

        private ValidationResultDTO ValidateImage(ImageEntity image, bool isNewImage)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(image.Name) ||
                !image.Name.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-'))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Image Name Is Not Valid";
                return validationResult;
            }

            if (isNewImage)
            {
                if (_uow.ImageRepository.Exists(h => h.Name == image.Name))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Image Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalImage = _uow.ImageRepository.GetById(image.Id);
                if (originalImage.Name != image.Name)
                {
                    if (_uow.ImageRepository.Exists(h => h.Name == image.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Image Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}