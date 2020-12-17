using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

public class PurpleGooHeightSystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;

		Entities
		.WithAll<NonUniformScale, PurpleGooCubeData>()
		.ForEach((ref NonUniformScale nonUniformScale, in PurpleGooCubeData cubeData) => {
			nonUniformScale.Value.y = cubeData.height;
		}).Schedule();
	}
}
