using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Silmoon.Net.Sockets
{
    public class Sock
    {
        public static ushort getNetCheckSum(byte[] ipTcpHeader, int index, int offset)
        {
            int app = 0;

            byte[] IPHeader = new byte[20];
            Array.Copy(ipTcpHeader, IPHeader, 20);
            byte[] TCPHeader = null;

            if (offset % 2 != 0)
            {
                TCPHeader = new byte[offset - 19];
                Array.Copy(ipTcpHeader, 20, TCPHeader, 0, TCPHeader.Length - 1);
            }
            else
                TCPHeader = new byte[offset - 20];



            uint sum = 0;
            // TCP Header
            for (int x = 0; x < TCPHeader.Length; x += 2)
                sum += ntoh(BitConverter.ToUInt16(TCPHeader, x));

            // Pseudo header - Source Address 
            sum += ntoh(BitConverter.ToUInt16(IPHeader, 12));
            sum += ntoh(BitConverter.ToUInt16(IPHeader, 14));
            // Pseudo header - Dest Address 
            sum += ntoh(BitConverter.ToUInt16(IPHeader, 16));
            sum += ntoh(BitConverter.ToUInt16(IPHeader, 18));
            // Pseudo header - Protocol 
            sum += ntoh(BitConverter.ToUInt16(new byte[] { 0, IPHeader[9] }, 0));
            // Pseudo header - TCP Header length 
            sum += (uint)(ipTcpHeader.Length - 20);
            // 16 bit 1's compliment 
            while ((sum >> 16) != 0) { sum = ((sum & 0xFFFF) + (sum >> 16)); }
            sum = ~sum;
            return (ushort)ntoh((UInt16)sum);
        }
        public static ushort getIpCheckSum(byte[] data, int len = 20)
        {
            int sum = 0;
            for (int i = 0; i < len; i = i + 2)
            {
                if (i == 10)
                    continue;
                sum += BitConverter.ToUInt16(makeInt16Data(BitConverter.ToUInt16(data, i)), 0);
            }
            if (sum > 0xffff)
            {
                int u = sum >> 16;
                sum = (ushort)sum;
                sum = sum + u;
            }
            return (ushort)~sum;
        }
        private static ushort ntoh(UInt16 In)
        {
            int x = IPAddress.NetworkToHostOrder(In);
            return (ushort)(x >> 16);
        }
        public static byte[] makeInt16Data(UInt16 i)
        {
            byte[] b = BitConverter.GetBytes(i);
            Array.Reverse(b);
            return b;
        }
    }
}
