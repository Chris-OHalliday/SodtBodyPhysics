using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshCreator : MonoBehaviour
{
    const int subdivisions = 4;
    const float VERTEXSPACE = 1 / subdivisions;
    public struct MassPointstruct {

        public Vector3 velocityVector;
        public Vector3 forceVector;
        public Vector3 gravityVector;
        public Vector3 positionVector;
        public float mass;

        public void massAccumulation()
        {
            ApplyForce(gravityVector);
            velocityVector += forceVector * (Time.deltaTime / mass);
            positionVector += velocityVector * Time.deltaTime;
        }

        public void ApplyForce(Vector3 force)
        {
            forceVector += force;
        }


    };


    private int CUBESIZE = 5;

    Mesh mesh;
    public Vector3[] vertices;
    public MassPointstruct[] masses;
    //public MassPoint[] massPoints;
    public SpringJoint[] springJoints;
    int[] triangles;



    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateCubeMesh();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < masses.Length; i++)
        {
            masses[i].massAccumulation();
        }
        UpdateMesh();
    }

    private void LateUpdate()
    {
        for (int i = 0; i < masses.Length; i++)
        {
            masses[i].forceVector = Vector3.zero;
        }
    }

    void CreateCubeMesh()
    {

        int cornerVertices = 8;
        int edgeVertices = (CUBESIZE + CUBESIZE + CUBESIZE - 3) * 4;

        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,1f,0),
            new Vector3(1f,0,0)
        };
        
        masses = new MassPointstruct[vertices.Length];

        triangles = new int[] 
        { 
            0,1,2
        };

        for (int i = 0; i < vertices.Length; i++)
        {
            masses[i].positionVector = vertices[i];
            masses[i].mass = 1;
            masses[i].gravityVector = new Vector3(0, -0.0f, 0) * masses[i].mass;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = masses[i].positionVector;
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    //visual
    private void OnDrawGizmos()
    {
        if (masses != null)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < masses.Length; i++)
            {
                Gizmos.DrawSphere(masses[i].positionVector, 0.01f);
            }
        }
    }


}
