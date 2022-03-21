using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeshClass : MonoBehaviour
{

    public int numOfVertices = 0;
    public int[] meshtriangles;
    public Vector3[] meshvertices;

    public abstract void GenerateMesh();
    public abstract void UpdateMesh(); 
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
