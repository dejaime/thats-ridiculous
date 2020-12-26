using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class PurpleCubeGrid : MonoBehaviour {
	[SerializeField]
	private GridSize gridSizeInspectorXZ;

	[SerializeField]
	private int cubeSize;

	[SerializeField]
	private int yCubePosition;

	[SerializeField]
	private GridSize cubePositionOffsetXZ;

	private int3 gridSize;

	[SerializeField]
	private GameObject cubePrefab;

	[SerializeField]
	private float initialCubeHeight = -1f;


	private Entity cubeEntityTemplate;
	private EntityManager entityManager;
	private BlobAssetStore blobAssetStore;


	void Start() {
		gridSize = gridSizeInspectorXZ;

		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		blobAssetStore = new BlobAssetStore();
		GameObjectConversionSettings goConversionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
		cubeEntityTemplate = GameObjectConversionUtility.ConvertGameObjectHierarchy(cubePrefab, goConversionSettings);

		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		for (int x = 0; x < gridSize.x; ++x) {
			for (int z = 0; z < gridSize.z; ++z) {
				//Calculate POSITION with offset
				int3 position = new int3 {
					x = cubePositionOffsetXZ.x + cubeSize * x,
					z = cubePositionOffsetXZ.z + cubeSize * z,
					y = yCubePosition
				};

				PurpleGooCubeData pgc = new PurpleGooCubeData {
					gridPosition = {
						x = position.x,
						y = position.y
					},
					//Negative height deactivates the cubes. Increasing their height beyond 0 will activate them.
					//By deactivate I don't mean making them actually innactive, but only positioned outside of the play area.
					height = initialCubeHeight
				};

				//SPAWN Cube entity at position, set scale
				Entity newCube = entityManager.Instantiate(cubeEntityTemplate);

				commandBuffer.SetComponent<PurpleGooCubeData>(newCube, pgc);
				commandBuffer.SetComponent<Translation>(newCube, new Translation { Value = position });
				float3 scale = new float3 { y = 50f * Mathf.Sin((float)x / 10f) * Mathf.Sin((float)z / 10f), x = 3.5f, z = 3.5f };
				commandBuffer.AddComponent<NonUniformScale>(newCube, new NonUniformScale { Value = scale });
			}
		}

		commandBuffer.Playback(entityManager);
		commandBuffer.Dispose();
	}

	void Update() {

	}

	private void OnDestroy() {
		blobAssetStore.Dispose();
	}
}

//Using this custom struct to avoid the confusion caused by X, Y in int2, or X, Y, Z with int3.
//  With this, we can simply have X and Z only, avoiding confusion in code and in the inspector. 
[System.Serializable]
public struct GridSize {
	public int x;
	public int z;
	public static implicit operator int3(GridSize gs) {
		return new int3 { x = gs.x, z = gs.z, y = 0 };
	}
}