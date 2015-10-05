using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;
using Models;

namespace DAL
{
    public class User
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(string userName)
        {
            return _context.Users.Any(h => h.Name == userName);
        }
        public bool Create(WdsUser user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Delete(int userId)
        {
            try
            {
                var user = _context.Users.Find(userId);
                _context.Users.Attach(user);
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }

            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public int GetAdminCount()
        {
          
            return _context.Users.Count(u => u.Membership == "Administrator");
            
        }

        public string GetTotalCount()
        {
         
                return _context.Users.Count().ToString();
            
        }

        public void Import()
        {
           throw new Exception("Not Implemented");
        }

        public WdsUser Read(int userId)
        {
            return _context.Users.FirstOrDefault(p => p.Id == userId);
        }

        public WdsUser Read(string userName)
        {
            return _context.Users.FirstOrDefault(p => p.Name == userName);
        }

        public List<WdsUser> Find(string searchString)
        {

            return (from u in _context.Users where u.Name.Contains(searchString) orderby u.Name select u).ToList();

        }

        public bool Update(WdsUser user, bool updatePassword)
        {

            try
            {
                var wdsUser = _context.Users.Find(user.Id);
                if (wdsUser == null) return false;
                if (updatePassword)
                {
                    wdsUser.Password = user.Password;
                    wdsUser.Salt = user.Salt;
                }
                wdsUser.Name = user.Name;
                wdsUser.Membership = user.Membership;
                wdsUser.GroupManagement = user.GroupManagement;
                wdsUser.OndAccess = user.OndAccess;
                wdsUser.DiagAccess = user.DiagAccess;
                wdsUser.DebugAccess = user.DebugAccess;
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