using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class SimpleGravitySystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;
		Entities.WithAll<SimpleGravityData, Translation>().ForEach((
			ref Translation translation,
			ref SimpleGravityData simpleGravityData) => {
				translation.Value.y += simpleGravityData.currentVerticalSpeed;
                simpleGravityData.currentVerticalSpeed += simpleGravityData.gravity * deltaTime;
			}).Schedule();
	}
}