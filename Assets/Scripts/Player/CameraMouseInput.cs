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


    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Assert(cameraTransform != null, "Player camera transform reference is NULL. This is an error.");
    }


    void Update()
    {
        rotation = new Vector2(
            Input.GetAxis("Mouse Y") * verticalSensitivity,
            Input.GetAxis("Mouse X") * horizontalSensitivity
        );

        transform.localEulerAngles += new Vector3 (
            0,
            rotation.y,
            0
        );

        cameraTransform.localEulerAngles += new Vector3 (
            -Mathf.Clamp(rotation.x, minX, maxX),
            0,
            0
        );
    }
}
