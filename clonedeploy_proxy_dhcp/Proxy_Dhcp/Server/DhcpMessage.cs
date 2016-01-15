using System;
using System.Collections.Generic;
using System.Net;

namespace CloneDeploy_Proxy_Dhcp.Server
{
    internal class DhcpData
    {
        private int _mBufferSize;

        public IPEndPoint Source { get; set; }

        public byte[] MessageBuffer { get; private set;
            // set { this.m_MessageBuffer = value; }
        }

        public int BufferSize
        {
            get { return _mBufferSize; }

            set
            {
                _mBufferSize = value;

                var oldBuffer = MessageBuffer;
                MessageBuffer = new byte[_mBufferSize];

                var copyLen = Math.Min(oldBuffer.Length, _mBufferSize);
                Array.Copy(oldBuffer, MessageBuffer, copyLen);
            }
        }

        public IAsyncResult Result { get; set; }

        public DhcpData(byte[] messageBuffer)
        {
            MessageBuffer = messageBuffer;
            _mBufferSize = messageBuffer.Length;
        }

        public DhcpData(IPEndPoint source, byte[] messageBuffer)
        {
            Source = source;
            MessageBuffer = messageBuffer;
            _mBufferSize = messageBuffer.Length;
        }
    }

    public enum DhcpOperation : byte
    {
        BootRequest = 0x01,
        BootReply
    }

    public enum HardwareType : byte
    {
        Ethernet = 0x01,
        ExperimentalEthernet,
        AmateurRadio,
        ProteonTokenRing,
        Chaos,
        IEEE802Networks,
        ArcNet,
        Hyperchnnel,
        Lanstar
    }

    public enum DhcpMessageType
    {
        Discover = 0x01,
        Offer,
        Request,
        Decline,
        Ack,
        Nak,
        Release,
        Inform,
        ForceRenew,
        LeaseQuery,
        LeaseUnassigned,
        LeaseUnknown,
        LeaseActive
    }

    public enum DhcpOption : byte
    {
        Pad = 0x00,
        SubnetMask = 0x01,
        TimeOffset = 0x02,
        Router = 0x03,
        TimeServer = 0x04,
        NameServer = 0x05,
        DomainNameServer = 0x06,
        Hostname = 0x0C,
        DomainNameSuffix = 0x0F,
        VendorSpecificInformation = 0x2B,
        RootPath = 0x11,
        AddressRequest = 0x32,
        AddressTime = 0x33,
        DhcpMessageType = 0x35,
        DhcpAddress = 0x36,
        ParameterList = 0x37,
        DhcpMessage = 0x38,
        DhcpMaxMessageSize = 0x39,
        ClassId = 0x3C,
        ClientId = 0x3D,
        TftpServer = 0x42,
        AutoConfig = 0x74,
        ClientSystemArch = 0x5D,
        PxePath = 0xD2,
        PathPrefix = 0xD2,
        End = 0xFF
    }

    public class DhcpMessage
    {
        private const uint DhcpOptionsMagicNumber = 1669485411;
        private const uint WinDhcpOptionsMagicNumber = 1666417251;
        private const int DhcpMinimumMessageSize = 236;

        private DhcpOperation _mOperation = DhcpOperation.BootRequest;
        private HardwareType _mHardware = HardwareType.Ethernet;
        private readonly byte[] _mClientAddress = new byte[4];
        private readonly byte[] _mAssignedAddress = new byte[4];
        private readonly byte[] _mNextServerAddress = new byte[4];
        private readonly byte[] _mRelayAgentAddress = new byte[4];
        private readonly byte[] _mClientHardwareAddress = new byte[16];
        private readonly byte[] _mServerHostName = new byte[64];
        private readonly byte[] _mBootFileName = new byte[128];
        private readonly byte[] _mVendorSpecificInformation = new byte[64];
        private byte[] _mOptionOrdering = {};

        private int _mOptionDataSize;
        private readonly Dictionary<DhcpOption, byte[]> _mOptions = new Dictionary<DhcpOption, byte[]>();

        public DhcpMessage()
        {
        }

        internal DhcpMessage(DhcpData data)
            : this(data.MessageBuffer)
        {
        }

        public DhcpMessage(byte[] data)
        {
            var offset = 0;
            _mOperation = (DhcpOperation) data[offset++];
            _mHardware = (HardwareType) data[offset++];
            HardwareAddressLength = data[offset++];
            Hops = data[offset++];

            SessionId = BitConverter.ToInt32(data, offset);
            offset += 4;

            var secondsElapsed = new byte[2];
            Array.Copy(data, offset, secondsElapsed, 0, 2);
            SecondsElapsed = BitConverter.ToUInt16(ReverseByteOrder(secondsElapsed), 0);
            offset += 2;

            Flags = BitConverter.ToUInt16(data, offset);
            offset += 2;

            Array.Copy(data, offset, _mClientAddress, 0, 4);
            offset += 4;
            Array.Copy(data, offset, _mAssignedAddress, 0, 4);
            offset += 4;
            Array.Copy(data, offset, _mNextServerAddress, 0, 4);
            offset += 4;
            Array.Copy(data, offset, _mRelayAgentAddress, 0, 4);
            offset += 4;
            Array.Copy(data, offset, _mClientHardwareAddress, 0, 16);
            offset += 16;
            Array.Copy(data, offset, _mServerHostName, 0, 64);
            offset += 64;
            Array.Copy(data, offset, _mBootFileName, 0, 128);
            offset += 128;
            //Array.Copy(data, offset, this.m_VendorSpecificInformation, 0, 64);
            //offset += 64;
            //offset += 192; // Skip server host name and boot file

            if (offset + 4 < data.Length &&
                (BitConverter.ToUInt32(data, offset) == DhcpOptionsMagicNumber ||
                 BitConverter.ToUInt32(data, offset) == WinDhcpOptionsMagicNumber))
            {
                offset += 4;
                var end = false;

                while (!end && offset < data.Length)
                {
                    var option = (DhcpOption) data[offset];
                    offset++;

                    int optionLen;

                    switch (option)
                    {
                        case DhcpOption.Pad:
                            continue;
                        case DhcpOption.End:
                            end = true;
                            continue;
                        default:
                            optionLen = data[offset];
                            offset++;
                            break;
                    }

                    var optionData = new byte[optionLen];

                    Array.Copy(data, offset, optionData, 0, optionLen);
                    offset += optionLen;

                    _mOptions.Add(option, optionData);
                    _mOptionDataSize += optionLen;
                }
            }
        }

        public string ClientArchitecture { get; set; }
        public string SourcePort { get; set; }

        public DhcpOperation Operation
        {
            get { return _mOperation; }
            set { _mOperation = value; }
        }

        public HardwareType Hardware
        {
            get { return _mHardware; }
            set { _mHardware = value; }
        }

        public byte HardwareAddressLength { get; set; }

        public byte Hops { get; set; }

        public int SessionId { get; set; }

        public ushort SecondsElapsed { get; set; }

        public ushort Flags { get; set; }

        public byte[] ClientAddress
        {
            get { return _mClientAddress; }
            set { CopyData(value, _mClientAddress); }
        }

        public byte[] AssignedAddress
        {
            get { return _mAssignedAddress; }
            set { CopyData(value, _mAssignedAddress); }
        }

        public byte[] NextServerAddress
        {
            get { return _mNextServerAddress; }
            set { CopyData(value, _mNextServerAddress); }
        }

        public byte[] RelayAgentAddress
        {
            get { return _mRelayAgentAddress; }
            set { CopyData(value, _mRelayAgentAddress); }
        }

        public byte[] ServerHostName
        {
            get { return _mServerHostName; }
            set { CopyData(value, _mServerHostName); }
        }

        public byte[] BootFileName
        {
            get { return _mBootFileName; }
            set { CopyData(value, _mBootFileName); }
        }

        public byte[] VendorSpecificInformation
        {
            get { return _mVendorSpecificInformation; }
            set { CopyData(value, _mVendorSpecificInformation); }
        }

        public byte[] ClientHardwareAddress
        {
            get
            {
                var hardwareAddress = new byte[HardwareAddressLength];
                Array.Copy(_mClientHardwareAddress, hardwareAddress, HardwareAddressLength);
                return hardwareAddress;
            }

            set
            {
                CopyData(value, _mClientHardwareAddress);
                HardwareAddressLength = (byte) value.Length;
                for (var i = value.Length; i < 16; i++)
                {
                    _mClientHardwareAddress[i] = 0;
                }
            }
        }

        public byte[] OptionOrdering
        {
            get { return _mOptionOrdering; }
            set { _mOptionOrdering = value; }
        }

        public byte[] GetOptionData(DhcpOption option)
        {
            if (_mOptions.ContainsKey(option))
            {
                return _mOptions[option];
            }
            return null;
        }

        public void AddOption(DhcpOption option, params byte[] data)
        {
            if (option == DhcpOption.Pad || option == DhcpOption.End)
            {
                throw new ArgumentException("The Pad and End DhcpOptions cannot be added explicitly.", "option");
            }

            _mOptions.Add(option, data);
            _mOptionDataSize += data.Length;
        }

        public bool RemoveOption(DhcpOption option)
        {
            if (_mOptions.ContainsKey(option))
            {
                _mOptionDataSize -= _mOptions[option].Length;
            }

            return _mOptions.Remove(option);
        }

        public void ClearOptions()
        {
            _mOptionDataSize = 0;
            _mOptions.Clear();
        }

        public byte[] ToArray()
        {
            var data =
                new byte[
                    DhcpMinimumMessageSize + (_mOptions.Count > 0 ? 4 + _mOptions.Count*2 + _mOptionDataSize + 1 : 0)];

            var offset = 0;

            data[offset++] = (byte) _mOperation;
            data[offset++] = (byte) _mHardware;
            data[offset++] = HardwareAddressLength;
            data[offset++] = Hops;

            BitConverter.GetBytes(SessionId).CopyTo(data, offset);
            offset += 4;

            ReverseByteOrder(BitConverter.GetBytes(SecondsElapsed)).CopyTo(data, offset);
            offset += 2;

            BitConverter.GetBytes(Flags).CopyTo(data, offset);
            offset += 2;

            _mClientAddress.CopyTo(data, offset);
            offset += 4;

            _mAssignedAddress.CopyTo(data, offset);
            offset += 4;

            _mNextServerAddress.CopyTo(data, offset);
            offset += 4;

            _mRelayAgentAddress.CopyTo(data, offset);
            offset += 4;

            _mClientHardwareAddress.CopyTo(data, offset);
            offset += 16;

            _mServerHostName.CopyTo(data, offset);
            offset += 64;

            _mBootFileName.CopyTo(data, offset);
            offset += 128;

            // this.m_VendorSpecificInformation.CopyTo(data, offset);
            //offset += 64;

            //offset += 192;

            if (_mOptions.Count > 0)
            {
                BitConverter.GetBytes(WinDhcpOptionsMagicNumber).CopyTo(data, offset);
                offset += 4;

                foreach (var optionId in _mOptionOrdering)
                {
                    var option = (DhcpOption) optionId;
                    if (_mOptions.ContainsKey(option))
                    {
                        data[offset++] = optionId;
                        if (_mOptions[option] != null && _mOptions[option].Length > 0)
                        {
                            data[offset++] = (byte) _mOptions[option].Length;
                            Array.Copy(_mOptions[option], 0, data, offset, _mOptions[option].Length);
                            offset += _mOptions[option].Length;
                        }
                    }
                }

                foreach (var option in _mOptions.Keys)
                {
                    if (Array.IndexOf(_mOptionOrdering, (byte) option) == -1)
                    {
                        data[offset++] = (byte) option;
                        if (_mOptions[option] != null && _mOptions[option].Length > 0)
                        {
                            data[offset++] = (byte) _mOptions[option].Length;
                            Array.Copy(_mOptions[option], 0, data, offset, _mOptions[option].Length);
                            offset += _mOptions[option].Length;
                        }
                    }
                }

                data[offset] = (byte) DhcpOption.End;
            }

            return data;
        }

        private void CopyData(byte[] source, byte[] dest)
        {
            if (source.Length > dest.Length)
            {
                throw new ArgumentException(string.Format("Values must be no more than {0} bytes.", dest.Length),
                    "value");
            }

            source.CopyTo(dest, 0);
        }

        public static byte[] ReverseByteOrder(byte[] source)
        {
            var reverse = new byte[source.Length];

            var j = 0;
            for (var i = source.Length - 1; i >= 0; i--)
            {
                reverse[j++] = source[i];
            }

            return reverse;
        }
    }
}
