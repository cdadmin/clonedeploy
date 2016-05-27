using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;
using Helpers;

namespace BLL
{
    /// <summary>
    /// Summary description for Ldap
    /// </summary>
    public class Ldap
    {
        public Ldap()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public bool Authenticate(string username, string pwd)
        {
            if (Settings.LdapEnabled != "1") return false;

            string path = "LDAP://" + Settings.LdapServer + ":" + Settings.LdapPort + "/" + Settings.LdapBaseDN;
            string _filterAttribute = null;
           
            DirectoryEntry entry = new DirectoryEntry(path,username,pwd);

            if(Settings.LdapAuthType == "Basic")
                entry.AuthenticationType = AuthenticationTypes.None;
            else if (Settings.LdapAuthType == "Secure")
                entry.AuthenticationType = AuthenticationTypes.Secure;
            else if (Settings.LdapAuthType == "SSL")
                entry.AuthenticationType = AuthenticationTypes.SecureSocketsLayer;


            try
            {
                // Bind to the native AdsObject to force authentication.
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(" + Settings.LdapAuthAttribute + "=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("memberOf");
             
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                // Update the new path to the user in the directory
                path = result.Path;
                _filterAttribute = (String)result.Properties["cn"][0];

                var groups = new List<String>();
                try
                {
                    //SearchResult groupResult = search.FindOne();
                    int propertyCount = result.Properties["memberOf"].Count;
                    String dn;
                    int equalsIndex, commaIndex;

                    for (int propertyCounter = 0; propertyCounter < propertyCount;
                         propertyCounter++)
                    {
                        dn = (String)result.Properties["memberOf"][propertyCounter];

                        equalsIndex = dn.IndexOf("=", 1);
                        commaIndex = dn.IndexOf(",", 1);
                        if (-1 == equalsIndex)
                        {
                            continue;
                            //return null;
                        }
                        groups.Add(dn.Substring((equalsIndex + 1),
                                          (commaIndex - equalsIndex) - 1));
                    
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error obtaining group names. " +
                      ex.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Could Not Authenticate User: " + username + " " + ex.Message);
                return false;
            }
            //GetGroups(_filterAttribute, path);
            return true;


        }

        public string GetGroups(string _filterAttribute, string _path)
        {
            var groups = new List<String>();
            DirectorySearcher search = new DirectorySearcher(_path);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();
            try
            {
                SearchResult result = search.FindOne();
                int propertyCount = result.Properties["memberOf"].Count;
                String dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount;
                     propertyCounter++)
                {
                    dn = (String)result.Properties["memberOf"][propertyCounter];

                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }
                    groups.Add(dn.Substring((equalsIndex + 1),
                                      (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " +
                  ex.Message);
            }
            return groupNames.ToString();
        }
    }
}