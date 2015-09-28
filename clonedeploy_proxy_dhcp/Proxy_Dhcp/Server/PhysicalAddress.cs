using System;
using System.Security.Cryptography;

namespace CloneDeploy_Proxy_Dhcp.Server
{
    public class PhysicalAddress : IComparable, IEquatable<PhysicalAddress>
    {
        private readonly byte[] _mAddress = {0, 0, 0, 0, 0, 0};

        public byte this[int index]
        {
            get { return _mAddress[index]; }
        }

        public PhysicalAddress(params byte[] address)
        {
            if (address == null || address.Length != 6)
            {
                throw new ArgumentException("Address must have a length of 6.", "address");
            }

            address.CopyTo(_mAddress, 0);
        }

        public int CompareTo(object obj)
        {
            var other = obj as PhysicalAddress;

            if (other == null)
            {
                return 1;
            }

            for (var i = 0; i < 6; i++)
            {
                if (_mAddress[i] > other._mAddress[i])
                {
                    return -1;
                }
                if (_mAddress[i] < other._mAddress[i])
                {
                    return 1;
                }
            }

            return 0;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PhysicalAddress);
        }

        public bool Equals(PhysicalAddress other)
        {
            return other != null &&
                   _mAddress[0] == other._mAddress[0] &&
                   _mAddress[1] == other._mAddress[1] &&
                   _mAddress[2] == other._mAddress[2] &&
                   _mAddress[3] == other._mAddress[3] &&
                   _mAddress[4] == other._mAddress[4] &&
                   _mAddress[5] == other._mAddress[5];
        }

        public override int GetHashCode()
        {
            var hashProvider = MD5.Create();
            return BitConverter.ToInt32(hashProvider.ComputeHash(_mAddress), 0);
        }

        public InternetAddress Copy()
        {
            return new InternetAddress(_mAddress[0], _mAddress[1], _mAddress[2], _mAddress[3], _mAddress[4],
                _mAddress[5]);
        }

        public byte[] ToArray()
        {
            var array = new byte[6];
            _mAddress.CopyTo(array, 0);
            return array;
        }

        public static PhysicalAddress Parse(string address)
        {
            var physical = new PhysicalAddress(0, 0, 0, 0, 0, 0);

            var index = 0;
            byte currentValue = 0;
            var first = true;

            foreach (var c in address)
            {
                byte digitValue;

                switch (c)
                {
                    case '0':
                        digitValue = 0;
                        break;
                    case '1':
                        digitValue = 1;
                        break;
                    case '2':
                        digitValue = 2;
                        break;
                    case '3':
                        digitValue = 3;
                        break;
                    case '4':
                        digitValue = 4;
                        break;
                    case '5':
                        digitValue = 5;
                        break;
                    case '6':
                        digitValue = 6;
                        break;
                    case '7':
                        digitValue = 7;
                        break;
                    case '8':
                        digitValue = 8;
                        break;
                    case '9':
                        digitValue = 9;
                        break;
                    case 'A':
                    case 'a':
                        digitValue = 10;
                        break;
                    case 'B':
                    case 'b':
                        digitValue = 11;
                        break;
                    case 'C':
                    case 'c':
                        digitValue = 12;
                        break;
                    case 'D':
                    case 'd':
                        digitValue = 13;
                        break;
                    case 'E':
                    case 'e':
                        digitValue = 14;
                        break;
                    case 'F':
                    case 'f':
                        digitValue = 15;
                        break;
                    default:
                        continue;
                }

                if (first)
                {
                    currentValue = (byte) (digitValue << 4);
                    first = false;
                }
                else
                {
                    currentValue |= digitValue;
                    physical._mAddress[index++] = currentValue;
                    first = true;
                }
            }

            return physical;
        }
    }
}
