using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Grit.Net.Common.Network
{
    public class NetworkHelper
    {
        public static bool Ping(string ipaddr)
        {
            return Ping(ipaddr, 1000);
        }
        public static bool Ping(string ipaddr, int timeout)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            string data = "ping";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            PingReply reply = pingSender.Send(ipaddr, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            return false;
        }

        private static string localIP = null;
        public static string GetLocalIP()
        {
            if (localIP == null)
            {
                NetworkInterface[] NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface NetworkIntf in NetworkInterfaces)
                {
                    IPInterfaceProperties IPInterfaceProperties = NetworkIntf.GetIPProperties();
                    UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                    foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                    {
                        if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            localIP = UnicastIPAddressInformation.Address.ToString();
                            break;
                        }
                    }
                }
                localIP = string.Empty;
            }
            return localIP;
        }

        public static IList<string> GetIPv4Array()
        {
            NetworkInterface[] NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            IList<string> ips = new List<string>();
            foreach (NetworkInterface NetworkIntf in NetworkInterfaces)
            {
                IPInterfaceProperties IPInterfaceProperties = NetworkIntf.GetIPProperties();
                UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                
                foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                {
                    if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ips.Add(UnicastIPAddressInformation.Address.ToString());
                    }
                }
            }
            return ips;
        }
    }
}
