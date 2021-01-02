using UnityEngine;
using Unity.Entities;
using Unity.Collections;

[AlwaysSynchronizeSystem]
public class MaxHitsDeleteSystem : SystemBase {
	EndSimulationEntityCommandBufferSystem endSimulationEcbSystem;
	protected override void OnCreate() {
		base.OnCreate();
		endSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
	}
	protected override void OnUpdate() {
		EntityCommandBuffer.ParallelWriter commandBuffer = endSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();
		

		Entities.WithAll<TimeToLiveData>().ForEach((Entity entity, int entityInQueryIndex, in ProjectileData projectileData) => {
			if (projectileData.hitsLeft == 0) {
				commandBuffer.AddComponent<DeleteEntityTag>(entityInQueryIndex, entity);
			}
		}).ScheduleParallel();

		endSimulationEcbSystem.AddJobHandleForProducer(Dependency);
	}
}
