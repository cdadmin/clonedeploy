using CloneDeploy_App.BLL;
using CloneDeploy_Services;

namespace CloneDeploy_App.Helpers
{
    public class Settings
    {
        public static string AdLoginDomain
        {
            get { return SettingServices.GetSetting("AD Login Domain").Value; }
        }

        public static string ClientReceiverArgs
        {
            get { return SettingServices.GetSetting("Client Receiver Args").Value; }
        }

        public static string DebugRequiresLogin
        {
            get { return SettingServices.GetSetting("Debug Requires Login").Value; }
        }

        public static string DefaultComputerView
        {
            get { return SettingServices.GetSetting("Default Computer View").Value; }
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
            get { return SettingServices.GetSetting("Udpcast End Port").Value; }
        }

        public static string ForceSsL
        {
            get { return SettingServices.GetSetting("Force SSL").Value; }
        }

        public static string GlobalComputerArgs
        {
            get { return SettingServices.GetSetting("Global Computer Args").Value; }
        }

        

        public static string PrimaryStoragePath
        {
            get { return DistributionPointServices.GetPrimaryDistributionPoint().PhysicalPath; }
        }

      

        public static string OnDemand
        {
            get { return SettingServices.GetSetting("On Demand").Value; }
        }

        public static string OnDemandRequiresLogin
        {
            get { return SettingServices.GetSetting("On Demand Requires Login").Value; }
        }

        public static string ProxyBiosFile
        {
            get { return SettingServices.GetSetting("Proxy Bios File").Value; }
        }

        public static string ProxyDhcp
        {
            get { return SettingServices.GetSetting("Proxy Dhcp").Value; }
        }

        public static string ProxyEfi32File
        {
            get { return SettingServices.GetSetting("Proxy Efi32 File").Value; }
        }

        public static string ProxyEfi64File
        {
            get { return SettingServices.GetSetting("Proxy Efi64 File").Value; }
        }

        public static string PxeMode
        {
            get { return SettingServices.GetSetting("PXE Mode").Value; }
        }

        public static string QueueSize
        {
            get { return SettingServices.GetSetting("Queue Size").Value; }
        }

      

        public static string RegisterRequiresLogin
        {
            get { return SettingServices.GetSetting("Register Requires Login").Value; }
        }

        public static string SenderArgs
        {
            get { return SettingServices.GetSetting("Sender Args").Value; }
        }

        public static string ServerIp
        {
            get { return SettingServices.GetSetting("Server IP").Value; }
        }

        public static string UniversalToken
        {
            get { return SettingServices.GetSetting("Universal Token").Value; }
        }

        public static string StartPort
        {
            get { return SettingServices.GetSetting("Udpcast Start Port").Value; }
        }

        public static string TftpPath
        {
            get { return SettingServices.GetSetting("Tftp Path").Value; }
        }

        public static string WebPath
        {
            get { return SettingServices.GetSetting("Web Path").Value; }
        }

        public static string WebServerPort
        {
            get { return SettingServices.GetSetting("Web Server Port").Value; }
        }

        public static string WebTaskRequiresLogin
        {
            get { return SettingServices.GetSetting("Web Task Requires Login").Value; }
        }

        public static string SmtpServer
        {
            get { return SettingServices.GetSetting("Smtp Server").Value; }
        }

        public static string SmtpPort
        {
            get { return SettingServices.GetSetting("Smtp Port").Value; }
        }

        public static string SmtpUsername
        {
            get { return SettingServices.GetSetting("Smtp Username").Value; }
        }

        public static string SmtpPassword
        {
            get { return SettingServices.GetSetting("Smtp Password Encrypted").Value; }
        }

        public static string SmtpMailFrom
        {
            get { return SettingServices.GetSetting("Smtp Mail From").Value; }
        }

        public static string SmtpMailTo
        {
            get { return SettingServices.GetSetting("Smtp Mail To").Value; }
        }

        public static string SmtpSsl
        {
            get { return SettingServices.GetSetting("Smtp Ssl").Value; }
        }

        public static string SmtpEnabled
        {
            get { return SettingServices.GetSetting("Smtp Enabled").Value; }
        }

        public static string RequireImageApproval
        {
            get { return SettingServices.GetSetting("Require Image Approval").Value; }
        }

        public static string MulticastDecompression
        {
            get { return SettingServices.GetSetting("Multicast Decompression").Value; }
        }

        public static string IpxeRequiresLogin
        {
            get { return SettingServices.GetSetting("Ipxe Requires Login").Value; }
        }

        public static string LdapEnabled
        {
            get { return SettingServices.GetSetting("Ldap Enabled").Value; }
        }

        public static string LdapServer
        {
            get { return SettingServices.GetSetting("Ldap Server").Value; }
        }

        public static string LdapPort
        {
            get { return SettingServices.GetSetting("Ldap Port").Value; }
        }

        public static string LdapAuthAttribute
        {
            get { return SettingServices.GetSetting("Ldap Auth Attribute").Value; }
        }

        public static string LdapBaseDN
        {
            get { return SettingServices.GetSetting("Ldap Base DN").Value; }
        }

        public static string LdapAuthType
        {
            get { return SettingServices.GetSetting("Ldap Auth Type").Value; }
        }

        public static string MunkiPathType
        {
            get { return SettingServices.GetSetting("Munki Path Type").Value; }
        }

        public static string MunkiBasePath
        {
            get { return SettingServices.GetSetting("Munki Base Path").Value; }
        }

        public static string MunkiSMBUsername
        {
            get { return SettingServices.GetSetting("Munki SMB Username").Value; }
        }

        public static string MunkiSMBPassword
        {
            get { return SettingServices.GetSetting("Munki SMB Password Encrypted").Value; }
        }

        public static string MunkiSMBDomain
        {
            get { return SettingServices.GetSetting("Munki SMB Domain").Value; }
        }

        public static string ClobberEnabled
        {
            get { return SettingServices.GetSetting("Clobber Enabled").Value; }
        }

        public static string ClobberProfileId
        {
            get { return SettingServices.GetSetting("Clobber ProfileId").Value; }
        }

        public static string ClobberRequiresLogin
        {
            get { return SettingServices.GetSetting("Clobber Requires Login").Value; }
        }

        public static string ClobberPromptComputerName
        {
            get { return SettingServices.GetSetting("Clobber Prompt Computer Name").Value; }
        }
    
    }
}