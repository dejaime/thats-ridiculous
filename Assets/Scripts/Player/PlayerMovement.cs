using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Inspector Variables
    [SerializeField]
    float moveSpeed;

    //Components
    Rigidbody rigidBody;

    //Control
    Vector3 movementDirection;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        float hMovement = Input.GetAxisRaw("Horizontal");
        float vMovement = Input.GetAxisRaw("Vertical");

        movementDirection = (hMovement * transform.right + vMovement * transform.forward).normalized;
    }

    private void FixedUpdate() {
        Move();
    }


    private void Move () {
        rigidBody.velocity = movementDirection * moveSpeed * Time.deltaTime;
    }
}
