using UnityEngine;

public class Mesh : MonoBehaviour
{
    void Start()
    {
        // Get the MeshFilter component attached to this GameObject
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("MeshFilter component or mesh not found!");
            return;
        }

        // Get the mesh and vertices array
        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;

        // Get the count of triangles and vertices
        int triangleCount = mesh.triangles.Length / 3;
        int vertexCount = vertices.Length;

        // Print counts
        Debug.Log("Triangle Count: " + triangleCount);
        Debug.Log("Vertex Count: " + vertexCount);

        // Loop through each triangle in the mesh
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            // Get the indices of the vertices for the current triangle
            int index1 = mesh.triangles[i];
            int index2 = mesh.triangles[i + 1];
            int index3 = mesh.triangles[i + 2];

            // Get the positions of the vertices
            Vector3 vertex1 = vertices[index1];
            Vector3 vertex2 = vertices[index2];
            Vector3 vertex3 = vertices[index3];

            // Print the positions of the vertices for the current triangle
            Debug.Log("Triangle " + (i / 3) + ":");
            Debug.Log("Vertex 1: " + vertex1);
            Debug.Log("Vertex 2: " + vertex2);
            Debug.Log("Vertex 3: " + vertex3);
        }
    }
}
