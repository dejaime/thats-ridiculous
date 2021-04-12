using Unity.Entities;

public class GooApplyPendingDamageSystem : SystemBase {
	protected override void OnUpdate() {
		GooCubeGrid cubeGrid = GooCubeGrid.Instance;
		Entities
		.WithAll<PurpleGooCubeData>()
		.WithNone<InactiveGooCubeTag>()
		.ForEach((ref PurpleGooCubeData cubeData) => {
			cubeGrid.SetCubeHeight(
					cubeData.gridIndex.x,
					cubeData.gridIndex.z,
					cubeGrid.GetCubeHeight(cubeData.gridIndex.x, cubeData.gridIndex.z) - cubeData.pendingDamage
				);
			cubeData.pendingDamage = 0;
		}).WithoutBurst().Run();
	}
}
