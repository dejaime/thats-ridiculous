using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class GooApplyPendingDamageSystem : SystemBase {
	protected override void OnUpdate() {

		Entities
		.WithAll<PurpleGooCubeData>()
		.WithNone<InactiveGooCubeTag>()
		.ForEach((ref PurpleGooCubeData cubeData) => {
			// cubeData.height -= cubeData.pendingDamage;
			GooCubeGrid.Instance.SetCubeHeight(
					cubeData.gridIndex.x,
					cubeData.gridIndex.z,
					cubeData.height - cubeData.pendingDamage
				);
			cubeData.pendingDamage = 0;
		}).WithoutBurst().Schedule();
	}
}
