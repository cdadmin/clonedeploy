using System.Collections.Generic;
using CloneDeploy_App.Helpers;
using CloneDeploy_Entities;
using Newtonsoft.Json;

namespace CloneDeploy_App.BLL
{
    /// <summary>
    ///     Summary description for Authenticate
    /// </summary>
    public class AuthenticationServices
    {
        public string ConsoleLogin(string username, string password, string task, string ip)
        {
            var result = new Dictionary<string, string>();
           
            var validationResult = GlobalLogin(username, password, "Console");
            
            if (!validationResult.Success)
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
            if (!validationResult.Success) return "goto Menu";
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

        public ActionResultEntity GlobalLogin(string userName, string password, string loginType)
        {
            var validationResult = new ActionResultEntity
            {
                Message = "Incorrect Username Or Password",
                Success = false
            };

            //Check if user exists in Clone Deploy
            var user = BLL.User.GetUser(userName);
            if (user == null)
            {
                //Check For a first time LDAP User Group Login
                if (Settings.LdapEnabled == "1")
                {
                    foreach (var ldapGroup in BLL.UserGroup.GetLdapGroups())
                    {
                        if (new BLL.Ldap().Authenticate(userName, password, ldapGroup.GroupLdapName))
                        {
                            //user is a valid ldap user via ldap group that has not yet logged in.
                            //Add the user and allow login.                         
                            var cdUser = new CloneDeployUserEntity
                            {
                                Name = userName,
                                Salt = Helpers.Utility.CreateSalt(64),
                                Token = Utility.GenerateKey(),
                                IsLdapUser = 1
                            };
                            //Create a local random db pass, should never actually be possible to use.
                            cdUser.Password = Helpers.Utility.CreatePasswordHash(new System.Guid().ToString(), cdUser.Salt);
                            if (BLL.User.AddUser(cdUser).Success)
                            {
                                //add user to group
                                var newUser = BLL.User.GetUser(userName);
                                BLL.UserGroup.AddNewGroupMember(ldapGroup,newUser);
                            }
                            validationResult.Message = "Success";
                            validationResult.Success = true;
                            break;
                        }
                    }
                }
                return validationResult;
            }

            if (BLL.UserLockout.AccountIsLocked(user.Id))
            {
                BLL.UserLockout.ProcessBadLogin(user.Id);
                validationResult.Message = "Account Is Locked";
                return validationResult;
            }

            //Check against AD
            if (user.IsLdapUser == 1 && Settings.LdapEnabled == "1")
            {
                //Check if user is authenticated against an ldap group
                if (user.UserGroupId != -1)
                {
                    //user is part of a group, is the group an ldap group?
                    var userGroup = BLL.UserGroup.GetUserGroup(user.UserGroupId);
                    if (userGroup != null)
                    {
                        if (userGroup.IsLdapGroup == 1)
                        {
                            //the group is an ldap group
                            //make sure user is still in that ldap group
                            if (new BLL.Ldap().Authenticate(userName, password, userGroup.GroupLdapName))
                            {
                                validationResult.Success = true;
                            }
                            else
                            {
                                //user is either not in that group anymore, not in the directory, or bad password
                                validationResult.Success = false;

                                if (new BLL.Ldap().Authenticate(userName, password))
                                {
                                    //password was good but user is no longer in the group
                                    //delete the user
                                    BLL.User.DeleteUser(user.Id);
                                }
                            }
                        }
                        else
                        {
                            //the group is not an ldap group
                            //still need to check creds against directory
                            if (new BLL.Ldap().Authenticate(userName, password)) validationResult.Success = true;
                        }
                    }
                    else
                    {
                        //group didn't exist for some reason
                        //still need to check creds against directory
                        if (new BLL.Ldap().Authenticate(userName, password)) validationResult.Success = true;
                    }
                }
                else
                {
                    //user is not part of a group, check creds against directory
                    if (new BLL.Ldap().Authenticate(userName, password)) validationResult.Success = true;
                }
               
            }
            else if (user.IsLdapUser == 1 && Settings.LdapEnabled != "1")
            {
                //prevent ldap user from logging in with local pass if ldap auth gets turned off
                validationResult.Success = false;
            }
            //Check against local DB
            else
            {
                var hash = Helpers.Utility.CreatePasswordHash(password, user.Salt);
                if (user.Password == hash) validationResult.Success = true;
            }
           
            if (validationResult.Success)
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