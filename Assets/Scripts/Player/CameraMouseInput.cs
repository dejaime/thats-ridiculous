using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseInput : MonoBehaviour
{
    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    float verticalSensitivity = 1.5f;

    [SerializeField]
    float horizontalSensitivity = 1.5f;

    [SerializeField]
    float minX = -60f;

    [SerializeField]
    float maxX = 60f;

    Vector2 rotation;

    //This is used to solve a problem where the angle circles back from 0 to 359.999~ when moving on the negative direction.
    float verticalEulerAngle;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //Get the initial camera angle along the X axis (vertical [up/down] angle)
        verticalEulerAngle = cameraTransform.localEulerAngles.x;
        //Offset min and max by the initial value
        minX += verticalEulerAngle;
        maxX += verticalEulerAngle;

        Debug.Assert(cameraTransform != null, "Player camera transform reference is NULL. This is an error.");
    }


    void Update()
    {
        transform.localEulerAngles += new Vector3(
            0,
            Input.GetAxis("Mouse X") * horizontalSensitivity,
            0
        );

        
        verticalEulerAngle -= Input.GetAxis("Mouse Y") * verticalSensitivity;
        verticalEulerAngle = Mathf.Clamp(verticalEulerAngle, minX, maxX);

        cameraTransform.localEulerAngles = new Vector3(
            verticalEulerAngle,
            0,
            0
        );
    }
}
