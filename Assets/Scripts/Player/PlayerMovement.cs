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
    float groundLevel;
    float verticalSpeed;

    bool _isGrounded;
    bool isGrounded {
        get {
            return _isGrounded;
        }
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerCollider = GetComponent<Collider>();
        groundLevel = transform.position.y;
    }

    // Update is called once per frame
    private void Update()
    {
        float hMovement = Input.GetAxisRaw("Horizontal");
        float vMovement = Input.GetAxisRaw("Vertical");
        float jumpMovement = Input.GetAxisRaw("Jump");

        _isGrounded = IsGroundedRayCast();

        if (!isGrounded) {
            jumpMovement = 0;
            verticalSpeed -= 0.01f;
        } else if (jumpMovement > 0) {
            verticalSpeed = jumpMovement;
        }

        movementDirection = (hMovement * transform.right + vMovement * transform.forward + verticalSpeed * transform.up).normalized;
    }

    private void FixedUpdate() {        
        Move(moveSpeed);
    }


    private void Move (float frameSpeed) {
        characterController.Move(movementDirection * frameSpeed * Time.deltaTime);
    }

    private bool IsGroundedRayCast (){
        return true;
    }
}
