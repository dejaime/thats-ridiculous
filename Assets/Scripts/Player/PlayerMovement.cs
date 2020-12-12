using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Inspector Variables
    [SerializeField]
    float moveSpeed;

    //Components
    CharacterController characterController;
    Collider playerCollider;

    //Control
    Vector3 movementDirection;
    RaycastHit hit;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    private void Update()
    {
        float hMovement = Input.GetAxisRaw("Horizontal");
        float vMovement = Input.GetAxisRaw("Vertical");

        movementDirection = (hMovement * transform.right + vMovement * transform.forward).normalized;
    }

    private void FixedUpdate() {
        float frameSpeed = moveSpeed;
        if (DetectWallHit(moveSpeed)) {
            frameSpeed = hit.distance;
        }
        
        Move(frameSpeed);
    }


    private void Move (float frameSpeed) {
        characterController.Move(movementDirection * frameSpeed * Time.deltaTime);
    }

}
