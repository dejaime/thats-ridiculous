using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class UpdateGridHeightSystem : SystemBase {
	protected override void OnUpdate() {
		GooCubeGrid cubeGrid = GooCubeGrid.Instance;
		Entities
		.WithAll<PurpleGooCubeData>()
		.ForEach((ref PurpleGooCubeData cubeData) => {
			cubeData.height = cubeGrid.GetCubeHeight(
				cubeData.gridIndex.x,
				cubeData.gridIndex.z
			);
		}).WithoutBurst().Run();
	}
}
