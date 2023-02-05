using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GlobeNetwork
{
    class TCPClientConnection : CommonConnection
    {
        public string ipAddrStr;
        public IPAddress ipAddress;
        public int port;

        public NetworkStream stream;
        public TcpClient client;

        private Thread sendThread;
        private Thread receiveThread;
        public bool connected;
        public DateTime lastUpdateTime;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public TCPClientConnection()
        {
            ipAddrStr = "";
            ipAddress = null;
            port = 0;
            
            stream = null;
            client = null;

            sendThread = null;
            receiveThread = null;
            connected = false;
            lastUpdateTime = DateTime.UtcNow;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public void SetConnectionDetails(string inIpAddrStr, int inPort)
        {
            ipAddrStr = inIpAddrStr;
            port = inPort;
            ipAddress = IPAddress.Parse(ipAddrStr);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        private void ConnectCallback(IAsyncResult result)
        {
            try
            {
                // Complete the connection.
                client.EndConnect(result);
                stream = client.GetStream();
                connected = true;

                Console.WriteLine("Socket connected to {0}", client.Client.RemoteEndPoint.ToString());

                sendThread = new Thread(new ThreadStart(sendThreadFunc));
                receiveThread = new Thread(new ThreadStart(receiveThreadFunc));
                sendThread.Start();
                receiveThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // ========================================================================================
        // override methods
        // ========================================================================================

        public override string Type()
        {
            return "TCPClient";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override string ConnectionDetailsString()
        {
            return "TCPClient" + " // name:" + name + " // " + ipAddrStr + ":" + port;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override void StartConnection()
        {
            connected = false;

            client = new TcpClient();
            client.BeginConnect(ipAddress, port, ConnectCallback, client);
        }

        public override void StopConnection()
        {
            // Stop threads and close UDP client
            connected = false;

            if (sendThread != null)
                sendThread.Join();
            if (receiveThread != null)
                receiveThread.Join();
            
            client.Close();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override void SendMessage(string msgData)
        {
            byte[] messageBuffer = Encoding.ASCII.GetBytes(msgData);
            stream.Write(messageBuffer, 0, messageBuffer.Length);
        }

        // ========================================================================================

        private void SendThreadFunc()
        {
            
        }

            
        private void ReceiveThreadFunc()
        {
            // Get the client and stream from the parameter.
            while (connected)
            {
                // Check if the client is still connected, of break out of the infinite loop, to the removal and exit.
                if (!client.Connected)
                {
                    Console.WriteLine("Client disconnected");
                    client.Close();
                    break;
                }

                // BLOCKING: Read data from the client stream.
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                Console.Write("Connection: " + name + " // Received: " + data);

                // create a new message object to add to the incoming message queue.
                QueueIncomingMessage(data);
            }
        }
    }
}
