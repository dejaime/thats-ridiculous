using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class PurpleGooDeactivationSystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;

		Entities
		.WithAll<Translation, PurpleGooCubeData>()
		.ForEach((ref Translation translation, in PurpleGooCubeData cubeData) => {
			if (cubeData.height <= 0) {
				translation.Value = new float3 {
					x = translation.Value.x,
					z = translation.Value.z,
					y = -100
				};
			}
		}).Schedule();
	}
}
