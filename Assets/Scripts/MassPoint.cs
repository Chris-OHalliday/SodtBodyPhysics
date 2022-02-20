using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassPoint : MonoBehaviour
{
    
    public Vector3 velocityVector;
    public Vector3 forceVector;
    public Vector3 gravityVector;
    public SpringJoint springJoint;

    [Range(0.0f, 1.0f)]
    public float mass = 1;

    private void Start()
    {
        gravityVector = new Vector3(0, 0f, 0) * mass;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyForce(gravityVector);
        velocityVector += forceVector * (Time.deltaTime / mass);
        transform.position += velocityVector * Time.deltaTime;
    }

    private void LateUpdate()
    {        
        forceVector = Vector3.zero;
    }

    public void ApplyForce(Vector3 force)
    {
        forceVector += force;
    }


}
