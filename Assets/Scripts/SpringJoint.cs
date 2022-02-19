using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringJoint : MonoBehaviour
{

    public MassPoint massPointA;
    public MassPoint massPointB;

    [Range(0.0f,1.0f)]
    public float springStiffness;
    [Range(0.0f, 1.0f)]
    public float springRestLength;
    [Range(0.0f, 1.0f)]
    public float springDampingFactor;

    private Vector3 forceA;
    private Vector3 forceB;


    // Update is called once per frame
    void Update()
    {

        Vector3 massPointVectorDiff = (massPointB.transform.position - massPointA.transform.position);
        Vector3 massPointVectorDiff2 = (massPointA.transform.position - massPointB.transform.position);
        Vector3 massPointVelocityDiff = (massPointB.velocityVector - massPointA.velocityVector);

        float massPointLengthDiff = massPointVectorDiff.magnitude;

        float differenceFromRestLength = massPointLengthDiff - springRestLength;

        float springForce = -springStiffness * springRestLength;
        //float springForce = springStiffness * springRestLength;

        float dampingForce = Vector3.Dot((massPointVectorDiff / massPointLengthDiff), massPointVelocityDiff) * springDampingFactor;

        float totalSpringForce = springForce + dampingForce;

        massPointA.forceVector += totalSpringForce * massPointVectorDiff.normalized;
        massPointB.forceVector += totalSpringForce * massPointVectorDiff2.normalized;

    }

    public Vector3 GetForceForA()
    {

        return forceA;
    }  

    public Vector3 GetForceForB()
    {
        return forceB;
    }


}


