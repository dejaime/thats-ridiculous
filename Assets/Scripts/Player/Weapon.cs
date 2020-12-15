using UnityEngine;
using Unity.Entities;
using Unity.Collections;

public class Weapon : MonoBehaviour {
	[SerializeField]
	CharacterController playerController;

	[SerializeField]
	int baseProjectileCount = 10;

	[SerializeField]
	float chargeUpRate = 20;

	[SerializeField]
	float maxCharge = 2000;

	float additionalProjectiles = 0;

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
			if (!Input.GetMouseButton(0)) {
				Shoot();
			} else {
				additionalProjectiles = Mathf.Min (additionalProjectiles +chargeUpRate * Time.deltaTime, maxCharge);
			}
		}
	}


	private void Shoot() {
		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		int totalProjectiles = baseProjectileCount + (int) additionalProjectiles;
		float increaseSpread = 1 + 5 * additionalProjectiles/maxCharge;

		additionalProjectiles = 0;

		float eulerMinAngle = -45;
		float eulerMaxAngle = 45;

		float angleStep = (eulerMaxAngle - eulerMinAngle) / totalProjectiles;

		for (int i = 0; i < totalProjectiles; ++i) {
			Entity newProjectile = entityManager.Instantiate(projectileEntity);
			InitialProjectileSpatialData spatialData = new InitialProjectileSpatialData {
				spawnPosition = transform.position,
				speed = {
					z = Random.Range(-projectileSpeedMultiplier * increaseSpread, projectileSpeedMultiplier * increaseSpread),
					x = Random.Range(-projectileSpeedMultiplier * increaseSpread, projectileSpeedMultiplier * increaseSpread),
					y = Random.Range(-projectileSpeedMultiplier, projectileSpeedMultiplier),
				},
				acceleration = {
					x = transform.forward.x * projectileAccelerationMultiplier,
					z = transform.forward.z * projectileAccelerationMultiplier,
					y = 0
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
