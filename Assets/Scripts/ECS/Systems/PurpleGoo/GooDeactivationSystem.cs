using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics;
using Unity.Transforms;

public class GooDeactivationSystem : SystemBase {

	EndSimulationEntityCommandBufferSystem endSimulationEcbSystem;

	protected override void OnCreate() {
		base.OnCreate();
		endSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
	}

	protected override void OnUpdate() {
		EntityCommandBuffer.ParallelWriter commandBuffer = endSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

		Entities
		.WithAll<PurpleGooCubeData>()
		.ForEach((Entity entity, int entityInQueryIndex, ref PurpleGooCubeData cubeData) => {
			if (cubeData.height <= 0) {
				//If cube height is zero or negative, move it out of the play area	
				if (cubeData.active) {
					cubeData.active = false;
					commandBuffer.AddComponent<InactiveGooCubeTag>(entityInQueryIndex, entity);
				}
			} else {
				// Move it back to the play area otherwise (but only if still inactive)
				if (!cubeData.active) {
					cubeData.active = true;
					commandBuffer.RemoveComponent<InactiveGooCubeTag>(entityInQueryIndex, entity);
				}
			}
		}).ScheduleParallel();

		endSimulationEcbSystem.AddJobHandleForProducer(Dependency);
	}
}

