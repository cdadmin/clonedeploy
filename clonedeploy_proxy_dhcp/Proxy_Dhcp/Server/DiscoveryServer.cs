using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CloneDeploy_Proxy_Dhcp.Server
{
    public class DiscoveryServer
    {
        private const int DhcpPort = 67;
        private const int DhcpClientPort = 68;
        private const int DhcpMessageMaxSize = 1024;

        private IPAddress _mDhcpInterfaceAddress;

        private readonly SortedList<PhysicalAddress, bool> _mAcl = new SortedList<PhysicalAddress, bool>();
        private bool _mAllowAny = true;
        private readonly Dictionary<PhysicalAddress, string> _mReservations = new Dictionary<PhysicalAddress, string>();
        private Dictionary<PhysicalAddress, ReservationOptions> m_Reservations = new Dictionary<PhysicalAddress, ReservationOptions>();
        private readonly ReaderWriterLock _mAclLock = new ReaderWriterLock();
        private readonly ReaderWriterLock _mAbortLock = new ReaderWriterLock();
        private Socket _mDhcpSocket;
        private bool _mAbort;

        public string UserNextServer { get; set; }
        public string UserNetworkInterface { get; set; }
        public string RootPath { get; set; }
        public string AppleBootFile { get; set; }
        public string VendorInfo { get; set; }
        public string BsdpMode { get; set; }

        private readonly Dictionary<PhysicalAddress, ReservationOptions> _dReservations =
            new Dictionary<PhysicalAddress, ReservationOptions>();

        public Dictionary<PhysicalAddress, ReservationOptions> DReservations
        {
            get { return _dReservations; }
        }

        public NetworkInterface DhcpInterface { get; set; }

        public bool AllowAny
        {
            get { return _mAllowAny; }
            set { _mAllowAny = value; }
        }

        public Dictionary<PhysicalAddress, ReservationOptions> Reservations
        {
            get { return this.m_Reservations; }
        }

        public struct ReservationOptions
        {
            public string ReserveNextServer;
            public string ReserveBootFile;
        }

        public void AddAcl(PhysicalAddress address, bool deny)
        {
            _mAclLock.AcquireWriterLock(-1);

            try
            {
                if (_mAcl.ContainsKey(address))
                {
                    _mAcl[address] = !deny;
                }
                else
                {
                    _mAcl.Add(address, !deny);
                }
            }
            finally
            {
                _mAclLock.ReleaseLock();
            }
        }

        public void RemoveAcl(PhysicalAddress address)
        {
            _mAclLock.AcquireWriterLock(-1);

            try
            {
                if (_mAcl.ContainsKey(address))
                {
                    _mAcl.Remove(address);
                }
            }
            finally
            {
                _mAclLock.ReleaseLock();
            }
        }

        public void ClearAcls()
        {
            _mAclLock.AcquireWriterLock(-1);

            try
            {
                _mAcl.Clear();
            }
            finally
            {
                _mAclLock.ReleaseLock();
            }
        }

        public void Start()
        {
            Trace.TraceInformation("Dhcp Server Starting...");

            
            if (DhcpInterface == null)
            {
                Trace.TraceInformation("Enumerating Network Interfaces.");
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    {
                        DhcpInterface = nic;
                    }
                    else if ((nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                              nic.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet ||
                              nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                    {
                        if (!string.IsNullOrEmpty(UserNetworkInterface))
                        {
                            foreach (var ip in nic.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily != AddressFamily.InterNetwork) continue;
                                if (ip.Address.ToString() != UserNetworkInterface) continue;
                                DhcpInterface = nic;
                                Trace.TraceInformation("Using Network Interface \"{0}\".", nic.Name);
                                break;
                            }
                            if (DhcpInterface != null)
                                break;
                        }
                        else
                        {
                            DhcpInterface = nic;
                            Trace.TraceInformation("Using Network Interface \"{0}\".", nic.Name);
                            break;
                        }
                    }
                }

#if TRACE
                if (DhcpInterface != null && DhcpInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    Trace.TraceInformation("Active Ethernet Network Interface Not Found. Using Loopback.");
                }
#endif
            }

            if (DhcpInterface != null)
                foreach (var interfaceAddress in DhcpInterface.GetIPProperties().UnicastAddresses)
                {
                    if (interfaceAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        _mDhcpInterfaceAddress = interfaceAddress.Address;
                    }
                }

            if (_mDhcpInterfaceAddress == null)
            {
                Trace.TraceError(
                    "Unabled to Set Dhcp Interface Address. Check the networkInterface property of your config file.");
                throw new InvalidOperationException("Unabled to Set Dhcp Interface Address.");
            }
            
            _mAbort = false;

           
            _mDhcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);         
            _mDhcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);          
            _mDhcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

            if (!Environment.OSVersion.ToString().Contains("Unix"))
            {
                var IOC_IN = 0x80000000;
                uint IOC_VENDOR = 0x18000000;
                var SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                _mDhcpSocket.IOControl((int) SIO_UDP_CONNRESET, new[] {Convert.ToByte(false)}, null);
            }

            try
            {


                this._mDhcpSocket.Bind(Environment.OSVersion.ToString().Contains("Unix")
                    ? new IPEndPoint(IPAddress.Any, DhcpPort)
                    : new IPEndPoint(this._mDhcpInterfaceAddress, DhcpPort));
            }
            catch
            {
                Trace.TraceError("Could Not Bind This Interface To Port 67.  It May Already Be In Use.");
                return;
            }
            
            Listen();

            Trace.TraceInformation("Dhcp Service Running On " + _mDhcpInterfaceAddress + ":67");
        }

        public void Stop()
        {
            _mAbortLock.AcquireWriterLock(-1);

            try
            {
                _mAbort = true;
                _mDhcpSocket.Close();
                _mDhcpSocket = null;
            }
            finally
            {
                _mAbortLock.ReleaseLock();
            }
        }

        private void Listen()
        {
            var messageBufer = new byte[DhcpMessageMaxSize];
            EndPoint source = new IPEndPoint(0, 0);

            _mAbortLock.AcquireReaderLock(-1);

            try
            {
                if (_mAbort)
                {
                    return;
                }

                _mDhcpSocket.BeginReceiveFrom(messageBufer, 0, DhcpMessageMaxSize, SocketFlags.None, ref source,
                    OnReceive, messageBufer);
            }
            finally
            {
                _mAbortLock.ReleaseLock();
            }
        }


        private void OnReceive(IAsyncResult result)
        {
            var data = new DhcpData((byte[]) result.AsyncState) {Result = result};

            if (_mAbort) return;
            Trace.TraceInformation("Dhcp Messages Received, Queued for Processing.");
            ThreadPool.QueueUserWorkItem(CompleteRequest, data);

            Listen();
        }

        private void CompleteRequest(object state)
        {
            var messageData = (DhcpData) state;
            EndPoint source = new IPEndPoint(0, 0);

            _mAbortLock.AcquireReaderLock(-1);


            try
            {
                if (_mAbort)
                {
                    return;
                }

                messageData.BufferSize = _mDhcpSocket.EndReceiveFrom(messageData.Result, ref source);
                messageData.Source = (IPEndPoint) source;
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
                _mAbortLock.ReleaseLock();
            }

            DhcpMessage message;

            try
            {
                message = new DhcpMessage(messageData);
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
                var messageVendorIdData = message.GetOptionData(DhcpOption.ClassId);
                if (messageVendorIdData != null)
                {
                    var strVendorId = Encoding.Default.GetString(messageVendorIdData);
                    string bootType = null;
                    if (strVendorId.Contains("PXEClient"))
                        bootType = "pxe";
                    else if (strVendorId.Contains("AAPLBSDPC"))
                        bootType = "bsdp";
                    else
                    {
                        Trace.TraceInformation("{0} Ignoring, Not A Boot Request",
                            ByteArrayToString(message.ClientHardwareAddress));
                        return;
                    }

                    if (BsdpMode == "enabled" && bootType == "bsdp")
                    {
                        if (strVendorId.Length >= 14)
                        {

                            if (strVendorId.Substring(0, 14) == "AAPLBSDPC/i386")
                            {
                                var vendorOptions = message.GetOptionData(DhcpOption.VendorSpecificInformation);
                                if (vendorOptions != null)
                                {
                                    var strVendorInformation = ByteArrayToString(vendorOptions);
                                    if (strVendorInformation.Length >= 12)
                                    {
                                        if (strVendorInformation.Substring(0, 12) != "010101020201" &&
                                            strVendorInformation.Substring(0, 12) != "010102020201")
                                        {
                                            Trace.TraceInformation(
                                                "{0} Ignoring, Not An Apple BSDP Request, Vendor Information Mismatch",
                                                ByteArrayToString(message.ClientHardwareAddress));
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    Trace.TraceInformation("{0} Ignoring, No Vendor Information Data To Parse",
                                        ByteArrayToString(message.ClientHardwareAddress));
                                    return;
                                }
                            }
                            else
                            {
                                Trace.TraceInformation("{0} Ignoring, Not An Apple BSDP Request",
                                    ByteArrayToString(message.ClientHardwareAddress));
                                return;
                            }

                        }
                    }
                }
                else
                {
                    Trace.TraceInformation("{0} Ignoring, No Vendor ID Data To Parse",
                        ByteArrayToString(message.ClientHardwareAddress));
                    return;
                }



                var messageTypeData = message.GetOptionData(DhcpOption.DhcpMessageType);

                if (messageTypeData != null && messageTypeData.Length == 1)
                {
                    var messageType = (DhcpMessageType) messageTypeData[0];

                    switch (messageType)
                    {
                        case DhcpMessageType.Discover:
                            Trace.TraceInformation("{0} Dhcp DISCOVER Message Received.", Thread.CurrentThread.ManagedThreadId);
                            this.DhcpDiscover(message);
                            Trace.TraceInformation("{0} Dhcp DISCOVER Message Processed.", Thread.CurrentThread.ManagedThreadId);
                            break;

                        case DhcpMessageType.Inform:
                            Trace.TraceInformation("{0}, {1} Dhcp Inform Message Received.",
                                Thread.CurrentThread.ManagedThreadId, ByteArrayToString(message.ClientHardwareAddress));
                            Bsdp(message);
                            Trace.TraceInformation("{0} Dhcp Inform Message Processed.",
                                Thread.CurrentThread.ManagedThreadId);
                            break;
                        default:
                            Trace.TraceWarning("Ignoring ({0}) Message On Port 67.", messageType);
                            break;
                    }
                }
                else
                {
                    Trace.TraceWarning("Unknown Dhcp Data Received, Ignoring.");
                }
            }
            else
            {
                Trace.TraceInformation("Ignoring, Not a DHCP Boot Request.");
            }
        }

        private static void TraceException(string prefix, Exception ex)
        {
            Trace.TraceError("{0}: ({1}) - {2}\r\n{3}", prefix, ex.GetType().Name, ex.Message, ex.StackTrace);

            if (ex.InnerException != null)
            {
                TraceException("    Inner Exception", ex.InnerException);
            }
        }

        private void DhcpDiscover(DhcpMessage message)
        {
            Byte[] addressRequestData = message.GetOptionData(DhcpOption.AddressRequest);
            if (addressRequestData == null)
            {
                addressRequestData = message.ClientAddress;
            }

            InternetAddress addressRequest = new InternetAddress(addressRequestData);

            // Assume we're on an ethernet network
            Byte[] hardwareAddressData = new Byte[6];
            Array.Copy(message.ClientHardwareAddress, hardwareAddressData, 6);
            PhysicalAddress clientHardwareAddress = new PhysicalAddress(hardwareAddressData);


            // If this client is explicitly allowed, or they are not denied and the allow any flag is set
            if (this._mAcl.ContainsKey(clientHardwareAddress) && this._mAcl[clientHardwareAddress] ||
                !this._mAcl.ContainsKey(clientHardwareAddress) && this._mAllowAny)
            {
                this.SendOffer(message);
            }
            else
            {
                this.SendNak(message);
            }
        }

        private void Bsdp(DhcpMessage message)
        {
            // Assume we're on an ethernet network
            var hardwareAddressData = new byte[6];
            Array.Copy(message.ClientHardwareAddress, hardwareAddressData, 6);
            var clientHardwareAddress = new PhysicalAddress(hardwareAddressData);

            // If this client is explicitly allowed, or they are not denied and the allow any flag is set
            if (_mAcl.ContainsKey(clientHardwareAddress) && _mAcl[clientHardwareAddress] ||
                !_mAcl.ContainsKey(clientHardwareAddress) && _mAllowAny)
            {
                SendAck(message);
            }
            else
            {
                SendNak(message);
            }
        }

        private void SendOffer(DhcpMessage message)
        {
            Trace.TraceInformation("{0} Sending Dhcp Offer.", Thread.CurrentThread.ManagedThreadId);

            DhcpMessage response = new DhcpMessage();
            response.Operation = DhcpOperation.BootReply;
            response.Hardware = HardwareType.Ethernet;
            response.HardwareAddressLength = 6;
            response.SecondsElapsed = message.SecondsElapsed;
            response.SessionId = message.SessionId;
            response.Flags = message.Flags;

            Byte[] hardwareAddressData = new Byte[6];
            Array.Copy(message.ClientHardwareAddress, hardwareAddressData, 6);
            PhysicalAddress clientHardwareAddress = new PhysicalAddress(hardwareAddressData);

            response.NextServerAddress = this._mDhcpInterfaceAddress.GetAddressBytes();
            response.ClientHardwareAddress = message.ClientHardwareAddress;

            response.AddOption(DhcpOption.DhcpMessageType, (Byte)DhcpMessageType.Offer);
            response.AddOption(DhcpOption.ClassId, Encoding.UTF8.GetBytes("PXEClient"));

            Byte[] paramList = message.GetOptionData(DhcpOption.ParameterList);
            if (paramList != null)
            {
                response.OptionOrdering = paramList;
            }

            response.AddOption(DhcpOption.DhcpAddress, this._mDhcpInterfaceAddress.GetAddressBytes());

            try
            {
                this._mDhcpSocket.SendTo(response.ToArray(), new IPEndPoint(IPAddress.Broadcast, DhcpClientPort));
            }
            catch (Exception ex)
            {
                TraceException("Error Sending Dhcp Reply", ex);
                return;
            }

            Trace.TraceInformation("{0} Dhcp Offer Sent.", Thread.CurrentThread.ManagedThreadId);
        }

        private void SendAck(DhcpMessage message)
        {
            Trace.TraceInformation("{0} Sending Dhcp Acknowledge.", Thread.CurrentThread.ManagedThreadId);

            var response = new DhcpMessage
            {
                Operation = DhcpOperation.BootReply,
                Hardware = HardwareType.Ethernet,
                HardwareAddressLength = 6,
                SecondsElapsed = message.SecondsElapsed,
                SessionId = message.SessionId
            };

            var hardwareAddressData = new byte[6];
            Array.Copy(message.ClientHardwareAddress, hardwareAddressData, 6);
            var clientHardwareAddress = new PhysicalAddress(hardwareAddressData);


            if (_mReservations.ContainsKey(clientHardwareAddress))
            {
                response.NextServerAddress =
                    IPAddress.Parse(_dReservations[clientHardwareAddress].ReserveNextServer).GetAddressBytes();
                response.BootFileName = Encoding.UTF8.GetBytes(_dReservations[clientHardwareAddress].ReserveBootFile);
            }
            else
            {
                response.NextServerAddress = !string.IsNullOrEmpty(UserNextServer)
                    ? IPAddress.Parse(UserNextServer).GetAddressBytes()
                    : _mDhcpInterfaceAddress.GetAddressBytes();
                response.BootFileName = Encoding.UTF8.GetBytes(AppleBootFile);
            }

            response.ClientAddress = message.ClientAddress;
            response.ClientHardwareAddress = message.ClientHardwareAddress;
            response.AddOption(DhcpOption.DhcpMessageType, (byte) DhcpMessageType.Ack);
            response.AddOption(DhcpOption.ClassId, Encoding.UTF8.GetBytes("AAPLBSDPC"));
            response.AddOption(DhcpOption.VendorSpecificInformation, StringToByteArray(VendorInfo));
            response.AddOption(DhcpOption.RootPath, Encoding.UTF8.GetBytes(RootPath));
            response.SourcePort = message.SourcePort;
            response.AddOption(DhcpOption.DhcpAddress, _mDhcpInterfaceAddress.GetAddressBytes());

            try
            {
                var clientIp = response.ClientAddress[0] + "." + response.ClientAddress[1] + "." +
                               response.ClientAddress[2] + "." + response.ClientAddress[3];
                _mDhcpSocket.SendTo(response.ToArray(), new IPEndPoint(IPAddress.Parse(clientIp), DhcpClientPort));
            }
            catch (Exception ex)
            {
                TraceException("Error Sending Dhcp Reply", ex);
                return;
            }

            Trace.TraceInformation("{0} Dhcp Acknowledge Sent.", Thread.CurrentThread.ManagedThreadId);
        }

        private void SendNak(DhcpMessage message)
        {
            Trace.TraceInformation("{0} Sending Dhcp Negative Acknowledge.", Thread.CurrentThread.ManagedThreadId);

            var response = new DhcpMessage
            {
                Operation = DhcpOperation.BootReply,
                Hardware = HardwareType.Ethernet,
                HardwareAddressLength = 6,
                SecondsElapsed = message.SecondsElapsed,
                SessionId = message.SessionId,
                ClientHardwareAddress = message.ClientHardwareAddress
            };


            response.AddOption(DhcpOption.DhcpMessageType, (byte) DhcpMessageType.Nak);
            response.AddOption(DhcpOption.DhcpAddress, _mDhcpInterfaceAddress.GetAddressBytes());

            try
            {
                var clientIp = response.ClientAddress[0] + "." + response.ClientAddress[1] + "." +
                               response.ClientAddress[2] + "." + response.ClientAddress[3];
                _mDhcpSocket.SendTo(response.ToArray(), new IPEndPoint(IPAddress.Parse(clientIp), DhcpClientPort));
            }
            catch (Exception ex)
            {
                TraceException("Error Sending Dhcp Reply", ex);
                return;
            }
            Trace.TraceInformation("{0} Dhcp Negative Acknowledge Sent.", Thread.CurrentThread.ManagedThreadId);
        }


        public static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length*2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] StringToByteArray(string hex)
        {
            var hexNoColons = hex.Replace(":", "");
            return Enumerable.Range(0, hexNoColons.Length)
                .Where(x => x%2 == 0)
                .Select(x => Convert.ToByte(hexNoColons.Substring(x, 2), 16))
                .ToArray();
        }
    }
}
