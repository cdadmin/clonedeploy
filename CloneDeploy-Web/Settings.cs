

using CloneDeploy_ApiCalls;

namespace CloneDeploy_Web
{
    public class Settings
    {
       
        public static string AdLoginDomain
        {
            get { return new APICall().SettingApi.GetSetting("AD Login Domain").Value; }
        }

        public static string ClientReceiverArgs
        {
            get { return new APICall().SettingApi.GetSetting("Client Receiver Args").Value; }
        }

        public static string DebugRequiresLogin
        {
            get { return new APICall().SettingApi.GetSetting("Debug Requires Login").Value; }
        }

        public static string DefaultComputerView
        {
            get { return new APICall().SettingApi.GetSetting("Default Computer View").Value; }
        }

        public static string DefaultKernel32
        {
            get { return "4.5"; }
        }
        
        public static string DefaultKernel64
        {
            get { return "4.5x64"; }
        }

        public static string DefaultInit
        {
            get { return "initrd.xz"; }
        }

        public static string EndPort
        {
            get { return new APICall().SettingApi.GetSetting("Udpcast End Port").Value; }
        }

        public static string ForceSsL
        {
            get { return new APICall().SettingApi.GetSetting("Force SSL").Value; }
        }

        public static string GlobalComputerArgs
        {
            get { return new APICall().SettingApi.GetSetting("Global Computer Args").Value; }
        }

        

        public static string PrimaryStoragePath
        {
            get { return new APICall().DistributionPointApi.GetPrimary().PhysicalPath; }
        }

      

        public static string OnDemand
        {
            get { return new APICall().SettingApi.GetSetting("On Demand").Value; }
        }

        public static string OnDemandRequiresLogin
        {
            get { return new APICall().SettingApi.GetSetting("On Demand Requires Login").Value; }
        }

        public static string ProxyBiosFile
        {
            get { return new APICall().SettingApi.GetSetting("Proxy Bios File").Value; }
        }

        public static string ProxyDhcp
        {
            get { return new APICall().SettingApi.GetSetting("Proxy Dhcp").Value; }
        }

        public static string ProxyEfi32File
        {
            get { return new APICall().SettingApi.GetSetting("Proxy Efi32 File").Value; }
        }

        public static string ProxyEfi64File
        {
            get { return new APICall().SettingApi.GetSetting("Proxy Efi64 File").Value; }
        }

        public static string PxeMode
        {
            get { return new APICall().SettingApi.GetSetting("PXE Mode").Value; }
        }

        public static string QueueSize
        {
            get { return new APICall().SettingApi.GetSetting("Queue Size").Value; }
        }

      

        public static string RegisterRequiresLogin
        {
            get { return new APICall().SettingApi.GetSetting("Register Requires Login").Value; }
        }

        public static string SenderArgs
        {
            get { return new APICall().SettingApi.GetSetting("Sender Args").Value; }
        }

        public static string ServerIp
        {
            get { return new APICall().SettingApi.GetSetting("Server IP").Value; }
        }

        public static string UniversalToken
        {
            get { return new APICall().SettingApi.GetSetting("Universal Token").Value; }
        }

        public static string StartPort
        {
            get { return new APICall().SettingApi.GetSetting("Udpcast Start Port").Value; }
        }

        public static string TftpPath
        {
            get { return new APICall().SettingApi.GetSetting("Tftp Path").Value; }
        }

        public static string WebPath
        {
            get { return new APICall().SettingApi.GetSetting("Web Path").Value; }
        }

        public static string WebServerPort
        {
            get { return new APICall().SettingApi.GetSetting("Web Server Port").Value; }
        }

        public static string WebTaskRequiresLogin
        {
            get { return new APICall().SettingApi.GetSetting("Web Task Requires Login").Value; }
        }

        public static string SmtpServer
        {
            get { return new APICall().SettingApi.GetSetting("Smtp Server").Value; }
        }

        public static string SmtpPort
        {
            get { return new APICall().SettingApi.GetSetting("Smtp Port").Value; }
        }

        public static string SmtpUsername
        {
            get { return new APICall().SettingApi.GetSetting("Smtp Username").Value; }
        }

        public static string SmtpPassword
        {
            get { return new APICall().SettingApi.GetSetting("Smtp Password Encrypted").Value; }
        }

        public static string SmtpMailFrom
        {
            get { return new APICall().SettingApi.GetSetting("Smtp Mail From").Value; }
        }

        public static string SmtpMailTo
        {
            get { return new APICall().SettingApi.GetSetting("Smtp Mail To").Value; }
        }

        public static string SmtpSsl
        {
            get { return new APICall().SettingApi.GetSetting("Smtp Ssl").Value; }
        }

        public static string SmtpEnabled
        {
            get { return new APICall().SettingApi.GetSetting("Smtp Enabled").Value; }
        }

        public static string RequireImageApproval
        {
            get { return new APICall().SettingApi.GetSetting("Require Image Approval").Value; }
        }

        public static string MulticastDecompression
        {
            get { return new APICall().SettingApi.GetSetting("Multicast Decompression").Value; }
        }

        public static string IpxeRequiresLogin
        {
            get { return new APICall().SettingApi.GetSetting("Ipxe Requires Login").Value; }
        }

        public static string LdapEnabled
        {
            get { return new APICall().SettingApi.GetSetting("Ldap Enabled").Value; }
        }

        public static string LdapServer
        {
            get { return new APICall().SettingApi.GetSetting("Ldap Server").Value; }
        }

        public static string LdapPort
        {
            get { return new APICall().SettingApi.GetSetting("Ldap Port").Value; }
        }

        public static string LdapAuthAttribute
        {
            get { return new APICall().SettingApi.GetSetting("Ldap Auth Attribute").Value; }
        }

        public static string LdapBaseDN
        {
            get { return new APICall().SettingApi.GetSetting("Ldap Base DN").Value; }
        }

        public static string LdapAuthType
        {
            get { return new APICall().SettingApi.GetSetting("Ldap Auth Type").Value; }
        }

        public static string MunkiPathType
        {
            get { return new APICall().SettingApi.GetSetting("Munki Path Type").Value; }
        }

        public static string MunkiBasePath
        {
            get { return new APICall().SettingApi.GetSetting("Munki Base Path").Value; }
        }

        public static string MunkiSMBUsername
        {
            get { return new APICall().SettingApi.GetSetting("Munki SMB Username").Value; }
        }

        public static string MunkiSMBPassword
        {
            get { return new APICall().SettingApi.GetSetting("Munki SMB Password Encrypted").Value; }
        }

        public static string MunkiSMBDomain
        {
            get { return new APICall().SettingApi.GetSetting("Munki SMB Domain").Value; }
        }

        public static string ClobberEnabled
        {
            get { return new APICall().SettingApi.GetSetting("Clobber Enabled").Value; }
        }

        public static string ClobberProfileId
        {
            get { return new APICall().SettingApi.GetSetting("Clobber ProfileId").Value; }
        }

        public static string ClobberRequiresLogin
        {
            get { return new APICall().SettingApi.GetSetting("Clobber Requires Login").Value; }
        }

        public static string ClobberPromptComputerName
        {
            get { return new APICall().SettingApi.GetSetting("Clobber Prompt Computer Name").Value; }
        }
    
    }
}