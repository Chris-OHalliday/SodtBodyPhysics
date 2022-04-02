using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassPoint : MonoBehaviour
{

    public struct MassPointObj
    {
        public Vector3 positionVector;
        public Vector3 velocityVector;
        public Vector3 forceVector;
    };

    public MeshClass objectBody;
    //public List<SpringStruct> springs = new List<SpringStruct>();
    public MassPointObj[] massPointObjs;
    public Vector3 gravityVector;
    //public Vector3[] positionVectors;
    public bool meshesGenerated = false;
    //public SpringJoint springJoint;

    [Range(0.0f, 1.0f)]
    public float mass = 1;

    private void Awake()
    {
        if (GetComponent<MeshCreator>() != null)
        {
            objectBody = GetComponent<MeshCreator>();
        }
        else
        {
            objectBody = GetComponent<IcoSphereGenerator>();
        }
    }

    private void Start()
    {

        gravityVector = new Vector3(0, -0.05f, 0) * mass;

        if (!meshesGenerated)
        {
            meshesGenerated = true;
            if (meshesGenerated)
            {
                massPointObjs = new MassPointObj[objectBody.numOfVertices];
                for (int i = 0; i < objectBody.meshvertices.Length; i++)
                {
                    massPointObjs[i].positionVector = objectBody.meshvertices[i] + transform.parent.position;
                    transform.localPosition = -(transform.parent.position);
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < objectBody.meshvertices.Length; i++)
        {       
            massPointObjs[i].forceVector += gravityVector;
            massPointObjs[i].velocityVector += massPointObjs[i].forceVector * (Time.deltaTime / mass);
            massPointObjs[i].positionVector += massPointObjs[i].velocityVector * Time.deltaTime;
            objectBody.meshvertices[i] = massPointObjs[i].positionVector;
            if (massPointObjs[i].positionVector.y <= 0)
            {

                massPointObjs[i].velocityVector = Vector3.zero;
            }

        }

    }

    private void LateUpdate()
    {
        for (int i = 0; i < objectBody.numOfVertices; i++)
        {
            massPointObjs[i].forceVector = Vector3.zero;
        }
    }

    public void ApplyForce(Vector3 force)
    {
        for (int i = 0; i < 2 ; i++)
        {
            massPointObjs[i].forceVector += force;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

    }
}
