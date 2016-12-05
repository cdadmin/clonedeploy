using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using CloneDeploy_App.Helpers;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaBE;
using CsvHelper;

namespace CloneDeploy_Services
{
    public class ImageServices
    {
        private readonly UnitOfWork _uow;

        //private readonly UserServices _userServices = new UserServices();
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

                var defaultProfile = SeedDefaultImageProfile(image.Id);
                defaultProfile.ImageId = image.Id;
                new ImageProfileServices().AddProfile(defaultProfile);

                try
                {
                    Directory.CreateDirectory(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar +
                                              image.Name);
                    new FileOps().SetUnixPermissionsImage(Settings.PrimaryStoragePath + "images" +
                                                          Path.DirectorySeparatorChar + image.Name);

                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                    actionResult.ErrorMessage = "Could Not Create Image Directory.";
                }


            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
                return actionResult;
            }





            return actionResult;
        }



        public ActionResultDTO DeleteImage(int imageId)
        {
            var image = GetImage(imageId);
            if (image == null)
                return new ActionResultDTO() {ErrorMessage = "Image Not Found", Id = 0};
            var result = new ActionResultDTO();

            if (Convert.ToBoolean(image.Protected))
            {
                result.ErrorMessage = "This Image Is Protected And Cannot Be Deleted";
                return result;
            }

            _uow.ImageRepository.Delete(image.Id);
            _uow.Save();

            if (string.IsNullOrEmpty(image.Name)) return result;
            DeleteAllUserManagementsForImage(image.Id);
            DeleteAllProfilesForImage(image.Id);
            try
            {
                if (Directory.Exists(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name))
                    Directory.Delete(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name,
                        true);

                result.Success = true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                result.ErrorMessage = "Could Not Delete Image Folder";
                result.Success = false;

            }


            return result;



        }

        public  ImageEntity GetImage(int imageId)
        {
            
                return _uow.ImageRepository.GetById(imageId);
            
        }

        public  void SendImageApprovedEmail(int imageId)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;

            var image = GetImage(imageId);
            foreach (var user in _userServices.SearchUsers("").Where(x => x.NotifyImageApproved == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                var mail = new CloneDeploy_App.Helpers.Mail
                {
                    MailTo = user.Email,
                    Body = image.Name + " Has Been Approved",
                    Subject = "Image Approved"
                };
                mail.Send();
            }
        }

        
        public  string ImageCountUser(int userId)
        {
            if (_userServices.GetUser(userId).Membership == "Administrator")
                return TotalCount();

            var userManagedImages = _userServices.GetUserImageManagements(userId);
            
            //If count is zero image management is not being used return total count
            return userManagedImages.Count == 0 ? TotalCount() : userManagedImages.Count.ToString();
        }

        public List<ImageEntity> SearchImagesForUser(int userId, string searchString = "")
        {
            if (_userServices.GetUser(userId).Membership == "Administrator")
                return SearchImages(searchString);


            var listOfImages = new List<ImageEntity>();

            var userManagedImages = _userServices.GetUserImageManagements(userId);
            if (userManagedImages.Count == 0)
                return SearchImages(searchString);

            else
            {

                listOfImages.AddRange(
                    userManagedImages.Select(
                        managedImage =>
                            _uow.ImageRepository.GetFirstOrDefault(
                                i => i.Name.Contains(searchString) && i.Id == managedImage.ImageId)));



                return listOfImages;
            }

        }


        public  List<ImageEntity> GetOnDemandImageList(int userId = 0)
        {
           
                if (userId == 0)
                    return _uow.ImageRepository.Get(i => i.IsVisible == 1 && i.Enabled == 1, orderBy: (q => q.OrderBy(p => p.Name)));
                else
                {
                    if (_userServices.GetUser(userId).Membership == "Administrator")
                        return _uow.ImageRepository.Get(i => i.IsVisible == 1 && i.Enabled == 1, orderBy: (q => q.OrderBy(p => p.Name)));

                    var userManagedImages = _userServices.GetUserImageManagements(userId);
                    if (userManagedImages.Count == 0)
                        return _uow.ImageRepository.Get(i => i.IsVisible == 1 && i.Enabled == 1, orderBy: (q => q.OrderBy(p => p.Name)));
                    else
                    {
                        var listOfImages = new List<ImageEntity>();
                         listOfImages.AddRange(userManagedImages.Select(managedImage => _uow.ImageRepository.GetFirstOrDefault(i => i.IsVisible == 1 && i.Id == managedImage.ImageId && i.Enabled == 1)));
                        return listOfImages;
                    }
                }
            
        }

        public  List<ImageEntity> SearchImages(string searchString = "")
        {
            
                return _uow.ImageRepository.Get(i => i.Name.Contains(searchString));
            
        }

        public ActionResultDTO UpdateImage(ImageEntity image, string originalName)
        {
            var i = GetImage(image.Id);
            if (i == null)
                return new ActionResultDTO() {ErrorMessage = "Image Not Found", Id = 0};
            var result = new ActionResultDTO();

            var validationResult = ValidateImage(image, false);
            if (validationResult.Success)
            {
                _uow.ImageRepository.Update(image, image.Id);
                _uow.Save();

                if (image.Name != originalName)
                {
                    try
                    {
                        new FileOps().RenameFolder(originalName, image.Name);
                        result.Success = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message);
                        result.ErrorMessage = "Could Not Rename Image Folder";
                        result.Success = false;
                    }
                }
                else
                {
                    result.Success = true;
                }


            }
            return result;

        }

        public bool DeleteAllUserManagementsForImage(int imageId)
        {

            _uow.UserImageManagementRepository.DeleteRange(x => x.ImageId == imageId);
            _uow.Save();
            return true;

        }

        public bool DeleteAllProfilesForImage(int imageId)
        {

            _uow.ImageProfileRepository.DeleteRange(x => x.ImageId == imageId);
            _uow.Save();
            return true;

        }

        public static string Calculate_Hash(string fileName)
        {
            long read = 0;
            var r = -1;
            const long bytesToRead = 100 * 1024 * 1024;
            const int bufferSize = 4096;
            var buffer = new byte[bufferSize];
            var sha = new SHA256Managed();

            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                while (read <= bytesToRead && r != 0)
                {
                    read += (r = stream.Read(buffer, 0, bufferSize));
                    sha.TransformBlock(buffer, 0, r, null, 0);
                }
            }
            sha.TransformFinalBlock(buffer, 0, 0);
            return string.Join("", sha.Hash.Select(x => x.ToString("x2")));
        }

        

        private  string TotalCount()
        {
           
                return _uow.ImageRepository.Count();
            
        }

        public int ImportCsv(string path)
        {
            var importCounter = 0;
            using (var csv = new CsvReader(new StreamReader(path)))
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

        public void ExportCsv(string path)
        {
            using (var csv = new CsvWriter(new StreamWriter(path)))
            {
                csv.Configuration.RegisterClassMap<ImageCsvMap>();
                csv.WriteRecords(SearchImages());
            }
        }

        public  ActionResultDTO CheckApprovalAndChecksum(ImageEntity image, int userId)
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

            if (Settings.RequireImageApproval.ToLower() == "true")
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

        private  ValidationResultDTO ValidateImage(ImageEntity image, bool isNewImage)
        {
            var validationResult = new ValidationResultDTO();

            if (string.IsNullOrEmpty(image.Name) || !image.Name.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-'))
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

        public List<ImageProfileEntity> SearchProfiles(int imageId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ImageProfileRepository.Get(p => p.ImageId == imageId,
                    orderBy: (q => q.OrderBy(p => p.Name)));
            }
        }

        public ImageProfileEntity SeedDefaultImageProfile(int imageId)
        {
            var image = GetImage(imageId);
            var imageProfile = new ImageProfileEntity();
            imageProfile.Kernel = Settings.DefaultKernel32;
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

        public string ImageSizeOnServerForGridView(string imageName, string hdNumber)
        {
            try
            {
                var imagePath = Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + imageName + Path.DirectorySeparatorChar + "hd" + hdNumber;
                var size = new FileOps().GetDirectorySize(new DirectoryInfo(imagePath)) / 1024f / 1024f / 1024f;
                return Math.Abs(size) < 0.1f ? "< 100M" : size.ToString("#.##") + " GB";
            }
            catch
            {
                return "N/A";
            }
        }

        public List<ImageFileInfo> GetPartitionImageFileInfoForGridView(int imageId, string selectedHd, string selectedPartition)
        {
            var image = GetImage(imageId);
            try
            {
                var imageFile =
                    Directory.GetFiles(
                        Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name + Path.DirectorySeparatorChar + "hd" + selectedHd +
                        Path.DirectorySeparatorChar,
                        "part" + selectedPartition + ".*").FirstOrDefault();

                var fi = new FileInfo(imageFile);
                var imageFileInfo = new ImageFileInfo
                {
                    FileName = fi.Name,
                    FileSize = (fi.Length / 1024f / 1024f).ToString("#.##") + " MB"
                };

                return new List<ImageFileInfo> { imageFileInfo };
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return null;
            }
        }
    }
}