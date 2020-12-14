using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;

public class Weapon : MonoBehaviour {
	[SerializeField]
	CharacterController playerController;

	[SerializeField]
	int baseProjectileCount = 10;

	int additionalProjectiles = 0;

	[SerializeField]
	int shotsPerRow = 20;

	[SerializeField]
	int firingSpreadEulerAngle = 45;

	[SerializeField]
	float projectileSpeedMultiplier = 100;

	[SerializeField]
	float projectileAccelerationMultiplier = 10;

	[SerializeField]
	float cooldown = 0.2f;

	float timeSinceLastShot = 0f;

	public GameObject projectilePrefab;
	private Entity projectileEntity;

	private EntityManager entityManager;
	private BlobAssetStore blobAssetStore;

	private void Awake() {
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		blobAssetStore = new BlobAssetStore();
		GameObjectConversionSettings goConversionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
		projectileEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(projectilePrefab, goConversionSettings);
	}


	private void Update() {
		timeSinceLastShot -= Time.deltaTime;

		if (timeSinceLastShot < 0) {
			timeSinceLastShot += cooldown;
			Shoot();
		}
	}


	private void Shoot() {
		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		int totalProjectiles = baseProjectileCount + additionalProjectiles;

		float eulerMinAngle = -45;
		float eulerMaxAngle = 45;

		float angleStep = (eulerMaxAngle - eulerMinAngle) / totalProjectiles;

		for (int i = 0; i < totalProjectiles; ++i) {
			Entity newProjectile = entityManager.Instantiate(projectileEntity);
			InitialProjectileSpatialData spatialData = new InitialProjectileSpatialData {
				spawnPosition = transform.position,
				speed = {
					z = transform.forward.z * projectileSpeedMultiplier + playerController.velocity.z/2,
					x = transform.forward.x * projectileSpeedMultiplier + playerController.velocity.x/2,
					y = 2
				},
				acceleration = { 
					x = transform.forward.x * projectileAccelerationMultiplier,
					z = transform.forward.z * projectileAccelerationMultiplier,
					y = -3f
				},
			};
			commandBuffer.AddComponent<InitialProjectileSpatialData>(newProjectile, spatialData);
		}

		commandBuffer.Playback(entityManager);
		commandBuffer.Dispose();
	}


	private void OnDestroy() {
		blobAssetStore.Dispose();
	}

}
