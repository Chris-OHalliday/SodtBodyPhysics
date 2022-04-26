using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringMassJoiner: MonoBehaviour
{

    public MassPoint massPoint;

    private float springStiffness = 80.0f;
    private float springDampingFactor = 0.8f;
    private float springRestLength = 0.0f;


    private void Start()
    {
        massPoint = GetComponent<MassPoint>();
    }

    // Update is called once per frame
    private void Update()
    {
            for (int i = 0; i <= massPoint.objectBody.massPointIndexes.Count -1; i+=2)
            {
                Vector3 massPointVectorDiff = (massPoint.massPointObjs[massPoint.objectBody.massPointIndexes[i+1]].positionVector - massPoint.massPointObjs[massPoint.objectBody.massPointIndexes[i]].positionVector);
                Vector3 massPointVectorDiff2 = (massPoint.massPointObjs[massPoint.objectBody.massPointIndexes[i]].positionVector - massPoint.massPointObjs[massPoint.objectBody.massPointIndexes[i+1]].positionVector);
                Vector3 massPointVelocityDiff = (massPoint.massPointObjs[massPoint.objectBody.massPointIndexes[i+1]].velocityVector - massPoint.massPointObjs[massPoint.objectBody.massPointIndexes[i]].velocityVector);

                float massPointLengthDiff = massPointVectorDiff.magnitude;
     

                if (massPointLengthDiff != 0)
                {

                    float differenceFromRestLength = massPointLengthDiff - springRestLength;

                    float springForce = springStiffness * differenceFromRestLength;
                    //float springForce = -springStiffness * springRestLength;

                    float dampingForce = Vector3.Dot((massPointVectorDiff / massPointLengthDiff), massPointVelocityDiff) * springDampingFactor;

                    float totalSpringForce = springForce + dampingForce;

                    massPoint.massPointObjs[massPoint.objectBody.massPointIndexes[i]].ApplyForce(totalSpringForce * massPointVectorDiff.normalized);           
                    massPoint.massPointObjs[massPoint.objectBody.massPointIndexes[i+1]].ApplyForce(totalSpringForce * massPointVectorDiff2.normalized);
        
                }
            }
    }

}


