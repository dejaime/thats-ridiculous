using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class PurpleGooHeightTestSystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = (float)Time.DeltaTime;

		float x = 0;

		Entities
		.WithAll<PurpleGooCubeData>()
		.ForEach((ref PurpleGooCubeData cubeData) => {
			x += 1f;
			//cubeData.height += 4 + Mathf.Sin(deltaTime + x);
		}).Run();
	}
}
