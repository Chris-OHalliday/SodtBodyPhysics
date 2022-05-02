using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class volume : MonoBehaviour
{

    private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        //float result = 0;
        //for (int i = 0; i < mesh.triangles.Length; i+= 3)
        //{
        //    result += (Vector3.Cross(mesh.vertices[mesh.triangles[i+1]] - mesh.vertices[mesh.triangles[i]], mesh.vertices[mesh.triangles[i + 2]]- mesh.vertices[mesh.triangles[i]])).magnitude;
        //}

        //print(" result " + result);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(mesh.bounds.center, mesh.bounds.size);
    }

}
