using System.Linq;
using UnityEngine;
using Unity.Mathematics;

public class BomberSpawner : MonoBehaviour {

	[SerializeField]
	BomberSpawnPointData[] bomberSpawns;

	int currentSpawnIndex = 0;
	float timeUntilNextBomber = 0;
	float defaultBomberCooldown = 5;
	float bomberCooldownRandomDelta = 5;

    Unity.Mathematics.Random rnd = new Unity.Mathematics.Random();

	void Start() {
		ShuffleSpawnArray();
	}

	void Update() {
		timeUntilNextBomber -= Time.deltaTime;
		TrySpawnBomber();
	}

	private void TrySpawnBomber() {
		if (timeUntilNextBomber > 0)
			return;

        BomberSpawnPointData spawnData = bomberSpawns[currentSpawnIndex++];

        //Spawn bomber using position system

        if (currentSpawnIndex == bomberSpawns.Count()) {
            ShuffleSpawnArray();
            currentSpawnIndex = 0;
        }
	}

	private void ShuffleSpawnArray() {
		bomberSpawns = bomberSpawns.OrderBy(x => rnd.NextFloat() > 0.5f).ToArray();
	}
}

[System.Serializable]
public struct BomberSpawnPointData {
	public Transform spawnPositionTransform;
	public float3 direction;
	public float timeSinceLastDrop;
	public float cooldown;
	public float bombSize;
}