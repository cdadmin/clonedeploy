using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.ModelBinding;
using DAL;
using Global;
using Helpers;
using Newtonsoft.Json;
using Partition;


namespace BLL
{
    public class Image
    {
        private readonly DAL.Image _da = new DAL.Image();

        public bool AddImage(Models.Image image)
        {
            if (_da.Exists(image))
            {
                Message.Text = "An Image With This Name Already Exists";
                return false;
            }
            if (_da.Create(image))
            {
                try
                {
                    Directory.CreateDirectory(Settings.ImageStorePath + image.Name);
                    Directory.CreateDirectory(Settings.ImageHoldPath + image.Name);
                    Message.Text = "Successfully Created Image";
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                    Message.Text = "Could Not Create Image Directories";
                    return false;
                }                    
            }
            else
            {
                Message.Text = "Could Not Create Image";
                return false;
            }
        }

        public bool DeleteImage(Models.Image image)
        {
            if (Convert.ToBoolean(image.Protected))
            {
                Message.Text = "This Image Is Protected And Cannot Be Deleted";
                return false;
            }

            if (_da.Delete(image.Id))
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
                    Message.Text = "Could Not Delete Image Folder";
                    return false;
                }

            }
            else
            {
                Message.Text = "Could Not Delete Image";
                return false;
            }

        }

        public Models.Image GetImage(int imageId)
        {
            return _da.Read(imageId);
        }

        public List<Models.Image> SearchImages(string searchString)
        {
            return _da.Find(searchString);
        }

        public bool UpdateImage(Models.Image image, string originalName)
        {
            if (!_da.Update(image)) return false;
            if (image.Name.ToLower() == originalName.ToLower()) return true;
            new FileOps().RenameFolder(originalName, image.Name);
            return true;
        }

        public string Calculate_Hash(string fileName)
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

        public bool Check_Checksum(Models.Image image)
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

        public string TotalCount()
        {
            return _da.GetTotalCount();
        }

        public void Import()
        {
            
        }

        public bool ValidateImageData(Models.Image image)
        {
            var validated = true;
            if (string.IsNullOrEmpty(image.Name) || image.Name.Contains(" "))
            {
                validated = false;
                Message.Text = "Image Name Cannot Be Empty Or Contain Spaces";
            }

            return validated;
        }
    }
}