using UnityEngine;
using Unity.Entities;
using Unity.Collections;

[AlwaysSynchronizeSystem]
public class TimeToLiveSystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;

		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		Entities.WithAll<TimeToLiveData>().ForEach((Entity entity, ref TimeToLiveData ttlData) => {
			ttlData.timeToLive -= deltaTime;
			if (ttlData.timeToLive < 0) {
				commandBuffer.AddComponent<DeleteEntityTag>(entity);
			}
		}).Run();

		commandBuffer.Playback(EntityManager);
		commandBuffer.Dispose();
	}
}
