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
				//If cube height is zero or negative, move it out of the play area	
				translation.Value = new float3 {
					x = translation.Value.x,
					z = translation.Value.z,
					y = -50
				};
			} else {
				//Move it back to the play area otherwise (or keep it in)
				translation.Value = new float3 {
					x = translation.Value.x,
					z = translation.Value.z,
					y = 1
				};
			}
		}).Schedule();
	}
}
