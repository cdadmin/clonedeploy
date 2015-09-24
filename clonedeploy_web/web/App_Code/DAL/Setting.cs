using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using Global;

namespace DAL
{
    public class Setting
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public void ExportDatabase()
        {
           throw new Exception("Not Implemented");
        }

        public Models.Setting Read(string settingName)
        {
            return _context.Settings.First(s => s.Name == settingName);
        }


        public bool Update(Models.Setting setting)
        {
            try
            {
                var oldSetting = _context.Settings.First(s => s.Name == setting.Name);
                oldSetting.Value = setting.Value;
                _context.SaveChanges();

            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }

            return true;
        }
    }
}