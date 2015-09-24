using BLL;

namespace Global
{
    public class Settings
    {
        public static string AdLoginDomain
        {
            get { return Setting.GetSetting("AD Login Domain"); }
        }

        public static string ClientReceiverArgs
        {
            get { return Setting.GetSetting("Client Receiver Args"); }
        }

        public static string CompressionAlgorithm
        {
            get { return Setting.GetSetting("Compression Algorithm"); }
        }

        public static string CompressionLevel
        {
            get { return Setting.GetSetting("Compression Level"); }
        }

        public static string DebugRequiresLogin
        {
            get { return Setting.GetSetting("Debug Requires Login"); }
        }

        public static string DefaultHostView
        {
            get { return Setting.GetSetting("Default Host View"); }
        }

        public static string DefaultKernel32
        {
            get { return "3.19.3-WDS"; }
        }

        public static string DefaultKernel64
        {
            get { return "3.19.3x64-WDS"; }
        }

        public static string EndPort
        {
            get { return Setting.GetSetting("Udpcast End Port"); }
        }

        public static string ForceSsL
        {
            get { return Setting.GetSetting("Force SSL"); }
        }

        public static string GlobalHostArgs
        {
            get { return Setting.GetSetting("Global Host Args"); }
        }

        public static string ImageChecksum
        {
            get { return Setting.GetSetting("Image Checksum"); }
        }

        public static string ImageHoldPath
        {
            get { return Setting.GetSetting("Image Hold Path"); }
        }

        public static string ImageStorePath
        {
            get { return Setting.GetSetting("Image Store Path"); }
        }

        public static string ImageTransferMode
        {
            get { return Setting.GetSetting("Image Transfer Mode"); }
        }

        public static string NfsDeployPath
        {
            get { return Setting.GetSetting("Nfs Deploy Path"); }
        }

        public static string NfsUploadPath
        {
            get { return Setting.GetSetting("Nfs Upload Path"); }
        }

        public static string OnDemand
        {
            get { return Setting.GetSetting("On Demand"); }
        }

        public static string OnDemandRequiresLogin
        {
            get { return Setting.GetSetting("On Demand Requires Login"); }
        }

        public static string ProxyBiosFile
        {
            get { return Setting.GetSetting("Proxy Bios File"); }
        }

        public static string ProxyDhcp
        {
            get { return Setting.GetSetting("Proxy Dhcp"); }
        }

        public static string ProxyEfi32File
        {
            get { return Setting.GetSetting("Proxy Efi32 File"); }
        }

        public static string ProxyEfi64File
        {
            get { return Setting.GetSetting("Proxy Efi64 File"); }
        }

        public static string PxeMode
        {
            get { return Setting.GetSetting("PXE Mode"); }
        }

        public static string QueueSize
        {
            get { return Setting.GetSetting("Queue Size"); }
        }

        public static string ReceiverArgs
        {
            get { return Setting.GetSetting("Receiver Args"); }
        }

        public static string RegisterRequiresLogin
        {
            get { return Setting.GetSetting("Register Requires Login"); }
        }

        public static string SenderArgs
        {
            get { return Setting.GetSetting("Sender Args"); }
        }

        public static string ServerIp
        {
            get { return Setting.GetSetting("Server IP"); }
        }

        public static string ServerIpWithPort
        {
            get { return Setting.GetServerIpWithPort(); }
        }

        public static string ServerKey
        {
            get { return Setting.GetSetting("Server Key"); }
        }

        public static string SmbPassword
        {
            get { return Setting.GetSetting("SMB Password"); }
        }

        public static string SmbPath
        {
            get { return Setting.GetSetting("SMB Path"); }
        }

        public static string SmbUserName
        {
            get { return Setting.GetSetting("SMB User Name"); }
        }

        public static string StartPort
        {
            get { return Setting.GetSetting("Udpcast Start Port"); }
        }

        public static string TftpPath
        {
            get { return Setting.GetSetting("Tftp Path"); }
        }

        public static string WebPath
        {
            get { return Setting.GetSetting("Web Path"); }
        }

        public static string WebServerPort
        {
            get { return Setting.GetSetting("Web Server Port"); }
        }

        public static string WebTaskRequiresLogin
        {
            get { return Setting.GetSetting("Web Task Requires Login"); }
        }

        public static string SmtpServer
        {
            get { return Setting.GetSetting("Smtp Server"); }
        }

        public static string SmtpPort
        {
            get { return Setting.GetSetting("Smtp Port"); }
        }

        public static string SmtpUsername
        {
            get { return Setting.GetSetting("Smtp Username"); }
        }

        public static string SmtpPassword
        {
            get { return Setting.GetSetting("Smtp Password"); }
        }

        public static string SmtpMailFrom
        {
            get { return Setting.GetSetting("Smtp Mail From"); }
        }

        public static string SmtpMailTo
        {
            get { return Setting.GetSetting("Smtp Mail To"); }
        }

        public static string SmtpSsl
        {
            get { return Setting.GetSetting("Smtp Ssl"); }
        }

        public static string NotifySuccessfulLogin
        {
            get { return Setting.GetSetting("Notify Successful Login"); }
        }

        public static string NotifyFailedLogin
        {
            get { return Setting.GetSetting("Notify Failed Login"); }
        }

        public static string NotifyTaskStarted
        {
            get { return Setting.GetSetting("Notify Task Started"); }
        }

        public static string NotifyTaskCompleted
        {
            get { return Setting.GetSetting("Notify Task Completed"); }
        }

        public static string NotifyImageApproved
        {
            get { return Setting.GetSetting("Notify Image Approved"); }
        }

        public static string NotifyResizeFailed
        {
            get { return Setting.GetSetting("Notify Resize Failed"); }
        }
    
    }
}