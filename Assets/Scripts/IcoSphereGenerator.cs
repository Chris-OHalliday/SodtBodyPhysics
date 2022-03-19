using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcoSphereGenerator : MonoBehaviour
{



    [Range(0.0f, 100.0f)]
    public int subdivisionLevel;

    private struct TriangleIndices
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

    Mesh mesh;
    [SerializeField] private List<Vector3> vertices;
    private List<TriangleIndices> triangles = new List<TriangleIndices>();
    List<int> holder = new List<int>();
    private Dictionary<long, int> middlePointIndexDict = new Dictionary<long, int>();
    private int indexer = 0;
    public int[] meshtriangles;
    private Vector3[] meshvertices;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "Icosphere";
        GenerateIcosphere();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int AddVertex(Vector3 newPoint)
    {
        vertices.Add(newPoint);
        return indexer++;
    }

    private int GetMidPoint(int vertex1, int vertex2)
    {
        // first check if we have it already
        bool firstIsSmaller = vertex1 < vertex2;
        long smallerIndex = firstIsSmaller ? vertex1 : vertex2;
        long greaterIndex = firstIsSmaller ? vertex2 : vertex1;
        long key = (smallerIndex << 32) + greaterIndex;

        int ret;
        if (this.middlePointIndexDict.TryGetValue(key, out ret))
        {
            return ret;
        }

        // not in cache, calculate it

        Vector3 point1 = vertices[vertex1];
        Vector3 point2 = vertices[vertex2];
        Vector3 middle = new Vector3((point1.x + point2.x) / 2.0f, (point1.y + point2.y) / 2.0f, (point1.z + point2.z) / 2.0f).normalized;

        // add vertex makes sure point is on unit sphere
        int i = AddVertex(middle);

        // store it, return index
        this.middlePointIndexDict.Add(key, i);
        return i;
    }
    

    void GenerateIcosphere()
    {
        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

        AddVertex(new Vector3(-1, t, 0).normalized);
        AddVertex(new Vector3(1, t, 0).normalized);
        AddVertex(new Vector3(-1, -t, 0).normalized);
        AddVertex(new Vector3(1, -t, 0).normalized);

        AddVertex(new Vector3(0, -1, t).normalized);
        AddVertex(new Vector3(0, 1, t).normalized);
        AddVertex(new Vector3(0, -1, -t).normalized);
        AddVertex(new Vector3(0, 1, -t).normalized);

        AddVertex(new Vector3(t, 0, -1).normalized);
        AddVertex(new Vector3(t, 0, 1).normalized);
        AddVertex(new Vector3(-t, 0, -1).normalized);
        AddVertex(new Vector3(-t, 0, 1).normalized);


        triangles.Add(new TriangleIndices(0, 11, 5));
        triangles.Add(new TriangleIndices(0, 5, 1));
        triangles.Add(new TriangleIndices(0, 1, 7));
        triangles.Add(new TriangleIndices(0, 7, 10));
        triangles.Add(new TriangleIndices(0, 10, 11));


        triangles.Add(new TriangleIndices(1, 5, 9));
        triangles.Add(new TriangleIndices(5, 11, 4));
        triangles.Add(new TriangleIndices(11, 10, 2));
        triangles.Add(new TriangleIndices(10, 7, 6));
        triangles.Add(new TriangleIndices(7, 1, 8));


        triangles.Add(new TriangleIndices(3, 9, 4));
        triangles.Add(new TriangleIndices(3, 4, 2));
        triangles.Add(new TriangleIndices(3, 2, 6));
        triangles.Add(new TriangleIndices(3, 6, 8));
        triangles.Add(new TriangleIndices(3, 8, 9));

        triangles.Add(new TriangleIndices(4, 9, 5));
        triangles.Add(new TriangleIndices(2, 4, 11));
        triangles.Add(new TriangleIndices(6, 2, 10));
        triangles.Add(new TriangleIndices(8, 6, 7));
        triangles.Add(new TriangleIndices(9, 8, 1));

        RefineIcoSphere();
    }

    void RefineIcoSphere()
    {
        TriangleIndices tri;
        for (int i = 0; i < subdivisionLevel; i++)
        {
          List<TriangleIndices> triangles2 = new List<TriangleIndices>();
            for (int j = 0; j < triangles.Count; j++)
            {
                tri = triangles[j];

                int a = GetMidPoint(tri.vertex1, tri.vertex2);
                int b = GetMidPoint(tri.vertex2, tri.vertex3);
                int c = GetMidPoint(tri.vertex3, tri.vertex1);
                
                triangles2.Add(new TriangleIndices(tri.vertex1, a, c));
                triangles2.Add(new TriangleIndices(tri.vertex2, b, a));
                triangles2.Add(new TriangleIndices(tri.vertex3, c, b));
                triangles2.Add(new TriangleIndices(a, b, c));

            }

            print("triangles2" + triangles2);
            triangles = triangles2;
        }


        foreach (var triangle in triangles)
        {
            holder.Add(triangle.vertex1);
            holder.Add(triangle.vertex2);
            holder.Add(triangle.vertex3);
        }

        print("holder" + holder);

        meshvertices = vertices.ToArray();
        meshtriangles = holder.ToArray();

        mesh.Clear();
        mesh.vertices = meshvertices;
        mesh.triangles = meshtriangles;
        mesh.RecalculateNormals();
    }
}
