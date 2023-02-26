using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GlobeNetwork
{
    class TCPServerClientConnection : CommonConnection
    {
        public string ipAddrStr;
        public IPAddress ipAddress;
        public int port;
        public bool running;
        public TcpListener listener;

        public NetworkStream stream;
        public TcpClient client;
        public DateTime lastUpdateTime;

        private Thread sendThread;
        private Thread receiveThread;

        public BlockingCollection<string> sendMsgQueue;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public TCPServerClientConnection()
        {
            ipAddrStr = "";
            ipAddress = null;
            port = 0;

            running = false;
            listener = null;

            sendMsgQueue = new BlockingCollection<string>();
        }

        public void SetConnectionDetails(TcpClient newTcpClient, NetworkStream newStream)
        {
            client = newTcpClient;
            stream = newStream;
        }

        public void SetConnectionDetails(string inIpAddrStr, int inPort)
        {
            ipAddrStr = inIpAddrStr;
            ipAddress = IPAddress.Parse(ipAddrStr);
            port = inPort;
        }

        // ========================================================================================
        // override methods
        // ========================================================================================

        public override string Type()
        {
            return "TCPServerClient";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override string ConnectionDetailsString()
        {
            return "TCPServerClient // name:" + name + " // " + ipAddrStr + ":" + port;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override void StartConnection()
        {
            running = true;

            // Process the client connection in a new thread.
            sendThread = new Thread(new ThreadStart(SendThreadFunc));
            sendThread.Start();

            receiveThread = new Thread(new ThreadStart(ReceiveThreadFunc));
            receiveThread.Start();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override void StopConnection()
        {
            // Set the running flag to false.
            running = false;

            if (listener != null)
            {
                listener.Stop();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override void SendMessage(string msgData)
        {
            //byte[] messageBuffer = Encoding.ASCII.GetBytes(msgData);
            //stream.Write(messageBuffer, 0, messageBuffer.Length);

            sendMsgQueue.Add(msgData);
        }

        // ========================================================================================

        void SendThreadFunc()
        {
            Console.WriteLine("THREAD FUNC START: TCPServerClientConnection.SendThreadFunc()");

            // Enter an infinite loop to process client connections.
            while (running)
            {
                //Console.WriteLine("BLOCKED ON NEXT SEND: TCPServerClientConnection.sendThreadFunc()");
                string nextMsg;
                
                if (sendMsgQueue.TryTake(out nextMsg, 1000))
                {
                    byte[] msgBuffer = Encoding.ASCII.GetBytes(nextMsg);
                    stream.Write(msgBuffer, 0, msgBuffer.Length);
                }
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        void ReceiveThreadFunc()
        {
            Console.WriteLine("THREAD FUNC START: TCPServerClientConnection.ReceiveThreadFunc()");

            // Enter a loop to continuously process client requests.
            while (running)
            {
                // Check if the client is still connected, of break out of the infinite loop, to the removal and exit.
                if (!client.Connected)
                {
                    Console.WriteLine("Client disconnected");
                    client.Close();
                    break;
                }

                // Timeout a client if not sent a message recently
                //DateTime currentTime = DateTime.UtcNow;
                //if (currentTime.Subtract(lastUpdateTime).TotalSeconds > 5)
                //{
                //    Console.WriteLine("Client inactive");
                //    client.Close();
                //    break;
                //}

                // Check if data is available on the client stream.
                if (stream.DataAvailable)
                {
                    // Update the timer if we have anything.
                    lastUpdateTime = DateTime.Now;

                    // Read data from the client stream.
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        // listener was closed or broken, exit loop
                        running = false;
                        continue; // to top of loop to exit
                    }
                    else
                    {
                        string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                        // create a new message object to add to the incoming message queue.
                        QueueIncomingMessage(data);
                    }
                }
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    } // class
} // namespace




