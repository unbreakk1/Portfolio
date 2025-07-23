using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    private List<Vector3> vertices = new();
    private List<int> triangles = new();
    private List<Vector2> uv = new();

    private List<Vector3> colliderVertices = new();
    private List<int> colliderTriangles = new();

    public MeshData WaterMesh;

    public MeshData(bool isMainMesh)
    {
        //prevent recursive call => exception
        if (isMainMesh)
            WaterMesh = new MeshData(false);
    }

    #region Properties

    public List<Vector3> Vertices
    {
        get => vertices;
        set => vertices = value;
    }

    public List<int> Triangles
    {
        get => triangles;
        set => triangles = value;
    }

    public List<Vector2> UV
    {
        get => uv;
        set => uv = value;
    }

    public List<Vector3> ColliderVertices
    {
        get => colliderVertices;
        set => colliderVertices = value;
    }

    public List<int> ColliderTriangles
    {
        get => colliderTriangles;
        set => colliderTriangles = value;
    }

    #endregion

    public void AddVertex(Vector3 vertex, bool vertexGeneratesCollider)
    {
        vertices.Add(vertex);
        if (vertexGeneratesCollider)
            colliderVertices.Add(vertex);
    }

    
    // triangles for quads
    public void AddQuadTriangles(bool quadGeneratesCollider)
    {
        triangles.Add(vertices.Count - 4); // index 0
        triangles.Add(vertices.Count - 3); // index 1
        triangles.Add(vertices.Count - 2); // index 2

        triangles.Add(vertices.Count - 4); // index 0
        triangles.Add(vertices.Count - 2); // index 2
        triangles.Add(vertices.Count - 1); // index 3

        if (quadGeneratesCollider)
        {
            colliderTriangles.Add(colliderVertices.Count - 4);
            colliderTriangles.Add(colliderVertices.Count - 3);
            colliderTriangles.Add(colliderVertices.Count - 2);

            colliderTriangles.Add(colliderVertices.Count - 4);
            colliderTriangles.Add(colliderVertices.Count - 2);
            colliderTriangles.Add(colliderVertices.Count - 1);
        }
    }
}