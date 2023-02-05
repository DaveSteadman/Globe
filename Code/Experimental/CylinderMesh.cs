using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMesh : MonoBehaviour
{
    public Vector3 p1 = new Vector3(0, 0, 0);
    public Vector3 p2 = new Vector3(0, 0, 5);
    public float p1radius = 1f;
    public float p2radius = 2f;
    public int segments = 12;

    Mesh cylinderMesh;

    GameObject p1Sphere;
    GameObject p2Sphere;

    void start()
    {
        Debug.Log("start");

        createMesh();
    }

    public void createMesh()
    {
        cylinderMesh = new Mesh();
        cylinderMesh.name = "CylinderMesh";

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // create sphere at p1
        p1Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        p1Sphere.name = "p1Sphere";
        p1Sphere.transform.parent = transform;
        p1Sphere.transform.position = p1;
        p1Sphere.transform.localScale = new Vector3(p1radius, p1radius, p1radius);
        var p1SphereRenderer = p1Sphere.GetComponent<Renderer>();
        p1SphereRenderer.material.SetColor("_Color", Color.red);

        // create sphere at p2
        p2Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        p2Sphere.name = "p2Sphere";
        p2Sphere.transform.parent = transform;
        p2Sphere.transform.position = p2;
        p2Sphere.transform.localScale = new Vector3(p2radius, p2radius, p2radius);
        var p2SphereRenderer = p2Sphere.GetComponent<Renderer>();
        p2SphereRenderer.material.SetColor("_Color", Color.green);

        // -------------------------------------------

        Quaternion direction = Quaternion.LookRotation(p2 - p1, Vector3.up);

        Vector3 p1CirlePoint = direction * new Vector3(0, p1radius, 0) + p1;

        Debug.Log("p1CirlePoint: " + p1CirlePoint);


        // Create the vertices

        
    }
}

/*
public class CylinderMesh : MonoBehaviour
{
    // The number of vertices in the circle
    public int vertexCount = 36;

    // The radius of the cylinder
    public float radius = 1f;

    // The height of the cylinder
    public float height = 1f;

    void Start()
    {
        // Create an array of Vector3 objects to store the vertices of the cylinder
        Vector3[] vertices = new Vector3[vertexCount * 2];

        // Create an array of int objects to store the triangles of the cylinder
        int[] triangles = new int[vertexCount * 12];

        // Create a Quaternion object to store the rotation of each vertex
        Quaternion rot = Quaternion.identity;

        // Set the initial rotation to point the z-axis in the direction of the cylinder's normal
        rot = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        // Loop through each vertex and set its position
        for (int i = 0; i < vertexCount; i++)
        {
            // Rotate the vertex around the circle
            rot *= Quaternion.Euler(0f, 360f / vertexCount, 0f);

            // Set the position of the top vertex
            vertices[i] = rot * (new Vector3(0f, radius, height / 2f));

            // Set the position of the bottom vertex
            vertices[i + vertexCount] = rot * (new Vector3(0f, radius, -height / 2f));

            Debug.Log(vertices[i]);
        }

        // Loop through each triangle and set its indices
        for (int i = 0; i < vertexCount; i++)
        {
            // Set the indices of the triangles for the top face
            triangles[i * 12] = i;
            triangles[i * 12 + 1] = (i + 1) % vertexCount;
            triangles[i * 12 + 2] = i + vertexCount;

            // Set the indices of the triangles for the bottom face
            triangles[i * 12 + 3] = i + vertexCount;
            triangles[i * 12 + 4] = (i + 1) % vertexCount;
            triangles[i * 12 + 5] = (i + 1) % vertexCount + vertexCount;

            // Set the indices of the triangles for the side face
            triangles[i * 12 + 6] = i;
            triangles[i * 12 + 7] = i + vertexCount;
            triangles[i * 12 + 8] = (i + 1) % vertexCount;
            triangles[i * 12 + 9] = (i + 1) % vertexCount;
            triangles[i * 12 + 10] = i + vertexCount;
            triangles[i * 12 + 11] = (i + 1) % vertexCount + vertexCount;
        }

        // Create a new Mesh object
        Mesh mesh = new Mesh();

        // Set the vertices and triangles of the Mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalculate the normals of the Mesh
        mesh.RecalculateNormals();

                // Create a new MeshFilter object and set the Mesh as its mesh
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // Create a new MeshRenderer object and set the Material of the Mesh
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
    }
}
*/