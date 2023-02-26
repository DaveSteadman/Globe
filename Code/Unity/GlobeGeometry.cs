using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DotNetMath;

// Class creating general purpose geometry

public class GlobeGeometry
{

    public static Mesh SphereMesh(float radius, int numRows, int numCols)
    {
        Mesh mesh = new Mesh();
        mesh.name = "SphereMesh";

        int numVerts = (numRows + 1) * (numCols + 1);
        List<Vector3> verts = new List<Vector3>(numVerts);
        List<Vector2> uvs = new List<Vector2>(numVerts);
        List<int> tris = new List<int>(numRows * numCols * 6);

        float dTheta = Mathf.PI / numRows;
        float dPhi = 2.0f * Mathf.PI / numCols;

        int vertIndex = 0;
        for (int i = 0; i <= numRows; i++)
        {
            float theta = i * dTheta;
            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);

            for (int j = 0; j <= numCols; j++)
            {
                float phi = j * dPhi;
                float sinPhi = Mathf.Sin(phi);
                float cosPhi = Mathf.Cos(phi);

                verts.Add(new Vector3(radius * sinTheta * cosPhi, radius * cosTheta, radius * sinTheta * sinPhi));
                uvs.Add(new Vector2((float)j / numCols, (float)i / numRows));
                vertIndex++;
            }
        }

        int triIndex = 0;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                int v00 = i * (numCols + 1) + j;
                int v10 = v00 + 1;
                int v01 = v00 + (numCols + 1);
                int v11 = v01 + 1;

                tris.Add(v00);
                tris.Add(v10);
                tris.Add(v01);

                tris.Add(v10);
                tris.Add(v11);
                tris.Add(v01);
            }
        }

        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(tris, 0);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    // ====================================================================================================
    // LLA Geometry
    // ====================================================================================================

    public static Mesh FlatTile(LatLonBox tileBox, int meshSize)
    {
        Mesh mesh = new Mesh();
        mesh.name = "FlatTile";

        int numVerts = (meshSize + 1) * (meshSize + 1);
        List<Vector3> verts = new List<Vector3>(numVerts);
        List<Vector2> uvs = new List<Vector2>(numVerts);
        List<int> tris = new List<int>(meshSize * meshSize * 6);

        double dLat = tileBox.LatSpanDegs() / meshSize;
        double dLon = tileBox.LonSpanDegs() / meshSize;

        int vertIndex = 0;
        for (int i = 0; i <= meshSize; i++)
        {
            double lat = tileBox.MinLatDegs + i * dLat;

            for (int j = 0; j <= meshSize; j++)
            {
                double lon = tileBox.MinLonDegs + j * dLon;


                Vector3 pntPos = UnityMathUtils.LLAToXYZPos(lat, lon, 0.0);

                verts.Add(pntPos);

                uvs.Add(new Vector2((float)j / meshSize, (float)i / meshSize));
                vertIndex++;
            }
        }

        //int triIndex = 0;
        for (int i = 0; i < meshSize; i++)
        {
            for (int j = 0; j < meshSize; j++)
            {
                int v00 = i * (meshSize + 1) + j;
                int v10 = v00 + 1;
                int v01 = v00 + (meshSize + 1);
                int v11 = v01 + 1;

                tris.Add(v00);
                tris.Add(v10);
                tris.Add(v01);

                tris.Add(v10);
                tris.Add(v11);
                tris.Add(v01);
            }
        }

        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(tris, 0);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

}
