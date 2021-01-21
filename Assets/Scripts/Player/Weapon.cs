using UnityEngine;
using Unity.Entities;
using Unity.Collections;

public class Weapon : MonoBehaviour {
	[SerializeField]
	CharacterController playerController;

	[SerializeField]
	[Tooltip("Time in seconds after which we can return MAX CHARGE when calculating total projectiles without doing the math." +
				"        Should be slightly over the time it would take the weapon to fully charge.")]
	int maxChargeTimeCutoff = 20;

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


	public GameObject projectilePrefab;
	private Entity projectileEntityTemplate;

	private EntityManager entityManager;
	private BlobAssetStore blobAssetStore;


	private Entity[] newProjectilesArray;
	int projectilesReady = 0;
	float timeSinceLastShot = 0;
	float timeUntilNextShot;


	private void Awake() {
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		blobAssetStore = new BlobAssetStore();
		GameObjectConversionSettings goConversionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
		projectileEntityTemplate = GameObjectConversionUtility.ConvertGameObjectHierarchy(projectilePrefab, goConversionSettings);

		
		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		// This ensures that the entity is disabled when created
		//	need to remove this component when Shoot is called and initial spatial
		//	components are populated
		commandBuffer.AddComponent<Disabled>(projectileEntityTemplate);
		
		commandBuffer.Playback(entityManager);
		commandBuffer.Dispose();

		timeUntilNextShot = cooldown;
		newProjectilesArray = new Entity[maxCharge];
	}


	private void Update() {
		timeUntilNextShot -= Time.deltaTime;
		timeSinceLastShot += Time.deltaTime;

		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		int totalProjectiles = GetTotalProjectiles();

		// Needs to be called before Shoot so there are enough projectiles prepared
		// Calling every Update also ensures we spread Entity creation through many frames
		PrepareProjectiles(commandBuffer, totalProjectiles);

		if (timeUntilNextShot < 0) {
			if (!Input.GetMouseButton(0)) {
				Shoot(commandBuffer, totalProjectiles);

				// This has to be done after calling Shot,
				//	as it will reset the weapon charge
				timeSinceLastShot = 0;
				timeUntilNextShot = cooldown;
			}
		}
		
		commandBuffer.Playback(entityManager);
		commandBuffer.Dispose();
	}


	private void Shoot(EntityCommandBuffer commandBuffer, int totalProjectiles) {
		float increaseSpread = 1 + 5 * totalProjectiles/maxCharge;

		int i;
		for (i = 0; i < totalProjectiles; ++i) {
			Entity newProjectile = newProjectilesArray[i];
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

		// Take over the i variable to continue where we stopped,
		int j;
		for (j = 0; i < projectilesReady; ++j) {
			newProjectilesArray[j] = newProjectilesArray[i++];
		}

		// Saving how many entities we saved in our cache.
		// I'm not nulling the rest just so I don't need to declare the
		//	array as nullable (i.e. Entity?[] newProjectiles)
		projectilesReady = j;
	}


	private void PrepareProjectiles(EntityCommandBuffer commandBuffer, int totalProjectiles) {
		int projectilesToPrepare = totalProjectiles - projectilesReady;

		int i;
		for (i = projectilesReady; i < totalProjectiles; ++i) {
			Entity newProjectile = entityManager.Instantiate(projectileEntityTemplate);
			newProjectilesArray[i] = newProjectile;
		}
		projectilesReady = i;

	}


	private int GetTotalProjectiles () {
		if (timeSinceLastShot > maxChargeTimeCutoff) return maxCharge;
		return Mathf.Min (Mathf.FloorToInt(chargeUpRate * timeSinceLastShot * (Mathf.Pow(1.1f, timeSinceLastShot))), maxCharge);
	}


	private void OnDestroy() {
		blobAssetStore.Dispose();
	}

}
