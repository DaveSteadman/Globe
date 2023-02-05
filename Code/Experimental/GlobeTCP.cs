using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

using UnityEngine;

public class GlobeTCP : MonoBehaviour
{
/*


    TcpListener listener;
    List<TcpClient> clients = new List<TcpClient>();

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    // Update is called once per frame
    void Update()
    {
        
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    void CreateTCPListener()
    {
        var port = 8080;

        // Create a new TcpListener and bind it to the specified port
        listener = new TcpListener(System.Net.IPAddress.Any, port);

        // Start the listener
        listener.Start();
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    void CreateTCPClient()
    {
        // Create a new TcpClient
        client = new TcpClient();

        var server = "127.0.0.1";
        var port = 8080;

        try
        {
            // Connect to the server
            client.Connect(server, port);
        }
        catch (System.Exception)
        {
            Debug.Log("Failed to connect to server");
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    void ListenerAcceptClient()
    {
        if (listener.Pending())
        {
            // Accept a new client connection
            client = listener.AcceptTcpClient();

            // Get the client's stream
            stream = client.GetStream();
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


*/

}
