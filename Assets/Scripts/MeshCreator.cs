using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshCreator : MeshClass
{

    private int[] backsideNumArr = new int[25] {5,11,17,23,29, 41,47,53,59,65, 77,83,89,95,101, 113,119,125,131,137, 149,155,161,167,173};
    private int[] rightsideNumArr = new int[25] {30,31,32,33,34, 66,67,68,69,70, 102,103,104,105,106, 138,139,140,141,142, 174,175,176,177,178};
    private int[] topbackNumArr = new int[5] {185,191,197,203,209};
    private int[] toprightsideNumArr = new int[5] {210,211,212,213,214};
    private int[] backrightcornerArr = new int[5] {35,71,107,143,179};
    private const int numOfCubeSubdivisions = 5;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "subdividedCube";
        GenerateMesh();
        UpdateMesh();
        FillJointArray();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
    }

    public override void GenerateMesh()
    {

        meshvertices = new Vector3[(numOfCubeSubdivisions + 1) * (numOfCubeSubdivisions + 1) * (numOfCubeSubdivisions+ 1)];
        numOfVertices = meshvertices.Length;

        for (int i = 0, y = 0; y <= numOfCubeSubdivisions; y ++)
        {
            //print("y" + y);
            for (int x = 0; x <= numOfCubeSubdivisions; x ++)
            {
                //print("X" + x);
                for (int z = 0; z <= numOfCubeSubdivisions; z ++)
                {
                    //vertices.Add(new Vector3(x, y, z));
                    meshvertices[i] = new Vector3(x,y,z) / (numOfCubeSubdivisions);
                    i++;
                }
            }

        }

        //meshvertices = vertices.ToArray();

        int indexCounter = 0;

        //front
        for (int i = 0; i < meshvertices.Length - 42; i+= 6)
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
        for (int i = 5; i < meshvertices.Length -42 ; i+=6)
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
        for (int i = 30; i < meshvertices.Length - 37; i+= 36)
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
        for (int i = 180; i < meshvertices.Length - 7; i++)
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

        //print(triangles.Count);

        foreach (var tri in triangles)
        {
            holder.Add(tri.vertex1);
            holder.Add(tri.vertex2);
            holder.Add(tri.vertex3);
        }

    }
    public override void FillJointArray()
    {
        for (int i = 0; i < meshvertices.Length; i++)
        {
            ArrangeJointArray(i);
        }
    }

    private void ArrangeJointArray(int index)
    {
        if (IsNuminArray(index,rightsideNumArr))
        {
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 1);
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 36);
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 37);
        }
        else if (IsNuminArray(index,backsideNumArr))
        {
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 6);
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 36);
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 42);
        }
        else if (IsNuminArray(index,toprightsideNumArr))
        {
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 1);
        }  
        else if (IsNuminArray(index,topbackNumArr))
        {
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 6);
        }
        else if (IsNuminArray(index,backrightcornerArr))
        {
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 36);
        }
        else if (index >= 180 && index != 215 && !IsNuminArray(index,topbackNumArr) && !IsNuminArray(index,toprightsideNumArr))
        {
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 1);
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 6);
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 7);
        }
        else
        {
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 1);
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 6); 
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 7);
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 36);
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 37);
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 42);
            massPointIndexes.Add(index);
            massPointIndexes.Add(index + 43);
        }

    }

    public override void UpdateMesh()
    {     
        mesh.Clear();
        mesh.vertices = meshvertices;
        mesh.triangles = holder.ToArray();
        mesh.RecalculateNormals();
    }
    
    private bool IsNuminArray(int value, int[] arr)
    {
        foreach (int num in arr)
        {
            if (num.Equals(value))
            {
                return true;
            }
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < meshvertices.Length; i++)
        {
            Gizmos.DrawSphere(meshvertices[i], 0.02f);
        }

    }
}
