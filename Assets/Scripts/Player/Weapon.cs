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
	int maxCharge = 2000;

	[SerializeField]
	float projectileSpeedMultiplier = 100;

	[SerializeField]
	float projectileAccelerationMultiplier = 10;

	[SerializeField]
	float cooldown = 0.2f;

	float timeUntilNextShot = 0f;

	public GameObject projectilePrefab;
	private Entity projectileEntityTemplate;

	private EntityManager entityManager;
	private BlobAssetStore blobAssetStore;


	private Entity[] newProjectiles;
	int projectilesReady = 0;
	float timeSinceLastShot = 0;


	private void Awake() {
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		blobAssetStore = new BlobAssetStore();
		GameObjectConversionSettings goConversionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
		projectileEntityTemplate = GameObjectConversionUtility.ConvertGameObjectHierarchy(projectilePrefab, goConversionSettings);
		newProjectiles = new Entity[maxCharge];
	}


	private void Update() {
		timeUntilNextShot -= Time.deltaTime;
		timeSinceLastShot += Time.deltaTime;

		// Needs to be called before Shoot so there are enough projectiles prepared
		// Calling every Update also ensures we spread Entity creation through many frames
		PrepareProjectiles();

		if (timeUntilNextShot < 0) {
			if (!Input.GetMouseButton(0)) {
				Shoot();

				// This has to be done after calling Shot,
				//	as it will reset the weapon charge
				timeSinceLastShot = 0;

				timeUntilNextShot += cooldown;
				if (timeUntilNextShot < 0) {
					timeUntilNextShot = 0;
				}
			}
		}
	}


	private void Shoot() {
		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		int totalProjectiles = GetTotalProjectiles();
		float increaseSpread = 6 * totalProjectiles/maxCharge;

		for (int i = 0; i < totalProjectiles; ++i) {
			Entity newProjectile = newProjectiles[i];
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
			commandBuffer.SetComponent<InitialProjectileSpatialData>(newProjectile, spatialData);
			commandBuffer.RemoveComponent<Disabled>(newProjectile);
		}

		commandBuffer.Playback(entityManager);
		commandBuffer.Dispose();

		// TODO Clean newProjectiles array from used entities by bringing entities
		//	with indexes >= totalProjectiles and < projectilesReady to the front of the array.

		// TODO Assign null to remaining space (indexes >= projectilesReady - totalProjectiles),
		//	and update projectilesReady to reflect it.
	}
	

	private void PrepareProjectiles() {
		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		int totalProjectiles = GetTotalProjectiles();
		int projectilesToPrepare = totalProjectiles - projectilesReady;

		for (int i = 0; i < totalProjectiles; ++i) {
			Entity newProjectile = entityManager.Instantiate(projectileEntityTemplate);
			
			commandBuffer.SetComponent<ProjectileData>(newProjectile, new ProjectileData{
				hitsLeft = 1,
				scale = 1f
			});

			// This ensures that the entity is disabled when created
			//	need to remove this component when Shoot is called and initial spatial
			//	components are populated
			commandBuffer.AddComponent<Disabled>(newProjectile);
		}

		commandBuffer.Playback(entityManager);
		commandBuffer.Dispose();
	}


	private int GetTotalProjectiles () {
		return Mathf.Min (Mathf.FloorToInt(chargeUpRate * timeSinceLastShot), maxCharge);
	}


	private void OnDestroy() {
		blobAssetStore.Dispose();
	}

}
