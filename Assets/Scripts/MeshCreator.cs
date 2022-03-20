using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshCreator : MonoBehaviour
{

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
    public Vector3[] meshvertices;
    int[] meshtriangles;



    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "subdividedCube";
        CreateCubeMesh();
    }

    // Update is called once per frame
    void Update()
    {
        //for (int i = 0; i < meshvertices.Length; i++)
        //{
        //    meshvertices[i] -= new Vector3(0, 0.2f, 0) * Time.deltaTime;
        //}
        UpdateMesh();
    }

    void CreateCubeMesh()
    {

        //int cornerVertices = 8;
        //int edgeVertices = (CUBESIZE + CUBESIZE + CUBESIZE - 3) * 4;

        for (float y = 0f; y < 1.2f; y += 0.2f)
        {
            //print("y" + y);
            for (float x = 0f; x < 1.2f; x += 0.2f)
            {
                //print("X" + x);
                for (float z = 0f; z < 1.2f; z += 0.2f)
                {
                    vertices.Add(new Vector3(x, y, z));
                }
            }

        }

        meshvertices = vertices.ToArray();

        int indexCounter = 0;

        //front
        for (int i = 0; i < vertices.Count - 42; i+= 6)
        {
            if (indexCounter == 5)
            {
                indexCounter = 0;
            }
            else
            {
                triangles.Add(new TriangleIndices(i, i + 36, i + 6));
                triangles.Add(new TriangleIndices(i + 36, i + 42, i + 6));
                indexCounter++;
            }
            
        }

        int indexCounter2 = 0;
        //back
        for (int i = 5; i < vertices.Count -42 ; i+=6)
        {
            if (indexCounter2 == 5)
            {
                indexCounter2 = 0;
            }
            else
            {
                triangles.Add(new TriangleIndices(i , i + 6, i + 36));
                triangles.Add(new TriangleIndices(i + 6, i + 42, i + 36));
                indexCounter2++;
            }
        }   


        //right
        for (int i = 30; i < vertices.Count - 37; i+= 36)
        {
            for (int j = i; j < i+5; j++)
            {
                    triangles.Add(new TriangleIndices(j, j + 36, j + 1));
                    triangles.Add(new TriangleIndices(j + 36, j + 37, j + 1));

            }
        }
              
        //left
        for (int i = 0; i < 185- 37; i+= 36)
        {
            for (int j = i; j < i+5; j++)
            {
                    triangles.Add(new TriangleIndices(j, j + 1, j + 37));
                    triangles.Add(new TriangleIndices(j + 37, j + 36, j));

            }
        }


        int indexCounter3 = 0;
        //bottom
        for (int i = 1; i < 31; i++)
        {
            if (indexCounter3 == 5)
            {
                indexCounter3 = 0;
            }
            else
            {
                triangles.Add(new TriangleIndices(i, i - 1, i + 6));
                triangles.Add(new TriangleIndices(i-1,i+5,i+6));
                indexCounter3++;
            }
        }     
        
        int indexCounter4 = 0;
        //top
        for (int i = 180; i < vertices.Count - 7; i++)
        {
            if (indexCounter4 == 5)
            {
                indexCounter4 = 0;
            }
            else
            {
                triangles.Add(new TriangleIndices(i, i + 1, i + 6));
                triangles.Add(new TriangleIndices(i+1,i+7,i+6));
                indexCounter4++;
            }
        }

        print(triangles.Count);

        foreach (var tri in triangles)
        {
            holder.Add(tri.vertex1);
            holder.Add(tri.vertex2);
            holder.Add(tri.vertex3);
        }

        meshtriangles = holder.ToArray();
        //meshvertices = new Vector3[]
        //{
        //    //front
        //    new Vector3(0,0,0), //0
        //    new Vector3(0,0.2f,0), //1
        //    new Vector3(0.2f,0,0), //2 
        //    new Vector3(0.2f,0.2f,0), //3

        //    //back
        //    new Vector3(0,0,0.2f), //4 
        //    new Vector3(0,0.2f,0.2f), //5
        //    new Vector3(0.2f,0,0.2f), //6 
        //    new Vector3(0.2f,0.2f,0.2f) //7
        //};

        //triangles = new int[]
        //{ 
        //    //front
        //    0,1,2,
        //    1,3,2,

        //    //right
        //    2,3,6,
        //    3,7,6,

        //    //back
        //    6,7,4,
        //    7,5,4,

        //    //left
        //    4,5,0,
        //    5,1,0,

        //    //top
        //    1,5,3,
        //    5,7,3,

        //    //bottom
        //    4,0,6,
        //    0,2,6

        //};

        
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = meshvertices;
        mesh.triangles = meshtriangles;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < meshvertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.05f);
        }

        //Gizmos.color = Color.red;
        //for (int i = 0; i < meshtriangles.Length - 1 ; i++)
        //{
        //    Gizmos.DrawLine(vertices[meshtriangles[i]], meshvertices[meshtriangles[i+1]]);
        //}

    }
}


//int indexCounter3 = 0;
////right
//for (int i = 30; i < vertices.Count - 37; i++)
//{
//    if (indexCounter3 == 5)
//    {
//        indexCounter3 = 0;
//    }
//    else
//    {
//        triangles.Add(new TriangleIndices(i, i + 36, i + 1));
//        triangles.Add(new TriangleIndices(i + 36, i + 37, i + 1));
//        indexCounter3++;
//    }
//}