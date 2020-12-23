using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class ConstantSpeedSystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;
		Entities.WithAll<ConstantSpeedData, Translation>().ForEach((
			Entity entity,
			ref Translation translation,
			in ConstantSpeedData constantSpeedData) => {
				translation.Value += constantSpeedData.constantSpeed * deltaTime;
			}).Schedule();
	}
}