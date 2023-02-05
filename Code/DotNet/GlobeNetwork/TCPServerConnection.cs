using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*

Create a listener
    listen for a connection - listener.AcceptTcpClient

    Create client class / thread

*/


namespace GlobeNetwork
{
    class TCPServerConnection : CommonConnection
    {
        public string ipAddrStr;
        public IPAddress ipAddress;
        public int port;
        public bool running;
        public TcpListener listener;

        public NetworkCommsHub commsHub;

        // A flag to indicate whether the server is running.
        private Thread serverThread;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public TCPServerConnection()
        {
            ipAddrStr = "";
            ipAddress = null;
            port = 0;

            running = false;
            listener = null;
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
            return "TCPServer";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override string ConnectionDetailsString()
        {
            return "TCPServer" + " // name:" + name + " // " + ipAddrStr + ":" + port;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public override void StartConnection()
        {
            // Process the client connection in a new thread.
            serverThread = new Thread(new ThreadStart(serverThreadFunc));
            serverThread.Start();
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
            //clientConfig.stream.Write(messageBuffer, 0, messageBuffer.Length);
        }

        // ========================================================================================

        private void ServerThreadFunc()
        {
            Console.WriteLine("THREAD FUNC START: TCPServerConnection.serverThreadFunc()");

            // Create a new TCP listener object.
            listener = new TcpListener(ipAddress, port);

            // Start listening for client connections.
            listener.Start();

            // Set the running flag to true.
            running = true;

            // Enter an infinite loop to process client connections.
            while (running)
            {
                TcpClient client;

                try
                {
                    // Accept a new client connection.
                    // BLOCKING. Will throw on listener.Stop() call when stopping thread
                    Console.WriteLine("Waiting for connections...");
                    client = listener.AcceptTcpClient();
                }
                catch (SocketException ex)
                {
                    // listener was closed or broken, exit loop
                    running = false;
                    continue; // to top of loop to exit
                }

                // Get the client's stream.
                Console.WriteLine("New connection...");
                NetworkStream stream = client.GetStream();


                TCPServerClientConnection newClient = new TCPServerClientConnection();
                newClient.name = name + "_client";
                newClient.stream = stream;
                newClient.client = client;
                newClient.setupIncomingQueue(incomingQueue);
                newClient.lastUpdateTime = DateTime.Now;

                newClient.startConnection();

                commsHub.connections.Add(newClient);
            }

            // Stop the listener.
            listener.Stop();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    } // class
} // namespace
