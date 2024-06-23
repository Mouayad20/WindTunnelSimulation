using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshUtils
{
    public static Mesh CreateSphere(float radius, int longitudeSegments, int latitudeSegments)
    {
        Mesh mesh = new Mesh();

        // Vertices and normals
        Vector3[] vertices = new Vector3[(longitudeSegments + 1) * latitudeSegments + 2];
        Vector3[] normals = new Vector3[vertices.Length];

        float pi = Mathf.PI;
        float twoPi = pi * 2f;

        vertices[0] = Vector3.up * radius;
        normals[0] = Vector3.up;

        for (int lat = 0; lat < latitudeSegments; lat++)
        {
            float a1 = pi * (float)(lat + 1) / (latitudeSegments + 1);
            float sin1 = Mathf.Sin(a1);
            float cos1 = Mathf.Cos(a1);

            for (int lon = 0; lon <= longitudeSegments; lon++)
            {
                float a2 = twoPi * (float)(lon == longitudeSegments ? 0 : lon) / longitudeSegments;
                float sin2 = Mathf.Sin(a2);
                float cos2 = Mathf.Cos(a2);

                vertices[lon + lat * (longitudeSegments + 1) + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
                normals[lon + lat * (longitudeSegments + 1) + 1] = vertices[lon + lat * (longitudeSegments + 1) + 1].normalized;
            }
        }

        vertices[vertices.Length - 1] = Vector3.down * radius;
        normals[normals.Length - 1] = Vector3.down;

        // Triangles
        int[] triangles = new int[(vertices.Length - 2) * 6];

        int triIndex = 0;
        for (int lon = 0; lon < longitudeSegments; lon++)
        {
            triangles[triIndex++] = lon + 2;
            triangles[triIndex++] = lon + 1;
            triangles[triIndex++] = 0;
        }

        for (int lat = 0; lat < latitudeSegments - 1; lat++)
        {
            for (int lon = 0; lon < longitudeSegments; lon++)
            {
                int current = lon + lat * (longitudeSegments + 1) + 1;
                int next = current + longitudeSegments + 1;

                triangles[triIndex++] = current;
                triangles[triIndex++] = current + 1;
                triangles[triIndex++] = next + 1;

                triangles[triIndex++] = current;
                triangles[triIndex++] = next + 1;
                triangles[triIndex++] = next;
            }
        }

        for (int lon = 0; lon < longitudeSegments; lon++)
        {
            triangles[triIndex++] = vertices.Length - 1;
            triangles[triIndex++] = vertices.Length - (lon + 2) - 1;
            triangles[triIndex++] = vertices.Length - (lon + 1) - 1;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;

        return mesh;
    }
}
