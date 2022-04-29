using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeshClass : MonoBehaviour
{

    public Mesh mesh;
    public int numOfVertices = 0;
    public Vector3[] meshvertices;
    public List<int> massPointIndexes = new List<int>();
    public List<int> holder = new List<int>();
    public List<TriangleIndices> triangles = new List<TriangleIndices>();
    public List<Vector3> vertices;
    public List<Vector3> normals;
    public bool meshesGenerated = false;
    public float volume;
    public float surfaceArea;

    public abstract void GenerateMesh();
    public abstract void UpdateMesh();
    public abstract void FillJointArray();
    public abstract bool CollisionCheck(MeshClass mesh2);


    public struct TriangleIndices
    {
        public int vertex1;
        public int vertex2;
        public int vertex3;

        public TriangleIndices(int vertex1, int vertex2, int vertex3)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            this.vertex3 = vertex3;
        }
    }

}
