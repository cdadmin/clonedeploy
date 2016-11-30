using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using CloneDeploy_App.Helpers;
using CloneDeploy_DataModel;
using CsvHelper;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public static class Image
    {
        public static ActionResultEntity AddImage(ImageEntity image)
        {
            var validationResult = ValidateImage(image, true);
            using (var uow = new UnitOfWork())
            {
                if (validationResult.Success)
                {
                    validationResult.Success = false;
                    uow.ImageRepository.Insert(image);
                    uow.Save();
                    
                        validationResult.Success = true;
                    

                }
               
            }
            if (validationResult.Success)
            {
                var defaultProfile = BLL.ImageProfile.SeedDefaultImageProfile(image.Id);
                defaultProfile.ImageId = image.Id;
                BLL.ImageProfile.AddProfile(defaultProfile);

                try
                {
                    Directory.CreateDirectory(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name);
                    new FileOps().SetUnixPermissionsImage(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name);
                    
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                  
                }                
            }
            return validationResult;
        }



        public static ActionResultEntity DeleteImage(int imageId)
        {
            var image = GetImage(imageId);
            var result = new ActionResultEntity() { Success = false };
            using (var uow = new UnitOfWork())
            {
                if (Convert.ToBoolean(image.Protected))
                {
                    result.Message = "This Image Is Protected And Cannot Be Deleted";
                    result.Success = false;
                    return result;
                }

                uow.ImageRepository.Delete(image.Id);
                uow.Save();
               
                    if (string.IsNullOrEmpty(image.Name)) return result;
                    BLL.UserImageManagement.DeleteImage(image.Id);
                    BLL.ImageProfile.DeleteImage(image.Id);
                    try
                    {
                        if (Directory.Exists(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name))
                            Directory.Delete(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name, true);

                        result.Success = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message);
                        result.Message = "Could Not Delete Image Folder";
                        result.Success = false;

                    }

              
                return result;
                
            }

        }

        public static ImageEntity GetImage(int imageId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ImageRepository.GetById(imageId);
            }
        }

        public static void SendImageApprovedEmail(int imageId)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;

            var image = BLL.Image.GetImage(imageId);
            foreach (var user in BLL.User.SearchUsers("").Where(x => x.NotifyImageApproved == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                var mail = new Helpers.Mail
                {
                    MailTo = user.Email,
                    Body = image.Name + " Has Been Approved",
                    Subject = "Image Approved"
                };
                mail.Send();
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

        public static List<ImageEntity> SearchImagesForUser(int userId, string searchString = "")
        {
            if (BLL.User.GetUser(userId).Membership == "Administrator")
                return SearchImages(searchString);


            var listOfImages = new List<ImageEntity>();

                var userManagedImages = BLL.UserImageManagement.Get(userId);
                if (userManagedImages.Count == 0)
                    return SearchImages(searchString);

                else
                {
                    using (var uow = new UnitOfWork())
                    {
                        listOfImages.AddRange(userManagedImages.Select(managedImage => uow.ImageRepository.GetFirstOrDefault(i => i.Name.Contains(searchString) && i.Id == managedImage.ImageId)));
                    }


                    return listOfImages;
                }
 
        }


        public static List<ImageEntity> GetOnDemandImageList(int userId = 0)
        {
            using (var uow = new UnitOfWork())
            {
                if (userId == 0)
                    return uow.ImageRepository.Get(i => i.IsVisible == 1 && i.Enabled == 1, orderBy: (q => q.OrderBy(p => p.Name)));
                else
                {
                    if (BLL.User.GetUser(userId).Membership == "Administrator")
                        return uow.ImageRepository.Get(i => i.IsVisible == 1 && i.Enabled == 1, orderBy: (q => q.OrderBy(p => p.Name)));

                    var userManagedImages = BLL.UserImageManagement.Get(userId);
                    if (userManagedImages.Count == 0)
                        return uow.ImageRepository.Get(i => i.IsVisible == 1 && i.Enabled == 1, orderBy: (q => q.OrderBy(p => p.Name)));
                    else
                    {
                        var listOfImages = new List<ImageEntity>();
                         listOfImages.AddRange(userManagedImages.Select(managedImage => uow.ImageRepository.GetFirstOrDefault(i => i.IsVisible == 1 && i.Id == managedImage.ImageId && i.Enabled == 1)));
                        return listOfImages;
                    }
                }
            }
        }

        public static List<ImageEntity> SearchImages(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ImageRepository.Get(i => i.Name.Contains(searchString));
            }
        }

        public static ActionResultEntity UpdateImage(ImageEntity image, string originalName)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateImage(image, false);
                if (validationResult.Success)
                {
                    validationResult.Success = false;
                    uow.ImageRepository.Update(image, image.Id);
                    uow.Save();
                    
                        if (image.Name != originalName)
                        {
                            try
                            {
                                new FileOps().RenameFolder(originalName, image.Name);
                                validationResult.Success = true;
                            }
                            catch (Exception ex)
                            {
                                Logger.Log(ex.Message);
                                
                            }
                        }
                        else
                        {
                            validationResult.Success = true;
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

        

        private static string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ImageRepository.Count();
            }
        }

        public static int ImportCsv(string path)
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

        public static void ExportCsv(string path)
        {
            using (var csv = new CsvWriter(new StreamWriter(path)))
            {
                csv.Configuration.RegisterClassMap<ImageCsvMap>();
                csv.WriteRecords(SearchImages());
            }
        }

        public static ActionResultEntity CheckApprovalAndChecksum(ImageEntity image, int userId)
        {
            var validationResult = new ActionResultEntity();
            if (image == null)
            {
                validationResult.Success = false;
                validationResult.Message = "Image Does Not Exist";
                return validationResult;
            }

            if (image.Enabled == 0)
            {
                validationResult.Success = false;
                validationResult.Message = "Image Is Not Enabled";
                return validationResult;
            }

            if (Settings.RequireImageApproval.ToLower() == "true")
            {
                var user = BLL.User.GetUser(userId);
                if (user.Membership != "Administrator") //administrators don't need image approval
                {
                    if (!Convert.ToBoolean(image.Approved))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "Image Has Not Been Approved";
                        return validationResult;
                    }
                }
            }

          

            validationResult.Success = true;
            return validationResult;
        }

        public static ActionResultEntity ValidateImage(ImageEntity image, bool isNewImage)
        {
            var validationResult = new ActionResultEntity();

            if (string.IsNullOrEmpty(image.Name) || !image.Name.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-'))
            {
                validationResult.Success = false;
                validationResult.Message = "Image Name Is Not Valid";
                return validationResult;
            }

            if (isNewImage)
            {
                using (var uow = new UnitOfWork())
                {
                    if (uow.ImageRepository.Exists(h => h.Name == image.Name))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "This Image Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new UnitOfWork())
                {
                    var originalImage = uow.ImageRepository.GetById(image.Id);
                    if (originalImage.Name != image.Name)
                    {
                        if (uow.ImageRepository.Exists(h => h.Name == image.Name))
                        {
                            validationResult.Success = false;
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