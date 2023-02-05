using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeGeometryOps
{
    public Mesh CreateHemisphereMesh(float radius, int numSegments)
    {
        // Create the mesh for the sphere
        Mesh mesh = new Mesh();

        // divide segments to balance angle per segment, and round.
        int vertSegments = (int)Mathf.Round((float)numSegments / 4f);

        float vertAngInc = 90f / (float)vertSegments;
        float horizAngInc = 360f / (float)numSegments;

        float uvXInc = 1f / (float)numSegments;
        float uvYInc = 1f / (float)vertSegments;

        // Create a List of Vector3 vertices for the sphere
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        // --- define the points on the sphere surface ---

        for (int i = 0; i < vertSegments+1; i++)
        {
            float angle1 =  (90f - (vertAngInc * i)) * Mathf.Deg2Rad;
            float y = radius * Mathf.Sin(angle1);
            float r = radius * Mathf.Cos(angle1);

            for (int j = 0; j < numSegments; j++)
            {
                float angle2 = (horizAngInc * j) * Mathf.Deg2Rad;
                float x = r * Mathf.Cos(angle2);
                float z = r * Mathf.Sin(angle2);

                Vector3 v = new Vector3(x, y, z);
                vertices.Add(v);

                float uvX = uvXInc * j;
                float uvY = uvYInc * i;
                Vector2 uv = new Vector2(uvX, uvY);
                uvs.Add(uv);
            }
        }

        // --- define the triangles ---

        // Create a List of integer triangle indices for the sphere
        List<int> triangles = new List<int>();

        for (int row=0; row<vertSegments; row++)
        {
            int rowtl = row * numSegments;
            for (int i = 0; i < numSegments; i++)
            {
                int index1 = rowtl + i;
                int index2 = index1 + 1;
                int index3 = index1 + numSegments - 1;
                int index4 = index1 + numSegments;

                triangles.Add(index1);
                triangles.Add(index2);
                triangles.Add(index4);

                triangles.Add(index1);
                triangles.Add(index4);
                triangles.Add(index3);
            }
        }

        // Set the vertices and triangles of the mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        return mesh;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public Mesh CreateSphereMesh(float radius, int numSegments)
    {
        // Create the mesh for the sphere
        Mesh mesh = new Mesh();

        // divide segments to balance angle per segment, and round.
        int vertSegments = (int)Mathf.Round((float)numSegments / 2f);

        float vertAngInc = 180f / (float)vertSegments;
        float horizAngInc = 360f / (float)numSegments;

        float uvXInc = 1f / (float)numSegments;
        float uvYInc = 1f / (float)vertSegments;

        // Create a List of Vector3 vertices for the sphere
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        // --- define the points on the sphere surface ---

        for (int i = 0; i < vertSegments+1; i++)
        {
            float angle1 =  (90f - (vertAngInc * i)) * Mathf.Deg2Rad;
            float y = radius * Mathf.Sin(angle1);
            float r = radius * Mathf.Cos(angle1);

            for (int j = 0; j < numSegments; j++)
            {
                float angle2 = (horizAngInc * j) * Mathf.Deg2Rad;
                float x = r * Mathf.Cos(angle2);
                float z = r * Mathf.Sin(angle2);

                Vector3 v = new Vector3(x, y, z);
                vertices.Add(v);

                float uvX = uvXInc * j;
                float uvY = uvYInc * i;
                Vector2 uv = new Vector2(uvX, uvY);
                uvs.Add(uv);
            }
        }

        // --- define the triangles ---

        // Create a List of integer triangle indices for the sphere
        List<int> triangles = new List<int>();

        for (int row=0; row<vertSegments; row++)
        {
            int rowtl = row * numSegments;
            for (int i = 0; i < numSegments; i++)
            {
                int index1 = rowtl + i;
                int index2 = index1 + 1;
                int index3 = index1 + numSegments - 1;
                int index4 = index1 + numSegments;

                triangles.Add(index1);
                triangles.Add(index2);
                triangles.Add(index4);

                triangles.Add(index1);
                triangles.Add(index4);
                triangles.Add(index3);
            }
        }

        // Set the vertices and triangles of the mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        return mesh;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public Mesh CreateCylinderMesh(Vector3 p1, Vector3 p2, float p1radius, float p2radius, int segments)
    {
        Mesh mesh = new Mesh();
        mesh.name = "CylinderMesh";

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        Quaternion direction = Quaternion.LookRotation(p2 - p1, Vector3.up);
        float angleStep = 360f / segments;

        float uvXInc = 1f / (float)segments;

        // --- create vertices ---

        for (int i=0; i<segments; i++)
        {
            float angle = (float)(i * angleStep) * Mathf.Deg2Rad;

            float p1x = p1radius * Mathf.Cos(angle);
            float p1y = p1radius * Mathf.Sin(angle);
            float p1z = 0f;

            Vector3 p1CirlePoint = direction * new Vector3(p1x, p1y, p1z) + p1;
            vertices.Add(p1CirlePoint);

            float uvX = uvXInc * i;
            float uvY = 0f;
            Vector2 uv = new Vector2(uvX, uvY);
            uvs.Add(uv);
        }

        for (int i=0; i<segments; i++)
        {
            float angle = (float)(i * angleStep) * Mathf.Deg2Rad;

            float p2x = p2radius * Mathf.Cos(angle);
            float p2y = p2radius * Mathf.Sin(angle);
            float p2z = 0f;
            Vector3 p2CirclePoint = direction * new Vector3(p2x, p2y, p2z) + p2;
            vertices.Add(p2CirclePoint);

            float uvX = uvXInc * i;
            float uvY = 1f;
            Vector2 uv = new Vector2(uvX, uvY);
            uvs.Add(uv);
        }

        // --- create triangles ---

        List<int> triangles = new List<int>();

        for (int i=0; i<segments; i++)
        {
            int i1 = i;
            int i2 = i + 1;
            int i3 = i + segments;
            int i4 = i + segments + 1;

            if (i2 >= segments)
            {
                i2 = 0;
                i4 = segments;
            }

            triangles.Add(i1);
            triangles.Add(i2);
            triangles.Add(i3);

            triangles.Add(i2);
            triangles.Add(i4);
            triangles.Add(i3);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public Mesh CreateFourPointVolume(Vector3 pF1, Vector3 pF2, Vector3 pF3, Vector3 pF4, float halfWidth)
    {
        Quaternion topDirection = Quaternion.LookRotation(pF1 - pF2, pF1 - pF3);

        // With the geometry of the gameobject anchored at pF1, offset the positions of the vertices
        Vector3 pO1 = Vector3.zero;
        Vector3 pO2 = pF2 - pF1;
        Vector3 pO3 = pF3 - pF1;
        Vector3 pO4 = pF4 - pF1;
        
        Vector3 offsetVect1 = new Vector3(halfWidth, 0f, 0f);
        Vector3 offsetVect2 = new Vector3(-halfWidth, 0f, 0f);

        List<Vector3> vertices = new List<Vector3>();

        Vector3 newPos;

        newPos = pO1 + topDirection * offsetVect1;     vertices.Add(newPos);
        newPos = pO1 + topDirection * offsetVect2;     vertices.Add(newPos);
        newPos = pO2 + topDirection * offsetVect1;     vertices.Add(newPos);
        newPos = pO2 + topDirection * offsetVect2;     vertices.Add(newPos);
        newPos = pO3 + topDirection * offsetVect1;     vertices.Add(newPos);
        newPos = pO3 + topDirection * offsetVect2;     vertices.Add(newPos);
        newPos = pO4 + topDirection * offsetVect1;     vertices.Add(newPos);
        newPos = pO4 + topDirection * offsetVect2;     vertices.Add(newPos);

        List<int> triangles = new List<int>();

        // Top
        triangles.Add(0); triangles.Add(2); triangles.Add(1);
        triangles.Add(1); triangles.Add(2); triangles.Add(3);

        // Side 1
        triangles.Add(0); triangles.Add(1); triangles.Add(4);
        triangles.Add(1); triangles.Add(5); triangles.Add(4);

        // Side 2
        triangles.Add(2); triangles.Add(6); triangles.Add(3);
        triangles.Add(3); triangles.Add(6); triangles.Add(7);

        // Bottom
        triangles.Add(4); triangles.Add(5); triangles.Add(6);
        triangles.Add(5); triangles.Add(7); triangles.Add(6);

        // Broad side 1
        triangles.Add(0); triangles.Add(4); triangles.Add(2);
        triangles.Add(2); triangles.Add(4); triangles.Add(6);

        // Broad side 2
        triangles.Add(1); triangles.Add(3); triangles.Add(5);
        triangles.Add(3); triangles.Add(7); triangles.Add(5);

        Mesh meshP1  = new Mesh();
        meshP1.name = "meshP1";
        meshP1.vertices = vertices.ToArray();
        meshP1.triangles = triangles.ToArray();

        meshP1.RecalculateNormals();

        return meshP1;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public static Mesh CreateMeshHoop(float outerRadius, float innerRadius, float depth, int numSegments)
    {
        Mesh mesh = new Mesh();

        float angleInc = 360.0f / numSegments;
        int twiceNumSegments = numSegments * 2;
        int thriceNumSegments = numSegments * 3;

        // create a lists
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // - - - - vertices - - - -

        // create the vertices for the top-outer edge of the washer
        for (int i = 0; i < numSegments; i++)
        {
            float angle = (float)i * angleInc;
            float x = outerRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = outerRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
            vertices.Add(new Vector3(x, y, 0));
        }

        // create the vertices for the top-inner edge of the washer
        for (int i = 0; i < numSegments; i++)
        {
            float angle = (float)i * angleInc;
            float x = innerRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = innerRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
            vertices.Add(new Vector3(x, y, 0));
        }

        // create the vertices for the bottom-outer face of the washer
        for (int i = 0; i < numSegments; i++)
        {
            float angle = (float)i * angleInc;
            float x = outerRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = outerRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
            vertices.Add(new Vector3(x, y, -depth));
        }

        // create the vertices for the bottom-inner face of the washer
        for (int i = 0; i < numSegments; i++)
        {
            float angle = (float)i * angleInc;
            float x = innerRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = innerRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
            vertices.Add(new Vector3(x, y, -depth));
        }

        // - - - - triangles - - - -

        // create the triangles for the top of the washer
        for (int i = 0; i < numSegments; i++)
        {
            int id1 = i;
            int id2 = (i + 1) % numSegments;
            int id3 = i + numSegments;
            int id4 = (i + 1) % numSegments + numSegments;

            triangles.Add(id1);
            triangles.Add(id2);
            triangles.Add(id3);

            triangles.Add(id2);
            triangles.Add(id4);
            triangles.Add(id3);
        }

        // create the triangles for the bottom of the washer
        for (int i = 0; i < numSegments; i++)
        {
            int id1 = i;
            int id2 = (i + 1) % numSegments;
            int id3 = i + numSegments;
            int id4 = (i + 1) % numSegments + numSegments;

            id1 += twiceNumSegments;
            id2 += twiceNumSegments;
            id3 += twiceNumSegments;
            id4 += twiceNumSegments;

            triangles.Add(id2);
            triangles.Add(id1);
            triangles.Add(id3);

            triangles.Add(id2);
            triangles.Add(id3);
            triangles.Add(id4);
        }

        // create the triangles for the inside edge of the washer
        for (int i = 0; i < numSegments; i++)
        {
            int id1 = i;
            int id2 = (i + 1) % numSegments;
            int id3 = i + numSegments;
            int id4 = (i + 1) % numSegments + numSegments;

            id1 += numSegments;
            id2 += numSegments;
            id3 += twiceNumSegments;
            id4 += twiceNumSegments;

            triangles.Add(id1);
            triangles.Add(id2);
            triangles.Add(id3);

            triangles.Add(id2);
            triangles.Add(id4);
            triangles.Add(id3);
        }

        // create the triangles for the outside edge of the washer
        for (int i = 0; i < numSegments; i++)
        {
            int id1 = i;
            int id2 = (i + 1) % numSegments;
            int id3 = i + numSegments;
            int id4 = (i + 1) % numSegments + numSegments;

            id1 += 0;
            id2 += 0;
            id3 += numSegments;
            id4 += numSegments;

            triangles.Add(id1);
            triangles.Add(id3);
            triangles.Add(id2);

            triangles.Add(id2);
            triangles.Add(id3);
            triangles.Add(id4);
        }


        // assign values to the mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }
}



