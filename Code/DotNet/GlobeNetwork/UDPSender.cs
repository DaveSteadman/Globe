using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;

namespace GlobeNetwork
{
    class UDPSender : CommonConnection
    {
        public string ipAddrStr;
        public int port;
        private IPEndPoint remoteHost;

        private UdpClient udpClient;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public UDPSender()
        {
            // Initialize send queue and threads
        }

        public void SetConnectionDetails(string inIpAddrStr, int inPort)
        {
            ipAddrStr = inIpAddrStr;
            port = inPort;
            remoteHost = new IPEndPoint(IPAddress.Parse(ipAddrStr), port);
        }

        // ========================================================================================
        // override methods
        // ========================================================================================

        public override string Type()
        {
            return "UDPSender";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override string ConnectionDetailsString()
        {
            return "type:UDPSender // name:" + name + " // " + ipAddrStr + ":" + port;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override void StartConnection()
        {
            // create UDP Port
            udpClient = new UdpClient();

            // Not a blocking call.
            udpClient.Connect(remoteHost);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override void StopConnection()
        {
            // Stop threads and close UDP client
            udpClient.Close();
        }

        // ========================================================================================

        public override void SendMessage(string msgData)
        {
            // Send opertion does not block.
            //udpClient.Send(Encoding.ASCII.GetBytes(msgData), msgData.Length);

            udpClient.SendAsync(Encoding.ASCII.GetBytes(msgData), msgData.Length, remoteHost);

        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    }
}
