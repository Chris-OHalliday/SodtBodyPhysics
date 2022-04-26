using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSpring : MonoBehaviour
{
    public struct MassPointObj
    {
        public Vector3 positionVector;
        public Vector3 velocityVector;
        public Vector3 forceVector;
    };
    public struct LinearSpring
    {
        public int pointIndexI, pointIndexJ;
        public float springRestLength;
        public Vector3 normal;
    };

    private const float R = 8.31446261815324f;
    private const float T = 10f;
    private const float n = 1.0f;

    public GameObject floor;
    public MeshClass objectBody;
    public MassPointObj[] massPointObjs;
    public LinearSpring[] linearSprings;
    public Vector3 gravityVector;
    public float mass = 1;
    
    private float pressure;
    private float springStiffness = 80.0f;
    private float springDampingFactor = 0.6f;
    public ObjectManager objectManager;

    private void Start()
    {
        gravityVector = new Vector3(0.0f,-9.8f,0.0f);
        if (GetComponent<MeshCreator>() != null)
        {
            objectBody = GetComponent<MeshCreator>();
        }
        else
        {
            objectBody = GetComponent<IcoSphereGenerator>();
        }

        if (objectBody.meshesGenerated)
        {
            massPointObjs = new MassPointObj[objectBody.numOfVertices];
            for (int i = 0; i < objectBody.meshvertices.Length; i++)
            {                   
                objectBody.meshvertices[i] += transform.parent.position;
                massPointObjs[i].positionVector = objectBody.meshvertices[i];
                transform.localPosition = -(transform.parent.position);
            }
        }
       
        linearSprings = new LinearSpring[objectBody.massPointIndexes.Count / 2];
        
        int index = 0;

        for (int i = 0; i < objectBody.massPointIndexes.Count; i+= 2)
        {

            linearSprings[index].pointIndexI = objectBody.massPointIndexes[i];
            linearSprings[index].pointIndexJ = objectBody.massPointIndexes[i+1];
            if (index < linearSprings.Length)
            {
                //print(index);
                index++;
            }
        }

        for (int i = 0; i < linearSprings.Length; i++)
        {
            //index = 0;
            linearSprings[i].springRestLength = (massPointObjs[linearSprings[i].pointIndexJ].positionVector - massPointObjs[linearSprings[i].pointIndexI].positionVector).magnitude;
            if (objectBody is IcoSphereGenerator)
            {
                float distance = (massPointObjs[linearSprings[i].pointIndexI].positionVector - massPointObjs[linearSprings[i].pointIndexJ].positionVector).magnitude;
                float xnormal = (massPointObjs[linearSprings[i].pointIndexI].positionVector - massPointObjs[linearSprings[i].pointIndexJ].positionVector).normalized.x;
                objectBody.volume += (0.5f * Mathf.Abs(massPointObjs[linearSprings[i].pointIndexI].positionVector.x - massPointObjs[linearSprings[i].pointIndexJ].positionVector.x) * Mathf.Abs(xnormal) * distance);

            }
        }
        if (objectBody is IcoSphereGenerator)
        {
            print("im an icosphere this is my volume " + objectBody.volume);
            objectBody.surfaceArea = (Mathf.Pow(Mathf.PI,0.333f)) * (Mathf.Pow((6 * objectBody.volume),0.666f));

            
        }

    }

    private void Update()
    {
        //massPointObjs[0].forceVector += -(Vector3.one * 5);
        //massPointObjs[massPointObjs.Length - 1].forceVector += (Vector3.one * 5);

        for (int i = 0; i < objectBody.meshvertices.Length; i++)
        {
            massPointObjs[i].forceVector += gravityVector * (Time.deltaTime / mass);                     
        }

        if (objectBody is IcoSphereGenerator)
        {

            foreach (var triangle in objectBody.triangles)
            {
                float facefield = 0.5f * (Vector3.Cross((objectBody.meshvertices[triangle.vertex2] - objectBody.meshvertices[triangle.vertex1]), (objectBody.meshvertices[triangle.vertex3] - objectBody.meshvertices[triangle.vertex1])).magnitude);
                //print(facefield);
                pressure = (1 / objectBody.volume) * R * T * n;
                Vector3 normal = Vector3.Cross((objectBody.meshvertices[triangle.vertex1] - objectBody.meshvertices[triangle.vertex2]), (objectBody.meshvertices[triangle.vertex1] - objectBody.meshvertices[triangle.vertex3])).normalized;
                //print(normal);
                Vector3 pressureVec = pressure * normal;
                //print(pressureVec);
                Vector3 pressureForce = pressureVec * facefield;
                //print(pressureForce);

                massPointObjs[triangle.vertex1].forceVector += pressureForce;
                massPointObjs[triangle.vertex2].forceVector += pressureForce;
                massPointObjs[triangle.vertex3].forceVector += pressureForce;
                               
            }

        }

        for (int i = 0; i < linearSprings.Length; i++)
        {
            Vector3 massPointVectorDiff = (massPointObjs[linearSprings[i].pointIndexJ].positionVector - massPointObjs[linearSprings[i].pointIndexI].positionVector);
            Vector3 massPointVelocityDiff = (massPointObjs[linearSprings[i].pointIndexJ].velocityVector - massPointObjs[linearSprings[i].pointIndexI].velocityVector);

            float massPointLengthDiff = massPointVectorDiff.magnitude;
            if (massPointLengthDiff != 0)
            {

                float differenceFromRestLength = massPointLengthDiff - linearSprings[i].springRestLength;

                float springForce = springStiffness * differenceFromRestLength;

                float dampingForce = Vector3.Dot((massPointVectorDiff / massPointLengthDiff), massPointVelocityDiff) * springDampingFactor;

                float totalSpringForce = springForce + dampingForce;

                massPointObjs[linearSprings[i].pointIndexI].forceVector += totalSpringForce * (massPointVectorDiff.normalized);
                massPointObjs[linearSprings[i].pointIndexJ].forceVector += -(totalSpringForce * (massPointVectorDiff.normalized));

            }

        }

        for (int i = 0; i < objectBody.meshvertices.Length; i++)
        {
            massPointObjs[i].velocityVector += massPointObjs[i].forceVector * Time.deltaTime;
           
            massPointObjs[i].positionVector = massPointObjs[i].positionVector + massPointObjs[i].velocityVector * Time.deltaTime;

            objectBody.meshvertices[i] = massPointObjs[i].positionVector;
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < objectBody.meshvertices.Length; i++)
        {
            massPointObjs[i].forceVector = Vector3.zero;
        }
    }

}
