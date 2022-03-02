using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringJoint : MonoBehaviour
{

    public MassPoint massPointA;
    public MassPoint massPointB;

    [Range(0.0f, 100.0f)]
    public float springStiffness;
    [Range(0.0f, 100.0f)]
    public float springRestLength;
    [Range(0.0f, 100.0f)]
    public float springDampingFactor;


    private void Start()
    {
        springRestLength = (massPointB.positionVector - massPointA.positionVector).magnitude;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 massPointVectorDiff = (massPointB.positionVector - massPointA.positionVector);
        Vector3 massPointVectorDiff2 = (massPointA.positionVector - massPointB.positionVector);
        Vector3 massPointVelocityDiff = (massPointB.velocityVector - massPointA.velocityVector);

        float massPointLengthDiff = massPointVectorDiff.magnitude;
        if (massPointVectorDiff.magnitude != 0)
        {

            float differenceFromRestLength = massPointLengthDiff - springRestLength;

            float springForce = springStiffness * differenceFromRestLength;
            //float springForce = -springStiffness * springRestLength;

            float dampingForce = Vector3.Dot((massPointVectorDiff / massPointLengthDiff), massPointVelocityDiff) * springDampingFactor;

            float totalSpringForce = springForce + dampingForce;

            massPointA.ApplyForce(totalSpringForce * massPointVectorDiff.normalized);
            massPointB.ApplyForce(totalSpringForce * massPointVectorDiff2.normalized);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(massPointA.positionVector, massPointB.positionVector);
    }


}


