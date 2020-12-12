using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseInput : MonoBehaviour
{
    [SerializeField]
    float xSensitivity = 0.35f;

    [SerializeField]
    float ySensitivity = 0.35f;

    [SerializeField]
    float minX = -60f;

    [SerializeField]
    float maxX = 60f;

    Vector2 rotation;


    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        rotation = new Vector2(
            Input.GetAxis("Mouse Y") * xSensitivity,
            Input.GetAxis("Mouse X") * ySensitivity
        );

        transform.localEulerAngles += new Vector3 (
            -Mathf.Clamp(rotation.x, minX, maxX),
            rotation.y,
            0
        );
    }
}
