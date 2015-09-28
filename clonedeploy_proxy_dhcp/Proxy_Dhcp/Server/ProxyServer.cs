using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using PhysicalAddress = CloneDeploy_Proxy_Dhcp.Server.PhysicalAddress;

namespace CloneDeploy_Proxy_Dhcp.Server
{
    public class ProxyServer
    {
        private const Int32 DhcpPort = 4011;
        private const Int32 DhcpClientProxyPort = 4011;
        private const Int32 DhcpClientPort = 68;
        private const Int32 DhcpMessageMaxSize = 1024;

        private NetworkInterface m_DhcpInterface;
        private IPAddress m_DhcpInterfaceAddress;

        private SortedList<PhysicalAddress, Boolean> m_Acl = new SortedList<PhysicalAddress, Boolean>();
        private Boolean m_AllowAny = true;
        private Dictionary<PhysicalAddress, ReservationOptions> m_Reservations = new Dictionary<PhysicalAddress, ReservationOptions>();

        private ReaderWriterLock m_AclLock = new ReaderWriterLock();
        private ReaderWriterLock m_AbortLock = new ReaderWriterLock();
        private Socket m_DhcpSocket;
        private Boolean m_Abort = false;

        public String UserNextServer { get; set; }
        public String UserNetworkInterface { get; set; }
        public String BiosBootFile { get; set; }
        public String Efi32BootFile { get; set; }
        public string Efi64BootFile { get; set; }

        public NetworkInterface DhcpInterface
        {
            get { return this.m_DhcpInterface; }
            set { this.m_DhcpInterface = value; }
        }

        public Boolean AllowAny
        {
            get { return this.m_AllowAny; }
            set { this.m_AllowAny = value; }
        }

        public struct ReservationOptions
        {
            public string ReserveNextServer;
            public string ReserveBootFile;
        }

        public Dictionary<PhysicalAddress, ReservationOptions> Reservations
        {
            get { return this.m_Reservations; }
        }

        public ProxyServer()
        {
        }

        public void AddAcl(PhysicalAddress address, Boolean deny)
        {
            this.m_AclLock.AcquireWriterLock(-1);

            try
            {
                if (this.m_Acl.ContainsKey(address))
                {
                    this.m_Acl[address] = !deny;
                }
                else
                {
                    this.m_Acl.Add(address, !deny);
                }
            }
            finally
            {
                this.m_AclLock.ReleaseLock();
            }
        }

        public void RemoveAcl(PhysicalAddress address)
        {
            this.m_AclLock.AcquireWriterLock(-1);

            try
            {
                if (this.m_Acl.ContainsKey(address))
                {
                    this.m_Acl.Remove(address);
                }
            }
            finally
            {
                this.m_AclLock.ReleaseLock();
            }
        }

        public void ClearAcls()
        {
            this.m_AclLock.AcquireWriterLock(-1);

            try
            {
                this.m_Acl.Clear();
            }
            finally
            {
                this.m_AclLock.ReleaseLock();
            }
        }

        public void Start()
        {

            Trace.TraceInformation("Dhcp Server Starting...");

            if (this.m_DhcpInterface == null)
            {
                Trace.TraceInformation("Enumerating Network Interfaces.");
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    {
                        this.m_DhcpInterface = nic;
                    }
                    else if ((nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet || nic.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet || nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                    {
                        if (!string.IsNullOrEmpty(UserNetworkInterface))
                        {
                            foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    if (ip.Address.ToString() == UserNetworkInterface)
                                    {
                                        this.m_DhcpInterface = nic;
                                        Trace.TraceInformation("Using Network Interface \"{0}\".", nic.Name);
                                        break;
                                    }
                                }
                            }
                            if (this.m_DhcpInterface != null)
                                break;
                        }
                        else
                        {
                            this.m_DhcpInterface = nic;
                            Trace.TraceInformation("Using Network Interface \"{0}\".", nic.Name);
                            break;
                        }
                    }
                }

#if TRACE
                if (this.m_DhcpInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    Trace.TraceInformation("Active Ethernet Network Interface Not Found. Using Loopback.");
                }
#endif
            }
            
            foreach (UnicastIPAddressInformation interfaceAddress in this.m_DhcpInterface.GetIPProperties().UnicastAddresses)
            {
                if (interfaceAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    this.m_DhcpInterfaceAddress = interfaceAddress.Address;
                }
            }

            if (this.m_DhcpInterfaceAddress == null)
            {
                Trace.TraceError("Unabled to Set Dhcp Interface Address. Check the networkInterface property of your config file.");
                throw new InvalidOperationException("Unabled to Set Dhcp Interface Address.");
            }

            this.m_Abort = false;

            this.m_DhcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.m_DhcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            this.m_DhcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            m_DhcpSocket.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);   

            try
            {
                this.m_DhcpSocket.Bind(new IPEndPoint(this.m_DhcpInterfaceAddress, DhcpPort));
            }
            catch
            {
                Trace.TraceError("Could Not Bind This Interface To Port 4011.  It May Already Be In Use.");
                return;
            }
            
            this.Listen();

            Trace.TraceInformation("Dhcp Service Running On " + this.m_DhcpInterfaceAddress.ToString() + ":4011");

        }

        public void Stop()
        {
            this.m_AbortLock.AcquireWriterLock(-1);

            try
            {
                this.m_Abort = true;
                this.m_DhcpSocket.Close();
                this.m_DhcpSocket = null;
            }
            finally
            {
                this.m_AbortLock.ReleaseLock();
            }
        }

        private void Listen()
        {
            Byte[] messageBufer = new Byte[DhcpMessageMaxSize];
            EndPoint source = new IPEndPoint(0, 0);

            this.m_AbortLock.AcquireReaderLock(-1);

            try
            {
                if (this.m_Abort)
                {
                    return;
                }

                var tmp = ((IPEndPoint)source).Port;
                Trace.TraceInformation("Listening For Dhcp Request.");
                //this.m_DhcpSocket.ReceiveFrom(messageBufer, ref source);
                this.m_DhcpSocket.BeginReceiveFrom(messageBufer, 0, DhcpMessageMaxSize, SocketFlags.None, ref source, new AsyncCallback(this.OnReceive), messageBufer);
               
            }
            finally
            {
                this.m_AbortLock.ReleaseLock();
            }
        }

       

        private void OnReceive(IAsyncResult result )
        {
            
            DhcpData data = new DhcpData((Byte[])result.AsyncState);
            data.Result = result;
            if (!this.m_Abort)
            {
                Trace.TraceInformation("Dhcp Messages Received, Queued for Processing.");

                // Queue this request for processing
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.CompleteRequest), data);

                this.Listen();
            }
        }

        private void CompleteRequest(Object state)
        {
            DhcpData messageData = (DhcpData)state;
            EndPoint source = new IPEndPoint(0, 0);

            this.m_AbortLock.AcquireReaderLock(-1);

            try
            {
                if (this.m_Abort)
                {
                    return;
                }

                messageData.BufferSize = this.m_DhcpSocket.EndReceiveFrom(messageData.Result, ref source);
                messageData.Source = (IPEndPoint)source;
            }
            catch (SocketException)
            {
                return;
            }
            catch (Exception ex)
            {
                TraceException("Error", ex);
                return;
            }

            finally
            {
                this.m_AbortLock.ReleaseLock();
            }

            DhcpMessage message;

            try
            {
                message = new DhcpMessage(messageData);
                message.SourcePort = messageData.Source.Port.ToString();
            }
            catch (ArgumentException ex)
            {
                TraceException("Error Parsing Dhcp Message", ex);
                return;
            }
            catch (InvalidCastException ex)
            {
                TraceException("Error Parsing Dhcp Message", ex);
                return;
            }
            catch (IndexOutOfRangeException ex)
            {
                TraceException("Error Parsing Dhcp Message", ex);
                return;
            }
            catch (Exception ex)
            {
                TraceException("Error Parsing Dhcp Message", ex);
                return;
            }

            if (message.Operation == DhcpOperation.BootRequest)
            {
                Byte[] messageVendorIDData = message.GetOptionData(DhcpOption.ClassId);
                if (messageVendorIDData != null)
                {
                    var str = System.Text.Encoding.Default.GetString(messageVendorIDData);
                    if (!str.Contains("PXEClient"))
                        return;
                    else
                    {
                        string[] values = str.Split(':');
                        if (values.Length >= 3)
                        {
                            message.ClientArchitecture = values[2];
                            if(message.ClientArchitecture != "00000" && message.ClientArchitecture != "00006" && message.ClientArchitecture!= "00007" && message.ClientArchitecture!="00009")
                                return;
                        }
                    }
                }
                else
                    return;

                Byte[] messageTypeData = message.GetOptionData(DhcpOption.DhcpMessageType);

                if (messageTypeData != null && messageTypeData.Length == 1)
                {
                    DhcpMessageType messageType = (DhcpMessageType)messageTypeData[0];

                    switch (messageType)
                    {
                      
                        case DhcpMessageType.Request:
                            Trace.TraceInformation("{0} Dhcp REQUEST Message Received.", Thread.CurrentThread.ManagedThreadId);
                            this.DhcpRequest(message);
                            Trace.TraceInformation("{0} Dhcp REQUEST Message Processed.", Thread.CurrentThread.ManagedThreadId);
                            break;
                        default:
                            Trace.TraceWarning("Ignoring ({0}) Message On Port 4011.", messageType.ToString());
                            break;
                    }
                }
                else
                {
                    Trace.TraceWarning("Unknown Dhcp Data Received, Ignoring.");
                }
            }
        }

        private static void TraceException(String prefix, Exception ex)
        {
            Trace.TraceError("{0}: ({1}) - {2}\r\n{3}", prefix, ex.GetType().Name, ex.Message, ex.StackTrace);

            if (ex.InnerException != null)
            {
                TraceException("    Inner Exception", ex.InnerException);
            }
        }

        private void DhcpRequest(DhcpMessage message)
        {
            Byte[] addressRequestData = message.GetOptionData(DhcpOption.AddressRequest);
            if (addressRequestData == null)
            {
                addressRequestData = message.ClientAddress;
            }

            InternetAddress addressRequest = new InternetAddress(addressRequestData);

            if (addressRequest.IsEmpty)
            {
                this.SendNak(message);
                return;
            }
            
            // Assume we're on an ethernet network
            Byte[] hardwareAddressData = new Byte[6];
            Array.Copy(message.ClientHardwareAddress, hardwareAddressData, 6);
            PhysicalAddress clientHardwareAddress = new PhysicalAddress(hardwareAddressData);
            
            // If this client is explicitly allowed, or they are not denied and the allow any flag is set
            if (this.m_Acl.ContainsKey(clientHardwareAddress) && this.m_Acl[clientHardwareAddress] ||
                !this.m_Acl.ContainsKey(clientHardwareAddress) && this.m_AllowAny)
            {
                this.SendAck(message);
            }
            else
            {
                this.SendNak(message);
            }
            
        }

        private void SendAck(DhcpMessage message)
        {
            Trace.TraceInformation("{0} Sending Dhcp Acknowledge.", Thread.CurrentThread.ManagedThreadId);

            DhcpMessage response = new DhcpMessage();
            response.Operation = DhcpOperation.BootReply;
            response.Hardware = HardwareType.Ethernet;
            response.HardwareAddressLength = 6;
            response.SecondsElapsed = message.SecondsElapsed;
            response.SessionId = message.SessionId;

            Byte[] hardwareAddressData = new Byte[6];
            Array.Copy(message.ClientHardwareAddress, hardwareAddressData, 6);
            PhysicalAddress clientHardwareAddress = new PhysicalAddress(hardwareAddressData);

            if (this.m_Reservations.ContainsKey(clientHardwareAddress))
            {
                response.NextServerAddress = IPAddress.Parse(this.m_Reservations[clientHardwareAddress].ReserveNextServer).GetAddressBytes();
                response.BootFileName = Encoding.UTF8.GetBytes(this.m_Reservations[clientHardwareAddress].ReserveBootFile);
            }
            else
            {
                if (!string.IsNullOrEmpty(UserNextServer))
                    response.NextServerAddress = IPAddress.Parse(UserNextServer).GetAddressBytes();
                else
                    response.NextServerAddress = this.m_DhcpInterfaceAddress.GetAddressBytes();

                switch (message.ClientArchitecture)
                {

                    case "00000":
                        response.BootFileName = Encoding.UTF8.GetBytes(BiosBootFile);
                        
                        break;
                    case "00006":
                        response.BootFileName = Encoding.UTF8.GetBytes(Efi32BootFile);
                        break;
                    case "00007":
                        response.BootFileName = Encoding.UTF8.GetBytes(Efi64BootFile);
                        break;
                    case "00009":
                        response.BootFileName = Encoding.UTF8.GetBytes(Efi64BootFile);
                        
                        break;
                    default:
                        response.BootFileName = Encoding.UTF8.GetBytes(BiosBootFile);
                        break;
                }

            }
            
            response.ClientAddress = message.ClientAddress;
            response.ClientHardwareAddress = message.ClientHardwareAddress;
            response.AddOption(DhcpOption.DhcpMessageType, (Byte)DhcpMessageType.Ack);
            response.SourcePort = message.SourcePort;
            this.SendReply(response,true);
            Trace.TraceInformation("{0} Dhcp Acknowledge Sent.", Thread.CurrentThread.ManagedThreadId);
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private void SendNak(DhcpMessage message)
        {
            Trace.TraceInformation("{0} Sending Dhcp Negative Acknowledge.", Thread.CurrentThread.ManagedThreadId);

            DhcpMessage response = new DhcpMessage();
            response.Operation = DhcpOperation.BootReply;
            response.Hardware = HardwareType.Ethernet;
            response.HardwareAddressLength = 6;
            response.SecondsElapsed = message.SecondsElapsed;
            response.SessionId = message.SessionId;

            response.ClientHardwareAddress = message.ClientHardwareAddress;

            response.AddOption(DhcpOption.DhcpMessageType, (Byte)DhcpMessageType.Nak);

            this.SendReply(response,false);
            Trace.TraceInformation("{0} Dhcp Negative Acknowledge Sent.", Thread.CurrentThread.ManagedThreadId);
        }

        private void SendReply(DhcpMessage response,bool success)
        {

            response.AddOption(DhcpOption.DhcpAddress, this.m_DhcpInterfaceAddress.GetAddressBytes());
            
            Byte[] sessionId = BitConverter.GetBytes(response.SessionId);

            if (success)
            {
                try
                {
                    if (response.SourcePort == "4011")
                    {
                        string clientIP = response.ClientAddress[0] + "." + response.ClientAddress[1] + "." + response.ClientAddress[2] + "." + response.ClientAddress[3];
                        this.m_DhcpSocket.SendTo(response.ToArray(), new IPEndPoint(IPAddress.Parse(clientIP), DhcpClientProxyPort));
                    }
                    else
                    {
                        string clientIP = response.ClientAddress[0] + "." + response.ClientAddress[1] + "." + response.ClientAddress[2] + "." + response.ClientAddress[3];
                        this.m_DhcpSocket.SendTo(response.ToArray(), new IPEndPoint(IPAddress.Parse(clientIP), DhcpClientPort));
                    }
                }
                catch (Exception ex)
                {
                    TraceException("Error Sending Dhcp Reply", ex);
                    return;
                }
            }
            else
            {
                try
                {
                    this.m_DhcpSocket.SendTo(response.ToArray(), new IPEndPoint(IPAddress.Broadcast, DhcpClientPort));
                }
                catch (Exception ex)
                {
                    TraceException("Error Sending Dhcp Reply", ex);
                    return;
                }
            }
        }
    }
}
