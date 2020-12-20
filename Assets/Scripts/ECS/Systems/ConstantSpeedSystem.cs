using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;

public class ConstantSpeedSystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = Time.deltaTime;
		Entities.WithAll<ConstantSpeedData, Translation>().ForEach((
			Entity entity,
			ref Translation translation,
			in ConstantSpeedData constantSpeedData) => {
				translation.Value += constantSpeedData.constantSpeed;
        }).Schedule();
	}
}