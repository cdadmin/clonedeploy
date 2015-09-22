using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Global;
using Models;
using Newtonsoft.Json;
using Partition;



namespace DAL
{
    public class Image
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(Models.Image image)
        {
            return _context.Images.Any(i => i.Name.ToLower() == image.Name.ToLower());
        }

        public bool Create(Models.Image image)
        {

            try
            {

                _context.Images.Add(image);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);

                return false;
            }
        }



        public bool Delete(int imageId)
        {
            try
            {
                var image = _context.Images.Find(imageId);
                _context.Images.Attach(image);
                _context.Images.Remove(image);
                _context.SaveChanges();
                return true;
            }

            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

      

        public string GetTotalCount()
        {
          
                return _context.Images.Count().ToString();
            
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

        public Models.Image Read(int imageId)
        {
            return _context.Images.FirstOrDefault(p => p.Id == imageId);
        }



        public List<Models.Image> Find(string searchString)
        {
            return (from i in _context.Images where i.Name.Contains(searchString) orderby i.Name select i).ToList();

        }

        public bool Update(Models.Image image)
        {
            try
            {
                var originalImage = _context.Images.Find(image.Id);

                if (originalImage == null) return false;
                _context.Entry(originalImage).CurrentValues.SetValues(image);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }
    }
}