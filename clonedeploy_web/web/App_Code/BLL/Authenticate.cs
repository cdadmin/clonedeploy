using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Web.Security;
using BLL;
using Helpers;
using Models;
using Newtonsoft.Json;

namespace Security
{
    /// <summary>
    ///     Summary description for Authenticate
    /// </summary>
    public class Authenticate
    {
        public string ConsoleLogin(string username, string password, string task, string ip)
        {
            var validationResult = GlobalLogin(username, password, "Console");
            var result = new Dictionary<string, string>();
            if (!validationResult.IsValid)
            {
                result.Add("valid", "false");
                result.Add("user_id", "");
                result.Add("user_token", "");
            }
            else
            {
                var wdsuser = BLL.User.GetUser(username);
                result.Add("valid", "true");
                result.Add("user_id", wdsuser.Id.ToString());
                result.Add("user_token", wdsuser.Token);
            }
         
            return JsonConvert.SerializeObject(result);
        }

       

        public string IpxeLogin(string username, string password, string kernel, string bootImage, string task)
        {
            var newLineChar = "\n";
            string wdsKey = null;
            if (Settings.DebugRequiresLogin == "No" || Settings.OnDemandRequiresLogin == "No" ||
               Settings.RegisterRequiresLogin == "No" || Settings.WebTaskRequiresLogin == "No")
                wdsKey = Settings.UniversalToken;
            else
            {
                wdsKey = "";
            }
            var globalComputerArgs = Settings.GlobalComputerArgs;
            var validationResult = GlobalLogin(username, password, "iPXE");
            if (!validationResult.IsValid) return "goto Menu";
            var lines = "#!ipxe" + newLineChar;
            lines += "kernel " + Settings.WebPath + "IpxeBoot?filename=" + kernel + "&type=kernel" +
                     " initrd=" + bootImage + " root=/dev/ram0 rw ramdisk_size=127000 " + " web=" +
                     Settings.WebPath + " USER_TOKEN=" + wdsKey + " task=" + task + " consoleblank=0 " +
                     globalComputerArgs + newLineChar;
            lines += "imgfetch --name " + bootImage + " " + Settings.WebPath + "IpxeBoot?filename=" +
                     bootImage + "&type=bootimage" + newLineChar;
            lines += "boot";

            return lines;
        }

        public Models.ValidationResult GlobalLogin(string userName, string password, string loginType)
        {
            var tmp = Helpers.Utility.CreatePasswordHash("password", "salt");

            var validationResult = new Models.ValidationResult
            {
                Message = "Login Was Not Successful",
                IsValid = false
            };

            //Check if user exists in CWDS
            var user = BLL.User.GetUser(userName);
            if (user == null) return validationResult;

            if (BLL.UserLockout.AccountIsLocked(user.Id))
            {
                BLL.UserLockout.ProcessBadLogin(user.Id);
                validationResult.Message = "Account Is Locked";
                return validationResult;
            }

            //Check against AD
            if (!string.IsNullOrEmpty(Settings.AdLoginDomain))
            {
                try
                {
                    var context = new PrincipalContext(ContextType.Domain, Settings.AdLoginDomain,
                        userName, password);
                    var adUser = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName);
                    if (adUser != null) validationResult.IsValid = true;
                }
                catch (Exception)
                {
                    //Fallback to local db in case ad auth isn't working
                    var hash = Helpers.Utility.CreatePasswordHash(password, user.Salt);
                    if (user.Password == hash) validationResult.IsValid = true;

                }
            }
            //Check against local DB
            else
            {
                var hash = Helpers.Utility.CreatePasswordHash(password, user.Salt);
                if (user.Password == hash) validationResult.IsValid = true;
            }
           
            if (validationResult.IsValid)
            {
                BLL.UserLockout.DeleteUserLockouts(user.Id);
                validationResult.Message = "Success";
                return validationResult;
            }
            else
            {
                BLL.UserLockout.ProcessBadLogin(user.Id);
                return validationResult;
            }
        }
    }
}