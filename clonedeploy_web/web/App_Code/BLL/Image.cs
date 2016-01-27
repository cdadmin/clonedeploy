using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using DAL;
using Helpers;
using Models;
using Newtonsoft.Json;
using Partition;

namespace BLL
{
    public static class Image
    {
        public static Models.ValidationResult AddImage(Models.Image image)
        {
            var validationResult = ValidateImage(image, true);
            using (var uow = new DAL.UnitOfWork())
            {
                if (validationResult.IsValid)
                {
                    validationResult.IsValid = false;
                    uow.ImageRepository.Insert(image);
                    if (uow.Save())
                    {
                        validationResult.IsValid = true;
                    }

                }
               
            }
            if (validationResult.IsValid)
            {
                BLL.ImageProfile.SeedDefaultImageProfile(image.Id);
                try
                {
                    Directory.CreateDirectory(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name);
                    
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                    throw;
                }                
            }
            return validationResult;
        }



        public static Models.ValidationResult DeleteImage(Models.Image image)
        {
            var result = new Models.ValidationResult(){IsValid = false};
            using (var uow = new DAL.UnitOfWork())
            {
                if (Convert.ToBoolean(image.Protected))
                {
                    result.Message = "This Image Is Protected And Cannot Be Deleted";
                    result.IsValid = false;
                    return result;
                }

                uow.ImageRepository.Delete(image.Id);
                if (uow.Save())
                {
                    if (string.IsNullOrEmpty(image.Name)) return result;
                    BLL.UserImageManagement.DeleteImage(image.Id);
                    BLL.ImageProfile.DeleteImage(image.Id);
                    try
                    {
                        if (Directory.Exists(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name))
                            Directory.Delete(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name, true);

                        result.IsValid = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message);
                        result.Message = "Could Not Delete Image Folder";
                        result.IsValid = false;

                    }

                }
                else
                {
                    result.Message = "Could Not Delete Image";
                    result.IsValid = false;
                }
                return result;
                
            }

        }

        public static Models.Image GetImage(int imageId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ImageRepository.GetById(imageId);
            }
        }

        public static string ImageCountUser(int userId)
        {
            if (BLL.User.GetUser(userId).Membership == "Administrator")
                return TotalCount();

            var userManagedImages = BLL.UserImageManagement.Get(userId);
            
            //If count is zero image management is not being used return total count
            return userManagedImages.Count == 0 ? TotalCount() : userManagedImages.Count.ToString();
        }

        public static List<Models.Image> SearchImagesForUser(int userId, string searchString = "")
        {
            if (BLL.User.GetUser(userId).Membership == "Administrator")
                return SearchImages(searchString);

           
                var listOfImages = new List<Models.Image>();

                var userManagedImages = BLL.UserImageManagement.Get(userId);
                if (userManagedImages.Count == 0)
                    return SearchImages(searchString);

                else
                {
                    using (var uow = new DAL.UnitOfWork())
                    {
                        listOfImages.AddRange(userManagedImages.Select(managedImage => uow.ImageRepository.GetFirstOrDefault(i => i.Name.Contains(searchString) && i.Id == managedImage.ImageId)));
                    }


                    return listOfImages;
                }
 
        }


        public static List<Models.Image> GetOnDemandImageList(int userId = 0)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                if (userId == 0)
                    return uow.ImageRepository.Get(i => i.IsVisible == 1, orderBy: (q => q.OrderBy(p => p.Name)));
                else
                {
                    if (BLL.User.GetUser(userId).Membership == "Administrator")
                        return uow.ImageRepository.Get(i => i.IsVisible == 1, orderBy: (q => q.OrderBy(p => p.Name)));

                    var userManagedImages = BLL.UserImageManagement.Get(userId);
                    if (userManagedImages.Count == 0)
                        return uow.ImageRepository.Get(i => i.IsVisible == 1, orderBy: (q => q.OrderBy(p => p.Name)));
                    else
                    {
                         var listOfImages = new List<Models.Image>();
                         listOfImages.AddRange(userManagedImages.Select(managedImage => uow.ImageRepository.GetFirstOrDefault(i => i.IsVisible == 1 && i.Id == managedImage.ImageId)));
                        return listOfImages;
                    }
                }
            }
        }

        public static List<Models.Image> SearchImages(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ImageRepository.Get(i => i.Name.Contains(searchString));
            }
        }

        public static Models.ValidationResult UpdateImage(Models.Image image, string originalName)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateImage(image, false);
                if (validationResult.IsValid)
                {
                    validationResult.IsValid = false;
                    uow.ImageRepository.Update(image, image.Id);
                    if (uow.Save())
                    {
                        if (image.Name != originalName)
                        {
                            try
                            {
                                new FileOps().RenameFolder(originalName, image.Name);
                                validationResult.IsValid = true;
                            }
                            catch (Exception ex)
                            {
                                Logger.Log(ex.Message);
                                throw;
                            }
                        }
                        else
                        {
                            validationResult.IsValid = true;
                        }
                    }

                }
                return validationResult;
            }
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

        public static bool Check_Checksum(Models.Image image)
        {
            if (Settings.ImageChecksum != "On") return true;
            try
            {
                var listPhysicalImageChecksums = new List<HdChecksum>();
                var path = Settings.PrimaryStoragePath + image.Name;
                var imageChecksum = new HdChecksum
                {
                    HdNumber = "hd1",
                    Path = path
                };
                listPhysicalImageChecksums.Add(imageChecksum);
                for (var x = 2; ; x++)
                {
                    imageChecksum = new HdChecksum();
                    var subdir = path + Path.DirectorySeparatorChar + "hd" + x;
                    if (Directory.Exists(subdir))
                    {
                        imageChecksum.HdNumber = "hd" + x;
                        imageChecksum.Path = subdir;
                        listPhysicalImageChecksums.Add(imageChecksum);
                    }
                    else
                        break;
                }

                foreach (var hd in listPhysicalImageChecksums)
                {
                    var listChecksums = new List<FileChecksum>();

                    var files = Directory.GetFiles(hd.Path, "*.*");
                    foreach (var file in files)
                    {
                        var fc = new FileChecksum
                        {
                            FileName = Path.GetFileName(file),
                            Checksum = Calculate_Hash(file)
                        };
                        listChecksums.Add(fc);
                    }
                    hd.Path = string.Empty;
                    hd.Fc = listChecksums.ToArray();
                }


                var physicalImageJson = JsonConvert.SerializeObject(listPhysicalImageChecksums);
                return physicalImageJson == image.Checksum;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return false;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ImageRepository.Count();
            }
        }

        public static void Import()
        {
            
        }

        public static Models.ValidationResult CheckApprovalAndChecksum(Models.Image image)
        {
            var validationResult = new Models.ValidationResult();
            if (image == null)
            {
                validationResult.IsValid = false;
                validationResult.Message = "Image Does Not Exist";
                return validationResult;
            }

            if (Settings.RequireImageApproval == "true")
            {
                if (!Convert.ToBoolean(image.Approved))
                {
                    validationResult.IsValid = false;
                    validationResult.Message = "Image Has Not Been Approved";
                    return validationResult;
                }
            }

            if (!Check_Checksum(image))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Image Checksum Does Not Match Original";
                return validationResult;
            }

            validationResult.IsValid = true;
            return validationResult;
        }

        public static Models.ValidationResult ValidateImage(Models.Image image, bool isNewImage)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(image.Name) || !image.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Image Name Is Not Valid";
                return validationResult;
            }

            if (isNewImage)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.ImageRepository.Exists(h => h.Name == image.Name))
                    {
                        validationResult.IsValid = false;
                        validationResult.Message = "This Image Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalImage = uow.ImageRepository.GetById(image.Id);
                    if (originalImage.Name != image.Name)
                    {
                        if (uow.ImageRepository.Exists(h => h.Name == image.Name))
                        {
                            validationResult.IsValid = false;
                            validationResult.Message = "This Image Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }
    }
}