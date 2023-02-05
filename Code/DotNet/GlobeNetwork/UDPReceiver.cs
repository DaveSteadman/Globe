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
    class UDPReceiver : CommonConnection
    {
        public int port;
        private UdpClient udpClient;
        private IPEndPoint localEndPoint;

        private Thread receiveThread;
        private bool receiveThreadValidFlag;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public UDPReceiver()
        {
            // Initialize send queue and threads
            // incomingMsgQueue = new BlockingCollection<string>();
            localEndPoint = null;
        }

        public void setConnectionDetails(int inPort)
        {
            port = inPort;
        }

        // ========================================================================================
        // override methods
        // ========================================================================================

        public override string Type()
        {
            return "UDPReceiver";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override string ConnectionDetailsString()
        {
            if (localEndPoint != null)
                return "type:UDPReceiver // name:" + name + " // " + localEndPoint.Address + ":" + port;
            else
                return "type:UDPReceiver // name:" + name + " // " + "<localEndPoint.Address>" + ":" + port;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override void StartConnection()
        {
            // create UDP Port
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            localEndPoint = (IPEndPoint)udpClient.Client.LocalEndPoint;

            receiveThread = new Thread(new ThreadStart(receiveThreadFunc));
            receiveThread.Start();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override void StopConnection()
        {
            receiveThreadValidFlag = false;
            udpClient.Close();
            receiveThread.Join();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override void SendMessage(string msgData)
        {
            // A receiver class, should be used to send.
        }

        // ========================================================================================

        private void ReceiveThreadFunc()
        {
            Console.WriteLine("THREAD FUNC START: UDPReceiver.receiveThreadFunc()");
            receiveThreadValidFlag = true;

            while (receiveThreadValidFlag)
            {
                try
                {
                    // Sender object records the incoming message details, recreated for each incoming message
                    IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

                    // Receive a message. BLOCKING.
                    Byte[] receiveBytes = udpClient.Receive(ref sender);
                    string returnData = Encoding.UTF8.GetString(receiveBytes);
                    QueueIncomingMessage(returnData);
                }
                catch (SocketException ex)
                {
                    // Interruped when the close-thread call closes the socket
                    if (ex.SocketErrorCode == SocketError.Interrupted)
                    {
                        // UdpClient was closed, exit loop
                        receiveThreadValidFlag = false;
                    }
                }
            }
        }
    }
}
