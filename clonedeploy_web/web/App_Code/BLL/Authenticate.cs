using System.Collections.Generic;
using Helpers;
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
            var result = new Dictionary<string, string>();
           
            var validationResult = GlobalLogin(username, password, "Console");
            
            if (!validationResult.IsValid)
            {
                result.Add("valid", "false");
                result.Add("user_id", "");
                result.Add("user_token", "");
            }
            else
            {
                var cloneDeployUser = BLL.User.GetUser(username);
                result.Add("valid", "true");
                result.Add("user_id", cloneDeployUser.Id.ToString());
                result.Add("user_token", cloneDeployUser.Token);
            }
         
            return JsonConvert.SerializeObject(result);
        }

       

        public string IpxeLogin(string username, string password, string kernel, string bootImage, string task)
        {
            var newLineChar = "\n";
            string userToken = null;
            if (Settings.DebugRequiresLogin == "No" || Settings.OnDemandRequiresLogin == "No" ||
               Settings.RegisterRequiresLogin == "No" || Settings.WebTaskRequiresLogin == "No")
                userToken = Settings.UniversalToken;
            else
            {
                userToken = "";
            }
            var globalComputerArgs = Settings.GlobalComputerArgs;
            var validationResult = GlobalLogin(username, password, "iPXE");
            if (!validationResult.IsValid) return "goto Menu";
            var lines = "#!ipxe" + newLineChar;
            lines += "kernel " + Settings.WebPath + "IpxeBoot?filename=" + kernel + "&type=kernel" +
                     " initrd=" + bootImage + " root=/dev/ram0 rw ramdisk_size=156000 " + " web=" +
                     Settings.WebPath + " USER_TOKEN=" + userToken + " task=" + task + " consoleblank=0 " +
                     globalComputerArgs + newLineChar;
            lines += "imgfetch --name " + bootImage + " " + Settings.WebPath + "IpxeBoot?filename=" +
                     bootImage + "&type=bootimage" + newLineChar;
            lines += "boot";

            return lines;
        }

        public Models.ValidationResult GlobalLogin(string userName, string password, string loginType)
        {
            var validationResult = new Models.ValidationResult
            {
                Message = "Login Was Not Successful",
                IsValid = false
            };

            //Check if user exists in Clone Deploy
            var user = BLL.User.GetUser(userName);
            if (user == null) return validationResult;

            if (BLL.UserLockout.AccountIsLocked(user.Id))
            {
                BLL.UserLockout.ProcessBadLogin(user.Id);
                validationResult.Message = "Account Is Locked";
                return validationResult;
            }

            //Check against AD
            if (user.IsLdapUser == 1 && Settings.LdapEnabled == "1")
            {
                if (new BLL.Ldap().Authenticate(userName, password)) validationResult.IsValid = true;
            }
            else if (user.IsLdapUser == 1 && Settings.LdapEnabled != "1")
            {
                //prevent ldap user from logging in with local pass if ldap auth gets turned off
                validationResult.IsValid = false;
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