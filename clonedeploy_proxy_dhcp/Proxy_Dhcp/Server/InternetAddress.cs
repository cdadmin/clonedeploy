using System;
using System.Net;

namespace CloneDeploy_Proxy_Dhcp.Server
{
    public class InternetAddress : IComparable, IEquatable<InternetAddress>
    {
        public static readonly InternetAddress Empty = new InternetAddress(0, 0, 0, 0);
        public static readonly InternetAddress Broadcast = new InternetAddress(255, 255, 255, 255);

        private readonly byte[] _mAddress = {0, 0, 0, 0};

        public InternetAddress(params byte[] address)
        {
            if (address == null || address.Length != 4)
            {
                throw new ArgumentException("Address must have a length of 4.", "address");
            }

            address.CopyTo(_mAddress, 0);
        }

        public byte this[int index]
        {
            get { return _mAddress[index]; }
        }

        public bool IsEmpty
        {
            get { return Equals(Empty); }
        }

        public bool IsBroadcast
        {
            get { return Equals(Broadcast); }
        }

        internal InternetAddress NextAddress()
        {
            var next = Copy();

            if (_mAddress[3] == 255)
            {
                next._mAddress[3] = 0;

                if (_mAddress[2] == 255)
                {
                    next._mAddress[2] = 0;

                    if (_mAddress[1] == 255)
                    {
                        next._mAddress[1] = 0;

                        if (_mAddress[0] == 255)
                        {
                            throw new InvalidOperationException();
                        }
                        next._mAddress[0] = (byte) (_mAddress[0] + 1);
                    }
                    else
                    {
                        next._mAddress[1] = (byte) (_mAddress[1] + 1);
                    }
                }
                else
                {
                    next._mAddress[2] = (byte) (_mAddress[2] + 1);
                }
            }
            else
            {
                next._mAddress[3] = (byte) (_mAddress[3] + 1);
            }

            return next;
        }

        public int CompareTo(object obj)
        {
            var other = obj as InternetAddress;

            if (other == null)
            {
                return 1;
            }

            for (var i = 0; i < 4; i++)
            {
                if (_mAddress[i] > other._mAddress[i])
                {
                    return 1;
                }
                if (_mAddress[i] < other._mAddress[i])
                {
                    return -1;
                }
            }

            return 0;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as InternetAddress);
        }

        public bool Equals(InternetAddress other)
        {
            return other != null &&
                   _mAddress[0] == other._mAddress[0] &&
                   _mAddress[1] == other._mAddress[1] &&
                   _mAddress[2] == other._mAddress[2] &&
                   _mAddress[3] == other._mAddress[3];
        }

        public override int GetHashCode()
        {
            return BitConverter.ToInt32(_mAddress, 0);
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", this[0], this[1], this[2], this[3]);
        }

        public InternetAddress Copy()
        {
            return new InternetAddress(_mAddress[0], _mAddress[1], _mAddress[2], _mAddress[3]);
        }

        public byte[] ToArray()
        {
            var array = new byte[4];
            _mAddress.CopyTo(array, 0);
            return array;
        }

        public static InternetAddress Parse(string address)
        {
            return new InternetAddress(IPAddress.Parse(address).GetAddressBytes());
        }
    }
}
