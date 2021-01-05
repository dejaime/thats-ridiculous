using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class GooCubeGrid : MonoBehaviour {

	private static GooCubeGrid _instance;

	public static GooCubeGrid Instance { get { return _instance; } }

	public const float DEACTIVATED_CUBE_Y_POSITION = -10000;

	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}


	[SerializeField]
	private GridSize gridSizeInspectorXZ;

	[SerializeField]
	private int cubeSize;

	[SerializeField]
	private float yCubePosition = GooCubeGrid.DEACTIVATED_CUBE_Y_POSITION;

	[SerializeField]
	private GridSize cubePositionOffsetXZ;

	private int3 gridSize;

	private static float[,] gridCubeHeightMatrix = null;

	[SerializeField]
	private GameObject cubePrefab;

	[SerializeField]
	private float initialCubeHeight = -1f;

	private Entity cubeEntityTemplate;
	private EntityManager entityManager;
	private BlobAssetStore blobAssetStore;


	[SerializeField]
	float spreadCooldown = 0.5f;

	[SerializeField]
	float spreadStrength = 2f;

	[SerializeField]
	private float spreadDeltaFactor = 4f;

	private float timeSinceLastSpread = 0f;


	void Start() {
		gridSize = gridSizeInspectorXZ;

		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		blobAssetStore = new BlobAssetStore();
		GameObjectConversionSettings goConversionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
		cubeEntityTemplate = GameObjectConversionUtility.ConvertGameObjectHierarchy(cubePrefab, goConversionSettings);

		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		gridCubeHeightMatrix = new float[gridSize.x, gridSize.z];

		for (int x = 0; x < gridSize.x; ++x) {
			for (int z = 0; z < gridSize.z; ++z) {
				//Calculate POSITION with offset
				float3 position = new float3 {
					x = cubePositionOffsetXZ.x + cubeSize * x,
					z = cubePositionOffsetXZ.z + cubeSize * z,
					y = yCubePosition
				};

				gridCubeHeightMatrix[x, z] = initialCubeHeight;

				PurpleGooCubeData pgc = new PurpleGooCubeData {
					gridIndex = {
						x = x,
						z = z,
						y = 0
					},
					//Negative height deactivates the cubes. Increasing their height beyond 0 will activate them.
					//By deactivate I don't mean making them actually innactive, but only positioned outside of the play area.
					height = gridCubeHeightMatrix[x, z]
				};

				//SPAWN Cube entity at position, set scale
				Entity newCube = entityManager.Instantiate(cubeEntityTemplate);

				if (gridCubeHeightMatrix[x, z] < 0) {
					commandBuffer.AddComponent<InactiveGooCubeTag>(newCube, new InactiveGooCubeTag { });
					pgc.active = false;
				} else {
					pgc.active = true;
				}

				commandBuffer.SetComponent<PurpleGooCubeData>(newCube, pgc);
				commandBuffer.SetComponent<Translation>(newCube, new Translation { Value = position });
			
			}
		}

		commandBuffer.Playback(entityManager);
		commandBuffer.Dispose();
	}

	private void OnDestroy() {
		blobAssetStore.Dispose();
	}


	private void Update() {
		SpreadGoo();
	}


	public void SetCubeHeight(int x, int z, float height) {
		if (gridCubeHeightMatrix == null) return;
		gridCubeHeightMatrix[x, z] = height;
	}

	public float GetCubeHeight(int x, int z) {
		return gridCubeHeightMatrix[x, z];
	}

	public void Explode(float bombDropX, float bombDropZ, float size) {
		int3 index = PositionToIndex(bombDropX, bombDropZ);
		int x = index.x;
		int z = index.z;

		for (int i = x - 5; i <= x + 5; ++i) {
			for (int j = z - 5; j <= z + 5; ++j) {
				if (i <= 0 || i >= gridSize.x - 1 || j <= 0 || j >= gridSize.z - 1) {
					continue;
				} else {
					if (gridCubeHeightMatrix[i, j] <= 0) {
						gridCubeHeightMatrix[i, j] = 0;
					}
					gridCubeHeightMatrix[i, j] += size;
				}
			}
		}
	}

	public int3 PositionToIndex(float x, float z) {
		float indexX = (x - cubePositionOffsetXZ.x) / cubeSize;
		float indexZ = (z - cubePositionOffsetXZ.z) / cubeSize;
		return new int3 {
			x = (int)indexX,
			y = 0,
			z = (int)indexZ
		};
	}


	private void SpreadGoo() {
		timeSinceLastSpread += Time.deltaTime;
		if (timeSinceLastSpread >= spreadCooldown) {
			timeSinceLastSpread -= spreadCooldown;
			for (int i = 1; i < gridSize.x - 1; ++i) {
				for (int j = 1; j < gridSize.z - 1; ++j) {
					SpreadCube(i, j);
				}
			}
		}
	}


	private void SpreadCube(int x, int z) {
		if (gridCubeHeightMatrix[x, z] - gridCubeHeightMatrix[x + 1, z] > spreadStrength * spreadDeltaFactor) {
			gridCubeHeightMatrix[x + 1, z] += spreadStrength;
			gridCubeHeightMatrix[x, z] -= spreadStrength;
		} else if (gridCubeHeightMatrix[x + 1, z] - gridCubeHeightMatrix[x, z] > spreadStrength * spreadDeltaFactor) {
			gridCubeHeightMatrix[x + 1, z] -= spreadStrength;
			gridCubeHeightMatrix[x, z] += spreadStrength;
		}

		if (gridCubeHeightMatrix[x, z] - gridCubeHeightMatrix[x, z + 1] > spreadStrength * spreadDeltaFactor) {
			gridCubeHeightMatrix[x, z + 1] += spreadStrength;
			gridCubeHeightMatrix[x, z] -= spreadStrength;
		} else if (gridCubeHeightMatrix[x, z + 1] - gridCubeHeightMatrix[x, z] > spreadStrength * spreadDeltaFactor) {
			gridCubeHeightMatrix[x, z + 1] -= spreadStrength;
			gridCubeHeightMatrix[x, z] += spreadStrength;
		}
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