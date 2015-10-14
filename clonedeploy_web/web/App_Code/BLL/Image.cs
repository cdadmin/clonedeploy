using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using DAL;
using Helpers;
using Newtonsoft.Json;
using Partition;

namespace BLL
{
    public static class Image
    {
        public static Models.ValidationResult AddImage(Models.Image image)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateImage(image, true);
                if (validationResult.IsValid)
                {
                    validationResult.IsValid = false;
                    uow.ImageRepository.Insert(image);
                    if (uow.Save())
                    {
                        try
                        {
                            Directory.CreateDirectory(Settings.ImageStorePath + image.Name);
                            validationResult.IsValid = true;
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex.Message);
                            throw;
                        }
                    }

                }
                return validationResult;
            }
        }

        public static bool DeleteImage(Models.Image image)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                if (Convert.ToBoolean(image.Protected))
                {
                    //Message.Text = "This Image Is Protected And Cannot Be Deleted";
                    return false;
                }

                uow.ImageRepository.Delete(image.Id);
                if (uow.Save())
                {
                    if (string.IsNullOrEmpty(image.Name)) return false;
                    try
                    {
                        if (Directory.Exists(Settings.ImageStorePath + image.Name))
                            Directory.Delete(Settings.ImageStorePath + image.Name, true);

                        if (Directory.Exists(Settings.ImageHoldPath + image.Name))
                            Directory.Delete(Settings.ImageHoldPath + image.Name, true);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message);
                        //Message.Text = "Could Not Delete Image Folder";
                        return false;
                    }

                }
                else
                {
                    //Message.Text = "Could Not Delete Image";
                    return false;
                }
            }

        }

        public static Models.Image GetImage(int imageId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ImageRepository.GetById(imageId);
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
                var path = Settings.ImageStorePath + image.Name;
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