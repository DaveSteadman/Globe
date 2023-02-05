using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace GlobeNetwork
{

    public struct CommsMessage
    {
        public string connectionName;
        public string msgData;
    }

    public abstract class CommonConnection
    {
        public string name { set; get; }

        public abstract string Type();
        public abstract string ConnectionDetailsString();

        public abstract void SendMessage(string msgData);

        public abstract void StartConnection();
        public abstract void StopConnection();

        // ========================================================================================
        // Incoming message queue
        // ========================================================================================

        public BlockingCollection<CommsMessage> incomingQueue;
        public void SetupIncomingQueue(BlockingCollection<CommsMessage> newIncomingQueue)
        {
            incomingQueue = newIncomingQueue;
        }

        public void QueueIncomingMessage(string msgData)
        {
            CommsMessage newMsg = new CommsMessage();

            newMsg.connectionName = name;
            newMsg.msgData = msgData;

            incomingQueue.Add(newMsg);
        }

    }

}
