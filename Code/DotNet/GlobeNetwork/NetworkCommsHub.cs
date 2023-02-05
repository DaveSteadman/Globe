using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace GlobeNetwork
{

    class NetworkCommsHub
    {
        public List<CommonConnection> connections;
        public BlockingCollection<CommsMessage> incomingQueue;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public NetworkCommsHub()
        {
            connections = new List<CommonConnection>();
            incomingQueue = new BlockingCollection<CommsMessage>();
        }

        ~NetworkCommsHub()
        {
            // Stop all threads
            foreach (CommonConnection connection in connections)
            {
                connection.stopConnection();
            }
            connections.Clear();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public void CreateConnection(string connName, string connType, string ipAddrStr, int port)
        {
            if (connType == "TCPClient")
            {
                // Create connection
                TCPClientConnection clientConnection = new TCPClientConnection();

                // Satisfy parent class calls
                clientConnection.name = connName;
                clientConnection.setupIncomingQueue(incomingQueue);

                connections.Add((CommonConnection)clientConnection);
                clientConnection.setConnectionDetails(ipAddrStr, port);
                clientConnection.startConnection();
            }
            else if (connType == "TCPServer")
            {
                // Create connection
                TCPServerConnection serverConnection = new TCPServerConnection();

                // Satisfy parent class calls
                serverConnection.name = connName;
                serverConnection.commsHub = this;
                serverConnection.setupIncomingQueue(incomingQueue);

                connections.Add((CommonConnection)serverConnection);
                serverConnection.setConnectionDetails(ipAddrStr, port);
                serverConnection.startConnection();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public void EndConnection(string connName)
        {
            foreach (CommonConnection conn in connections)
            {
                if (conn.name == connName)
                {
                    Console.WriteLine("Stopping connection: " + connName);
                    conn.stopConnection();
                    connections.Remove(conn);
                    break;
                }
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public void SendMessage(string connName, string msgData)
        {
            foreach (CommonConnection conn in connections)
            {
                if (conn.name == connName)
                {
                    Console.WriteLine("Sending message to " + connName);
                    conn.sendMessage(msgData);
                }
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public bool HasIncomingMessage()
        {
            return (incomingQueue.Count > 0);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public bool getIncomingMessage(out CommsMessage msg)
        {
            return incomingQueue.TryTake(out msg, 100);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public string IncomingQueueDump()
        {
            string dump = "";
            foreach (CommsMessage msg in incomingQueue)
            {
                dump += msg.ToString() + "\n";
            }
            return dump;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public string DebugDump()
        {
            string outTxt = "";

            foreach (CommonConnection conn in connections)
            {
                outTxt += "Connection name: " + conn.name + " // type: " + conn.type() + "\n";
            }

            return outTxt;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    }
}
