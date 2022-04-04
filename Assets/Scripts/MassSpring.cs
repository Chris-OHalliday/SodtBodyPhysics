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
    };

    public MeshClass objectBody;
    public MassPointObj[] massPointObjs;
    public LinearSpring[] linearSprings;
    public Vector3 gravityVector;
    public float mass = 1;
    private float springStiffness = 80.0f;
    private float springDampingFactor = 0.8f;
    

    private void Start()
    {
        gravityVector = new Vector3(0.0f,0.5f,0.0f);
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
            linearSprings[i].springRestLength = (massPointObjs[linearSprings[i].pointIndexJ].positionVector - massPointObjs[linearSprings[i].pointIndexI].positionVector).magnitude;            
        }

    }

    private void Update()
    {

        for (int i = 0; i < objectBody.meshvertices.Length; i++)
        {
            //massPointObjs[i].forceVector = Vector3.zero;
            massPointObjs[i].forceVector += gravityVector * (Time.deltaTime / mass);
            
            
        }
        for (int i = 0; i < linearSprings.Length; i++)
        {
            Vector3 massPointVectorDiff = (massPointObjs[linearSprings[i].pointIndexJ].positionVector - massPointObjs[linearSprings[i].pointIndexI].positionVector);
            Vector3 massPointVectorDiff2 = (massPointObjs[linearSprings[i].pointIndexI].positionVector - massPointObjs[linearSprings[i].pointIndexJ].positionVector);
            Vector3 massPointVelocityDiff = (massPointObjs[linearSprings[i].pointIndexJ].velocityVector - massPointObjs[linearSprings[i].pointIndexI].velocityVector);

            float massPointLengthDiff = massPointVectorDiff.magnitude;
            if (massPointLengthDiff != 0)
            {

                float differenceFromRestLength = massPointLengthDiff - linearSprings[i].springRestLength;

                float springForce = springStiffness * differenceFromRestLength;
                //float springForce = -springStiffness * springRestLength;

                float dampingForce = Vector3.Dot((massPointVectorDiff / massPointLengthDiff), massPointVelocityDiff) * springDampingFactor;

                float totalSpringForce = springForce + dampingForce;

                massPointObjs[linearSprings[i].pointIndexI].forceVector += totalSpringForce * (massPointVectorDiff.normalized);
                massPointObjs[linearSprings[i].pointIndexJ].forceVector += totalSpringForce * (massPointVectorDiff2.normalized);

            }

        }
        for (int i = 0; i < objectBody.meshvertices.Length; i++)
        {
            massPointObjs[i].velocityVector += massPointObjs[i].forceVector * Time.deltaTime;
            
            massPointObjs[i].positionVector = massPointObjs[i].positionVector + massPointObjs[i].velocityVector * Time.deltaTime;
            //print(massPointObjs[i].positionVector);
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
