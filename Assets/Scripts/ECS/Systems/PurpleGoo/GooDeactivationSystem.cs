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
		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		Entities
		.WithAll<Translation, PurpleGooCubeData>()
		.ForEach((Entity entity, int entityInQueryIndex, ref Translation translation, ref PurpleGooCubeData cubeData) => {
			if (cubeData.height <= 0) {
				//If cube height is zero or negative, move it out of the play area	
				if (cubeData.active) {
					cubeData.active = false;
					translation.Value.y = -1000f;
					commandBuffer.AddComponent<InactiveGooCubeTag>(entity);
				}
			} else {
				// Move it back to the play area otherwise (but only if still inactive)
				if (!cubeData.active) {
					cubeData.active = true;
					translation.Value.y = 1;
					commandBuffer.RemoveComponent<InactiveGooCubeTag>(entity);
				}
			}
		}).Run();

		commandBuffer.Playback(EntityManager);
		commandBuffer.Dispose();
	}
}

