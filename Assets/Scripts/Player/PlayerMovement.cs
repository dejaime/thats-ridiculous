using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
		//Inspector Variables
		[SerializeField]
		float _moveSpeed = 100f;

        [SerializeField]
		float airSpeedMultiplier = 1.1f;

        [SerializeField]
        float jumpIntensity = 1.5f;

        [SerializeField]
        float gravity = 0.01f;

        float moveSpeed {
            get{ return isGrounded ? _moveSpeed : _moveSpeed * airSpeedMultiplier; }
        }

        

		[SerializeField]
		float isGroundedDistance = 1.5f;

		//Components
		CharacterController characterController;
		Collider playerCollider;
		MeshRenderer meshRenderer;

		//Control
		Vector3 movementDirection;

		//  '- - Jump
		RaycastHit groundRaycastHit;
		float groundLevel;
		float verticalInertialSpeed;

		bool isGrounded;


		private void Awake() {
				characterController = GetComponent<CharacterController>();
				playerCollider = GetComponent<Collider>();
				meshRenderer = GetComponentInChildren<MeshRenderer>();
				groundLevel = transform.position.y;
		}


		// Update is called once per frame
		private void Update() {
				float hMovement = Input.GetAxisRaw("Horizontal");
				float frontMovement = Input.GetAxisRaw("Vertical");
				float jumpMovement = Input.GetAxisRaw("Jump");

				GroundedRayCastTest();

				if (isGrounded) {
						if (jumpMovement > 0) {
								verticalInertialSpeed = jumpMovement * jumpIntensity;
						} else {
								verticalInertialSpeed = 0;
						}
				} else {
						//Is not grounded, apply Gravity
						verticalInertialSpeed -= gravity;
				}

				movementDirection = (hMovement * transform.right + frontMovement * transform.forward + verticalInertialSpeed * transform.up).normalized;
		}

		private void FixedUpdate() {
				Move();
		}


		private void Move() {
				Debug.Log("movement direction " +  (movementDirection * moveSpeed * Time.deltaTime).sqrMagnitude);
				characterController.Move(movementDirection * moveSpeed * Time.deltaTime);
		}


		private void GroundedRayCastTest() {
				isGrounded = Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out groundRaycastHit, isGroundedDistance);

				if (isGrounded) {
						meshRenderer.material.color = Color.green;
				} else {
						meshRenderer.material.color = Color.red;
				}
		}


		private void OnDrawGizmos() {
				Debug.DrawRay(transform.position, Vector3.down * isGroundedDistance, Color.red);
		}
}
