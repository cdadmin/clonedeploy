using Models;

namespace Global
{
    public class Settings
    {
        public static string AdLoginDomain
        {
            get { return Setting.Read("AD Login Domain"); }
        }

        public static string ClientReceiverArgs
        {
            get { return Setting.Read("Client Receiver Args"); }
        }

        public static string CompressionAlgorithm
        {
            get { return Setting.Read("Compression Algorithm"); }
        }

        public static string CompressionLevel
        {
            get { return Setting.Read("Compression Level"); }
        }

        public static string DebugRequiresLogin
        {
            get { return Setting.Read("Debug Requires Login"); }
        }

        public static string DefaultHostView
        {
            get { return Setting.Read("Default Host View"); }
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
            get { return Setting.Read("Udpcast End Port"); }
        }

        public static string ForceSsL
        {
            get { return Setting.Read("Force SSL"); }
        }

        public static string GlobalHostArgs
        {
            get { return Setting.Read("Global Host Args"); }
        }

        public static string ImageChecksum
        {
            get { return Setting.Read("Image Checksum"); }
        }

        public static string ImageHoldPath
        {
            get { return Setting.Read("Image Hold Path"); }
        }

        public static string ImageStorePath
        {
            get { return Setting.Read("Image Store Path"); }
        }

        public static string ImageTransferMode
        {
            get { return Setting.Read("Image Transfer Mode"); }
        }

        public static string NfsDeployPath
        {
            get { return Setting.Read("Nfs Deploy Path"); }
        }

        public static string NfsUploadPath
        {
            get { return Setting.Read("Nfs Upload Path"); }
        }

        public static string OnDemand
        {
            get { return Setting.Read("On Demand"); }
        }

        public static string OnDemandRequiresLogin
        {
            get { return Setting.Read("On Demand Requires Login"); }
        }

        public static string ProxyBiosFile
        {
            get { return Setting.Read("Proxy Bios File"); }
        }

        public static string ProxyDhcp
        {
            get { return Setting.Read("Proxy Dhcp"); }
        }

        public static string ProxyEfi32File
        {
            get { return Setting.Read("Proxy Efi32 File"); }
        }

        public static string ProxyEfi64File
        {
            get { return Setting.Read("Proxy Efi64 File"); }
        }

        public static string PxeMode
        {
            get { return Setting.Read("PXE Mode"); }
        }

        public static string QueueSize
        {
            get { return Setting.Read("Queue Size"); }
        }

        public static string ReceiverArgs
        {
            get { return Setting.Read("Receiver Args"); }
        }

        public static string RegisterRequiresLogin
        {
            get { return Setting.Read("Register Requires Login"); }
        }

        public static string SenderArgs
        {
            get { return Setting.Read("Sender Args"); }
        }

        public static string ServerIp
        {
            get { return Setting.Read("Server IP"); }
        }

        public static string ServerIpWithPort
        {
            get { return Setting.GetServerIpWithPort(); }
        }

        public static string ServerKey
        {
            get { return Setting.Read("Server Key"); }
        }

        public static string SmbPassword
        {
            get { return Setting.Read("SMB Password"); }
        }

        public static string SmbPath
        {
            get { return Setting.Read("SMB Path"); }
        }

        public static string SmbUserName
        {
            get { return Setting.Read("SMB User Name"); }
        }

        public static string StartPort
        {
            get { return Setting.Read("Udpcast Start Port"); }
        }

        public static string TftpPath
        {
            get { return Setting.Read("Tftp Path"); }
        }

        public static string WebPath
        {
            get { return Setting.Read("Web Path"); }
        }

        public static string WebServerPort
        {
            get { return Setting.Read("Web Server Port"); }
        }

        public static string WebTaskRequiresLogin
        {
            get { return Setting.Read("Web Task Requires Login"); }
        }

        public static string SmtpServer
        {
            get { return Setting.Read("Smtp Server"); }
        }

        public static string SmtpPort
        {
            get { return Setting.Read("Smtp Port"); }
        }

        public static string SmtpUsername
        {
            get { return Setting.Read("Smtp Username"); }
        }

        public static string SmtpPassword
        {
            get { return Setting.Read("Smtp Password"); }
        }

        public static string SmtpMailFrom
        {
            get { return Setting.Read("Smtp Mail From"); }
        }

        public static string SmtpMailTo
        {
            get { return Setting.Read("Smtp Mail To"); }
        }

        public static string SmtpSsl
        {
            get { return Setting.Read("Smtp Ssl"); }
        }

        public static string NotifySuccessfulLogin
        {
            get { return Setting.Read("Notify Successful Login"); }
        }

        public static string NotifyFailedLogin
        {
            get { return Setting.Read("Notify Failed Login"); }
        }

        public static string NotifyTaskStarted
        {
            get { return Setting.Read("Notify Task Started"); }
        }

        public static string NotifyTaskCompleted
        {
            get { return Setting.Read("Notify Task Completed"); }
        }

        public static string NotifyImageApproved
        {
            get { return Setting.Read("Notify Image Approved"); }
        }

        public static string NotifyResizeFailed
        {
            get { return Setting.Read("Notify Resize Failed"); }
        }
    
    }
}