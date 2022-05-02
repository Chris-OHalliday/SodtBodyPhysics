using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraMovement : MonoBehaviour
{
    CharacterController Controller;

    public float Speed;
    public float sens = 5.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private const float YMin = -80.0f;
    private const float YMax = 80.0f;
    private bool lockMovement = false;
    // Start is called before the first frame update
    void Start()
    {

        Controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {

        currentX += Input.GetAxis("Mouse X") * sens;
        currentY -= Input.GetAxis("Mouse Y") * sens;
        currentX = Mathf.Repeat(currentX, 360);
        currentY = Mathf.Clamp(currentY, YMin, YMax);
        Camera.main.transform.rotation = Quaternion.Euler(currentY, currentX, 0);


        float horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        //float jump = Input.GetAxis("Jump") * Speed * Time.deltaTime;


        Vector3 Movement = transform.right * horizontal + transform.forward * vertical;

        if (!lockMovement)
        {
            Controller.Move(Movement);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleLockMovement();
        }


    }
    private void ToggleLockMovement()
    {
        if (lockMovement)
        {
            lockMovement = false;
        }
        else
        {
            lockMovement = true;
        }
    }

}
