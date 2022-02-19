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
        gravityVector = new Vector3(0, 0, 0)*mass;
    }

    // Update is called once per frame
    void Update()
    {
        forceVector = Vector3.zero;
        Vector3 forcetoAdd = gravityVector;
        forceVector += forcetoAdd;
        velocityVector += (forceVector * Time.deltaTime) / mass;
        transform.position += velocityVector * Time.deltaTime;
    }
}
