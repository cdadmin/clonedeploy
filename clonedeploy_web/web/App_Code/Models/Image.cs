/*  
    CrucibleWDS A Windows Deployment Solution
    Copyright (C) 2011  Jon Dolny

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Global;
using Newtonsoft.Json;
using Partition;

namespace Models
{
    [Table("images", Schema = "public")]
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_id", Order = 1)]
        public int Id { get; set; }

        [Column("image_name", Order = 2)]
        public string Name { get; set; }

        [Column("image_os", Order = 3)]
        public string Os { get; set; }

        [Column("image_description", Order = 4)]
        public string Description { get; set; }

        [Column("image_is_protected", Order = 5)]
        public int Protected { get; set; }

        [Column("image_is_viewable_ond", Order = 6)]
        public int IsVisible { get; set; }

        [Column("image_checksum", Order = 7)]
        public string Checksum { get; set; }

        [NotMapped]
        public string ClientSize { get; set; }

        [NotMapped]
        public string ClientSizeCustom { get; set; }

       

      
        public string Calculate_Hash(string fileName)
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
                    read += (r = stream.Read(buffer, 0, bufferSize));
                    sha.TransformBlock(buffer, 0, r, null, 0);
                }
            }
            sha.TransformFinalBlock(buffer, 0, 0);
            return string.Join("", sha.Hash.Select(x => x.ToString("x2")));
        }

        public bool Check_Checksum()
        {
            if (Settings.ImageChecksum != "On") return true;
            try
            {
                var listPhysicalImageChecksums = new List<HdChecksum>();
                var path = Settings.ImageStorePath + Name;
                var imageChecksum = new HdChecksum
                {
                    HdNumber = "hd1",
                    Path = path
                };
                listPhysicalImageChecksums.Add(imageChecksum);
                for (var x = 2;; x++)
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
                return physicalImageJson == Checksum;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return false;
            }
        }

        public void Create()
        {
            using (var db = new DB())
            {
                try
                {
                    if (db.Images.Any(i => i.Name.ToLower() == Name.ToLower()))
                    {
                        Utility.Message = "This image already exists";
                        return;
                    }
                    db.Images.Add(this);
                    db.SaveChanges();
                    Directory.CreateDirectory(Settings.ImageStorePath + Name);
                    Directory.CreateDirectory(Settings.ImageHoldPath + Name);
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Create Image.  Check The Exception Log For More Info.";
                    return;
                }
            }

            GetImageId();
            var history = new History
            {
                Event = "Create",
                Type = "Image",
                TypeId = Id.ToString()
            };
            history.CreateEvent();
            Utility.Message = "Successfully Created " + Name;
 

           
        }

        public void Delete()
        {
            if (Protected == 1)
            {
                Utility.Message += "Could Not Delete " + Name + " - It Is Protected" + "<br>";
                return;
            }
            using (var db = new DB())
            {
                try
                {
                    db.Images.Attach(this);
                    db.Images.Remove(this);
                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(Name))
                    {
                        if (Directory.Exists(Settings.ImageStorePath + Name))
                            Directory.Delete(Settings.ImageStorePath + Name, true);

                        if (Directory.Exists(Settings.ImageHoldPath + Name))
                            Directory.Delete(Settings.ImageHoldPath + Name, true);
                    }
                }

                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Delete Image.  Check The Exception Log For More Info.";
                    return;
                }

                Utility.Message = "Successfully Deleted " + Name;
                var history = new History
                {
                    Event = "Delete",
                    Type = "Image",
                    TypeId = Id.ToString()
                };
                history.CreateEvent();
            }
           
        }

        public void GetImageId()
        {
            using (var db = new DB())
            {
                var image = db.Images.First(i => i.Name.ToLower() == Name.ToLower());
                Id = image.Id;
            }          
        }

        public string GetTotalCount()
        {
            using (var db = new DB())
            {
                return db.Images.Count().ToString();
            }
        }

        public void Import()
        {
            var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                        Path.DirectorySeparatorChar + "csvupload" + Path.DirectorySeparatorChar + "images.csv";
            using (var db = new DB())
            {
                var importCount = db.Database.ExecuteSqlCommand("copy images(imagename,imageos,imagedesc,imageclientsize,imageclientsizecustom,checksum) from '" + path + "' DELIMITER ',' csv header FORCE NOT NULL imagedesc,checksum;");
                Utility.Message = importCount + " Images(s) Imported Successfully";
            }       
        }

        public void Read()
        {
            if (string.IsNullOrEmpty(Id.ToString()) && !string.IsNullOrEmpty(Name))
                GetImageId();

            using (var db = new DB())
            {
                var image = db.Images.First(i => i.Id == Id);

                Name = image.Name;
                Os = image.Os;
                Description = image.Description;
                Protected = image.Protected;
                IsVisible = image.IsVisible;
                ClientSize = image.ClientSize;
                ClientSizeCustom = image.ClientSizeCustom;
                Checksum = image.Checksum;
            }
          
        }

        public List<Image> Search(string searchString)
        {
            List<Image> list = new List<Image>();
            using (var db = new DB())
            {
                list.AddRange(from i in db.Images where i.Name.Contains(searchString) orderby i.Name select i);
            }

            return list;
        }

        public bool Update()
        {
            using (var db = new DB())
            {
                try
                {
                    var image = db.Images.Find(Id);
                    if (image != null)
                    {
                        image.Name = Name;
                        image.Description = Description;
                        image.Protected = Protected;
                        image.IsVisible = IsVisible;
                        image.Checksum = Checksum;
                        image.ClientSize = ClientSize;
                        image.ClientSizeCustom = ClientSizeCustom;
                        db.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Update Image.  Check The Exception Log For More Info.";
                    return false;
                }
            }

            var history = new History
            {
                Event = "Edit",
                Type = "Image",
                TypeId = Id.ToString()
            };
            history.CreateEvent();
            //Utility.Message = "Successfully Updated " + Name;

            return true;
        }

        

        public bool ValidateImageData()
        {
            var validated = true;
            if (string.IsNullOrEmpty(Name) || Name.Contains(" "))
            {
                validated = false;
                Utility.Message = "Image Name Cannot Be Empty Or Contain Spaces";
            }

            return validated;
        }
    }
}