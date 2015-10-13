using System.Web.Security;
using BLL;
using Helpers;
using Models;

namespace Security
{
    /// <summary>
    ///     Summary description for Authenticate
    /// </summary>
    public class Authenticate
    {
        public string ConsoleLogin(string username, string password, string task, string isWebTask, string ip)
        {
            if (!GlobalLogin(username, password, "Console")) return "false";
            var wdsuser = BLL.User.GetUser(username);


            //FIX ME
            /*if ((task == "ond" && wdsuser.OndAccess == "1") || (task == "debug" && wdsuser.DebugAccess == "1") ||
                (task == "diag" && wdsuser.DiagAccess == "1") || (task == "register") || (isWebTask == "push") ||
                (isWebTask == "pull"))
            {
                    
                   
                return "true," + wdsuser.Id + "," + Settings.ServerKey;
            }*/


            return "false";
        }

        private string CreatePasswordHash(string pwd, string salt)
        {
            var saltAndPwd = string.Concat(pwd, salt);
            var hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");
            return hashedPwd;
        }

        public string IpxeLogin(string username, string password, string kernel, string bootImage, string task)
        {
            string lines = null;
            var wdsKey = Settings.WebTaskRequiresLogin == "No" ? Settings.ServerKey : "";
            var globalHostArgs = Settings.GlobalHostArgs;
            if (!GlobalLogin(username, password, "iPXE")) return lines;
            lines = "#!ipxe\r\n";
            lines += "kernel " + Settings.WebPath + "IpxeBoot?filename=" + kernel + "&type=kernel" +
                     " initrd=" + bootImage + " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp " + " web=" +
                     Settings.WebPath + " WDS_KEY=" + wdsKey + " task=" + task + " consoleblank=0 " +
                     globalHostArgs + "\r\n";
            lines += "imgfetch --name " + bootImage + " " + Settings.WebPath + "IpxeBoot?filename=" +
                     bootImage + "&type=bootimage" + "\r\n";
            lines += "boot";

            return lines;
        }

        public bool GlobalLogin(string userName, string password, string loginType)
        {
            bool validated = false;
            //Check if user exists in CWDS
            var user = BLL.User.GetUser(userName);

            if (user.Id != null)
            {
                //FIX ME
                //Check against AD
                /*
                if (!string.IsNullOrEmpty(Settings.AdLoginDomain))
                {
                    var context = new PrincipalContext(ContextType.Domain, Settings.AdLoginDomain,
                        userName, password);
                    var adUser = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName);
                    if (adUser != null) validated = true;
                }
                //Check against local DB
                else
                {
                    var hash = CreatePasswordHash(password, user.Salt);
                    if (user.Password == hash) validated = true;
                }*/
                var hash = CreatePasswordHash(password, user.Salt);
                if (user.Password == hash) validated = true;
            }

            if (validated)
            {
                var history = new History
                {
                    //Ip = ip,
                    Event = "Successful " + loginType + " Login",
                    Type = "User",
                    EventUser = userName,
                    TypeId = user.Id.ToString(),
                    Notes = ""
                };
                //history.CreateEvent();

                var mail = new Mail
                {
                    Subject = "Successful " + loginType+ " Login",
                    Body = userName
                };
                mail.Send("Successful Login");
                return true;
            }
            else
            {
                var history = new History
                {
                    //Ip = ip,
                    Event = "Failed " + loginType + " Login",
                    Type = "User",
                    EventUser = userName,
                    Notes = password
                };
                //history.CreateEvent();

                var mail = new Mail
                {
                    Subject = "Failed " + loginType + " Login",
                    Body = userName
                };
                mail.Send("Failed Login");
                return false;
            }
        }
    }
}