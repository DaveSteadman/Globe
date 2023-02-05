using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;


using UnityEngine;



public class WaveUDP : MonoBehaviour
{
    UdpClient client;
    WaveData waveData;

    public Mesh mesh;
    List<Vector3> vertices;
    List<Color32> color;
    List<Vector2> uvs;
    List<int> triangles;



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public static byte[] WaveUDPMsgToByteArray(WaveUDPMsg msg)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        writer.Write(msg.freqMin);
        writer.Write(msg.freqInt);
        for (int i = 0; i < 100; i++)
            writer.Write(msg.freq[i]);

        writer.Write(msg.next);

        return stream.ToArray();
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public static WaveUDPMsg ByteArrayToWaveUDPMsg(byte[] data)
    {

        int x = data.Length;
        //Debug.Log("ByteArrayToWaveUDPMsg" + x);

        MemoryStream stream = new MemoryStream(data);
        BinaryReader reader = new BinaryReader(stream);

        WaveUDPMsg msg = new WaveUDPMsg();
        msg.freq = new double[100];

        msg.freqMin = reader.ReadDouble();
        msg.freqInt = reader.ReadDouble();
        for (int i = 0; i < 100; i++)
            msg.freq[i] = reader.ReadDouble();


        msg.next = reader.ReadInt32();
        return msg;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    void CreateMesh()
    {

        mesh = new Mesh();
        vertices = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<int>();
        color = new List<Color32>();

        for (int width=0; width<1000; width++)
        {
            for (int timedepth=0; timedepth<100; timedepth++)
            {
                vertices.Add(new Vector3(width, timedepth, 0));
                uvs.Add(new Vector2(width, timedepth));
                color.Add(new Color32(10,10,10,255));
            }
        }

        for (int width = 0; width < 999; width++)
        {
            for (int timedepth = 0; timedepth < 99; timedepth++)
            {
                triangles.Add(width * 100 + timedepth);
                triangles.Add((width + 1) * 100 + timedepth);
                triangles.Add(width * 100 + timedepth + 1);

                triangles.Add((width + 1) * 100 + timedepth);
                triangles.Add((width + 1) * 100 + timedepth + 1);
                triangles.Add(width * 100 + timedepth + 1);
            }
        }

        // Set the vertices and triangles of the mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors32 = color.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    // Start is called before the first frame update
    void Start()
    {
        // Create a UDP socket
        client = new UdpClient(new IPEndPoint(IPAddress.Any, 11223));

        waveData = new WaveData();

        CreateMesh();

        GameObject GO1 = new GameObject("mesh", typeof(MeshFilter), typeof(MeshRenderer));
        GO1.GetComponent<MeshFilter>().mesh = mesh;

        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();

         // Set the material of the MeshRenderer to the Particle Shader
        GO1.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));



        GO1.transform.localPosition = new Vector3(0, 0, 0);
        GO1.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (client == null)
            client = new UdpClient(new IPEndPoint(IPAddress.Any, 11223));

        if (client.Available > 0)
        {
            IPEndPoint remoteEP = null;
            byte[] data = client.Receive(ref remoteEP);
            WaveUDPMsg msg = ByteArrayToWaveUDPMsg(data);
            Debug.Log("Received: " + msg.freqMin + " " + msg.freqInt + " " + msg.freq[0] + " >> " + msg.next);

            waveData.AddWaveUDPMsg(msg);


        }

        for (int width = 0; width < 1000; width++)
        {
            for (int timedepth = 0; timedepth < 100; timedepth++)
            {
                Vector3 p = vertices[width * 100 + timedepth];
                p.z = (float)waveData.arrData[width, timedepth];
                vertices[width * 100 + timedepth] = p;

                int val = (int)waveData.arrData[width, timedepth];
                if (val > 255) val = 255;

                color[width * 100 + timedepth] = new Color32(10, 10, (byte)val, 255);

            }
        }
        mesh.vertices = vertices.ToArray();

    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


}
 