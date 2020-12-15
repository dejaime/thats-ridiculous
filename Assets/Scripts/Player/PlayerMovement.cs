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
	float jumpIntensity = 45f;

	[SerializeField]
	float gravity = 0.8f;

	float moveSpeed {
		get { return isGrounded ? _moveSpeed : _moveSpeed * airSpeedMultiplier; }
	}



	[SerializeField]
	float isGroundedDistance = 1.5f;

	//Components
	CharacterController characterController;
	Collider playerCollider;
	MeshRenderer meshRenderer;

	//Control
	Queue<Vector3> spawnPositionQueue;

	[SerializeReference]
	Vector3 movementDirection;

	//  '- - Jump
	RaycastHit groundRaycastHit;
	float verticalInertialSpeed;

	bool isGrounded;


	private void Awake() {
		characterController = GetComponent<CharacterController>();
		playerCollider = GetComponent<Collider>();
		meshRenderer = GetComponentInChildren<MeshRenderer>();
		spawnPositionQueue = new Queue<Vector3>();
		FillSpawnPositionQueue(transform.position);
	}


	// Update is called once per frame
	private void Update() {
		float hMovement = Input.GetAxisRaw("Horizontal");
		float frontMovement = Input.GetAxisRaw("Vertical");
		float jumpMovement = Input.GetAxisRaw("Jump");

		SphereCastTestGrounded();

		if (isGrounded) {
			if (jumpMovement > 0) {
				verticalInertialSpeed = jumpMovement * jumpIntensity;
			} else {
				if (verticalInertialSpeed < 0) {
					verticalInertialSpeed = 0;
				}
			}
		} else {
			//Is not grounded, apply Gravity
			verticalInertialSpeed -= gravity;
		}

		movementDirection = (hMovement * transform.right + frontMovement * transform.forward).normalized;
	}

	private void FixedUpdate() {
		Move();
	}


	private void Move() {
		characterController.Move((movementDirection * moveSpeed + verticalInertialSpeed * transform.up) * Time.deltaTime);
	}


	private void SphereCastTestGrounded() {
		isGrounded = Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out groundRaycastHit, isGroundedDistance);

		if (groundRaycastHit.collider != null && groundRaycastHit.collider.tag == "KillTrigger") {
			// transform.position = spawnPosition;
			KillPlayer();
		}

		if (isGrounded) {
			spawnPositionQueue.Enqueue(transform.position);
			spawnPositionQueue.Dequeue();
			meshRenderer.material.color = Color.green;
		} else {
			meshRenderer.material.color = Color.red;
		}
	}


	private void KillPlayer() {
		characterController.enabled = false;
		characterController.transform.position = spawnPositionQueue.Dequeue();
		characterController.enabled = true;
		FillSpawnPositionQueue(transform.position);
	}


	private void OnDrawGizmos() {
		Debug.DrawRay(transform.position, Vector3.down * isGroundedDistance, Color.red);
	}


	private void FillSpawnPositionQueue(Vector3 position) {
		spawnPositionQueue.Clear();
		for (int i = 0; i < 5; ++i) {
			spawnPositionQueue.Enqueue(position);
		}
	}
}
