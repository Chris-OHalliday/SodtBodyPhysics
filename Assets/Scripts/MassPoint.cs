using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassPoint : MonoBehaviour
{
    
    public MeshClass objectBody;
    public Vector3[] velocityVector;
    public Vector3[] forceVector;
    public Vector3 gravityVector;
    public Vector3[] positionVector;
    bool meshesGenerated = false;
    //public SpringJoint springJoint;

    [Range(0.0f, 1.0f)]
    public float mass = 1;

    private void Start()
    {
        if (GetComponent<MeshCreator>() != null)
        {
            objectBody = GetComponent<MeshCreator>();
        }
        else 
        { 
            objectBody = GetComponent<IcoSphereGenerator>();
        }
        gravityVector = new Vector3(0, -0.1f, 0) * mass;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!meshesGenerated)
        {
            meshesGenerated = true;
            if (meshesGenerated)
            {
                velocityVector = new Vector3[objectBody.numOfVertices];
                positionVector = new Vector3[objectBody.numOfVertices];
                forceVector = new Vector3[objectBody.numOfVertices];
            }
        }

        for (int i = 0; i < objectBody.numOfVertices; i++)
        {
            positionVector[i] = objectBody.meshvertices[i];
            forceVector[i] += gravityVector;
            velocityVector[i] += forceVector[i] * (Time.deltaTime / mass);
            positionVector[i] += velocityVector[i] * Time.deltaTime;
            objectBody.meshvertices[i] = positionVector[i];           
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < objectBody.numOfVertices; i++)
        {
            forceVector[i] = Vector3.zero;
        }
    }

    public void ApplyForce(Vector3 force)
    {
        for (int i = 0; i < 2 ; i++)
        {
            forceVector[i] += force;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < positionVector.Length; i++)
        {
            Gizmos.DrawSphere(positionVector[i], 0.01f);
        }

    }
}