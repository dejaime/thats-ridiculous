using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class BomberSpawner : MonoBehaviour {

	[SerializeField]
	BomberSpawnData[] bomberSpawns;

	[SerializeField]
	GameObject bomberPrefab;

	[SerializeField]
	GameObject bombPrefab;

	float defaultBomberHeight = 200f;
	int currentSpawnIndex = 0;
	float timeUntilNextBomber = 0;
	float defaultBomberCooldown = 0;
	float bomberCooldownRandomDelta = 1;


	//ECS variables
	private Entity bomberEntityTemplate;
	private Entity bombEntityTemplate;
	private EntityManager entityManager;
	private BlobAssetStore blobAssetStore;

	void Start() {
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		blobAssetStore = new BlobAssetStore();
		GameObjectConversionSettings goConversionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
		bomberEntityTemplate = GameObjectConversionUtility.ConvertGameObjectHierarchy(bomberPrefab, goConversionSettings);
		bombEntityTemplate = GameObjectConversionUtility.ConvertGameObjectHierarchy(bombPrefab, goConversionSettings);

		ShuffleSpawnArray();
	}

	void Update() {
		timeUntilNextBomber -= Time.deltaTime;
		TrySpawnBomber();
	}

	private void OnDestroy() {
		blobAssetStore.Dispose();
	}

	private void TrySpawnBomber() {
		if (timeUntilNextBomber > 0)
			return;

		timeUntilNextBomber += defaultBomberCooldown + UnityEngine.Random.Range(0f, 1f) * bomberCooldownRandomDelta;

		SpawnBomberAt(bomberSpawns[currentSpawnIndex]);

		++currentSpawnIndex;
		if (currentSpawnIndex == bomberSpawns.Count()) {
			ShuffleSpawnArray();
			currentSpawnIndex = 0;
		}
	}


	private void SpawnBomberAt(BomberSpawnData spawnData) {
		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		Entity bomber = entityManager.Instantiate(bomberEntityTemplate);
		commandBuffer.SetComponent<Translation>(bomber, new Translation {
			Value = new float3 {
				x = spawnData.spawnPositionTransform.position.x,
				z = spawnData.spawnPositionTransform.position.z,
				y = defaultBomberHeight
			}
		});

		commandBuffer.SetComponent<ConstantSpeedData>(bomber, new ConstantSpeedData {
			constantSpeed = spawnData.direction
		});

		commandBuffer.SetComponent<GooBomberData>(bomber, new GooBomberData {
			timeSinceLastDrop = spawnData.timeSinceLastDrop,
			cooldown = spawnData.cooldown,
			bombSize = spawnData.bombSize,
			bombEntityTemplate = bombEntityTemplate
		});

		commandBuffer.AddComponent<Rotation>(bomber, new Rotation {
			Value = Quaternion.LookRotation(spawnData.direction, Vector3.up)
		});

		commandBuffer.Playback(entityManager);
		commandBuffer.Dispose();
	}


	private void ShuffleSpawnArray() {
		bomberSpawns = bomberSpawns.OrderBy(x => UnityEngine.Random.Range(0f, 1f) > 0.5f).ToArray();
	}
}

[System.Serializable]
public struct BomberSpawnData {
	public Transform spawnPositionTransform;
	public float3 direction;
	public float timeSinceLastDrop;
	public float cooldown;
	public float bombSize;
}
