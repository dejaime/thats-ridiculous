using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class PurpleGooHeightTestSystem : SystemBase {
	protected override void OnUpdate() {
		float elapsedTime = (float)Time.ElapsedTime;

		float x = 0;

		Entities
		.WithAll<PurpleGooCubeData>()
		.ForEach((ref PurpleGooCubeData cubeData) => {
			x += 1.01f;
			cubeData.height = 4 + Mathf.Sin(elapsedTime + x) * 30;
		}).Run();
	}
}
